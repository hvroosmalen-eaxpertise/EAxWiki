# Error Log + Config Pages Implementation Plan (issue #87)

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add two monitor-generated status pages to the wiki — `wiki/status/errors.md` (filtered error log) and `wiki/status/config.md` (resolved configuration + scheduled-task snapshot) — so failures and settings are visible without opening the machine's log path.

**Architecture:** The monitor renders both pages every loop cycle in `MonitorLoop.RenderAndSave` alongside the existing `health.md`. The monitor log format gains a severity token (`[INF|WRN|ERR]`) so `ErrorLogPageRenderer` can filter. `ConfigPageRenderer` displays resolved `MonitorOptions` (secrets masked) plus a schedule snapshot obtained by a new `IScheduledTaskSnapshot` service (live `Get-ScheduledTask` via pwsh, cached 5 min). Nav entries in `status/.pages` are existence-gated by `InfrastructureWriter`, and `health-template.md` links to both pages.

**Tech Stack:** .NET 10, xUnit + Moq (existing `EAxWiki.Tests` project), System.Text.Json, Microsoft.Extensions.Logging, pwsh (already located via `MonitorPaths.FindPowerShell()`).

## Global Constraints

- Every `dotnet` build/test command runs with `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';` inline first (required for the Interop.EA reference). Example: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj`.
- New/changed files: LF line endings + UTF-8 no BOM (matches the repo; git warns about CRLF on Windows — cosmetic, ignore).
- Commit messages: exact lowercase conventional commits with `(issue #87)` suffix (e.g. `feat(monitor): add severity to monitor log lines (issue #87)`). Stage only the files named in the step.
- Never stage: `model/`, `wiki/`, `.eaxwiki-monitor/*/` (per-instance state), `.eaxwiki`, `.venv/`, `.pip_cache/`, `.mkdocs_temp/`, `bin/`, `obj/`, `.validation-report.json`. **Exception:** `.eaxwiki-monitor/errors-template.md` and `.eaxwiki-monitor/health-template.md` are tracked repo files and ARE staged.
- The full .NET suite is `dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj` (with EAPath). Known flakes that pass on rerun: `PropertyBasedTests.EscapeCell_LengthAtLeastInputLength`, `Export_StatusEditorScript`.
- Push + the live-monitor smoke (running `EAxWiki.Monitor.exe`, verifying pages serve via mkdocs) + committing the generated `wiki/` happen via the export-cycle skill with a human partner — not inside these tasks.
- `ScheduledTaskSnapshot` queries the scheduled task by action `-Execute` == `Environment.ProcessPath`. This matches `register-scheduled-task.ps1` (registers the `.exe` apphost via `New-ScheduledTaskAction -Execute $monitorExe`). A hand-registered `dotnet EAxWiki.Monitor.dll` action won't match and renders "Schedule info unavailable" — accepted, no fallback.

---

### Task 1: Add severity token to monitor log lines

**Files:**
- Modify: `src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs:41-50`
- Test: `src/EAxWiki.Tests/MonitorFileLoggerTests.cs`

**Interfaces:**
- Produces: monitor log lines shaped `yyyy-MM-dd HH:mm:ss [INF|WRN|ERR] [phase] message`. Consumed by `ErrorLogPageRenderer` (Task 2) and the scheduled-task-diagnostics skill (human-facing, unparsed elsewhere).

- [ ] **Step 1: Update the existing format test and add a severity test**

Replace the second assertion in `LogsToDateStampedFile` (MonitorFileLoggerTests.cs:34) with:

```csharp
        Assert.Contains("[INF] [ExportRunner] hello 42", content);
        Assert.Matches("^\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} \\[INF\\] \\[ExportRunner\\] hello 42", content);
```

Add this test below `LogsToDateStampedFile`:

```csharp
    [Fact]
    public void LogsSeverityTokenPerLevel()
    {
        using var provider = new MonitorFileLoggerProvider(_dir);
        var logger = provider.CreateLogger("EAxWiki.Monitor.ExportRunner");
        logger.LogInformation("plain message");
        logger.LogWarning("careful now");
        logger.LogError(new InvalidOperationException("boom"), "failed");
        provider.Dispose();

        var content = File.ReadAllText(Directory.GetFiles(Path.Combine(_dir, "logs"), "monitor-*.log").Single());
        Assert.Contains("[INF] [ExportRunner] plain message", content);
        Assert.Contains("[WRN] [ExportRunner] careful now", content);
        Assert.Contains("[ERR] [ExportRunner] failed boom", content);
    }
```

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~MonitorFileLoggerTests"`
Expected: FAIL — `[INF] [ExportRunner]` not found in the current `[ExportRunner]`-only format.

- [ ] **Step 3: Implement the severity token**

In `MonitorFileLoggerProvider.cs`, change the `Log` method (lines 43-50) to:

```csharp
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception != null) message += $" {exception.Message}";
            var severity = logLevel switch
            {
                LogLevel.Warning => "WRN",
                LogLevel.Error or LogLevel.Critical => "ERR",
                _ => "INF",
            };
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{severity}] [{_name}] {message}";
            var stamp = DateTime.Now.ToString("yyyy-MM-dd");
            File.AppendAllText(Path.Combine(_parent._logDir, $"monitor-{stamp}.log"), line + Environment.NewLine);
        }
```

- [ ] **Step 4: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~MonitorFileLoggerTests"`
Expected: PASS (both tests).

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs src/EAxWiki.Tests/MonitorFileLoggerTests.cs
git commit -m "feat(monitor): add severity token to monitor log lines (issue #87)"
```

---

### Task 2: `ErrorLogPageRenderer` + `errors-template.md`

**Files:**
- Create: `src/EAxWiki.Monitor/ErrorLogPageRenderer.cs`
- Create: `.eaxwiki-monitor/errors-template.md`
- Test: `src/EAxWiki.Tests/ErrorLogPageRendererTests.cs`

**Interfaces:**
- Consumes: log lines shaped `yyyy-MM-dd HH:mm:ss [INF|WRN|ERR] [phase] message` (Task 1). Caller passes `logsDir = Path.Combine(stateDir, "logs")` and the instance's own secrets (`MonitorOptions.WebhookUrl`, `TeamsWebhookUrl`, `TelegramBotToken`).
- Produces: `public class ErrorLogPageRenderer` with ctor `(string templatePath, string wikiDir, string logsDir, string[] secrets)` and `public void Render(DateTime now)`. Renders `{wikiDir}/status/errors.md` from `errors-template.md`, replacing `@@GENERATED_AT@@`, `@@ERRORS@@`, `@@RECENT@@`. Consumed by `MonitorApp.Build` + `MonitorLoop.RenderAndSave` (Task 5).

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/ErrorLogPageRendererTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ErrorLogPageRendererTests : IDisposable
{
    private readonly string _dir;

    public ErrorLogPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_errors_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private const string Template = """
        # Error Log
        Last checked: @@GENERATED_AT@@
        ## Issues (last 7 days)
        @@ERRORS@@
        ## Recent activity (last 20 lines, all levels)
        @@RECENT@@
        """;

    private ErrorLogPageRenderer Create(string[] secrets, string logDate, string[] lines)
    {
        var logsDir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(logsDir);
        File.WriteAllLines(Path.Combine(logsDir, $"monitor-{logDate}.log"), lines);
        File.WriteAllText(Path.Combine(_dir, "errors-template.md"), Template);
        return new ErrorLogPageRenderer(Path.Combine(_dir, "errors-template.md"), Path.Combine(_dir, "wiki"), logsDir, secrets);
    }

    [Fact]
    public void Render_KeepsOnlyWarnErrorWithin7Days_NewestFirst()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var sixDaysAgo = now.AddDays(-6).ToString("yyyy-MM-dd");
        var eightDaysAgo = now.AddDays(-8).ToString("yyyy-MM-dd");
        var renderer = Create([], eightDaysAgo, [$"{eightDaysAgo} 09:00:00 [ERR] [MonitorLoop] old failure"]);
        File.WriteAllLines(Path.Combine(_dir, "logs", $"monitor-{sixDaysAgo}.log"),
            [$"{sixDaysAgo} 10:00:00 [ERR] [MonitorLoop] six days ago failure"]);
        File.WriteAllLines(Path.Combine(_dir, "logs", $"monitor-{today}.log"),
            [$"{today} 11:00:00 [INF] [MonitorLoop] ok", $"{today} 11:01:00 [WRN] [Supervisor] retrying serve"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("[WRN] [Supervisor] retrying serve", output);
        Assert.Contains("[ERR] [MonitorLoop] six days ago failure", output);
        Assert.DoesNotContain("[INF] [MonitorLoop] ok", output);
        Assert.DoesNotContain("old failure", output);
        Assert.True(output.IndexOf("[WRN]", StringComparison.Ordinal) < output.IndexOf("[ERR] [MonitorLoop] six", StringComparison.Ordinal),
            "newest entries come first");
    }

    [Fact]
    public void Render_NoIssues_ShowsHappyState()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var renderer = Create([], today, [$"{today} 11:00:00 [INF] [MonitorLoop] ok"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("No issues found in the last 7 days.", output);
    }

    [Fact]
    public void Render_CapsAt100Entries()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var lines = Enumerable.Range(0, 120)
            .Select(i => $"{today} 10:00:{i:00} [ERR] [MonitorLoop] failure {i}").ToArray();
        var renderer = Create([], today, lines);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        var issuesSection = output.Substring(output.IndexOf("## Issues", StringComparison.Ordinal),
            output.IndexOf("## Recent activity", StringComparison.Ordinal) - output.IndexOf("## Issues", StringComparison.Ordinal));
        Assert.Equal(100, System.Text.RegularExpressions.Regex.Matches(issuesSection, "failure \\d+").Count);
        Assert.Contains("failure 20", issuesSection);   // oldest kept (newest 100 of 120)
        Assert.Contains("failure 119", issuesSection);  // newest
        Assert.DoesNotContain("failure 19", issuesSection);
    }

    [Fact]
    public void Render_RecentBlock_ShowsLast20AllLevels()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        var lines = Enumerable.Range(0, 25)
            .Select(i => $"{today} 10:00:{i:00} [INF] [MonitorLoop] line {i}").ToArray();
        var renderer = Create([], today, lines);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("line 5", output);   // oldest of the last 20 (indices 5..24)
        Assert.Contains("line 24", output);  // newest
        Assert.DoesNotContain("line 4", output);
    }

    [Fact]
    public void Render_RedactsSecretsAndConnectionStringPassword()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        var today = now.ToString("yyyy-MM-dd");
        const string webhook = "https://hooks.slack.com/services/T00000000/B00000000/secretToken";
        var renderer = Create([webhook], today,
            [$"{today} 10:00:00 [ERR] [Alert] alert failed posting to {webhook}",
             $"{today} 10:01:00 [ERR] [ExportRunner] Data Source=server;Initial Catalog=ea;Password=hunter2"]);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.DoesNotContain(webhook, output);
        Assert.Contains("posting to ***", output);
        Assert.DoesNotContain("hunter2", output);
        Assert.Contains("Password=***", output);
    }

    [Fact]
    public void Render_MissingLogsDir_ShowsEmptyStates()
    {
        var now = new DateTime(2026, 8, 18, 12, 0, 0);
        File.WriteAllText(Path.Combine(_dir, "errors-template.md"), Template);
        var renderer = new ErrorLogPageRenderer(Path.Combine(_dir, "errors-template.md"),
            Path.Combine(_dir, "wiki"), Path.Combine(_dir, "no-logs"), []);

        renderer.Render(now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "errors.md"));
        Assert.Contains("No issues found in the last 7 days.", output);
        Assert.Contains("(no log lines yet)", output);
        Assert.Contains("2026-08-18 12:00:00", output);
    }
}
```

Note on `Render_CapsAt100Entries`: reading order is newest-file-first then reverse line order, so the newest 100 of 120 (`failure 119`..`failure 20`) are kept and `failure 0`..`failure 19` dropped. The count assertion is scoped to the Issues section only (the Recent section repeats 20 of the same lines).

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ErrorLogPageRendererTests"`
Expected: FAIL — type `ErrorLogPageRenderer` not found (compile error).

- [ ] **Step 3: Create the committed template**

Create `.eaxwiki-monitor/errors-template.md`:

```markdown
# Error Log

*Generated by EAxWiki.Monitor - recent Warn/Error operation-log entries (export, serve, API, LLM supervision). Last checked @@GENERATED_AT@@.*

## Issues (last 7 days)

@@ERRORS@@

## Recent activity (last 20 lines, all levels)

@@RECENT@@
```

- [ ] **Step 4: Implement the renderer**

Create `src/EAxWiki.Monitor/ErrorLogPageRenderer.cs`:

```csharp
using System.Globalization;
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

/// <summary>
/// Renders errors-template.md → {wikiDir}/status/errors.md. Reads the instance's own
/// {logsDir}/monitor-*.log files (Task 1 severity format), keeps [WRN]/[ERR] lines within a
/// 7-day window (newest first, capped at 100), redacts the instance's secrets and
/// connection-string passwords, and fills @@GENERATED_AT@@ / @@ERRORS@@ / @@RECENT@@.
/// </summary>
public class ErrorLogPageRenderer
{
    private static readonly Regex SeverityRegex = new(@"\[(INF|WRN|ERR)\]", RegexOptions.Compiled);
    private static readonly Regex ConnectionStringRegex = new(@"(?i)(Password|Pwd)\s*=[^;]*", RegexOptions.Compiled);

    private readonly string _templatePath;
    private readonly string _outputPath;
    private readonly string _logsDir;
    private readonly string[] _secrets;

    public ErrorLogPageRenderer(string templatePath, string wikiDir, string logsDir, string[] secrets)
    {
        _templatePath = templatePath;
        _outputPath = Path.Combine(wikiDir, "status", "errors.md");
        _logsDir = logsDir;
        _secrets = secrets ?? [];
    }

    public void Render(DateTime now)
    {
        var lines = ReadLogLines(now);

        var errors = lines
            .Where(l => l.Severity is "WRN" or "ERR")
            .Take(100)
            .Select(l => Redact(l.Text))
            .ToList();

        var recent = lines.Take(20).Select(l => Redact(l.Text)).ToList();

        var template = File.ReadAllText(_templatePath);
        template = template.Replace("@@GENERATED_AT@@", now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        template = template.Replace("@@ERRORS@@", errors.Count == 0
            ? "No issues found in the last 7 days."
            : string.Join(Environment.NewLine, errors.Select(e => "- `" + e + "`")));
        template = template.Replace("@@RECENT@@", recent.Count == 0
            ? "(no log lines yet)"
            : string.Join(Environment.NewLine, recent.Select(e => "- `" + e + "`")));

        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);
        File.WriteAllText(_outputPath, template);
    }

    private List<(string Severity, string Text)> ReadLogLines(DateTime now)
    {
        var result = new List<(string Severity, string Text)>();
        if (!Directory.Exists(_logsDir)) return result;

        var files = Directory.GetFiles(_logsDir, "monitor-*.log")
            .Where(f =>
            {
                var name = Path.GetFileName(f);
                if (name.Length < 19) return false;
                return DateTime.TryParseExact(name.Substring(8, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var d)
                    && d.Date >= now.Date.AddDays(-6) && d.Date <= now.Date;
            })
            .OrderByDescending(File.GetLastWriteTime)
            .ToArray();

        foreach (var file in files)
        {
            foreach (var line in File.ReadLines(file).Reverse())
            {
                var m = SeverityRegex.Match(line);
                var severity = m.Success ? m.Groups[1].Value : "INF"; // pre-Task-1 lines count as INF
                result.Add((severity, line));
            }
        }
        return result;
    }

    private string Redact(string line)
    {
        var result = line;
        foreach (var secret in _secrets)
        {
            if (!string.IsNullOrEmpty(secret) && secret.Length >= 3)
                result = result.Replace(secret, "***");
        }
        return ConnectionStringRegex.Replace(result, "$1=***");
    }
}
```

- [ ] **Step 5: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ErrorLogPageRendererTests"`
Expected: PASS (6 tests).

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/ErrorLogPageRenderer.cs .eaxwiki-monitor/errors-template.md src/EAxWiki.Tests/ErrorLogPageRendererTests.cs
git commit -m "feat(monitor): add error log status page renderer (issue #87)"
```

---

### Task 3: `ScheduledTaskSnapshot` service (interface, parser, pwsh query, cache)

**Files:**
- Create: `src/EAxWiki.Monitor/ScheduledTaskSnapshot.cs`
- Create: `src/EAxWiki.Monitor/ScheduledTaskJsonParser.cs`
- Test: `src/EAxWiki.Tests/ScheduledTaskSnapshotTests.cs`

**Interfaces:**
- Consumes: `MonitorPaths.FindPowerShell()`, `Environment.ProcessPath`.
- Produces:
  - `public record ScheduledTaskInfo(string TaskName, string State, bool WakeToRun, string ExecutionTimeLimit, string MultipleInstances, IReadOnlyList<string> Triggers);`
  - `public interface IScheduledTaskSnapshot { ScheduledTaskInfo? Get(); }`
  - `public sealed class ScheduledTaskSnapshot : IScheduledTaskSnapshot` — ctor `(Func<string?>? queryJson = null, TimeSpan? cacheTtl = null)`; `Get()` returns a cached `ScheduledTaskInfo?` (TTL default 5 min), `null` when the task isn't found or the query fails.
  - `public static class ScheduledTaskJsonParser` — `public static ScheduledTaskInfo? Parse(string? json)` and `public static string? FormatIsoDuration(string? iso)`.
  - Consumed by `ConfigPageRenderer` (Task 4) and wired in `MonitorApp.Build` (Task 5).

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/ScheduledTaskSnapshotTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ScheduledTaskSnapshotTests
{
    private static readonly string DailyJson = """
        {
          "TaskName": "EAxWiki-Monitor",
          "State": "Ready",
          "WakeToRun": false,
          "ExecutionTimeLimit": "PT72H",
          "MultipleInstances": 1,
          "Triggers": [
            { "Kind": "MSFT_TaskDailyTrigger", "StartBoundary": "2026-08-01T00:00:00",
              "RepetitionInterval": "PT4H", "RepetitionDuration": "PT8H", "DaysInterval": 1, "DaysOfWeek": 0 }
          ]
        }
        """;

    private static readonly string WeeklyJson = """
        {
          "TaskName": "EAxWiki-Monitor",
          "State": "Ready",
          "WakeToRun": true,
          "ExecutionTimeLimit": "PT72H",
          "MultipleInstances": 1,
          "Triggers": [
            { "Kind": "MSFT_TaskWeeklyTrigger", "StartBoundary": "2026-08-03T08:00:00",
              "RepetitionInterval": "PT10M", "RepetitionDuration": "PT10H", "DaysInterval": 1, "DaysOfWeek": 62 }
          ]
        }
        """;

    [Fact]
    public void Parse_DailyTrigger_FormatsDescription()
    {
        var info = ScheduledTaskJsonParser.Parse(DailyJson);
        Assert.NotNull(info);
        Assert.Equal("EAxWiki-Monitor", info.TaskName);
        Assert.Equal("Ready", info.State);
        Assert.Equal("IgnoreNew", info.MultipleInstances);
        Assert.Equal("PT72H", info.ExecutionTimeLimit);
        Assert.Single(info.Triggers);
        Assert.Contains("Daily at 00:00", info.Triggers[0]);
        Assert.Contains("every 4 h (for 8 h)", info.Triggers[0]);
    }

    [Fact]
    public void Parse_WeeklyTrigger_ListsWeekdaysAndInterval()
    {
        var info = ScheduledTaskJsonParser.Parse(WeeklyJson);
        Assert.NotNull(info);
        Assert.True(info.WakeToRun);
        Assert.Single(info.Triggers);
        Assert.Contains("Mon, Tue, Wed, Thu, Fri at 08:00", info.Triggers[0]);
        Assert.Contains("every 10 min (for 10 h)", info.Triggers[0]);
    }

    [Fact]
    public void Parse_NullJson_ReturnsNull()
    {
        Assert.Null(ScheduledTaskJsonParser.Parse(null));
        Assert.Null(ScheduledTaskJsonParser.Parse("null"));
        Assert.Null(ScheduledTaskJsonParser.Parse("   "));
    }

    [Fact]
    public void Parse_NotAnObject_ReturnsNull()
    {
        Assert.Null(ScheduledTaskJsonParser.Parse("[]"));
        Assert.Null(ScheduledTaskJsonParser.Parse("{}"));
    }

    [Fact]
    public void Parse_UnknownTriggerKind_FallsBack()
    {
        var json = """
            { "TaskName": "EAxWiki-Monitor", "State": "Ready", "WakeToRun": false,
              "ExecutionTimeLimit": "PT72H", "MultipleInstances": 0,
              "Triggers": [ { "Kind": "MSFT_TaskLogonTrigger", "StartBoundary": null,
                              "RepetitionInterval": null, "RepetitionDuration": null,
                              "DaysInterval": 0, "DaysOfWeek": 0 } ] }
            """;
        var info = ScheduledTaskJsonParser.Parse(json);
        Assert.NotNull(info);
        Assert.Equal("Parallel", info.MultipleInstances);
        Assert.Contains("Logon at , single run", info.Triggers[0]);
    }

    [Fact]
    public void FormatIsoDuration_Variants()
    {
        Assert.Equal("4 h", ScheduledTaskJsonParser.FormatIsoDuration("PT4H"));
        Assert.Equal("10 min", ScheduledTaskJsonParser.FormatIsoDuration("PT10M"));
        Assert.Equal("10 h 30 min", ScheduledTaskJsonParser.FormatIsoDuration("PT10H30M"));
        Assert.Equal("1 d", ScheduledTaskJsonParser.FormatIsoDuration("P1D"));
        Assert.Null(ScheduledTaskJsonParser.FormatIsoDuration(null));
        Assert.Null(ScheduledTaskJsonParser.FormatIsoDuration("PT0S"));
    }

    [Fact]
    public void Get_CachesWithinTtl()
    {
        var calls = 0;
        string? Query() { calls++; return DailyJson; }
        var snapshot = new ScheduledTaskSnapshot(Query);

        snapshot.Get();
        snapshot.Get();

        Assert.Equal(1, calls);
    }

    [Fact]
    public void Get_ReQueriesAfterTtl()
    {
        var calls = 0;
        string? Query() { calls++; return DailyJson; }
        var snapshot = new ScheduledTaskSnapshot(Query, TimeSpan.FromMilliseconds(10));

        snapshot.Get();
        System.Threading.Thread.Sleep(30);
        snapshot.Get();

        Assert.Equal(2, calls);
    }

    [Fact]
    public void Get_QueryReturnsNull_ReturnsNull()
    {
        var snapshot = new ScheduledTaskSnapshot(() => null);
        Assert.Null(snapshot.Get());
    }
}
```

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ScheduledTaskSnapshotTests"`
Expected: FAIL — `ScheduledTaskSnapshot`/`ScheduledTaskJsonParser` not found.

- [ ] **Step 3: Implement the parser**

Create `src/EAxWiki.Monitor/ScheduledTaskJsonParser.cs`:

```csharp
using System.Text.Json;

namespace EAxWiki.Monitor;

public record ScheduledTaskInfo(string TaskName, string State, bool WakeToRun,
    string ExecutionTimeLimit, string MultipleInstances, IReadOnlyList<string> Triggers);

/// <summary>
/// Parses the JSON emitted by ScheduledTaskSnapshot's pwsh query into ScheduledTaskInfo.
/// Pure and unit-testable; the pwsh side lives only in ScheduledTaskSnapshot.
/// </summary>
public static class ScheduledTaskJsonParser
{
    public static ScheduledTaskInfo? Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        if (json.Trim() == "null") return null;

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.Array)
            root = root.EnumerateArray().FirstOrDefault();
        if (root.ValueKind != JsonValueKind.Object) return null;
        if (!root.TryGetProperty("TaskName", out var taskNameProp)) return null;
        if (taskNameProp.ValueKind != JsonValueKind.String) return null;

        var triggers = new List<string>();
        if (root.TryGetProperty("Triggers", out var trig) && trig.ValueKind == JsonValueKind.Array)
        {
            foreach (var t in trig.EnumerateArray())
                triggers.Add(FormatTrigger(t));
        }

        return new ScheduledTaskInfo(
            taskNameProp.GetString() ?? "",
            AsString(root, "State") ?? "",
            root.TryGetProperty("WakeToRun", out var w) && w.ValueKind == JsonValueKind.True,
            AsString(root, "ExecutionTimeLimit") ?? "",
            AsString(root, "MultipleInstances") ?? AsInt(root, "MultipleInstances") switch
            {
                1 => "IgnoreNew",
                2 => "Queue",
                _ => "Parallel",
            },
            triggers);
    }

    private static string FormatTrigger(JsonElement t)
    {
        var kind = AsString(t, "Kind") ?? "";
        var start = AsString(t, "StartBoundary") ?? "";
        var interval = AsString(t, "RepetitionInterval");
        var duration = AsString(t, "RepetitionDuration");
        var daysInterval = AsInt(t, "DaysInterval");
        var daysOfWeek = AsInt(t, "DaysOfWeek");

        var when = interval == null
            ? "single run"
            : $"every {FormatIsoDuration(interval)}" + (duration == null ? "" : $" (for {FormatIsoDuration(duration)})");

        var clock = start.Length >= 16 ? start.Substring(11, 5) : "";
        return kind switch
        {
            "MSFT_TaskDailyTrigger" => $"Daily at {clock}" + (daysInterval > 1 ? $" (every {daysInterval} days)" : "") + $", {when}",
            "MSFT_TaskWeeklyTrigger" => $"{WeekdayNames(daysOfWeek)} at {clock}" + (daysInterval > 1 ? $" (every {daysInterval} weeks)" : "") + $", {when}",
            _ => $"{kind.Replace("MSFT_Task", "").Replace("Trigger", "")} at {clock}, {when}",
        };
    }

    private static string WeekdayNames(int mask)
    {
        var names = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        var hits = new List<string>();
        for (var i = 0; i < 7; i++)
            if ((mask & (1 << i)) != 0) hits.Add(names[i]);
        return hits.Count == 0 ? "(no weekdays)" : string.Join(", ", hits);
    }

    public static string? FormatIsoDuration(string? iso)
    {
        if (string.IsNullOrEmpty(iso)) return null;
        var text = iso.Trim();
        if (text == "PT0S") return null;
        if (text.StartsWith("P", StringComparison.Ordinal))
        {
            var daysPart = text.Substring(1).Split('T', 2)[0];
            if (daysPart.Length > 0 && int.TryParse(daysPart.TrimEnd('D'), out var days) && days > 0)
            {
                var timePart = text.Contains('T') ? text.Split('T', 2)[1] : "";
                var h = 0; var m = 0;
                ParseTimePart(timePart, ref h, ref m);
                return h > 0 ? $"{days} d {h} h" + (m > 0 ? $" {m} min" : "") : $"{days} d";
            }
        }
        if (text.StartsWith("PT", StringComparison.Ordinal))
        {
            var h = 0; var m = 0;
            ParseTimePart(text.Substring(2), ref h, ref m);
            if (h > 0 && m > 0) return $"{h} h {m} min";
            if (h > 0) return $"{h} h";
            if (m > 0) return $"{m} min";
        }
        return iso;
    }

    private static void ParseTimePart(string part, ref int h, ref int m)
    {
        var hIndex = part.IndexOf('H');
        var mIndex = part.IndexOf('M');
        if (hIndex >= 0 && int.TryParse(part.Substring(0, hIndex), out var hv))
            h = hv;
        if (mIndex >= 0)
        {
            var start = Math.Max(0, hIndex + 1);
            var mText = part.Substring(start, mIndex - start);
            if (mText.Length > 0 && int.TryParse(mText, out var mv))
                m = mv;
        }
    }

    private static string? AsString(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;

    private static int AsInt(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.Number ? p.GetInt32() : 0;
}
```

- [ ] **Step 4: Implement the snapshot (pwsh query + cache)**

Create `src/EAxWiki.Monitor/ScheduledTaskSnapshot.cs`:

```csharp
using System.Diagnostics;
using System.Text;

namespace EAxWiki.Monitor;

public interface IScheduledTaskSnapshot
{
    ScheduledTaskInfo? Get();
}

/// <summary>
/// Queries the registered Task Scheduler task whose action -Execute matches the monitor's own
/// exe (Environment.ProcessPath), via pwsh Get-ScheduledTask serialized to JSON. Results are
/// cached (default 5 min) so the 30 s monitor loop doesn't shell out every cycle. Any query or
/// parse failure surfaces as null ("schedule unavailable"), never an exception.
/// </summary>
public sealed class ScheduledTaskSnapshot : IScheduledTaskSnapshot
{
    private readonly Func<string?> _queryJson;
    private readonly TimeSpan _cacheTtl;
    private ScheduledTaskInfo? _cached;
    private DateTime _cachedAt = DateTime.MinValue;

    public ScheduledTaskSnapshot(Func<string?>? queryJson = null, TimeSpan? cacheTtl = null)
    {
        _queryJson = queryJson ?? RunPwshQuery;
        _cacheTtl = cacheTtl ?? TimeSpan.FromMinutes(5);
    }

    public ScheduledTaskInfo? Get()
    {
        if (_cached != null && DateTime.Now - _cachedAt < _cacheTtl)
            return _cached;
        _cached = ScheduledTaskJsonParser.Parse(_queryJson());
        _cachedAt = DateTime.Now;
        return _cached;
    }

    private static string? RunPwshQuery()
    {
        var exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath)) return null;

        var script = $"""
            $exe = '{exePath.Replace("'", "''")}'
            $t = Get-ScheduledTask | Where-Object {{ $_.Actions | Where-Object {{ $_.Execute -eq $exe }} }} | Select-Object -First 1
            if (-not $t) {{ Write-Output 'null'; exit }}
            $triggers = foreach ($tr in $t.Triggers) {{
                [pscustomobject]@{{
                    Kind = $tr.CimClass.CimClassName
                    StartBoundary = $tr.StartBoundary
                    RepetitionInterval = $tr.Repetition.Interval
                    RepetitionDuration = $tr.Repetition.Duration
                    DaysInterval = $tr.DaysInterval
                    DaysOfWeek = $tr.DaysOfWeek
                }}
            }}
            [pscustomobject]@{{
                TaskName = $t.TaskName
                State = $t.State
                WakeToRun = $t.Settings.WakeToRun
                ExecutionTimeLimit = $t.Settings.ExecutionTimeLimit
                MultipleInstances = $t.Settings.MultipleInstances
                Triggers = @($triggers)
            }} | ConvertTo-Json -Depth 6
            """;

        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(script));
        var psi = new ProcessStartInfo
        {
            FileName = MonitorPaths.FindPowerShell(),
            Arguments = $"-NoProfile -EncodedCommand {encoded}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = Process.Start(psi);
        if (process == null) return null;
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit(30_000);
        return string.IsNullOrWhiteSpace(output) ? null : output.Trim();
    }
}
```

- [ ] **Step 5: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ScheduledTaskSnapshotTests"`
Expected: PASS (9 tests).

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/ScheduledTaskSnapshot.cs src/EAxWiki.Monitor/ScheduledTaskJsonParser.cs src/EAxWiki.Tests/ScheduledTaskSnapshotTests.cs
git commit -m "feat(monitor): add scheduled-task snapshot service (issue #87)"
```

---

### Task 4: `ConfigPageRenderer`

**Files:**
- Create: `src/EAxWiki.Monitor/ConfigPageRenderer.cs`
- Test: `src/EAxWiki.Tests/ConfigPageRendererTests.cs`

**Interfaces:**
- Consumes: `MonitorOptions` (Task 3 records + options shape unchanged), `IScheduledTaskSnapshot` (Task 3).
- Produces: `public class ConfigPageRenderer` with ctor `(string wikiDir, IScheduledTaskSnapshot schedule)` and `public void Render(MonitorOptions options, DateTime now)`. Writes `{wikiDir}/status/config.md` (fully code-generated markdown). Consumed by `MonitorApp.Build` + `MonitorLoop.RenderAndSave` (Task 5).

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/ConfigPageRendererTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ConfigPageRendererTests : IDisposable
{
    private readonly string _dir;

    public ConfigPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_config_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private sealed class StubSnapshot : IScheduledTaskSnapshot
    {
        public ScheduledTaskInfo? Value { get; set; }
        public ScheduledTaskInfo? Get() => Value;
    }

    private static MonitorOptions Options() => new()
    {
        RepoPath = @"C:\models\repo.qea",
        WikiDir = @"C:\wiki",
        WikiPort = 8000,
        ApiPort = 8001,
        LlmPort = 8080,
        ExportIntervalMinutes = 30,
        CheckIntervalSeconds = 30,
        MaxRetries = 3,
        RetryDelaySeconds = 30,
        MinElementFraction = 0.5,
        Force = false,
        ForceEveryNRuns = 4,
        Brand = "ACME",
        AiMode = "openai",
        AiEndpoint = "https://api.openai.com/v1",
        AiModel = "gpt-4o-mini",
        WebhookUrl = "https://hooks.slack.com/services/secret",
    };

    [Fact]
    public void Render_ShowsOperationalValuesAndSchedule()
    {
        var stub = new StubSnapshot
        {
            Value = new ScheduledTaskInfo("EAxWiki-Monitor", "Ready", false, "PT72H", "IgnoreNew",
                ["Daily at 00:00, every 4 h (for 8 h)"]),
        };
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), stub);

        renderer.Render(Options(), new DateTime(2026, 8, 18, 12, 0, 0));

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("| Wiki port | 8000 |", output);
        Assert.Contains("| API port | 8001 |", output);
        Assert.Contains("| Export interval | 30 min |", output);
        Assert.Contains("| Max retries | 3 |", output);
        Assert.Contains("| Force every N runs | every 4 runs |", output);
        Assert.Contains("| AI model | gpt-4o-mini |", output);
        Assert.Contains("| Task name | `EAxWiki-Monitor` |", output);
        Assert.Contains("Daily at 00:00, every 4 h (for 8 h)", output);
        Assert.Contains("2026-08-18 12:00:00", output);
    }

    [Fact]
    public void Render_RedactsRepoPassword()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());
        var options = Options() with { RepoPath = "Data Source=server;Initial Catalog=ea;Password=hunter2" };

        renderer.Render(options, DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("Password=***", output);
        Assert.DoesNotContain("hunter2", output);
    }

    [Fact]
    public void Render_AlertDestinations_ConfiguredOrNot_NoSecrets()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());

        renderer.Render(Options(), DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("| Slack | configured |", output);
        Assert.Contains("| Teams | not configured |", output);
        Assert.Contains("| Telegram | not configured |", output);
        Assert.DoesNotContain("hooks.slack.com", output);
    }

    [Fact]
    public void Render_ScheduleUnavailable_ShowsMessage()
    {
        var renderer = new ConfigPageRenderer(Path.Combine(_dir, "wiki"), new StubSnapshot());

        renderer.Render(Options(), DateTime.Now);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "config.md"));
        Assert.Contains("Schedule info unavailable", output);
    }
}
```

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ConfigPageRendererTests"`
Expected: FAIL — type `ConfigPageRenderer` not found.

- [ ] **Step 3: Implement the renderer**

Create `src/EAxWiki.Monitor/ConfigPageRenderer.cs`:

```csharp
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

/// <summary>
/// Renders {wikiDir}/status/config.md (fully code-generated — no template): resolved
/// MonitorOptions with secrets masked, alert destinations as configured/not-configured, and the
/// cached scheduled-task snapshot. Read-only; never contains webhook URLs, tokens, or keys.
/// </summary>
public class ConfigPageRenderer
{
    private static readonly Regex ConnectionStringRegex = new(@"(?i)(Password|Pwd)\s*=[^;]*", RegexOptions.Compiled);

    private readonly string _outputPath;
    private readonly IScheduledTaskSnapshot _schedule;

    public ConfigPageRenderer(string wikiDir, IScheduledTaskSnapshot schedule)
    {
        _outputPath = Path.Combine(wikiDir, "status", "config.md");
        _schedule = schedule;
    }

    public void Render(MonitorOptions options, DateTime now)
    {
        var schedule = _schedule.Get();
        var sb = new StringBuilder();
        sb.AppendLine("# Configuration");
        sb.AppendLine();
        sb.AppendLine($"*Generated by EAxWiki.Monitor at {now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} - current resolved runtime configuration, read-only.*");
        sb.AppendLine();
        sb.AppendLine("## Run settings");
        sb.AppendLine();
        sb.AppendLine("| Setting | Value |");
        sb.AppendLine("|---|---|");
        sb.AppendLine($"| Wiki dir | `{options.WikiDir}` |");
        sb.AppendLine($"| Repository | `{RedactRepo(options.RepoPath)}` |");
        sb.AppendLine($"| Wiki port | {options.WikiPort} |");
        sb.AppendLine($"| API port | {options.ApiPort} |");
        sb.AppendLine($"| LLM port | {options.LlmPort} |");
        sb.AppendLine($"| Export interval | {options.ExportIntervalMinutes} min |");
        sb.AppendLine($"| Check interval | {options.CheckIntervalSeconds} s |");
        sb.AppendLine($"| Max retries | {options.MaxRetries} |");
        sb.AppendLine($"| Retry delay | {options.RetryDelaySeconds} s |");
        sb.AppendLine($"| Min element fraction | {options.MinElementFraction.ToString(CultureInfo.InvariantCulture)} |");
        sb.AppendLine($"| Force | {(options.Force ? "enabled" : "disabled")} |");
        sb.AppendLine($"| Force every N runs | {(options.ForceEveryNRuns == 0 ? "off" : $"every {options.ForceEveryNRuns} runs")} |");
        sb.AppendLine($"| Brand | {options.Brand ?? "(default)"} |");
        sb.AppendLine($"| AI mode | {options.AiMode} |");
        sb.AppendLine($"| AI endpoint | {options.AiEndpoint ?? "(not set)"} |");
        sb.AppendLine($"| AI model | {options.AiModel ?? "(not set)"} |");
        sb.AppendLine();
        sb.AppendLine("## Alert destinations");
        sb.AppendLine();
        sb.AppendLine("| Destination | Status |");
        sb.AppendLine("|---|---|");
        sb.AppendLine($"| Slack | {(string.IsNullOrEmpty(options.WebhookUrl) ? "not configured" : "configured")} |");
        sb.AppendLine($"| Teams | {(string.IsNullOrEmpty(options.TeamsWebhookUrl) ? "not configured" : "configured")} |");
        sb.AppendLine($"| Telegram | {(string.IsNullOrEmpty(options.TelegramBotToken) ? "not configured" : "configured")} |");
        sb.AppendLine();
        sb.AppendLine("## Schedule");
        sb.AppendLine();
        if (schedule == null)
        {
            sb.AppendLine("Schedule info unavailable (the scheduled task could not be queried).");
        }
        else
        {
            sb.AppendLine("| Field | Value |");
            sb.AppendLine("|---|---|");
            sb.AppendLine($"| Task name | `{schedule.TaskName}` |");
            sb.AppendLine($"| State | {schedule.State} |");
            sb.AppendLine($"| Wake to run | {(schedule.WakeToRun ? "enabled" : "disabled")} |");
            sb.AppendLine($"| Execution time limit | {schedule.ExecutionTimeLimit} |");
            sb.AppendLine($"| Multiple instances | {schedule.MultipleInstances} |");
            sb.AppendLine();
            sb.AppendLine("### Triggers");
            sb.AppendLine();
            foreach (var trigger in schedule.Triggers)
                sb.AppendLine($"- {trigger}");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);
        File.WriteAllText(_outputPath, sb.ToString());
    }

    private static string RedactRepo(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "(not set)";
        return ConnectionStringRegex.Replace(value, "$1=***");
    }
}
```

- [ ] **Step 4: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~ConfigPageRendererTests"`
Expected: PASS (4 tests).

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Monitor/ConfigPageRenderer.cs src/EAxWiki.Tests/ConfigPageRendererTests.cs
git commit -m "feat(monitor): add config status page renderer (issue #87)"
```

---

### Task 5: Wire both renderers into the monitor loop

**Files:**
- Modify: `src/EAxWiki.Monitor/MonitorLoop.cs` (fields 17-26, ctor 31-59, RenderAndSave 223-234)
- Modify: `src/EAxWiki.Monitor/MonitorApp.cs:15-46`
- Test: `src/EAxWiki.Tests/MonitorLoopTests.cs`

**Interfaces:**
- Consumes: `ErrorLogPageRenderer.Render(DateTime)` (Task 2), `ConfigPageRenderer.Render(MonitorOptions, DateTime)` + `ScheduledTaskSnapshot` (Tasks 3-4).
- Produces: `MonitorLoop` ctor gains two params after `HealthPageRenderer pageRenderer`: `ErrorLogPageRenderer errorsPageRenderer, ConfigPageRenderer configPageRenderer`. `RenderAndSave()` renders all three pages.

- [ ] **Step 1: Write the failing test (add to MonitorLoopTests.cs)**

Add `StubSnapshot` next to the other stubs (after `FakeHealthStore`, line 50):

```csharp
    private sealed class StubSnapshot : IScheduledTaskSnapshot
    {
        public ScheduledTaskInfo? Get() =>
            new("EAxWiki-Monitor", "Ready", false, "PT72H", "IgnoreNew", ["Daily at 00:00, every 4 h (for 8 h)"]);
    }
```

Add `out string wikiDirOut` to the `Build` signature (line 52-55) — after `out FakeHealthStore store`:

```csharp
    private static MonitorLoop Build(
        out StubExportRunner exportRunner, out StubDigest digest, out StubAlerts alerts,
        out StubSupervisor supervisor, out HealthState state, out FakeHealthStore store,
        out string wikiDirOut, string? wikiDir = null, int checkInterval = 0, bool local = false)
```

At the top of `Build`, resolve the wiki dir once and assign it out (currently `options.WikiDir` is set to `wikiDir ?? Path.Combine(dir, "wiki")` on line 68 — replace that line and add the out assignment just before the `MonitorLoop` construction):

```csharp
        var resolvedWikiDir = wikiDir ?? Path.Combine(dir, "wiki");
        ...
        var options = new MonitorOptions
        {
            RepoPath = @"C:\models\repo.qea",
            WikiDir = resolvedWikiDir,
            ...
        };
        ...
        File.WriteAllText(Path.Combine(dir, "errors-template.md"), "# Error Log\n@@ERRORS@@\n@@RECENT@@\n");
        wikiDirOut = resolvedWikiDir;
```

Update the `MonitorLoop` construction (lines 86-94) to pass the two new renderers:

```csharp
        var loop = new MonitorLoop(
            options, state, store,
            new HealthPageRenderer(Path.Combine(dir, "health-template.md"), options.WikiDir),
            new ErrorLogPageRenderer(Path.Combine(dir, "errors-template.md"), options.WikiDir, Path.Combine(dir, "logs"), Array.Empty<string>()),
            new ConfigPageRenderer(options.WikiDir, new StubSnapshot()),
            dir,
            exportRunner, digest, alerts, supervisor,
            new ServiceSpec("serve", Path.Combine(dir, "serve.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("api", Path.Combine(dir, "api.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("llm", Path.Combine(dir, "llm.pid"), "cmd.exe", Array.Empty<string>(), dir),
            NullLogger.Instance);
```

Update every existing `Build(...)` call to pass `out _` for the new 7th argument (lines 101, 113, 124, 135, 145). Add the new test:

```csharp
    [Fact]
    public void RunOnce_WritesErrorAndConfigPages()
    {
        var loop = Build(out _, out _, out _, out _, out _, out _, out var wikiDir);
        loop.RunOnce();

        Assert.True(File.Exists(Path.Combine(wikiDir, "status", "errors.md")), "errors.md should be rendered");
        Assert.True(File.Exists(Path.Combine(wikiDir, "status", "config.md")), "config.md should be rendered");
    }
```

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~MonitorLoopTests"`
Expected: FAIL — `MonitorLoop` ctor doesn't accept the new arguments (compile error).

- [ ] **Step 3: Update `MonitorLoop`**

Add fields after `_pageRenderer` (line 17):

```csharp
    private readonly ErrorLogPageRenderer _errorsPageRenderer;
    private readonly ConfigPageRenderer _configPageRenderer;
```

Add ctor params after `HealthPageRenderer pageRenderer` (line 35) and assignments after `_pageRenderer = pageRenderer;` (line 49):

```csharp
    public MonitorLoop(
        MonitorOptions options,
        HealthState state,
        HealthStore healthStore,
        HealthPageRenderer pageRenderer,
        ErrorLogPageRenderer errorsPageRenderer,
        ConfigPageRenderer configPageRenderer,
        string stateDir,
        ...
    {
        ...
        _pageRenderer = pageRenderer;
        _errorsPageRenderer = errorsPageRenderer;
        _configPageRenderer = configPageRenderer;
        ...
    }
```

Replace `RenderAndSave` (lines 223-234):

```csharp
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
```

- [ ] **Step 4: Update `MonitorApp.Build`**

After the `pageRenderer` construction (lines 17-19), add:

```csharp
        var errorsPageRenderer = new ErrorLogPageRenderer(
            Path.Combine(templateDir, "errors-template.md"),
            options.WikiDir,
            Path.Combine(stateDir, "logs"),
            new[] { options.WebhookUrl ?? "", options.TeamsWebhookUrl ?? "", options.TelegramBotToken ?? "" });
        var configPageRenderer = new ConfigPageRenderer(options.WikiDir, new ScheduledTaskSnapshot());
```

Change the `return new MonitorLoop(...)` call (lines 43-45) to:

```csharp
        return new MonitorLoop(options, state, healthStore, pageRenderer, errorsPageRenderer, configPageRenderer,
            stateDir, exportRunner, digestTracker, alerts, supervisor, serveSpec, apiSpec, llmSpec!,
            loggerFactory.CreateLogger("MonitorLoop"));
```

- [ ] **Step 5: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~MonitorLoopTests"`
Expected: PASS (7 tests, including the new `RunOnce_WritesErrorAndConfigPages`).

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/MonitorLoop.cs src/EAxWiki.Monitor/MonitorApp.cs src/EAxWiki.Tests/MonitorLoopTests.cs
git commit -m "feat(monitor): render error and config pages in the monitor loop (issue #87)"
```

---

### Task 6: Nav entries for the new pages (`InfrastructureWriter`)

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs:31-42`
- Test: `src/EAxWiki.Tests/StatusPagesNavTests.cs`

**Interfaces:**
- Consumes: `InfrastructureWriter(IOutputWriter)` + `WritePagesFileAsync(string outputDir, CancellationToken ct = default)` (existing).
- Produces: `status/.pages` containing `  - Error Log: status/errors.html` and `  - Configuration: status/config.html` only when the respective `.md` exists on disk. Consumed by mkdocs nav; plain `export.ps1`/`export-and-serve.ps1` runs (no monitor) must not link to missing pages.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/StatusPagesNavTests.cs`:

```csharp
using EAxWiki.Export;
using EAxWiki.Export.Exporters;

namespace EAxWiki.Tests;

public class StatusPagesNavTests : IDisposable
{
    private readonly string _outPath;

    public StatusPagesNavTests()
    {
        _outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_statusnav_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_outPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_outPath))
            Directory.Delete(_outPath, recursive: true);
    }

    private async Task WritePages()
    {
        await new InfrastructureWriter(new FileOutputWriter()).WritePagesFileAsync(_outPath);
    }

    [Fact]
    public async Task NoStatusFiles_OmitsErrorConfigAndHealthEntries()
    {
        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.DoesNotContain("Error Log", pages);
        Assert.DoesNotContain("Configuration", pages);
        Assert.DoesNotContain("Pipeline Health", pages);
    }

    [Fact]
    public async Task WithErrorAndConfigFiles_IncludesEntries()
    {
        var statusDir = Path.Combine(_outPath, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "errors.md"), "x");
        File.WriteAllText(Path.Combine(statusDir, "config.md"), "x");

        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.Contains("  - Error Log: status/errors.html", pages);
        Assert.Contains("  - Configuration: status/config.html", pages);
    }

    [Fact]
    public async Task WithHealthFile_IncludesPipelineHealthEntry()
    {
        var statusDir = Path.Combine(_outPath, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "health.md"), "x");

        await WritePages();

        var pages = File.ReadAllText(Path.Combine(_outPath, ".pages"));
        Assert.Contains("  - Pipeline Health: status/health.html", pages);
    }
}
```

- [ ] **Step 2: Run to verify it fails**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~StatusPagesNavTests"`
Expected: FAIL — `.pages` currently has no Error Log / Configuration entries even when the files exist.

