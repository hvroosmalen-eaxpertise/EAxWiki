using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using Microsoft.Extensions.Logging;

namespace EAxWiki.EA;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class EaReaderStaDispatcher : IEaReader, IDisposable
{
    private readonly ILogger _logger;
    private readonly BlockingCollection<WorkItem> _workQueue = new(new ConcurrentQueue<WorkItem>());
    private readonly ManualResetEventSlim _initComplete = new(false);
    private readonly Thread _staThread;
    private Exception? _initException;
    private bool _disposed;
    private volatile bool _isHealthy;

    /// <summary>
    /// True after a successful COM work item, false after a reconnect attempt fails. Reflects the
    /// dispatcher's last observed EA COM state so <c>/readyz</c> can report "not ready" the moment
    /// the model is unreachable instead of waiting for the next explicit probe.
    /// </summary>
    public bool IsHealthy => _isHealthy;

    public EaReaderStaDispatcher(ILogger logger, string repositoryPath)
    {
        _logger = logger;
        _staThread = new Thread(() => RunStaPump(repositoryPath))
        {
            Name = "EaReader STA",
            IsBackground = true
        };
        _staThread.SetApartmentState(ApartmentState.STA);
        _staThread.Start();
        _initComplete.Wait(TimeSpan.FromSeconds(30));
        if (_initException != null)
            throw new InvalidOperationException("Failed to initialize EA COM on STA thread.", _initException);
    }

    public string RepositoryPath =>
        throw new NotSupportedException("EaReaderStaDispatcher opens the repository in its constructor.");

    public EaRepository Open(string connectionString, CancellationToken ct = default) =>
        throw new NotSupportedException("EaReaderStaDispatcher opens the repository in its constructor.");

    public bool TestConnection(string connectionString, out string? error)
    {
        using var reader = new EaReader();
        return reader.TestConnection(connectionString, out error);
    }

    public void Close() =>
        throw new NotSupportedException("Use Dispose to shut down the STA dispatcher.");

    private void RunStaPump(string repositoryPath)
    {
        EaReader? reader = null;
        try
        {
            reader = OpenNewReader(repositoryPath);
            _isHealthy = true;
        }
        catch (Exception ex)
        {
            _initException = ex;
            return;
        }
        finally
        {
            _initComplete.Set();
        }

        if (_initException != null) return;

        try
        {
            foreach (var work in _workQueue.GetConsumingEnumerable())
            {
                ExecuteWithReconnect(
                    execute: () => work.Execute(reader!),
                    reconnect: () =>
                    {
                        reader!.Dispose();
                        try
                        {
                            reader = OpenNewReader(repositoryPath);
                        }
                        catch (Exception reconnectEx)
                        {
                            _logger.LogError(reconnectEx, "EA reconnection failed.");
                            throw;
                        }
                        _logger.LogInformation("EA reconnection succeeded.");
                    },
                    shouldRetry: ex => ex is COMException && !_disposed,
                    onRetry: (ex, retries, maxRetries) =>
                    {
                        _isHealthy = false;
                        _logger.LogWarning(ex, "EA COM disconnected (retry {Retry}/{MaxRetries}); reconnecting.", retries, maxRetries);
                    },
                    onFailure: ex => work.SetException(ex),
                    onHealthy: () => _isHealthy = true,
                    maxRetries: 1);
            }
        }
        finally
        {
            reader?.Dispose();
        }
    }

    private EaReader OpenNewReader(string repositoryPath)
    {
        var newReader = new EaReader(_logger as ILogger<EaReader>);
        newReader.Open(repositoryPath);
        return newReader;
    }

    // The work-item retry loop from RunStaPump, extracted so the reconnect semantics are
    // unit-testable without a live EA repository. Semantics identical to the original loop:
    //   * up to maxRetries + 1 attempts total
    //   * on shouldRetry(ex) with retries remaining -> onRetry(ex, retryNumber, maxRetries),
    //     then reconnect(); if reconnect throws -> onFailure(reconnectEx) and stop
    //   * on non-retryable exception or retries exhausted -> onFailure(ex) and stop
    //   * on success -> onHealthy() and stop
    internal static void ExecuteWithReconnect(
        Action execute,
        Action reconnect,
        Func<Exception, bool> shouldRetry,
        Action<Exception, int, int> onRetry,
        Action<Exception> onFailure,
        Action onHealthy,
        int maxRetries)
    {
        var retries = 0;
        var executed = false;
        while (!executed && retries <= maxRetries)
        {
            try
            {
                execute();
                onHealthy();
                executed = true;
            }
            catch (Exception ex) when (shouldRetry(ex) && retries < maxRetries)
            {
                retries++;
                onRetry(ex, retries, maxRetries);
                try
                {
                    reconnect();
                }
                catch (Exception reconnectEx)
                {
                    onFailure(reconnectEx);
                    executed = true;
                }
            }
            catch (Exception ex)
            {
                onFailure(ex);
                executed = true;
            }
        }
    }

    private T Dispatch<T>(Func<EaReader, T> func) =>
        DispatchAsync(func).GetAwaiter().GetResult();

    private void DispatchVoid(Action<EaReader> action) =>
        DispatchVoidAsync(action).GetAwaiter().GetResult();

