using System.Diagnostics;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Core.Monitoring;
using EAxWiki.EA;
using EAxWiki.Export;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

// ── Output metrics ─────────────────────────────────────────────────────────

public interface IWikiOutputMetrics
{
    /// <summary>Count of generated markdown pages (elements + diagrams together).</summary>
    int CountMarkdownFiles(string wikiDir);

    /// <summary>Count of markdown pages under a "diagrams" path segment.</summary>
    int CountDiagramFiles(string wikiDir);
}

public class WikiOutputMetrics : IWikiOutputMetrics
{
    public int CountMarkdownFiles(string wikiDir)
    {
        if (!Directory.Exists(wikiDir)) return 0;
        return Directory.EnumerateFiles(wikiDir, "*.md", SearchOption.AllDirectories).Count();
    }

    public int CountDiagramFiles(string wikiDir)
    {
        if (!Directory.Exists(wikiDir)) return 0;
        return Directory.EnumerateFiles(wikiDir, "*.md", SearchOption.AllDirectories)
            .Count(f => f.Replace('\\', '/').Contains("/diagrams/"));
    }
}

// ── STA in-process export ──────────────────────────────────────────────────

public interface IStaExporter
{
    /// <summary>Run one full export on an STA thread (EaReader → optional write-back scan → MarkdownExporter).</summary>
    void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? brand, string? aiEndpoint);
}

/// <summary>
/// In-process export on an STA thread, mirroring EaReaderStaDispatcher's threading: EA COM is
/// apartment-threaded, so the export runs on a dedicated STA thread and the caller blocks on the
/// result. A broad catch rethrows so ExportRunner's per-run crash boundary can record the failure
/// and continue the loop instead of killing the monitor.
/// </summary>
public class StaMarkdownExporter : IStaExporter
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

    public StaMarkdownExporter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("Export");
        _loggerFactory = loggerFactory;
    }

    internal ILogger<MarkdownExporter> CreateExporterLogger() =>
        _loggerFactory.CreateLogger<MarkdownExporter>();

    public void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? brand, string? aiEndpoint)
    {
        Exception? failure = null;
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            try
            {
                // Nested block so reader.Dispose() runs INSIDE this try and BEFORE the
                // TCS signals completion. Signalling first would let the caller race
                // the STA thread's cleanup — e.g. MonitorLoop's finally could kill the
                // EA.exe RCW mid-Dispose, and the resulting throw would land in the
                // catch below and fail a second SetException on an already-completed TCS.
                using (var reader = new EaReader(_loggerFactory.CreateLogger<EaReader>()))
                {
                    var repository = reader.Open(repoPath);

                    if (writeBack && Directory.Exists(outputPath))
                    {
                        _logger.LogInformation("Running write-back scan...");
                        var scanner = new WriteBackScanner(reader, _loggerFactory.CreateLogger<WriteBackScanner>());
                        var scanResult = scanner.Scan(outputPath);
                        if (scanResult.StatusChanges.Count == 0 && scanResult.NotesChanges.Count == 0)
                            _logger.LogInformation("Write-back: no changes detected.");
                        else
                            _logger.LogInformation("Write-back: applied {Status} status and {Notes} notes change(s).",
                                scanResult.StatusChanges.Count, scanResult.NotesChanges.Count);
                    }

                    var writer = new FileOutputWriter();
                    var exporter = new MarkdownExporter(writer, CreateExporterLogger());
                    var result = exporter.ExportAsync(repository, null, outputPath, reader, force)
                        .GetAwaiter().GetResult();
                    _logger.LogInformation("Export finished: {Total} pages, {Failed} failed, {Diagrams} diagrams.",
                        result.TotalElements, result.FailedElements, result.DiagramsExported);
                }
                tcs.TrySetResult();
            }
            catch (Exception ex)
            {
                failure = ex;
                tcs.TrySetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        tcs.Task.GetAwaiter().GetResult();
        if (failure != null) throw failure;
    }
}

// ── ExportRunner ───────────────────────────────────────────────────────────

public interface IExportRunner
{
    bool ShouldForce(int runsSinceForce);
    Task<bool> RunExportAsync(bool effectiveForce, WritebackDelta writebacks, CancellationToken ct);
}

public class ExportRunner : IExportRunner
{
    private readonly MonitorOptions _options;
    private readonly IStaExporter _exporter;
    private readonly IWikiOutputMetrics _metrics;
    private readonly HealthState _state;
    private readonly IAlertDispatcher _alerts;
    private readonly ILogger<ExportRunner> _logger;

    public ExportRunner(
        MonitorOptions options,
        IStaExporter exporter,
        IWikiOutputMetrics metrics,
        HealthState state,
        IAlertDispatcher alerts,
        ILogger<ExportRunner> logger)
    {
        _options = options;
        _exporter = exporter;
        _metrics = metrics;
        _state = state;
        _alerts = alerts;
        _logger = logger;
    }

    public bool ShouldForce(int runsSinceForce) =>
        _options.Force || (_options.ForceEveryNRuns > 0 && runsSinceForce >= _options.ForceEveryNRuns);

