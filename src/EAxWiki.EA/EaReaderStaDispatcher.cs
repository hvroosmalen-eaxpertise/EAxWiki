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
            reader = new EaReader(_logger as ILogger<EaReader>);
            reader.Open(repositoryPath);
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
                int retries = 0;
                const int maxRetries = 1;
                bool executed = false;
                while (!executed && retries <= maxRetries)
                {
                    try
                    {
                        work.Execute(reader);
                        executed = true;
                    }
                    catch (COMException ex) when (!_disposed && retries < maxRetries)
                    {
                        retries++;
                        _logger.LogWarning(ex,
                            "EA COM disconnected (retry {Retry}/{MaxRetries}); reconnecting.",
                            retries, maxRetries);
                        reader?.Dispose();
                        try
                        {
                            reader = new EaReader(_logger as ILogger<EaReader>);
                            reader.Open(repositoryPath);
                            _logger.LogInformation("EA reconnection succeeded.");
                        }
                        catch (Exception reconnectEx)
                        {
                            _logger.LogError(reconnectEx, "EA reconnection failed.");
                            work.SetException(reconnectEx);
                            executed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        work.SetException(ex);
                        executed = true;
                    }
                }
            }
        }
        finally
        {
            reader?.Dispose();
        }
    }

    private T Dispatch<T>(Func<EaReader, T> func)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_initException != null)
            throw new InvalidOperationException("EA COM initialization failed on STA thread.", _initException);

        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        _workQueue.Add(new WorkItem(
            reader => tcs.TrySetResult(func(reader)),
            ex => tcs.TrySetException(ex)));

        return tcs.Task.GetAwaiter().GetResult();
    }

    private void DispatchVoid(Action<EaReader> action)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_initException != null)
            throw new InvalidOperationException("EA COM initialization failed on STA thread.", _initException);

        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _workQueue.Add(new WorkItem(
            reader => { action(reader); tcs.TrySetResult(); },
            ex => tcs.TrySetException(ex)));

        tcs.Task.GetAwaiter().GetResult();
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

    public bool ExportDiagramImage(string diagramGuid, string filePath) =>
        Dispatch(r => r.ExportDiagramImage(diagramGuid, filePath));

    public EaElementSummary? GetElementSummary(int elementId) =>
        Dispatch(r => r.GetElementSummary(elementId));

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _workQueue.CompleteAdding();
        if (_staThread.IsAlive)
            _staThread.Join(TimeSpan.FromSeconds(5));

        GC.SuppressFinalize(this);
    }

    private sealed class WorkItem
    {
        private readonly Action<EaReader> _execute;
        private readonly Action<Exception> _onError;

        public WorkItem(Action<EaReader> execute, Action<Exception> onError)
        {
            _execute = execute;
            _onError = onError;
        }

        public void Execute(EaReader reader)
        {
            try { _execute(reader); }
            catch (Exception ex) { _onError(ex); }
        }

        public void SetException(Exception ex) => _onError(ex);
    }
}