    // Async siblings return the underlying TaskCompletionSource.Task without blocking. The write-back
    // server's ASP.NET request thread can go back to the pool while the STA thread processes the
    // work item (issue #85). The STA thread stays single-threaded — COM requires it — only the
    // caller side is freed.
    private Task<T> DispatchAsync<T>(Func<EaReader, T> func)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_initException != null)
            throw new InvalidOperationException("EA COM initialization failed on STA thread.", _initException);

        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        _workQueue.Add(new WorkItem(
            reader => tcs.TrySetResult(func(reader)),
            ex => tcs.TrySetException(ex)));

        return tcs.Task;
    }

    private Task DispatchVoidAsync(Action<EaReader> action)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_initException != null)
            throw new InvalidOperationException("EA COM initialization failed on STA thread.", _initException);

        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _workQueue.Add(new WorkItem(
            reader => { action(reader); tcs.TrySetResult(); },
            ex => tcs.TrySetException(ex)));

        return tcs.Task;
    }

    public IReadOnlyList<string> GetStatusTypes() => Dispatch(r => r.GetStatusTypes());

    public string GetElementStatus(int elementId) => Dispatch(r => r.GetElementStatus(elementId));

    public void UpdateElementStatus(int elementId, string newStatus) =>
        DispatchVoid(r => r.UpdateElementStatus(elementId, newStatus));

    public void UpdateElementNotes(int elementId, string newNotesHtml) =>
        DispatchVoid(r => r.UpdateElementNotes(elementId, newNotesHtml));

    public void UpdateDiagramNotes(int diagramId, string newNotesHtml) =>
        DispatchVoid(r => r.UpdateDiagramNotes(diagramId, newNotesHtml));

    public void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml) =>
        DispatchVoid(r => r.UpdateAttributeNotes(elementId, attributeName, attributeType, newNotesHtml));

    public void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml) =>
        DispatchVoid(r => r.UpdateMethodNotes(elementId, methodName, returnType, isStatic, newNotesHtml));

    public void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml) =>
        DispatchVoid(r => r.UpdateTaggedValueNotes(elementId, tagName, tagValue, newNotesHtml));

    public void UpdatePackageNotes(int packageId, string newNotesHtml) =>
        DispatchVoid(r => r.UpdatePackageNotes(packageId, newNotesHtml));

    public bool ExportDiagramImage(string diagramGuid, string filePath) =>
        Dispatch(r => r.ExportDiagramImage(diagramGuid, filePath));

    public EaElementSummary? GetElementSummary(int elementId) =>
        Dispatch(r => r.GetElementSummary(elementId));

    public EaDiagramSummary? GetDiagramSummary(int diagramId) =>
        Dispatch(r => r.GetDiagramSummary(diagramId));

    // Real async overrides — these are what WikiWritebackServer's endpoints call. Everything else
    // (WriteBackScanner, tests) can stay on the sync API and eat the block on GetResult; only the
    // HTTP request path benefits from freeing its ASP.NET thread while the STA hop runs.
    public Task<IReadOnlyList<string>> GetStatusTypesAsync(CancellationToken ct = default) =>
        DispatchAsync(r => r.GetStatusTypes());

    public Task<EaElementSummary?> GetElementSummaryAsync(int elementId, CancellationToken ct = default) =>
        DispatchAsync(r => r.GetElementSummary(elementId));

    public Task<EaDiagramSummary?> GetDiagramSummaryAsync(int diagramId, CancellationToken ct = default) =>
        DispatchAsync(r => r.GetDiagramSummary(diagramId));

    public Task UpdateElementStatusAsync(int elementId, string newStatus, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateElementStatus(elementId, newStatus));

    public Task UpdateElementNotesAsync(int elementId, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateElementNotes(elementId, newNotesHtml));

    public Task UpdateDiagramNotesAsync(int diagramId, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateDiagramNotes(diagramId, newNotesHtml));

    public Task UpdateAttributeNotesAsync(int elementId, string attributeName, string attributeType, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateAttributeNotes(elementId, attributeName, attributeType, newNotesHtml));

    public Task UpdateMethodNotesAsync(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateMethodNotes(elementId, methodName, returnType, isStatic, newNotesHtml));

    public Task UpdateTaggedValueNotesAsync(int elementId, string tagName, string tagValue, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdateTaggedValueNotes(elementId, tagName, tagValue, newNotesHtml));

    public Task UpdatePackageNotesAsync(int packageId, string newNotesHtml, CancellationToken ct = default) =>
        DispatchVoidAsync(r => r.UpdatePackageNotes(packageId, newNotesHtml));

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _workQueue.CompleteAdding();
        if (_staThread.IsAlive)
            _staThread.Join(TimeSpan.FromSeconds(5));

        GC.SuppressFinalize(this);
    }

    internal sealed class WorkItem
    {
        private readonly Action<EaReader> _execute;
        private readonly Action<Exception> _onError;

        public WorkItem(Action<EaReader> execute, Action<Exception> onError)
        {
            _execute = execute;
            _onError = onError;
        }

        // Deliberately does NOT swallow exceptions: RunStaPump's ExecuteWithReconnect needs to
        // observe COMException to trigger a reconnect. The pump routes non-retryable failures via
        // SetException.
        public void Execute(EaReader reader) => _execute(reader);

        public void SetException(Exception ex) => _onError(ex);
    }
}
