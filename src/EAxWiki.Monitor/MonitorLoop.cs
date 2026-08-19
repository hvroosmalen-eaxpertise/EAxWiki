using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

/// <summary>
/// The monitor's while(true) cycle: reset skip flags, export when due (edit-lock-aware), digest
/// accounting, health page + state save, then serve/API/LLM watchdogs with recovery/give-up
/// alerts. The constructor takes resolved specs so the caller (MonitorApp) wires the real child
/// processes; tests substitute stubs.
/// </summary>
public class MonitorLoop
{
    private readonly MonitorOptions _options;
    private readonly HealthState _state;
    private readonly HealthStore _healthStore;
    private readonly HealthPageRenderer _pageRenderer;
    private readonly ErrorLogPageRenderer _errorsPageRenderer;
    private readonly ConfigPageRenderer _configPageRenderer;
    private readonly string _stateDir;
    private readonly IExportRunner _exportRunner;
    private readonly IDigestTracker _digestTracker;
    private readonly IAlertDispatcher _alerts;
    private readonly IProcessSupervisor _supervisor;
    private readonly ServiceSpec _serveSpec;
    private readonly ServiceSpec _apiSpec;
    private readonly ServiceSpec _llmSpec;
    private readonly ILogger _logger;

    private DateTime _lastExportTime = DateTime.MinValue;
    private bool _deferredByEditLock;