- [ ] **Step 3: Implement the gating**

Replace the `var statusLines = ...` block (InfrastructureWriter.cs:31-42) with:

```csharp
        var statusLines = new List<string>
        {
            "  - Status: status/",
            "  - Model Health: status/model-health.html",
        };
        if (File.Exists(Path.Combine(outputDir, "status", "health.md")))
            statusLines.Insert(1, "  - Pipeline Health: status/health.html");
        if (File.Exists(Path.Combine(outputDir, "status", "errors.md")))
            statusLines.Add("  - Error Log: status/errors.html");
        if (File.Exists(Path.Combine(outputDir, "status", "config.md")))
            statusLines.Add("  - Configuration: status/config.html");
```

The `.pages` writer at lines 44-54 uses `.. statusLines,` so no further change is needed.

- [ ] **Step 4: Run to verify it passes**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --filter "FullyQualifiedName~StatusPagesNavTests"`
Expected: PASS (3 tests).

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs src/EAxWiki.Tests/StatusPagesNavTests.cs
git commit -m "feat(export): nav-gate error and config status pages (issue #87)"
```

---

### Task 7: Link both pages from the Health page

**Files:**
- Modify: `.eaxwiki-monitor/health-template.md` (tracked)

- [ ] **Step 1: Add the links**