    public async Task<bool> RunExportAsync(bool effectiveForce, WritebackDelta writebacks, CancellationToken ct)
    {
        // Expose API port / AI endpoint / brand to MarkdownExporter exactly like EAxWiki/Program.cs.
        Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", _options.ApiPort.ToString());
        if (!string.IsNullOrEmpty(_options.AiEndpoint))
            Environment.SetEnvironmentVariable("EAXWIKI_AI_ENDPOINT", _options.AiEndpoint);
        Environment.SetEnvironmentVariable("EAXWIKI_BRAND", _options.Brand ?? string.Empty);

        _state.LastMode = effectiveForce ? "full (--force)" : "incremental";
        _logger.LogInformation("Mode: {Mode}.", _state.LastMode);

        // Recovery detection looks at the state carried INTO this run, not failures accrued by it.
        var wasFailing = _state.ConsecutiveFailures > 0;
        var succeeded = false;
        var lastExitCode = 1;
        var elementCount = 0;
        var previousCount = _state.LastElementCount ?? 0;
        var diagramCount = 0;
        var outputTail = "";
        var succeededOnAttempt = 0;
        var lastAttempt = 0;
        var stopwatch = Stopwatch.StartNew();

        for (var attempt = 1; attempt <= _options.MaxRetries && !succeeded; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            lastAttempt = attempt;
            _logger.LogInformation("Attempt {Attempt}/{MaxRetries} starting.", attempt, _options.MaxRetries);
            lastExitCode = 1;
            try
            {
                _exporter.ExportOnSta(
                    _options.RepoPath ?? string.Empty,
                    _options.WikiDir,
                    effectiveForce,
                    _options.ApiPort > 0,
                    _options.ApiPort,
                    _options.Brand,
                    _options.AiEndpoint);
                lastExitCode = 0;

                elementCount = _metrics.CountMarkdownFiles(_options.WikiDir);
                diagramCount = _metrics.CountDiagramFiles(_options.WikiDir);
                var floor = Math.Floor(previousCount * _options.MinElementFraction);
                if (previousCount > 0 && elementCount < floor)
                {
                    _logger.LogWarning("Sanity check failed: element count {Count} below floor {Floor} (previous {Previous}).",
                        elementCount, floor, previousCount);
                    outputTail = $"Sanity check failed: element count {elementCount} below floor {floor} (previous {previousCount}).";
                    lastExitCode = 1;
                    _state.ConsecutiveFailures++;
                    break;
                }

                succeeded = true;
                succeededOnAttempt = attempt;
                _state.LastElementCount = elementCount;
            }
            catch (Exception ex)
            {
                outputTail = ex.Message;
                _state.ConsecutiveFailures++;
                _logger.LogWarning("Attempt {Attempt} failed: {Error}", attempt, ex.Message);
            }

            if (!succeeded && attempt < _options.MaxRetries)
            {
                var delay = _options.RetryDelaySeconds * attempt;
                _logger.LogInformation("Retrying in {Delay} seconds.", delay);
                await Task.Delay(TimeSpan.FromSeconds(delay), ct);
            }
        }
        stopwatch.Stop();

        _state.LastExitCode = lastExitCode;

        if (succeeded)
        {
            _state.LastSuccessTime = DateTimeOffset.Now;
            _state.ConsecutiveFailures = 0;
            _state.RunsSinceForce = effectiveForce ? 0 : _state.RunsSinceForce + 1;
            _state.LastApiPort = _options.ApiPort;
            if (wasFailing)
                _alerts.Dispatch($"Export succeeded, recovering from a prior failure.", AlertKind.Recovery);

            _state.LastDiagramCount = diagramCount;
            var pageDelta = elementCount - previousCount;
            var deltaLabel = pageDelta >= 0 ? $"+{pageDelta}" : pageDelta.ToString();

            var validationSuffix = BuildValidationSuffix();
            var writebackSuffix = "";
            if (_options.NotifyOnStart)
            {
                if (writebacks.Total > 0)
                {
                    var parts = writebacks.Kinds
                        .OrderByDescending(kv => kv.Value)
                        .Select(kv => $"{kv.Value} {kv.Key}");
                    writebackSuffix = $" - write-backs: {string.Join(", ", parts)}";
                }
                _alerts.Dispatch(
                    $"Export finished in {stopwatch.Elapsed:mm\\:ss} - {elementCount} page(s) total ({diagramCount} diagram, {elementCount - diagramCount} element), {deltaLabel} vs previous run.{validationSuffix}{writebackSuffix}",
                    AlertKind.Finish);
            }
            _logger.LogInformation("Succeeded on attempt {Attempt} in {Elapsed}.", succeededOnAttempt, stopwatch.Elapsed.ToString("mm\\:ss"));
        }
        else
        {
            _state.LastFailureTime = DateTimeOffset.Now;
            _logger.LogWarning("Gave up after {Attempts} attempt(s).", lastAttempt);
            _alerts.Dispatch($"Export failed after {lastAttempt} attempt(s) (exit code {lastExitCode}).\n```\n{outputTail}\n```",
                AlertKind.Failure);
        }

        return succeeded;
    }

    private string BuildValidationSuffix()
    {
        var reportPath = Path.Combine(_options.WikiDir, ".validation-report.json");
        if (!File.Exists(reportPath)) return "";
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(reportPath));
            var root = doc.RootElement;
            var errors = root.TryGetProperty("Errors", out var e) && e.TryGetInt32(out var ev) ? ev : 0;
            var warnings = root.TryGetProperty("Warnings", out var w) && w.TryGetInt32(out var wv) ? wv : 0;
            var passed = root.TryGetProperty("Passed", out var p) && p.TryGetInt32(out var pv) ? pv : 0;
            var files = root.TryGetProperty("FilesValidated", out var f) && f.TryGetInt32(out var fv) ? fv : 0;

            var parts = new List<string>();
            if (errors > 0) parts.Add($"{errors} error(s)");
            if (warnings > 0) parts.Add($"{warnings} warning(s)");
            return parts.Count > 0
                ? $" - validation: {string.Join(", ", parts)} ({passed}/{files} files clean)"
                : $" - all {files} files validated clean";
        }
        catch (System.Text.Json.JsonException)
        {
            return "";
        }
        catch (IOException)
        {
            return ""; // validation report momentarily locked/being rewritten - treat as absent
        }
    }
}