    public MonitorLoop(
        MonitorOptions options,
        HealthState state,
        HealthStore healthStore,
        HealthPageRenderer pageRenderer,
        ErrorLogPageRenderer errorsPageRenderer,
        ConfigPageRenderer configPageRenderer,
        string stateDir,
        IExportRunner exportRunner,
        IDigestTracker digestTracker,
        IAlertDispatcher alerts,
        IProcessSupervisor supervisor,
        ServiceSpec serveSpec,
        ServiceSpec apiSpec,
        ServiceSpec llmSpec,
        ILogger logger)
    {
        _options = options;
        _state = state;
        _healthStore = healthStore;
        _pageRenderer = pageRenderer;
        _errorsPageRenderer = errorsPageRenderer;
        _configPageRenderer = configPageRenderer;
        _stateDir = stateDir;
        _exportRunner = exportRunner;
        _digestTracker = digestTracker;
        _alerts = alerts;
        _supervisor = supervisor;
        _serveSpec = serveSpec;
        _apiSpec = apiSpec;
        _llmSpec = llmSpec;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            RunOnce();
            _logger.LogInformation("Sleeping for {Seconds} seconds.", _options.CheckIntervalSeconds);
            await Task.Delay(TimeSpan.FromSeconds(_options.CheckIntervalSeconds), ct);
        }
    }

    public void RunOnce()
    {
        var exportDue = _lastExportTime == DateTime.MinValue ||
                        (DateTime.UtcNow - _lastExportTime).TotalMinutes >= _options.ExportIntervalMinutes;

        // skipServe is service-lifecycle scoped (reset each cycle); skipExport is one-shot and
        // must NOT be cleared here or a user Stop would be silently swallowed before the check
        // below (see the export block for its check-then-clear).
        _state.SkipServe = false;

        if (exportDue)
        {
            if (EditLock.IsActive(_options.WikiDir))
            {
                _logger.LogInformation("Deferring export - edit in progress, retry next cycle.");
                exportDue = false;
                _deferredByEditLock = true;
            }
        }

        var writebackSummary = new WritebackDelta(0, new Dictionary<string, int>());
        if (exportDue)
        {
            var effectiveForce = _exportRunner.ShouldForce(_state.RunsSinceForce);
            _logger.LogInformation("Full export (mode={Force}).", effectiveForce ? "force" : "incremental");

            if (_state.SkipExport)
            {
                _logger.LogInformation("Skipped by user request (skipExport flag).");
                _alerts.Dispatch("Export skipped by user request.", AlertKind.UserStop);
                _state.SkipExport = false;
            }
            else
            {
                if (_options.NotifyOnStart)
                    _alerts.Dispatch(
                        effectiveForce ? "Scheduled run starting (forced full rebuild)." : "Scheduled run starting (incremental).",
                        AlertKind.Start);
                writebackSummary = _digestTracker.CountNewWritebacks();
                var _ = ExportProtectedAsync(effectiveForce, writebackSummary).GetAwaiter().GetResult();
            }
            _lastExportTime = DateTime.UtcNow;
        }
        else if (!_deferredByEditLock)
        {
            _logger.LogInformation("Skipping export (next due in {Interval} min).", _options.ExportIntervalMinutes);
        }

        _deferredByEditLock = false;

        _state.PageReadsToday += _digestTracker.CountNewPageReads();
        _state.WritebacksToday += writebackSummary.Total;

        var digestMessage = _digestTracker.MaybeComposeDailyDigest(DateTime.Now);
        if (digestMessage != null)
            _alerts.Dispatch(digestMessage, AlertKind.DailyDigest);

        RenderAndSave();

        Watchdog("serve", _serveSpec, () => _state.ServeConsecutiveFailures,
            v => _state.ServeConsecutiveFailures = v,
            () => _state.LastServeSuccessTime = DateTimeOffset.Now,
            () => _state.LastServeFailureTime = DateTimeOffset.Now,
            AlertKind.ServeRecovery, AlertKind.ServeFailure,
            "mkdocs serve");

        if (_options.ApiPort > 0)
        {
            Watchdog("api", _apiSpec, () => _state.ApiConsecutiveFailures,
                v => _state.ApiConsecutiveFailures = v,
                () => _state.LastApiSuccessTime = DateTimeOffset.Now,
                () => _state.LastApiFailureTime = DateTimeOffset.Now,
                AlertKind.ApiRecovery, AlertKind.ApiFailure,
                "write-back API server");
        }
        else
        {
            _logger.LogInformation("API server not configured (ApiPort not set).");
        }

        if (_options.AiMode == "local" &&
            File.Exists(_options.LlamaExePath) && File.Exists(_options.LlamaModelPath))
        {
            Watchdog("llm", _llmSpec, () => _state.LlmConsecutiveFailures,
                v => _state.LlmConsecutiveFailures = v,
                () => _state.LastLlmSuccessTime = DateTimeOffset.Now,
                () => _state.LastLlmFailureTime = DateTimeOffset.Now,
                AlertKind.LlmRecovery, AlertKind.LlmFailure,
                "LLM server");
        }
        else
        {
            _logger.LogInformation("LLM not configured (AiMode={AiMode}).", _options.AiMode);
        }
    }

    private async Task<bool> ExportProtectedAsync(bool effectiveForce, WritebackDelta writebacks)
    {
        try
        {
            return await _exportRunner.RunExportAsync(effectiveForce, writebacks, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export crashed; recording as failure.");
            return false;
        }
    }

    private void Watchdog(
        string name, ServiceSpec spec,
        Func<int> getFailures, Action<int> setFailures,
        Action onSuccess, Action onFailure,
        AlertKind recoveryKind, AlertKind failureKind,
        string displayName)
    {
        if (_state.SkipServe && name == "serve")
        {
            _logger.LogInformation("Serve restart blocked by user (skipServe flag).");
            return;
        }

        if (_supervisor.IsAlive(spec))
        {
            _logger.LogInformation("{Name} already running.", displayName);
            return;
        }

        _logger.LogInformation("{Name} not running; attempting to (re)start.", displayName);
        var up = _supervisor.EnsureRunningAsync(spec, _options.MaxRetries, _options.RetryDelaySeconds, CancellationToken.None).GetAwaiter().GetResult();
        var attempts = _supervisor.AttemptsUsed;

        if (up)
        {
            var wasFailing = getFailures() > 0;
            setFailures(0);
            onSuccess();
            _logger.LogInformation("{Name} started on attempt {Attempt}.", displayName, attempts);
            if (wasFailing)
                _alerts.Dispatch($"{displayName} restarted successfully after {attempts} attempt(s).", recoveryKind);
        }
        else
        {
            setFailures(getFailures() + 1);
            onFailure();
            _logger.LogWarning("Gave up starting {Name} after {MaxRetries} attempt(s).", displayName, _options.MaxRetries);
            _alerts.Dispatch($"{displayName} failed to start after {_options.MaxRetries} attempt(s).", failureKind);
        }

        RenderAndSave();
    }

    private void RenderAndSave()
    {
        try
        {
            _pageRenderer.Render(_state);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to render health page: {Error}", ex.Message);
        }
        try
        {
            _errorsPageRenderer.Render(DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to render error log page: {Error}", ex.Message);
        }
        try
        {
            _configPageRenderer.Render(_options, DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to render config page: {Error}", ex.Message);
        }
        _healthStore.Save(Path.Combine(_stateDir, "health.json"), _state);
    }
}