Insert after the `*Generated by EAxWiki.Monitor ...*` line (line 3) of `.eaxwiki-monitor/health-template.md`:

```markdown
- [Error log](errors.html) - [Configuration](config.html)
```

- [ ] **Step 2: Commit**

```bash
git add .eaxwiki-monitor/health-template.md
git commit -m "docs: link error log and config pages from health page (issue #87)"
```

---

### Task 8: Full verification

**Files:** (none unless fixes are needed)

- [ ] **Step 1: Run the full .NET suite**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj`
Expected: ALL PASS (~443 unit tests + ~82 Pester-triggered script tests, now plus the ~23 new ones). Known flakes pass on rerun: `PropertyBasedTests.EscapeCell_LengthAtLeastInputLength`, `Export_StatusEditorScript`. If a task's implementation introduced a failure, fix it here in a follow-up commit.

- [ ] **Step 2: Run the Pester script suite**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; Invoke-Pester tests/scripts/`
Expected: ALL PASS (no scripts changed; the script-inspection tests must still hold). The C# `StatusPagesNavTests` (Task 6) covers the design doc's "plain export produces no error/config nav links" guarantee — a source-inspection Pester test cannot reach C# logic.

- [ ] **Step 3: Commit any fixes**

If Step 1 or 2 needed fixes, commit them:

```bash
git add <fixed files>
git commit -m "fix(monitor): ... (issue #87)"
```

- [ ] **Step 4: Hand off to the export-cycle skill**

The live-monitor smoke (build + run `EAxWiki.Monitor.exe`, verify `errors.md`/`config.md` render and serve via mkdocs on 8000, confirm the schedule snapshot shows the registered task), committing the generated `wiki/` (including the new status pages), pushing to `origin/master`, and commenting on issue #87 all happen via the export-cycle skill with a human partner.

---

## Self-Review

**Spec coverage:**
- Log format severity token → Task 1. ✔
- `errors.md` (7-day window, WRN/ERR only, newest-first, 100 cap, recent fallback, redaction, template, empty states) → Task 2. ✔
- `config.md` (operational values, alert dest configured/not-configured, repo redaction, schedule + unavailable, generated-at) → Task 4. ✔
- Live `Get-ScheduledTask` snapshot, task located by exe path, 5-min cache, never throws → Task 3. ✔
- Rendered every loop cycle alongside health.md with per-renderer try/catch → Task 5. ✔
- Nav entries existence-gated in `InfrastructureWriter` (same pattern as `health.md`), awesome-pages `.html` quirk → Task 6. ✔
- Health-page links → Task 7. ✔
- Multi-instance isolation: each renderer gets its own `{stateDir}/logs` and its own secrets (MonitorApp passes `Path.Combine(stateDir, "logs")` and this instance's `WebhookUrl`/`TeamsWebhookUrl`/`TelegramBotToken`) → Task 5. ✔
- Out of scope honored: no validation-report surfacing, no `--status-pages`, no schedule.json, DigestTracker untouched. ✔

**Placeholder scan:** every code step contains full implementation; no TBD/TODO/"add validation" placeholders.

**Type consistency:** `ErrorLogPageRenderer(string, string, string, string[])` and `ConfigPageRenderer(string, IScheduledTaskSnapshot)` are constructed identically in MonitorApp (Task 5) and in tests (Tasks 2, 4). `ScheduledTaskInfo` record shape is produced by `ScheduledTaskJsonParser.Parse` (Task 3) and consumed by `ConfigPageRenderer` (Task 4) + `StubSnapshot` (Tasks 4-5). `MonitorLoop` ctor arg order is updated in exactly two places (MonitorApp.Build, MonitorLoopTests.Build). ✔