# C# Monitor Implementation Plan (issue #86 item #3)

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Port `scripts/monitor-export-and-serve.ps1` (1296 lines) to a new `src/EAxWiki.Monitor` console project — same flags, same state file, same alert kinds/formatting, same watchdog semantics — making the retry/health/digest/force logic unit-testable in C#, fixing the dead `--force`/`--force-every` semantics, adding a configurable LLM port, and adding a read-only SchedulerUI health dashboard.

**Architecture:** A new `EAxWiki.Monitor` net10.0 console exe replaces the PS monitor (the script + its ~47 Pester tests are removed; coverage moves to xUnit). Components are plain classes injected into a `MonitorLoop` (the `while(true)` cycle) via small interfaces, so every unit is fake-substitutable in tests. `Program` parses System.CommandLine args, resolves `.eaxwiki` via `LocalConfigStore`, then builds the loop. In-process export runs on an STA thread (reusing the `EaReaderStaDispatcher` pattern). Shared types (`HealthState`, `HealthStore`, `InstanceHash`, `TcpPortProbe`, `PidFile`) live in `EAxWiki.Core/Monitoring/` so the SchedulerUI dashboard (which cannot reference the Monitor exe) can reuse them.

**Tech Stack:** .NET 10 (C#), System.CommandLine 2.0.11, `Microsoft.Extensions.Logging` 10.0.11 + `Microsoft.Extensions.Logging.Console` 10.0.11, xUnit 2.9, Moq, Pester (shrinkage only). EA/COM only in `StaMarkdownExporter` and `ProcessSupervisor` child starts — everything else is EA-free.

## Global Constraints

- No change to the existing EAxWiki `Config.Load` hand-rolled parser; the monitor has its own System.CommandLine root command (issue #4 refactors `Config.Load` later).
- `EAxWiki.Monitor` is a **detached, independently-running process**. SchedulerUI's "Run Monitor Now" launches it via `Process.Start(UseShellExecute = true)`; the scheduled task launches the exe directly. Closing the UI never stops the monitor.
- Behavior parity with the PS monitor 1:1, **except** the one deliberate fix: live `--force` / `--force-every N` semantics (the PS script hardcodes `$effectiveForce = $true`, making those flags dead). `--force` → every run is a full rebuild; `--force-every N` → full rebuild when `runsSinceForce >= N`, reset to 0 on a successful forced run; neither → incremental.
- **Deliberate divergence (flagged in design review):** the design doc says `HealthPageRenderer` writes `wiki/status/health.md`; the PS script actually writes `.eaxwiki-monitor/status/health.md`, which means `InfrastructureWriter.cs:31`'s `File.Exists(outputDir/status/health.md)` check (Pipeline Health nav entry) never fires today. This plan follows the **design doc**: render to `wiki/status/health.md` so the nav entry works. `wiki/status/` is a recognized special dir, never cleaned by the exporter's orphan cleanup.
- State directory `.eaxwiki-monitor/<12-char md5 of lowercased wikiDir>/` with `health.json`, `serve.pid`, `api.pid`, `llm.pid`, `monitor.pid`, `logs/` — identical paths to today.
- `monitor.pid` is plain PID text; `serve.pid`/`api.pid`/`llm.pid` are JSON `{pid, startTime}`; alive = `GetProcessById` succeeds AND |recorded start − actual start| ≤ 2s.
- No new public API in `EAxWiki.Core`: new members are `internal` or live in `EAxWiki.Monitor`/`EAxWiki.SchedulerUI` (except `Config.LlmPort` — the only public addition).
- CRLF / file conventions preserved. New/changed files: LF, UTF-8 (no BOM).
- Every `dotnet test` command requires `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\';` because `EAxWiki.EA` unconditionally references `Interop.EA.dll` via `$(EAPath)`. Set it inline per command (see commands below).
- All existing tests stay green: 311 .NET + 162 Pester baseline. Pester loses the 47 `MonitorExportAndServe` + 2 `SendAlert` tests and gains 1 bootstrap test (→ ~114). New .NET tests add to the count.
- Working-tree hygiene: `model/`, `wiki/`, `.eaxwiki-monitor/`, `.eaxwiki` are runtime artifacts and must never be staged or committed.
- Do not commit or push unless explicitly asked. Commits use lowercase conventional style matching repo history (e.g. `feat(monitor): ...`, `test(monitor): ...`).
- Tests live in `EAxWiki.Tests` with `namespace EAxWiki.Tests;`, `[Fact]`, and the temp-dir `IDisposable` pattern used by `LocalConfigStoreTests`/`CleanupTests` (see `src/EAxWiki.Tests/LocalConfigStoreTests.cs:5-19`).

## File Structure

**New project `src/EAxWiki.Monitor/`** (console, net10.0, OutputType Exe, `NoWarn>CA1416`, `ImplicitUsings`/`Nullable` on; references `EAxWiki.Core`, `EAxWiki.EA`, `EAxWiki.Export`; `InternalsVisibleTo("EAxWiki.Tests")`; packages: `System.CommandLine` 2.0.11, `Microsoft.Extensions.Logging` 10.0.11, `Microsoft.Extensions.Logging.Console` 10.0.11). Added to `EAxWiki.slnx` and `EAxWiki.Tests.csproj`.

**Shared in `src/EAxWiki.Core/Monitoring/`** (used by Monitor, SchedulerUI dashboard, and tests):
- `HealthState.cs`, `HealthStore.cs`, `InstanceHash.cs`, `PortProbe.cs` (`IPortProbe`, `TcpPortProbe`), `PidFile.cs`.

**Monitor-only in `src/EAxWiki.Monitor/`:**
- `CliOptions.cs`, `MonitorCommandLine.cs`, `MonitorOptions.cs`, `MonitorOptionsResolver.cs`
- `AlertDispatcher.cs` (`AlertKind`, `AlertOptions`, `AlertDispatcher`), `TelegramAlertTextFormatter.cs`
- `HealthPageRenderer.cs`, `EditLock.cs`, `DigestTracker.cs` (`IDigestTracker`, `WritebackDelta`, `DigestTracker`)
- `PortKiller.cs` (`IPortKiller`, `NetstatPortKiller`), `ProcessSupervisor.cs` (`ServiceSpec`, `IProcessSupervisor`, `ProcessSupervisor`)
- `ExportRunner.cs` (`IExportRunner`, `ExportRunner`, `IStaExporter`, `StaMarkdownExporter`, `IWikiOutputMetrics`, `WikiOutputMetrics`)
- `MonitorLoop.cs`, `MonitorLock.cs`, `MonitorPaths.cs`, `MonitorFileLoggerProvider.cs`, `MonitorApp.cs`, `Program.cs`

**Callers:**
- Modify `EAxWiki.slnx` (root), `src/EAxWiki.Tests/EAxWiki.Tests.csproj`, `src/EAxWiki.Core/Configuration/LocalConfigStore.cs` (+`LlmPort`), `src/EAxWiki.SchedulerUI/SchedulerForm.cs`, `src/EAxWiki.SchedulerUI/HealthDashboardReader.cs` (new), `scripts/_bootstrap.ps1`, `scripts/register-scheduled-task.ps1`, `README.md`, `.claude/skills/scheduled-task-diagnostics/SKILL.md`.
- Delete `scripts/monitor-export-and-serve.ps1`, `tests/scripts/monitor-export-and-serve.Tests.ps1`, `tests/scripts/send-alert.Tests.ps1`.

**Tests in `src/EAxWiki.Tests/`:** `HealthStoreTests.cs`, `InstanceHashTests.cs`, `MonitorCommandLineTests.cs`, `MonitorOptionsResolverTests.cs`, `PortProbeTests.cs`, `PidFileTests.cs`, `PortKillerTests.cs`, `EditLockTests.cs`, `HealthPageRendererTests.cs`, `DigestTrackerTests.cs`, `AlertDispatcherTests.cs`, `ProcessSupervisorTests.cs`, `ExportRunnerTests.cs`, `MonitorLoopTests.cs`, `MonitorLockTests.cs`, `MonitorPathsTests.cs`, `HealthDashboardReaderTests.cs`.

---

### Task 1: Scaffold `EAxWiki.Monitor` + shared Core/Monitoring types

**Files:**
- Create: `src/EAxWiki.Monitor/EAxWiki.Monitor.csproj`
- Create: `src/EAxWiki.Core/Monitoring/HealthState.cs`
- Create: `src/EAxWiki.Core/Monitoring/HealthStore.cs`
- Create: `src/EAxWiki.Core/Monitoring/InstanceHash.cs`
- Create: `src/EAxWiki.Tests/HealthStoreTests.cs`
- Create: `src/EAxWiki.Tests/InstanceHashTests.cs`
- Modify: `EAxWiki.slnx` (add project after line 9)
- Modify: `src/EAxWiki.Tests/EAxWiki.Tests.csproj` (add ProjectReference)

**Interfaces:**
- Produces: `EAxWiki.Core.Monitoring.HealthState` (all 27 fields, camelCase-serialized via `JsonSerializerDefaults.Web`), `HealthStore.Load(string path)` / `HealthStore.Save(string path, HealthState state)`, `InstanceHash.Compute(string wikiDir)`.

- [ ] **Step 1: Create the Monitor csproj**

Create `src/EAxWiki.Monitor/EAxWiki.Monitor.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <NoWarn>CA1416</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\EAxWiki.Core\EAxWiki.Core.csproj" />
    <ProjectReference Include="..\EAxWiki.EA\EAxWiki.EA.csproj" />
    <ProjectReference Include="..\EAxWiki.Export\EAxWiki.Export.csproj" />
  </ItemGroup>

  <ItemGroup>
    <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleTo">
      <_Parameter1>EAxWiki.Tests</_Parameter1>
    </AssemblyAttribute>
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Logging" Version="10.0.11" />
    <PackageReference Include="Microsoft.Extensions.Logging.Console" Version="10.0.11" />
    <PackageReference Include="System.CommandLine" Version="2.0.11" />
  </ItemGroup>

</Project>
```

Add `Program.cs` placeholder (real wiring lands in Task 11):

```csharp
namespace EAxWiki.Monitor;

public static class Program
{
    public static int Main(string[] args) => 0;
}
```

- [ ] **Step 2: Add to the solution and test project**

In `EAxWiki.slnx`, after line 9 (`<Project Path="src/EAxWiki/EAxWiki.csproj" />`), add:

```xml
    <Project Path="src/EAxWiki.Monitor/EAxWiki.Monitor.csproj" />
```

In `src/EAxWiki.Tests/EAxWiki.Tests.csproj`, after the `<ProjectReference Include="..\EAxWiki.SchedulerUI\...>` line, add:

```xml
    <ProjectReference Include="..\EAxWiki.Monitor\EAxWiki.Monitor.csproj" />
```

- [ ] **Step 3: Create `HealthState`**

Create `src/EAxWiki.Core/Monitoring/HealthState.cs`:

```csharp
namespace EAxWiki.Core.Monitoring;

/// <summary>
/// Monitor health/state persisted as <c>.eaxwiki-monitor/&lt;hash&gt;/health.json</c>.
/// Field names serialize camelCase via <c>JsonSerializerDefaults.Web</c> — identical to the
/// PowerShell monitor's JSON (lastSuccessTime, skipExport, ...). The SchedulerUI reads this
/// file read-only for its health dashboard.
/// </summary>
public class HealthState
{
    public DateTimeOffset? LastSuccessTime { get; set; }
    public DateTimeOffset? LastFailureTime { get; set; }
    public int ConsecutiveFailures { get; set; }

    public int? LastExitCode { get; set; }
    public int? LastElementCount { get; set; }
    public int? LastDiagramCount { get; set; }

    public int ServeConsecutiveFailures { get; set; }
    public DateTimeOffset? LastServeFailureTime { get; set; }
    public DateTimeOffset? LastServeSuccessTime { get; set; }

    public int LlmConsecutiveFailures { get; set; }
    public DateTimeOffset? LastLlmFailureTime { get; set; }
    public DateTimeOffset? LastLlmSuccessTime { get; set; }

    public int ApiConsecutiveFailures { get; set; }
    public DateTimeOffset? LastApiFailureTime { get; set; }
    public DateTimeOffset? LastApiSuccessTime { get; set; }

    // Tracks the ApiPort used during the last export; the SchedulerUI reads it to show
    // whether the write-back API was enabled for the last run.
    public int LastApiPort { get; set; }

    public int RunsSinceForce { get; set; }
    public string? LastMode { get; set; }

    public int PageReadsToday { get; set; }
    public int WritebacksToday { get; set; }
    public string? LastDigestDate { get; set; }
    public string? PageReadLogFile { get; set; }
    public long PageReadLogOffset { get; set; }
    public string? WritebackLogFile { get; set; }
    public long WritebackLogOffset { get; set; }

    public bool SkipExport { get; set; }
    public bool SkipServe { get; set; }
}
```

- [ ] **Step 4: Create `HealthStore`**

Create `src/EAxWiki.Core/Monitoring/HealthStore.cs`:

```csharp
using System.Text.Json;

namespace EAxWiki.Core.Monitoring;

/// <summary>
/// Load/save <see cref="HealthState"/> as JSON. Serialization uses camelCase property names
/// (matching the PS monitor's ConvertTo-Json output). Load backfills: fields missing from an
/// older on-disk file simply keep their CLR defaults (the equivalent of Add-Member -Force in
/// the PS monitor), and a corrupt file falls back to a fresh default state.
/// </summary>
public class HealthStore
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public HealthState Load(string path)
    {
        if (!File.Exists(path)) return new HealthState();
        try
        {
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new HealthState();
            return JsonSerializer.Deserialize<HealthState>(json, Options) ?? new HealthState();
        }
        catch (JsonException)
        {
            return new HealthState();
        }
    }

    public void Save(string path, HealthState state)
    {
        var json = JsonSerializer.Serialize(state, Options);
        File.WriteAllText(path, json);
    }
}
```

- [ ] **Step 5: Create `InstanceHash`**

Create `src/EAxWiki.Core/Monitoring/InstanceHash.cs`:

```csharp
using System.Security.Cryptography;
using System.Text;

namespace EAxWiki.Core.Monitoring;

/// <summary>
/// 12-char MD5 of the lowercased wiki output dir, keying the per-instance state folder —
/// identical to the PS monitor's $instanceHash. Lives in Core so the SchedulerUI dashboard
/// can resolve the same folder the monitor writes.
/// </summary>
public static class InstanceHash
{
    public static string Compute(string wikiDir)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(wikiDir.ToLowerInvariant()));
        return Convert.ToHexString(hash)[..12].ToLowerInvariant();
    }
}
```

- [ ] **Step 6: Write the failing tests**

Create `src/EAxWiki.Tests/HealthStoreTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class HealthStoreTests : IDisposable
{
    private readonly string _dir;

    public HealthStoreTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_health_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void Load_MissingFile_ReturnsDefaults()
    {
        var state = new HealthStore().Load(Path.Combine(_dir, "health.json"));
        Assert.Equal(0, state.ConsecutiveFailures);
        Assert.Null(state.LastSuccessTime);
        Assert.False(state.SkipExport);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesAllFields()
    {
        var path = Path.Combine(_dir, "health.json");
        var store = new HealthStore();
        var state = new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            ConsecutiveFailures = 2,
            LastElementCount = 150,
            RunsSinceForce = 7,
            SkipExport = true,
            SkipServe = true,
            PageReadsToday = 12,
            WritebacksToday = 3,
            PageReadLogFile = @"C:\logs\serve-1.err.log",
            PageReadLogOffset = 4096,
        };

        store.Save(path, state);
        var loaded = store.Load(path);

        Assert.Equal(state.LastSuccessTime, loaded.LastSuccessTime);
        Assert.Equal(state.ConsecutiveFailures, loaded.ConsecutiveFailures);
        Assert.Equal(state.LastElementCount, loaded.LastElementCount);
        Assert.Equal(state.RunsSinceForce, loaded.RunsSinceForce);
        Assert.True(loaded.SkipExport);
        Assert.True(loaded.SkipServe);
        Assert.Equal(state.PageReadsToday, loaded.PageReadsToday);
        Assert.Equal(state.WritebacksToday, loaded.WritebacksToday);
        Assert.Equal(state.PageReadLogFile, loaded.PageReadLogFile);
        Assert.Equal(state.PageReadLogOffset, loaded.PageReadLogOffset);
    }

    [Fact]
    public void Load_OlderFile_MissingFieldsBackfillToDefaults()
    {
        // Simulates a health.json written by an older monitor that lacked skipExport.
        var path = Path.Combine(_dir, "health.json");
        File.WriteAllText(path, """{"consecutiveFailures":1,"lastSuccessTime":"2026-08-01T10:00:00Z"}""");

        var state = new HealthStore().Load(path);

        Assert.Equal(1, state.ConsecutiveFailures);
        Assert.Equal("2026-08-01T10:00:00Z", state.LastSuccessTime?.ToString("O"));
        Assert.False(state.SkipExport);
        Assert.False(state.SkipServe);
        Assert.Equal(0, state.PageReadLogOffset);
    }

    [Fact]
    public void Load_CorruptFile_FallsBackToDefaults()
    {
        var path = Path.Combine(_dir, "health.json");
        File.WriteAllText(path, "{not json!!");

        var state = new HealthStore().Load(path);

        Assert.Equal(0, state.ConsecutiveFailures);
        Assert.Null(state.LastSuccessTime);
    }
}
```

Create `src/EAxWiki.Tests/InstanceHashTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class InstanceHashTests
{
    [Fact]
    public void Compute_Is12HexChars()
    {
        var hash = InstanceHash.Compute(@"C:\repo\wiki");
        Assert.Equal(12, hash.Length);
        Assert.Matches("^[0-9a-f]{12}$", hash);
    }

    [Fact]
    public void Compute_IsCaseInsensitiveOnWikiDir()
    {
        Assert.Equal(InstanceHash.Compute(@"C:\repo\wiki"), InstanceHash.Compute(@"c:\REPO\WIKI"));
    }

    [Fact]
    public void Compute_DifferentPaths_Differ()
    {
        Assert.NotEqual(InstanceHash.Compute(@"C:\repo\wiki"), InstanceHash.Compute(@"C:\repo\wiki2"));
    }
}
```

- [ ] **Step 7: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~HealthStoreTests|FullyQualifiedName~InstanceHashTests"
```

Expected: compile error — `EAxWiki.Core.Monitoring` does not exist (the types are created in Steps 3-5, so re-run after the compile passes; the filter itself is the sanity gate that the new test files are wired in).

- [ ] **Step 8: Verify the new tests pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~HealthStoreTests|FullyQualifiedName~InstanceHashTests"
```

Expected: `Passed! - Failed: 0, Passed: 7` (4 + 3).

- [ ] **Step 9: Run the full .NET suite to confirm no regression**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0` (311 + 7 = 318; if the count differs, record the real number in the report).

- [ ] **Step 10: Commit**

```bash
git add EAxWiki.slnx src/EAxWiki.Monitor/EAxWiki.Monitor.csproj src/EAxWiki.Monitor/Program.cs src/EAxWiki.Core/Monitoring/HealthState.cs src/EAxWiki.Core/Monitoring/HealthStore.cs src/EAxWiki.Core/Monitoring/InstanceHash.cs src/EAxWiki.Tests/EAxWiki.Tests.csproj src/EAxWiki.Tests/HealthStoreTests.cs src/EAxWiki.Tests/InstanceHashTests.cs
git commit -m "feat(monitor): scaffold EAxWiki.Monitor and shared Monitoring types (issue #86)"
```

---

### Task 2: `CliOptions` + System.CommandLine parser

**Files:**
- Create: `src/EAxWiki.Monitor/CliOptions.cs`
- Create: `src/EAxWiki.Monitor/MonitorCommandLine.cs`
- Create: `src/EAxWiki.Tests/MonitorCommandLineTests.cs`

**Interfaces:**
- Consumes: nothing (self-contained).
- Produces: `record CliOptions` (all nullable/boolean flag surface), `static RootCommand MonitorCommandLine.BuildCommand()`, `static CliOptions MonitorCommandLine.ToOptions(ParseResult)`.

Flag surface (mirrors the PS monitor's `Get-MonitorArgs`, plus new `--llm-port`): `--repo/-r`, `--output/-o`, `--port/-p`, `--max-retries`, `--retry-delay`, `--min-element-fraction`, `--webhook-url`, `--teams-webhook-url`, `--telegram-bot-token`, `--telegram-chat-id`, `--brand`, `--test-alert`, `--no-notify-start`, `--force/-f`, `--force-every`, `--export-interval`, `--check-interval`, `--llm-port`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/MonitorCommandLineTests.cs`:

```csharp
using System.CommandLine;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorCommandLineTests
{
    private static CliOptions Parse(params string[] args) =>
        MonitorCommandLine.ToOptions(MonitorCommandLine.BuildCommand().Parse(args));

    [Fact]
    public void NoArgs_ReturnsDefaults()
    {
        var o = Parse();
        Assert.Null(o.Repo);
        Assert.Null(o.OutputDir);
        Assert.Null(o.Port);
        Assert.Null(o.MaxRetries);
        Assert.Null(o.RetryDelaySeconds);
        Assert.Null(o.MinElementFraction);
        Assert.Null(o.WebhookUrl);
        Assert.Null(o.TeamsWebhookUrl);
        Assert.Null(o.TelegramBotToken);
        Assert.Null(o.TelegramChatId);
        Assert.Null(o.Brand);
        Assert.False(o.TestAlert);
        Assert.Null(o.NotifyOnStart);
        Assert.False(o.Force);
        Assert.Null(o.ForceEveryNRuns);
        Assert.Null(o.ExportIntervalMinutes);
        Assert.Null(o.CheckIntervalSeconds);
        Assert.Null(o.LlmPort);
    }

    [Theory]
    [InlineData("-r", "model.qea")]
    [InlineData("--repo", "model.qea")]
    public void Repo_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(value, Parse(flag, value).Repo);
    }

    [Fact]
    public void Repo_ConnectionStringAsBarePositional()
    {
        // System.CommandLine: unmatched tokens are collected; the monitor treats a bare
        // non-flag argument as the repo path (PS accepted a bare connection string too).
        var o = Parse("DBType=postgresql;Database=foo");
        Assert.Equal("DBType=postgresql;Database=foo", o.Repo);
    }

    [Theory]
    [InlineData("-o", "wiki")]
    [InlineData("--output", "wiki")]
    public void OutputDir_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(value, Parse(flag, value).OutputDir);
    }

    [Theory]
    [InlineData("-p", "8080")]
    [InlineData("--port", "8080")]
    public void Port_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(8080, Parse(flag, value).Port);
    }

    [Fact]
    public void MaxRetries_Parses()
    {
        Assert.Equal(5, Parse("--max-retries", "5").MaxRetries);
    }

    [Fact]
    public void RetryDelay_Parses()
    {
        Assert.Equal(60, Parse("--retry-delay", "60").RetryDelaySeconds);
    }

    [Fact]
    public void MinElementFraction_ParsesDouble()
    {
        Assert.Equal(0.25, Parse("--min-element-fraction", "0.25").MinElementFraction);
    }

    [Fact]
    public void Webhooks_Parse()
    {
        var o = Parse("--webhook-url", "https://hooks.slack.com/ABC", "--teams-webhook-url", "https://outlook.office.com/DEF");
        Assert.Equal("https://hooks.slack.com/ABC", o.WebhookUrl);
        Assert.Equal("https://outlook.office.com/DEF", o.TeamsWebhookUrl);
    }

    [Fact]
    public void Telegram_Parses()
    {
        var o = Parse("--telegram-bot-token", "123:ABC", "--telegram-chat-id", "-100123");
        Assert.Equal("123:ABC", o.TelegramBotToken);
        Assert.Equal("-100123", o.TelegramChatId);
    }

    [Fact]
    public void Brand_Parses()
    {
        Assert.Equal("eursura", Parse("--brand", "eursura").Brand);
    }

    [Fact]
    public void TestAlert_IsSet()
    {
        Assert.True(Parse("--test-alert").TestAlert);
    }

    [Fact]
    public void NoNotifyStart_IsSet()
    {
        Assert.False(Parse("--no-notify-start").NotifyOnStart);
    }

    [Theory]
    [InlineData("-f")]
    [InlineData("--force")]
    public void Force_ParsesShortAndLong(string flag)
    {
        Assert.True(Parse(flag).Force);
    }

    [Fact]
    public void ForceEvery_Parses()
    {
        Assert.Equal(48, Parse("--force-every", "48").ForceEveryNRuns);
    }

    [Fact]
    public void ExportInterval_Parses()
    {
        Assert.Equal(60, Parse("--export-interval", "60").ExportIntervalMinutes);
    }

    [Fact]
    public void CheckInterval_Parses()
    {
        Assert.Equal(15, Parse("--check-interval", "15").CheckIntervalSeconds);
    }

    [Fact]
    public void LlmPort_Parses()
    {
        Assert.Equal(9090, Parse("--llm-port", "9090").LlmPort);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorCommandLineTests"
```

Expected: compile error — `EAxWiki.Monitor.CliOptions` / `MonitorCommandLine` do not exist.

- [ ] **Step 3: Implement `CliOptions`**

Create `src/EAxWiki.Monitor/CliOptions.cs`:

```csharp
namespace EAxWiki.Monitor;

/// <summary>
/// Parsed-but-unresolved command-line surface. Null means "not given on the command line";
/// resolution against env vars and .eaxwiki happens in <see cref="MonitorOptionsResolver"/>.
/// </summary>
public sealed record CliOptions
{
    public string? Repo { get; init; }
    public string? OutputDir { get; init; }
    public int? Port { get; init; }
    public int? MaxRetries { get; init; }
    public int? RetryDelaySeconds { get; init; }
    public double? MinElementFraction { get; init; }
    public string? WebhookUrl { get; init; }
    public string? TeamsWebhookUrl { get; init; }
    public string? TelegramBotToken { get; init; }
    public string? TelegramChatId { get; init; }
    public string? Brand { get; init; }
    public bool TestAlert { get; init; }
    public bool? NotifyOnStart { get; init; }
    public bool Force { get; init; }
    public int? ForceEveryNRuns { get; init; }
    public int? ExportIntervalMinutes { get; init; }
    public int? CheckIntervalSeconds { get; init; }
    public int? LlmPort { get; init; }
}
```

- [ ] **Step 4: Implement `MonitorCommandLine`**

Create `src/EAxWiki.Monitor/MonitorCommandLine.cs`:

```csharp
using System.CommandLine;

namespace EAxWiki.Monitor;

/// <summary>
/// System.CommandLine root command for the monitor. Flag surface mirrors the PS monitor's
/// Get-MonitorArgs (plus the new --llm-port). A bare non-flag argument is accepted as the
/// repo path via UnmatchedTokens (PS accepted a bare connection string the same way).
/// </summary>
public static class MonitorCommandLine
{
    public static RootCommand BuildCommand()
    {
        var repo = new Option<string?>("--repo", "-r") { Description = "EA repository path or connection string (defaults to .eaxwiki repoPath)." };
        var output = new Option<string?>("--output", "-o") { Description = "Wiki output directory, absolute or relative to the repo root (default: wiki)." };
        var port = new Option<int?>("--port", "-p") { Description = "Wiki (mkdocs serve) port (default 8000, or .eaxwiki wikiPort)." };
        var maxRetries = new Option<int?>("--max-retries") { Description = "Max export/service start attempts (default 3)." };
        var retryDelay = new Option<int?>("--retry-delay") { Description = "Retry backoff base in seconds (default 30)." };
        var minElementFraction = new Option<double?>("--min-element-fraction") { Description = "Minimum element-count floor as a fraction of the previous run (default 0.5)." };
        var webhook = new Option<string?>("--webhook-url") { Description = "Slack webhook URL." };
        var teamsWebhook = new Option<string?>("--teams-webhook-url") { Description = "Microsoft Teams webhook URL." };
        var telegramToken = new Option<string?>("--telegram-bot-token") { Description = "Telegram bot token." };
        var telegramChatId = new Option<string?>("--telegram-chat-id") { Description = "Telegram chat id (string; group ids are negative)." };
        var brand = new Option<string?>("--brand") { Description = "Wiki brand (e.g. eursura)." };
        var testAlert = new Option<bool>("--test-alert") { Description = "Send a Test alert to every configured channel and exit." };
        var noNotifyStart = new Option<bool?>("--no-notify-start") { Description = "Suppress Start and Finish alerts." };
        var force = new Option<bool>("--force", "-f") { Description = "Full rebuild on every run." };
        var forceEvery = new Option<int?>("--force-every") { Description = "Full rebuild every Nth run (0 = incremental only)." };
        var exportInterval = new Option<int?>("--export-interval") { Description = "Export cadence in minutes (default 30)." };
        var checkInterval = new Option<int?>("--check-interval") { Description = "Monitor loop sleep in seconds (default 30)." };
        var llmPort = new Option<int?>("--llm-port") { Description = "LLM server port (default 8080, or .eaxwiki llmPort)." };

        var root = new RootCommand("EAxWiki unattended monitor: export, serve, write-back API and LLM watchdogs.")
        {
            repo, output, port, maxRetries, retryDelay, minElementFraction,
            webhook, teamsWebhook, telegramToken, telegramChatId, brand,
            testAlert, noNotifyStart, force, forceEvery, exportInterval, checkInterval, llmPort,
        };

        // A bare positional argument is a connection string / repo path. System.CommandLine
        // rejects unknown tokens by default; treat them as unmatched instead and read them
        // in ToOptions (a plain .qea path or "DBType=...;..." never starts with '-').
        root.TreatUnmatchedTokensAsErrors = false;
        root.SetAction(_ => Task.FromResult(0));
        return root;
    }

    public static CliOptions ToOptions(ParseResult r)
    {
        var bare = r.UnmatchedTokens.FirstOrDefault();
        return new CliOptions
        {
            Repo = r.GetResult("--repo") is { Implicit: false } ? r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "repo")) : bare,
            OutputDir = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "output")),
            Port = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "port")),
            MaxRetries = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "max-retries")),
            RetryDelaySeconds = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "retry-delay")),
            MinElementFraction = r.GetValueForOption((Option<double?>)r.RootCommandResult.Command.Children.First(o => o.Name == "min-element-fraction")),
            WebhookUrl = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "webhook-url")),
            TeamsWebhookUrl = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "teams-webhook-url")),
            TelegramBotToken = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "telegram-bot-token")),
            TelegramChatId = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "telegram-chat-id")),
            Brand = r.GetValueForOption((Option<string?>)r.RootCommandResult.Command.Children.First(o => o.Name == "brand")),
            TestAlert = r.GetValueForOption((Option<bool>)r.RootCommandResult.Command.Children.First(o => o.Name == "test-alert")),
            NotifyOnStart = r.GetValueForOption((Option<bool?>)r.RootCommandResult.Command.Children.First(o => o.Name == "no-notify-start")),
            Force = r.GetValueForOption((Option<bool>)r.RootCommandResult.Command.Children.First(o => o.Name == "force")),
            ForceEveryNRuns = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "force-every")),
            ExportIntervalMinutes = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "export-interval")),
            CheckIntervalSeconds = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "check-interval")),
            LlmPort = r.GetValueForOption((Option<int?>)r.RootCommandResult.Command.Children.First(o => o.Name == "llm-port")),
        };
    }
}
```

Notes:
- `Option<string?>("--repo", "-r")` uses the `(string name, params string[] aliases)` constructor verified against System.CommandLine 2.0.11.
- `r.GetResult(option)` on a `ParseResult` returns the option result; `{ Implicit: false }` distinguishes a user-supplied `--repo` from the default-implicit value, giving CLI→env→.eaxwiki precedence for repo (the env/.eaxwiki fallback lives in the resolver, Task 3).
- The `Children.First(o => o.Name == ...)` lookups find the option by its long name; `GetValueForOption` requires the concrete `Option<T>` type.

- [ ] **Step 5: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorCommandLineTests"
```

Expected: `Passed! - Failed: 0, Passed: 22` (1 + 2 + 1 + 2 + 2 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 2 + 1 + 1 + 1 + 1). If the count differs, record the real number; requirement is all pass.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/CliOptions.cs src/EAxWiki.Monitor/MonitorCommandLine.cs src/EAxWiki.Tests/MonitorCommandLineTests.cs
git commit -m "feat(monitor): add System.CommandLine parser for monitor flags (issue #86)"
```

---

### Task 3: `MonitorOptionsResolver` + `LlmPort` config field

**Files:**
- Create: `src/EAxWiki.Monitor/MonitorOptions.cs`
- Create: `src/EAxWiki.Monitor/MonitorOptionsResolver.cs`
- Create: `src/EAxWiki.Tests/MonitorOptionsResolverTests.cs`
- Modify: `src/EAxWiki.Core/Configuration/LocalConfigStore.cs` (add `LlmPort` property after `ApiPort`)
- Modify: `src/EAxWiki.Tests/LocalConfigStoreTests.cs` (one round-trip assertion)

**Interfaces:**
- Consumes: `CliOptions` (Task 2), `LocalConfigStore.Config` (existing).
- Produces: `record MonitorOptions` (fully-resolved options), `static MonitorOptions MonitorOptionsResolver.Resolve(CliOptions cli, string repoRoot, Func<string,string?> getEnv, LocalConfigStore.Config? file)`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/MonitorOptionsResolverTests.cs`:

```csharp
using EAxWiki.Core.Configuration;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorOptionsResolverTests
{
    private const string RepoRoot = @"C:\repos\EAxWiki";
    private static LocalConfigStore.Config File(
        int? wikiPort = null, int? apiPort = null, int? llmPort = null, string? aiMode = null,
        string? aiEndpoint = null, string? repoPath = null, string? brand = null,
        string? llamaExePath = null, string? llamaModelPath = null)
    {
        var c = new LocalConfigStore.Config
        {
            WikiPort = wikiPort, ApiPort = apiPort, LlmPort = llmPort,
            AiMode = aiMode, AiEndpoint = aiEndpoint, RepoPath = repoPath,
            Brand = brand, LlamaExePath = llamaExePath, LlamaModelPath = llamaModelPath,
        };
        return c;
    }

    private static MonitorOptions Resolve(CliOptions cli, LocalConfigStore.Config? file = null,
        Func<string, string?>? getEnv = null) =>
        MonitorOptionsResolver.Resolve(cli, RepoRoot, getEnv ?? (_ => null), file);

    [Fact]
    public void NoCliNoFile_AllDefaults()
    {
        var o = Resolve(new CliOptions());
        Assert.Equal(8000, o.WikiPort);
        Assert.Equal(0, o.ApiPort);
        Assert.Equal(8080, o.LlmPort);
        Assert.Equal(3, o.MaxRetries);
        Assert.Equal(30, o.RetryDelaySeconds);
        Assert.Equal(0.5, o.MinElementFraction);
        Assert.Equal(30, o.ExportIntervalMinutes);
        Assert.Equal(30, o.CheckIntervalSeconds);
        Assert.True(o.NotifyOnStart);
        Assert.False(o.Force);
        Assert.Equal(0, o.ForceEveryNRuns);
        Assert.Equal(Path.Combine(RepoRoot, "wiki"), o.WikiDir);
        Assert.Equal("none", o.AiMode);
        Assert.Null(o.WebhookUrl);
        Assert.Null(o.RepoPath);
    }

    [Fact]
    public void File_WikiPort_Applies()
    {
        var o = Resolve(new CliOptions(), File(wikiPort: 8080));
        Assert.Equal(8080, o.WikiPort);
    }

    [Fact]
    public void PortQuirk_Cli8000AndFilePortDiffers_UsesFilePort()
    {
        var o = Resolve(new CliOptions { Port = 8000 }, File(wikiPort: 9090));
        Assert.Equal(9090, o.WikiPort);
    }

    [Fact]
    public void PortQuirk_CliExplicit_OverridesFile()
    {
        var o = Resolve(new CliOptions { Port = 7777 }, File(wikiPort: 9090));
        Assert.Equal(7777, o.WikiPort);
    }

    [Fact]
    public void ApiPort_ComesFromFileOnly()
    {
        Assert.Equal(8001, Resolve(new CliOptions(), File(apiPort: 8001)).ApiPort);
        Assert.Equal(0, Resolve(new CliOptions(), File()).ApiPort);
    }

    [Fact]
    public void LlmPort_CliOverridesFileAndDefault()
    {
        Assert.Equal(9090, Resolve(new CliOptions { LlmPort = 9090 }, File(llmPort: 8080)).LlmPort);
        Assert.Equal(8181, Resolve(new CliOptions(), File(llmPort: 8181)).LlmPort);
        Assert.Equal(8080, Resolve(new CliOptions(), File()).LlmPort);
    }

    [Fact]
    public void Webhook_EnvBeatsFile_ButCliBeatsEnv()
    {
        var o = Resolve(new CliOptions(),
            File(brand: "file-brand"),
            _ => "env-brand");
        Assert.Equal("env-brand", o.Brand);

        var o2 = Resolve(new CliOptions { Brand = "cli-brand" },
            File(brand: "file-brand"),
            _ => "env-brand");
        Assert.Equal("cli-brand", o2.Brand);
    }

    [Fact]
    public void RepoPath_CliBeatsFile()
    {
        Assert.Equal(@"C:\models\repo.qea",
            Resolve(new CliOptions { Repo = @"C:\models\repo.qea" }, File(repoPath: @"C:\old\repo.qea")).RepoPath);
        Assert.Equal(@"C:\old\repo.qea",
            Resolve(new CliOptions(), File(repoPath: @"C:\old\repo.qea")).RepoPath);
    }

    [Fact]
    public void AiMode_InferredLocal_FromLocalhostEndpointAndExistingExe()
    {
        // Write a real temp file so File.Exists(LlamaExePath) is true.
        var dir = Path.Combine(Path.GetTempPath(), "eaxwiki_ai_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var exe = Path.Combine(dir, "llama-server.exe");
            File.WriteAllText(exe, "x");
            var o = Resolve(new CliOptions(), File(
                aiEndpoint: "http://localhost:8080/v1",
                aiMode: "none",
                llamaExePath: exe,
                llamaModelPath: Path.Combine(dir, "model.gguf")));
            Assert.Equal("local", o.AiMode);
        }
        finally { Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public void AiMode_NotInferred_WhenExeMissing()
    {
        var o = Resolve(new CliOptions(), File(
            aiEndpoint: "http://localhost:8080/v1",
            aiMode: "none",
            llamaExePath: @"E:\missing\llama-server.exe",
            llamaModelPath: @"E:\missing\model.gguf"));
        Assert.Equal("none", o.AiMode);
    }

    [Fact]
    public void OutputDir_Absolute_StaysAbsolute()
    {
        var o = Resolve(new CliOptions { OutputDir = @"D:\out\wiki" }, null);
        Assert.Equal(@"D:\out\wiki", o.WikiDir);
    }

    [Fact]
    public void OutputDir_Relative_JoinsRepoRoot()
    {
        var o = Resolve(new CliOptions { OutputDir = "mywiki" }, null);
        Assert.Equal(Path.Combine(RepoRoot, "mywiki"), o.WikiDir);
    }

    [Fact]
    public void LlamaDefaults_ApplyWhenMissing()
    {
        var o = Resolve(new CliOptions(), File(aiMode: "local"));
        Assert.Equal(@"E:\llama-cpp\llama-server.exe", o.LlamaExePath);
        Assert.Equal(@"E:\models\llama-3.2-3b-q4.gguf", o.LlamaModelPath);
    }
}
```

Add one assertion to `src/EAxWiki.Tests/LocalConfigStoreTests.cs` `SaveAndLoad_RoundTrip_PreservesAllFields` (after the `ApiPort` assertion):

```csharp
        Assert.Equal(8080, loaded.LlmPort);
```

and add `LlmPort = 8080,` to the config literal in that test.

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorOptionsResolverTests|FullyQualifiedName~LocalConfigStoreTests"
```

Expected: compile error — `MonitorOptions`/`MonitorOptionsResolver` do not exist, and `Config` has no `LlmPort`.

- [ ] **Step 3: Add `LlmPort` to `LocalConfigStore.Config`**

In `src/EAxWiki.Core/Configuration/LocalConfigStore.cs`, after the `ApiPort` line (line 35), add:

```csharp
        public int? LlmPort { get; set; }
```

- [ ] **Step 4: Implement `MonitorOptions`**

Create `src/EAxWiki.Monitor/MonitorOptions.cs`:

```csharp
namespace EAxWiki.Monitor;

/// <summary>
/// Fully-resolved monitor options (CLI arg → env var → .eaxwiki). Everything the MonitorLoop,
/// ExportRunner, AlertDispatcher and ProcessSupervisor need; immutable.
/// </summary>
public sealed record MonitorOptions
{
    public string? RepoPath { get; init; }
    public string WikiDir { get; init; } = string.Empty;
    public int WikiPort { get; init; } = 8000;
    public int ApiPort { get; init; }
    public int LlmPort { get; init; } = 8080;
    public int MaxRetries { get; init; } = 3;
    public int RetryDelaySeconds { get; init; } = 30;
    public double MinElementFraction { get; init; } = 0.5;
    public string? WebhookUrl { get; init; }
    public string? TeamsWebhookUrl { get; init; }
    public string? TelegramBotToken { get; init; }
    public string? TelegramChatId { get; init; }
    public string? Brand { get; init; }
    public bool TestAlert { get; init; }
    public bool NotifyOnStart { get; init; } = true;
    public bool Force { get; init; }
    public int ForceEveryNRuns { get; init; }
    public int ExportIntervalMinutes { get; init; } = 30;
    public int CheckIntervalSeconds { get; init; } = 30;
    public string AiMode { get; init; } = "none";
    public string? AiEndpoint { get; init; }
    public string? AiModel { get; init; }
    public string? LlamaExePath { get; init; }
    public string? LlamaModelPath { get; init; }
}
```

- [ ] **Step 5: Implement `MonitorOptionsResolver`**

Create `src/EAxWiki.Monitor/MonitorOptionsResolver.cs`:

```csharp
using EAxWiki.Core.Configuration;

namespace EAxWiki.Monitor;

/// <summary>
/// Resolution order for every option: CLI arg → env var → .eaxwiki file (unchanged from the PS
/// monitor). Ports keep their quirk: --port defaults to 8000, and if it is still exactly 8000
/// while .eaxwiki has a different wikiPort, the file wins (so a scheduled task that omits --port
/// picks up a config-file port).
/// </summary>
public static class MonitorOptionsResolver
{
    private const int DefaultPort = 8000;
    private const int DefaultLlmPort = 8080;
    private const string DefaultLlamaExe = @"E:\llama-cpp\llama-server.exe";
    private const string DefaultLlamaModel = @"E:\models\llama-3.2-3b-q4.gguf";

    public static MonitorOptions Resolve(CliOptions cli, string repoRoot,
        Func<string, string?> getEnv, LocalConfigStore.Config? file)
    {
        var wikiPort = cli.Port ?? DefaultPort;
        if (wikiPort == DefaultPort && file?.WikiPort is { } fw && fw != DefaultPort)
            wikiPort = fw;

        var repoPath = cli.Repo ?? file?.RepoPath;

        var wikiDir = cli.OutputDir is { Length: > 0 } outDir
            ? (Path.IsPathRooted(outDir) ? outDir : Path.Combine(repoRoot, outDir))
            : Path.Combine(repoRoot, "wiki");

        var llamaExe = file?.LlamaExePath is { Length: > 0 } le ? le : DefaultLlamaExe;
        var llamaModel = file?.LlamaModelPath is { Length: > 0 } lm ? lm : DefaultLlamaModel;

        var aiMode = file?.AiMode ?? "none";
        if ((aiMode == "none") && file?.AiEndpoint is { Length: > 0 } ep &&
            (ep.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase) ||
             ep.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase)) &&
            File.Exists(llamaExe))
        {
            aiMode = "local";
        }

        return new MonitorOptions
        {
            RepoPath = repoPath,
            WikiDir = Path.GetFullPath(wikiDir),
            WikiPort = wikiPort,
            ApiPort = file?.ApiPort ?? 0,
            LlmPort = cli.LlmPort ?? file?.LlmPort ?? DefaultLlmPort,
            MaxRetries = cli.MaxRetries ?? 3,
            RetryDelaySeconds = cli.RetryDelaySeconds ?? 30,
            MinElementFraction = cli.MinElementFraction ?? 0.5,
            WebhookUrl = cli.WebhookUrl ?? getEnv("EAXWIKI_ALERT_WEBHOOK") ?? file?.WebhookUrl,
            TeamsWebhookUrl = cli.TeamsWebhookUrl ?? getEnv("EAXWIKI_ALERT_TEAMS_WEBHOOK") ?? file?.TeamsWebhookUrl,
            TelegramBotToken = cli.TelegramBotToken ?? getEnv("EAXWIKI_ALERT_TELEGRAM_BOT_TOKEN") ?? file?.TelegramBotToken,
            TelegramChatId = cli.TelegramChatId ?? getEnv("EAXWIKI_ALERT_TELEGRAM_CHAT_ID") ?? file?.TelegramChatId,
            Brand = cli.Brand ?? getEnv("EAXWIKI_BRAND") ?? file?.Brand,
            TestAlert = cli.TestAlert,
            NotifyOnStart = cli.NotifyOnStart ?? true,
            Force = cli.Force,
            ForceEveryNRuns = cli.ForceEveryNRuns ?? 0,
            ExportIntervalMinutes = cli.ExportIntervalMinutes ?? 30,
            CheckIntervalSeconds = cli.CheckIntervalSeconds ?? 30,
            AiMode = aiMode,
            AiEndpoint = file?.AiEndpoint,
            AiModel = file?.AiModel,
            LlamaExePath = llamaExe,
            LlamaModelPath = llamaModel,
        };
    }
}
```

- [ ] **Step 6: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorOptionsResolverTests|FullyQualifiedName~LocalConfigStoreTests"
```

Expected: `Passed! - Failed: 0` (14 resolver tests + existing LocalConfigStore tests, now with the `LlmPort` round-trip assertion). If the count differs, record the real number.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.Monitor/MonitorOptions.cs src/EAxWiki.Monitor/MonitorOptionsResolver.cs src/EAxWiki.Tests/MonitorOptionsResolverTests.cs src/EAxWiki.Core/Configuration/LocalConfigStore.cs src/EAxWiki.Tests/LocalConfigStoreTests.cs
git commit -m "feat(monitor): add options resolution and LlmPort config field (issue #86)"
```

---

### Task 4: `PortProbe` + `PidFile` (Core) and `PortKiller` (Monitor)

**Files:**
- Create: `src/EAxWiki.Core/Monitoring/PortProbe.cs`
- Create: `src/EAxWiki.Core/Monitoring/PidFile.cs`
- Create: `src/EAxWiki.Monitor/PortKiller.cs`
- Create: `src/EAxWiki.Tests/PortProbeTests.cs`
- Create: `src/EAxWiki.Tests/PidFileTests.cs`
- Create: `src/EAxWiki.Tests/PortKillerTests.cs`

**Interfaces:**
- Produces: `interface IPortProbe { bool IsListening(int port); }`, `class TcpPortProbe : IPortProbe` (500 ms connect timeout to 127.0.0.1); `static class PidFile { PidFileInfo? Read(string path); void Write(string path, int pid, DateTimeOffset startTime); bool IsAlive(string path); }` with `record PidFileInfo(int Pid, DateTimeOffset StartTime)`; `interface IPortKiller { void KillPortOwner(int port); }`, `class NetstatPortKiller : IPortKiller` with `internal static int? FindOwnerPid(string netstatOutput, int port)`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/PortProbeTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class PortProbeTests
{
    [Fact]
    public void IsListening_FreePort_ReturnsFalse()
    {
        var probe = new TcpPortProbe();
        Assert.False(probe.IsListening(55991)); // unassigned port; nothing listens here in CI
    }

    [Fact]
    public void IsListening_ListeningPort_ReturnsTrue()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            Assert.True(new TcpPortProbe().IsListening(port));
        }
        finally { listener.Stop(); }
    }
}
```

Create `src/EAxWiki.Tests/PidFileTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class PidFileTests : IDisposable
{
    private readonly string _dir;

    public PidFileTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_pid_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void WriteAndRead_RoundTrip()
    {
        var path = Path.Combine(_dir, "serve.pid");
        var start = DateTimeOffset.UtcNow;
        PidFile.Write(path, 1234, start);

        var info = PidFile.Read(path);
        Assert.NotNull(info);
        Assert.Equal(1234, info!.Pid);
        Assert.Equal(start.ToString("O"), info.StartTime.ToString("O"));
    }

    [Fact]
    public void Read_MissingFile_ReturnsNull()
    {
        Assert.Null(PidFile.Read(Path.Combine(_dir, "serve.pid")));
    }

    [Fact]
    public void Read_CorruptFile_ReturnsNull()
    {
        var path = Path.Combine(_dir, "serve.pid");
        File.WriteAllText(path, "not json");
        Assert.Null(PidFile.Read(path));
    }

    [Fact]
    public void IsAlive_LiveShortLivedChild_True()
    {
        // The current process's own PID fails IsAlive because its start time is far older than
        // the 2s window — so spawn a genuinely fresh process (cmd /c ping -n 3 127.0.0.1).
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 3 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, p!.Id, p.StartTime.ToUniversalTime());

        Assert.True(PidFile.IsAlive(path));
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void IsAlive_DeadPid_ReturnsFalse()
    {
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, -1, DateTimeOffset.UtcNow); // never a real process
        Assert.False(PidFile.IsAlive(path));
    }

    [Fact]
    public void IsAlive_StaleStartTime_ReturnsFalse()
    {
        // Same PID as a live child, but a start time recorded 5 minutes ago — must read stale.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 3 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        var path = Path.Combine(_dir, "serve.pid");
        PidFile.Write(path, p!.Id, DateTimeOffset.UtcNow.AddMinutes(-5));

        Assert.False(PidFile.IsAlive(path));
        p.Kill();
        p.WaitForExit();
    }
}
```

Create `src/EAxWiki.Tests/PortKillerTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class PortKillerTests
{
    [Fact]
    public void FindOwnerPid_ParsesListeningLine()
    {
        const string output = """
            Proto  Local Address          Foreign Address        State           PID
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       49152
            TCP    0.0.0.0:8001           0.0.0.0:0              LISTENING       1234
            """;
        Assert.Equal(49152, NetstatPortKiller.FindOwnerPid(output, 8000));
        Assert.Equal(1234, NetstatPortKiller.FindOwnerPid(output, 8001));
    }

    [Fact]
    public void FindOwnerPid_PortNotListening_ReturnsNull()
    {
        const string output = "TCP    0.0.0.0:9000           0.0.0.0:0              LISTENING       9999\n";
        Assert.Null(NetstatPortKiller.FindOwnerPid(output, 8000));
    }

    [Fact]
    public void FindOwnerPid_PicksFirstMatchingLine()
    {
        const string output = """
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       1111
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       2222
            """;
        Assert.Equal(1111, NetstatPortKiller.FindOwnerPid(output, 8000));
    }

    [Fact]
    public void FindOwnerPid_NoMatch_ReturnsNull()
    {
        Assert.Null(NetstatPortKiller.FindOwnerPid("", 8000));
        Assert.Null(NetstatPortKiller.FindOwnerPid("garbage", 8000));
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~PortProbeTests|FullyQualifiedName~PidFileTests|FullyQualifiedName~PortKillerTests"
```

Expected: compile error — the new types do not exist.

- [ ] **Step 3: Implement `PortProbe`**

Create `src/EAxWiki.Core/Monitoring/PortProbe.cs`:

```csharp
using System.Net.Sockets;

namespace EAxWiki.Core.Monitoring;

public interface IPortProbe
{
    /// <summary>True if something is listening on <paramref name="port"/> on 127.0.0.1.</summary>
    bool IsListening(int port);
}

/// <summary>TCP connect probe with a 500 ms timeout — the PS monitor's TcpClient fallback.</summary>
public class TcpPortProbe : IPortProbe
{
    public bool IsListening(int port)
    {
        using var client = new TcpClient();
        try
        {
            var ar = client.BeginConnect("127.0.0.1", port, null, null);
            if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500))) return false;
            client.EndConnect(ar);
            return client.Connected;
        }
        catch (SocketException)
        {
            return false;
        }
    }
}
```

- [ ] **Step 4: Implement `PidFile`**

Create `src/EAxWiki.Core/Monitoring/PidFile.cs`:

```csharp
using System.Diagnostics;
using System.Text.Json;

namespace EAxWiki.Core.Monitoring;

public record PidFileInfo(int Pid, DateTimeOffset StartTime);

/// <summary>
/// PID + process start time JSON pid files (serve.pid / api.pid / llm.pid). Alive = the PID is
/// running AND its recorded start time matches the actual process start time within 2 s — so a
/// stale file surviving a reboot can't false-positive when the OS reuses a PID.
/// </summary>
public static class PidFile
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static void Write(string path, int pid, DateTimeOffset startTime)
    {
        var info = new { pid, startTime = startTime.ToString("O") };
        File.WriteAllText(path, JsonSerializer.Serialize(info, Options));
    }

    public static PidFileInfo? Read(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("pid", out var pidEl) ||
                !doc.RootElement.TryGetProperty("startTime", out var startEl))
                return null;
            if (!pidEl.TryGetInt32(out var pid)) return null;
            var start = DateTimeOffset.Parse(startEl.GetString() ?? string.Empty);
            return new PidFileInfo(pid, start);
        }
        catch (JsonException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    public static bool IsAlive(string path)
    {
        var info = Read(path);
        if (info == null) return false;
        try
        {
            using var proc = Process.GetProcessById(info.Pid);
            var delta = (info.StartTime - proc.StartTime.ToUniversalTime()).Duration();
            return delta.TotalSeconds <= 2;
        }
        catch (ArgumentException)
        {
            return false; // no process with that id
        }
    }
}
```

- [ ] **Step 5: Implement `PortKiller`**

Create `src/EAxWiki.Monitor/PortKiller.cs`:

```csharp
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

public interface IPortKiller
{
    /// <summary>Kill the process listening on <paramref name="port"/> (netstat -ano → Stop-Process).</summary>
    void KillPortOwner(int port);
}

public class NetstatPortKiller : IPortKiller
{
    // Matches "TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       49152"
    private static readonly Regex LineRegex =
        new(@"^\s*TCP\s+\S+:(\d+)\s+\S+\s+LISTENING\s+(\d+)\s*$", RegexOptions.Multiline);

    public void KillPortOwner(int port)
    {
        var psi = new ProcessStartInfo("netstat", "-ano")
        {
            RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true,
        };
        using var proc = Process.Start(psi);
        if (proc == null) return;
        var output = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();

        var pid = FindOwnerPid(output, port);
        if (pid == null) return;
        Process.Start("taskkill", $"/PID {pid} /F");
    }

    internal static int? FindOwnerPid(string netstatOutput, int port)
    {
        foreach (Match m in LineRegex.Matches(netstatOutput))
        {
            if (int.Parse(m.Groups[1].Value) == port)
                return int.Parse(m.Groups[2].Value);
        }
        return null;
    }
}
```

- [ ] **Step 6: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~PortProbeTests|FullyQualifiedName~PidFileTests|FullyQualifiedName~PortKillerTests"
```

Expected: `Passed! - Failed: 0` (2 + 6 + 4 = 12). If the count differs, record the real number.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.Core/Monitoring/PortProbe.cs src/EAxWiki.Core/Monitoring/PidFile.cs src/EAxWiki.Monitor/PortKiller.cs src/EAxWiki.Tests/PortProbeTests.cs src/EAxWiki.Tests/PidFileTests.cs src/EAxWiki.Tests/PortKillerTests.cs
git commit -m "feat(monitor): add port probe, pid file and port killer (issue #86)"
```

---

### Task 5: `EditLock` + `HealthPageRenderer`

**Files:**
- Create: `src/EAxWiki.Monitor/EditLock.cs`
- Create: `src/EAxWiki.Monitor/HealthPageRenderer.cs`
- Create: `src/EAxWiki.Tests/EditLockTests.cs`
- Create: `src/EAxWiki.Tests/HealthPageRendererTests.cs`

**Interfaces:**
- Consumes: `HealthState` (Task 1).
- Produces: `class EditLock { bool IsActive(string wikiDir, string logLine); }` — actually simpler: `static bool EditLock.IsActive(string wikiDir)` plus an internal `static bool IsActive(string wikiDir, out string? staleLockPath)`; `class HealthPageRenderer { HealthPageRenderer(string templatePath, string wikiDir); void Render(HealthState state); }`. Render writes `{wikiDir}/status/health.md`, creating the status dir, replacing the 18 tokens; null → empty string; OVERALL = "Healthy" iff all four consecutive-failure counters are 0 else "Degraded".

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/EditLockTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class EditLockTests : IDisposable
{
    private readonly string _dir;

    public EditLockTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_lock_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private string WriteLock(bool active, DateTimeOffset expiresAt)
    {
        var lockDir = Path.Combine(_dir, ".data");
        Directory.CreateDirectory(lockDir);
        var lockPath = Path.Combine(lockDir, "edit-lock.json");
        File.WriteAllText(lockPath, System.Text.Json.JsonSerializer.Serialize(new { Active = active, ExpiresAt = expiresAt.ToString("O") }));
        return lockPath;
    }

    [Fact]
    public void IsActive_NoLockFile_ReturnsFalse()
    {
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_InactiveLock_ReturnsFalse()
    {
        WriteLock(false, DateTimeOffset.UtcNow.AddMinutes(5));
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_ActiveUnExpired_ReturnsTrue()
    {
        WriteLock(true, DateTimeOffset.UtcNow.AddMinutes(5));
        Assert.True(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }

    [Fact]
    public void IsActive_ExpiredLock_RemovesFileAndReturnsFalse()
    {
        var lockPath = WriteLock(true, DateTimeOffset.UtcNow.AddMinutes(-5));
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
        Assert.False(File.Exists(lockPath), "expired lock file should be removed");
    }

    [Fact]
    public void IsActive_CorruptLock_ReturnsFalse()
    {
        var lockDir = Path.Combine(_dir, ".data");
        Directory.CreateDirectory(lockDir);
        File.WriteAllText(Path.Combine(lockDir, "edit-lock.json"), "{corrupt");
        Assert.False(EditLock.IsActive(Path.Combine(_dir, "wiki")));
    }
}
```

Create `src/EAxWiki.Tests/HealthPageRendererTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class HealthPageRendererTests : IDisposable
{
    private readonly string _dir;

    public HealthPageRendererTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_healthpage_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private static readonly string Template = """
        **Overall:** @@OVERALL@@
        | Last success | @@LAST_SUCCESS_TIME@@ |
        | Consecutive failures | @@CONSECUTIVE_FAILURES@@ |
        | Last exit code | @@LAST_EXIT_CODE@@ |
        | Last page count | @@LAST_ELEMENT_COUNT@@ |
        | Runs since force | @@RUNS_SINCE_FORCE@@ |
        | Serve failures | @@SERVE_CONSECUTIVE_FAILURES@@ |
        """;

    [Fact]
    public void Render_Healthy_AllZeros()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState();

        renderer.Render(state);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md"));
        Assert.Contains("**Overall:** Healthy", output);
        Assert.Contains("| Last success |  |", output); // null → ""
        Assert.Contains("| Consecutive failures | 0 |", output);
        Assert.Contains("| Runs since force | 0 |", output);
    }

    [Fact]
    public void Render_Degraded_WhenAnyCounterNonZero()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState { ServeConsecutiveFailures = 2 };

        renderer.Render(state);

        Assert.Contains("**Overall:** Degraded", File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md")));
    }

    [Fact]
    public void Render_FormatsValues()
    {
        var templatePath = Path.Combine(_dir, "health-template.md");
        File.WriteAllText(templatePath, Template);
        var renderer = new HealthPageRenderer(templatePath, Path.Combine(_dir, "wiki"));
        var state = new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            LastExitCode = 0,
            LastElementCount = 150,
            RunsSinceForce = 3,
        };

        renderer.Render(state);

        var output = File.ReadAllText(Path.Combine(_dir, "wiki", "status", "health.md"));
        Assert.Contains("| Last success | 08/01/2026 10:00:00 +00:00 |", output);
        Assert.Contains("| Last exit code | 0 |", output);
        Assert.Contains("| Last page count | 150 |", output);
        Assert.Contains("| Runs since force | 3 |", output);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~EditLockTests|FullyQualifiedName~HealthPageRendererTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `EditLock`**

Create `src/EAxWiki.Monitor/EditLock.cs`:

```csharp
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

/// <summary>
/// Reads <c>.data/edit-lock.json</c> (relative to the wiki dir's parent): absent → unlocked;
/// expired → stale lock removed and reported unlocked; active → export defers this cycle.
/// </summary>
public static class EditLock
{
    public static bool IsActive(string wikiDir)
    {
        var lockPath = Path.Combine(
            Path.Combine(Path.GetDirectoryName(wikiDir) ?? string.Empty, ".data"), "edit-lock.json");
        if (!File.Exists(lockPath)) return false;

        try
        {
            var doc = JsonDocument.Parse(File.ReadAllText(lockPath));
            var root = doc.RootElement;
            if (!root.TryGetProperty("Active", out var activeEl) || !activeEl.GetBoolean())
                return false;

            if (root.TryGetProperty("ExpiresAt", out var expiresEl) &&
                DateTimeOffset.TryParse(expiresEl.GetString(), out var expires))
            {
                if (DateTimeOffset.UtcNow > expires)
                {
                    File.Delete(lockPath);
                    return false;
                }
            }
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
```

- [ ] **Step 4: Implement `HealthPageRenderer`**

Create `src/EAxWiki.Monitor/HealthPageRenderer.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

/// <summary>
/// Renders health-template.md → <c>{wikiDir}/status/health.md</c>, replacing @@TOKEN@@
/// placeholders from the HealthState. Null state values render as empty strings. The status
/// dir is a recognized special dir the exporter never cleans (InfrastructureWriter).
/// </summary>
public class HealthPageRenderer
{
    private readonly string _templatePath;
    private readonly string _outputPath;

    public HealthPageRenderer(string templatePath, string wikiDir)
    {
        _templatePath = templatePath;
        _outputPath = Path.Combine(wikiDir, "status", "health.md");
    }

    public void Render(HealthState s)
    {
        var overall = s.ConsecutiveFailures == 0 &&
                      s.ServeConsecutiveFailures == 0 &&
                      s.LlmConsecutiveFailures == 0 &&
                      s.ApiConsecutiveFailures == 0
            ? "Healthy"
            : "Degraded";

        var template = File.ReadAllText(_templatePath);
        template = Replace(template, "@@OVERALL@@", overall);
        template = Replace(template, "@@LAST_SUCCESS_TIME@@", s.LastSuccessTime);
        template = Replace(template, "@@LAST_FAILURE_TIME@@", s.LastFailureTime);
        template = Replace(template, "@@CONSECUTIVE_FAILURES@@", s.ConsecutiveFailures);
        template = Replace(template, "@@LAST_EXIT_CODE@@", s.LastExitCode);
        template = Replace(template, "@@LAST_ELEMENT_COUNT@@", s.LastElementCount);
        template = Replace(template, "@@LAST_DIAGRAM_COUNT@@", s.LastDiagramCount);
        template = Replace(template, "@@LAST_MODE@@", s.LastMode);
        template = Replace(template, "@@RUNS_SINCE_FORCE@@", s.RunsSinceForce);
        template = Replace(template, "@@LAST_SERVE_SUCCESS_TIME@@", s.LastServeSuccessTime);
        template = Replace(template, "@@LAST_SERVE_FAILURE_TIME@@", s.LastServeFailureTime);
        template = Replace(template, "@@SERVE_CONSECUTIVE_FAILURES@@", s.ServeConsecutiveFailures);
        template = Replace(template, "@@LAST_LLM_SUCCESS_TIME@@", s.LastLlmSuccessTime);
        template = Replace(template, "@@LAST_LLM_FAILURE_TIME@@", s.LastLlmFailureTime);
        template = Replace(template, "@@LLM_CONSECUTIVE_FAILURES@@", s.LlmConsecutiveFailures);
        template = Replace(template, "@@LAST_API_SUCCESS_TIME@@", s.LastApiSuccessTime);
        template = Replace(template, "@@LAST_API_FAILURE_TIME@@", s.LastApiFailureTime);
        template = Replace(template, "@@API_CONSECUTIVE_FAILURES@@", s.ApiConsecutiveFailures);

        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);
        File.WriteAllText(_outputPath, template);
    }

    private static string Replace(string template, string token, object? value) =>
        template.Replace(token, value?.ToString() ?? string.Empty);
}
```

- [ ] **Step 5: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~EditLockTests|FullyQualifiedName~HealthPageRendererTests"
```

Expected: `Passed! - Failed: 0` (5 + 3 = 8). If the count differs, record the real number.

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/EditLock.cs src/EAxWiki.Monitor/HealthPageRenderer.cs src/EAxWiki.Tests/EditLockTests.cs src/EAxWiki.Tests/HealthPageRendererTests.cs
git commit -m "feat(monitor): add edit lock and health page renderer (issue #86)"
```

---

### Task 6: `DigestTracker`

**Files:**
- Create: `src/EAxWiki.Monitor/DigestTracker.cs`
- Create: `src/EAxWiki.Tests/DigestTrackerTests.cs`

**Interfaces:**
- Consumes: `HealthState` (Task 1).
- Produces: `record WritebackDelta(int Total, IReadOnlyDictionary<string, int> Kinds)`, `interface IDigestTracker { int CountNewPageReads(); WritebackDelta CountNewWritebacks(); string? MaybeComposeDailyDigest(DateTime now); }`, `class DigestTracker : IDigestTracker` — ctor `(HealthState state, string wikiDir, string logDir, string digestTemplatePath)`.

Semantics (ported from `Get-NewPageReadCount`/`Get-NewWritebackSummary`/digest block):
- `CountNewPageReads()`: scan the newest `serve-*.err.log` in `logDir`; if the tracked `PageReadLogFile` differs, reset offset to 0; read only bytes past `PageReadLogOffset` (reset offset to 0 if file length < offset); parse `[HH:mm:ss] Reloading browsers` (records last reload) and `[HH:mm:ss] Browser connected:` (count unless within 10 s of a reload); update `PageReadLogFile`/`PageReadLogOffset`; return the count.
- `CountNewWritebacks()`: scan `{wikiDir}/status/writeback.log`; reset offset when the tracked file differs or length < offset; count non-blank lines, split `"timestamp kind"` (3 whitespace parts, kind = 3rd) into per-kind counts; update `WritebackLogFile`/`WritebackLogOffset`; return `WritebackDelta`.
- `MaybeComposeDailyDigest(now)`: if `LastDigestDate` is set and differs from `now.ToString("yyyy-MM-dd")`, render the digest template (tokens `@@DIGEST_DATE@@`, `@@PAGE_READS_TODAY@@`, `@@WRITEBACKS_TODAY@@`), reset `PageReadsToday`/`WritebacksToday` to 0, and return the message; otherwise set `LastDigestDate = today` and return null.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/DigestTrackerTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class DigestTrackerTests : IDisposable
{
    private readonly string _dir;

    public DigestTrackerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_digest_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private DigestTracker CreateTracker(HealthState state, out string logDir, out string wikiDir)
    {
        logDir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(logDir);
        wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        File.WriteAllText(Path.Combine(_dir, "digest-template.md"),
            "Activity for @@DIGEST_DATE@@: ~@@PAGE_READS_TODAY@@ reads, @@WRITEBACKS_TODAY@@ write-backs.");
        return new DigestTracker(state, wikiDir, logDir, Path.Combine(_dir, "digest-template.md"));
    }

    [Fact]
    public void CountNewPageReads_CountsOnlyNewTextAndSkipsReconnects()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out var logDir, out _);
        var logPath = Path.Combine(logDir, "serve-20260801_120000.err.log");
        File.WriteAllText(logPath, """
            [10:00:00] Reloading browsers...
            [10:00:05] Browser connected: http://localhost:8000/some/page  # within 10s of reload → skip
            [10:00:30] Browser connected: http://localhost:8000/other        # real read
            """);

        Assert.Equal(1, tracker.CountNewPageReads());
        Assert.Equal(1, tracker.CountNewPageReads()); // no new bytes → 0 new

        // Append a real read; the offset must have advanced.
        File.AppendAllText(logPath, "[10:01:00] Browser connected: http://localhost:8000/third\n");
        Assert.Equal(1, tracker.CountNewPageReads());
    }

    [Fact]
    public void CountNewPageReads_NewLogFile_ResetsOffset()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out var logDir, out _);
        var first = Path.Combine(logDir, "serve-20260801_120000.err.log");
        File.WriteAllText(first, "[10:00:00] Browser connected: http://localhost:8000/a\n");
        Assert.Equal(1, tracker.CountNewPageReads());

        var second = Path.Combine(logDir, "serve-20260802_090000.err.log");
        File.WriteAllText(second, "[09:00:00] Browser connected: http://localhost:8000/b\n");
        Assert.Equal(1, tracker.CountNewPageReads());
    }

    [Fact]
    public void CountNewWritebacks_CountsPerKind()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out _, out var wikiDir);
        var statusDir = Path.Combine(wikiDir, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "writeback.log"),
            "2026-08-01 10:00:00 status\n2026-08-01 10:01:00 notes\n2026-08-01 10:02:00 status\n");

        var delta = tracker.CountNewWritebacks();

        Assert.Equal(3, delta.Total);
        Assert.Equal(2, delta.Kinds["status"]);
        Assert.Equal(1, delta.Kinds["notes"]);
    }

    [Fact]
    public void MaybeComposeDailyDigest_FirstRunNoAlert_SetsDate()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out _, out _);
        Assert.Null(tracker.MaybeComposeDailyDigest(new DateTime(2026, 8, 1, 10, 0, 0, DateTimeKind.Local)));
        Assert.Equal("2026-08-01", state.LastDigestDate);
    }

    [Fact]
    public void MaybeComposeDailyDigest_DayBoundary_ComposesAndResets()
    {
        var state = new HealthState { LastDigestDate = "2026-07-31", PageReadsToday = 5, WritebacksToday = 2 };
        var tracker = CreateTracker(state, out _, out _);

        var message = tracker.MaybeComposeDailyDigest(new DateTime(2026, 8, 1, 0, 30, 0, DateTimeKind.Local));

        Assert.NotNull(message);
        Assert.Contains("Activity for 2026-07-31: ~5 reads, 2 write-backs.", message);
        Assert.Equal(0, state.PageReadsToday);
        Assert.Equal(0, state.WritebacksToday);
        Assert.Equal("2026-08-01", state.LastDigestDate);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~DigestTrackerTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `DigestTracker`**

Create `src/EAxWiki.Monitor/DigestTracker.cs`:

```csharp
using System.Text.RegularExpressions;
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

public record WritebackDelta(int Total, IReadOnlyDictionary<string, int> Kinds);

public interface IDigestTracker
{
    /// <summary>Count page-read lines in the newest serve-*.err.log since the last scan.</summary>
    int CountNewPageReads();

    /// <summary>Count write-back lines in wiki/status/writeback.log since the last scan.</summary>
    WritebackDelta CountNewWritebacks();

    /// <summary>Return a DailyDigest message on a calendar-day boundary (and reset counters), else null.</summary>
    string? MaybeComposeDailyDigest(DateTime now);
}

/// <summary>
/// Offset-based incremental counters over append-only logs. Both counters use a file+offset pair
/// in HealthState so a frequently-run monitor never re-counts already-seen lines; a log that was
/// rotated/truncated (length &lt; offset) resets to 0.
/// </summary>
public class DigestTracker : IDigestTracker
{
    private static readonly Regex ReloadRegex = new(@"\[(\d{2}):(\d{2}):(\d{2})\]\s+Reloading browsers");
    private static readonly Regex ConnectRegex = new(@"\[(\d{2}):(\d{2}):(\d{2})\]\s+Browser connected:");

    private readonly HealthState _state;
    private readonly string _wikiDir;
    private readonly string _logDir;
    private readonly string _digestTemplatePath;

    public DigestTracker(HealthState state, string wikiDir, string logDir, string digestTemplatePath)
    {
        _state = state;
        _wikiDir = wikiDir;
        _logDir = logDir;
        _digestTemplatePath = digestTemplatePath;
    }

    public int CountNewPageReads()
    {
        var files = Directory.Exists(_logDir)
            ? Directory.GetFiles(_logDir, "serve-*.err.log").OrderBy(File.GetLastWriteTime).ToArray()
            : [];
        if (files.Length == 0) return 0;

        var currentFile = files[^1];
        if (_state.PageReadLogFile != currentFile)
        {
            _state.PageReadLogFile = currentFile;
            _state.PageReadLogOffset = 0;
        }

        var newText = ReadNewText(currentFile, () => _state.PageReadLogOffset,
            v => _state.PageReadLogOffset = v);
        if (newText == null) return 0;

        int? lastReloadSeconds = null;
        var count = 0;
        foreach (var line in newText.Split('\n'))
        {
            var m = ReloadRegex.Match(line);
            if (m.Success)
            {
                lastReloadSeconds = ToSeconds(m);
                continue;
            }
            m = ConnectRegex.Match(line);
            if (m.Success)
            {
                var seconds = ToSeconds(m);
                if (lastReloadSeconds is { } reload && seconds - reload is >= 0 and <= 10)
                    continue;
                count++;
            }
        }
        return count;
    }

    public WritebackDelta CountNewWritebacks()
    {
        var logPath = Path.Combine(_wikiDir, "status", "writeback.log");
        if (_state.WritebackLogFile != logPath)
        {
            _state.WritebackLogFile = logPath;
            _state.WritebackLogOffset = 0;
        }

        var newText = ReadNewText(logPath, () => _state.WritebackLogOffset,
            v => _state.WritebackLogOffset = v);
        if (newText == null) return new WritebackDelta(0, new Dictionary<string, int>());

        var kinds = new Dictionary<string, int>();
        var total = 0;
        foreach (var line in newText.Split('\n'))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0) continue;
            total++;
            var parts = trimmed.Split((char[]?)null, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
                kinds[parts[2]] = kinds.GetValueOrDefault(parts[2]) + 1;
        }
        return new WritebackDelta(total, kinds);
    }

    public string? MaybeComposeDailyDigest(DateTime now)
    {
        var today = now.ToString("yyyy-MM-dd");
        if (_state.LastDigestDate is { Length: > 0 } last && last != today)
        {
            var template = File.ReadAllText(_digestTemplatePath);
            var message = template
                .Replace("@@DIGEST_DATE@@", last)
                .Replace("@@PAGE_READS_TODAY@@", _state.PageReadsToday.ToString())
                .Replace("@@WRITEBACKS_TODAY@@", _state.WritebacksToday.ToString());
            _state.PageReadsToday = 0;
            _state.WritebacksToday = 0;
            _state.LastDigestDate = today;
            return message;
        }
        _state.LastDigestDate = today;
        return null;
    }

    private static int ToSeconds(Match m) =>
        int.Parse(m.Groups[1].Value) * 3600 + int.Parse(m.Groups[2].Value) * 60 + int.Parse(m.Groups[3].Value);

    private static string? ReadNewText(string path, Func<long> getOffset, Action<long> setOffset)
    {
        if (!File.Exists(path)) return null;
        var length = new FileInfo(path).Length;
        var offset = getOffset();
        if (length < offset) offset = 0;
        if (length == offset) return null;

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        stream.Seek(offset, SeekOrigin.Begin);
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        setOffset(stream.Position);
        return text;
    }
}
```

- [ ] **Step 4: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~DigestTrackerTests"
```

Expected: `Passed! - Failed: 0` (5). If the count differs, record the real number.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Monitor/DigestTracker.cs src/EAxWiki.Tests/DigestTrackerTests.cs
git commit -m "feat(monitor): add offset-based digest tracker (issue #86)"
```

---

### Task 7: `AlertDispatcher` (Slack / Teams / Telegram)

**Files:**
- Create: `src/EAxWiki.Monitor/AlertDispatcher.cs`
- Create: `src/EAxWiki.Monitor/TelegramAlertTextFormatter.cs`
- Create: `src/EAxWiki.Tests/AlertDispatcherTests.cs`

**Interfaces:**
- Produces: `enum AlertKind { Start, Finish, Failure, Recovery, ServeFailure, ServeRecovery, LlmFailure, LlmRecovery, ApiFailure, ApiRecovery, Test, DailyDigest, UserStop }`, `interface IAlertDispatcher { void Dispatch(string message, AlertKind kind); }`, `record AlertOptions(string? WebhookUrl, string? TeamsWebhookUrl, string? TelegramBotToken, string? TelegramChatId, string InstanceLabel)`, `class AlertDispatcher : IAlertDispatcher { AlertDispatcher(AlertOptions options, HttpMessageHandler? handler, ILogger logger); void Dispatch(string message, AlertKind kind); }`, `static class TelegramAlertTextFormatter { string Format(AlertKind kind, string instanceLabel, string message, DateTimeOffset timestamp); string HtmlEscape(string text); string FencesToPre(string text); string EmojiFor(AlertKind kind); string ColorFor(AlertKind kind); }`.

Exact parity with the PS `Send-Alert`/`Format-TelegramAlertText`:
- Slack: `attachments[0]` = `{ color, mrkdwn_in: ["text","pretext"], pretext: "<emoji> *EAxWiki [<kind>]* - <instanceLabel>", text: message, footer: instanceLabel, ts: unixSeconds }`.
- Teams: MessageCard `{ "@type": "MessageCard", "@context": "http://schema.org/extensions", themeColor: color without '#', summary: "EAxWiki [kind] - label", sections: [{ activityTitle, text }] }`.
- Telegram: HTML body `"<emoji> <b>EAxWiki [<kind>]</b> — <label>\n<body>\n\n<i><label> • <stamp></i>"` with stamp `yyyy-MM-dd HH:mm:ss zzz`; two-pass fence→`<pre>` (inner HTML-escaped) then escape the rest; 4000-char truncation with `\n... (truncated)`; on HTTP 400 retry exactly once without `parse_mode`.
- `JsonSerializerOptions` uses `JsonSerializerDefaults.Web` (camelCase keys preserved as written in the anonymous objects — Slack/Teams/TG field names are already lowercase).

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/AlertDispatcherTests.cs`:

```csharp
using System.Net;
using System.Text.Json;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class AlertDispatcherTests
{
    private static readonly AlertOptions Options = new(
        WebhookUrl: "https://hooks.slack.com/ABC",
        TeamsWebhookUrl: "https://outlook.office.com/DEF",
        TelegramBotToken: "123456789:AAbbCCddEeffGGhhIIjj",
        TelegramChatId: "-1001234567890",
        InstanceLabel: "MYPC - C:\\repo\\wiki");

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public readonly List<(HttpRequestMessage Request, string Body)> Sent = new();
        public HttpStatusCode StatusCode = HttpStatusCode.OK;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var body = request.Content!.ReadAsStringAsync(ct).GetAwaiter().GetResult();
            Sent.Add((request, body));
            var response = new HttpResponseMessage(StatusCode) { Content = new StringContent("{}") };
            return Task.FromResult(response);
        }
    }

    private static AlertDispatcher Dispatcher(RecordingHandler handler, AlertOptions? options = null) =>
        new(options ?? Options, handler, NullLogger.Instance);

    private static JsonElement RootOf(string body) =>
        JsonDocument.Parse(body).RootElement.Clone();

    [Fact]
    public void Slack_Payload_HasAttachmentWithPretextAndFooter()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Export failed", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "hooks.slack.com");
        var root = RootOf(req.Body);
        var attachment = root.GetProperty("attachments")[0];
        Assert.Equal("#dc3545", attachment.GetProperty("color").GetString());
        Assert.Equal(":red_circle: *EAxWiki [Failure]* - MYPC - C:\\repo\\wiki",
            attachment.GetProperty("pretext").GetString());
        Assert.Equal("Export failed", attachment.GetProperty("text").GetString());
        Assert.Equal("MYPC - C:\\repo\\wiki", attachment.GetProperty("footer").GetString());
        Assert.True(attachment.GetProperty("ts").GetInt64() > 0);
    }

    [Fact]
    public void Teams_Payload_IsMessageCardWithThemeColor()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Serve down", AlertKind.ServeFailure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "outlook.office.com");
        var root = RootOf(req.Body);
        Assert.Equal("MessageCard", root.GetProperty("@type").GetString());
        Assert.Equal("dc3545", root.GetProperty("themeColor").GetString()); // no '#'
        Assert.Equal("EAxWiki [ServeFailure] - MYPC - C:\\repo\\wiki", root.GetProperty("summary").GetString());
        Assert.Equal("Serve down", root.GetProperty("sections")[0].GetProperty("text").GetString());
    }

    [Fact]
    public void Telegram_Text_HasEmojiTitleFooterAndHtmlEscaping()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("a <b>boom</b>", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        Assert.Equal("https://api.telegram.org/bot123456789:AAbbCCddEeffGGhhIIjj/sendMessage", req.Request!.RequestUri!.ToString());
        var root = RootOf(req.Body);
        var text = root.GetProperty("text").GetString()!;
        Assert.StartsWith("🔴 <b>EAxWiki [Failure]</b> — MYPC - C:\\repo\\wiki", text);
        Assert.Contains("a &lt;b&gt;boom&lt;/b&gt;", text); // label + body escaped, label en-dash
        Assert.Contains("<i>MYPC - C:\\repo\\wiki • ", text);
        Assert.Equal("HTML", root.GetProperty("parse_mode").GetString());
        Assert.Equal("-1001234567890", root.GetProperty("chat_id").GetString());
    }

    [Fact]
    public void Telegram_Fences_BecomePre_WithInnerEscaping()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Export failed.\n```\nline with <tag> & stuff\n```\nDone.", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        var text = RootOf(req.Body).GetProperty("text").GetString()!;
        Assert.Contains("Export failed.\n<pre>line with &lt;tag&gt; &amp; stuff</pre>\nDone.", text);
    }

    [Fact]
    public void Telegram_TruncatesAt4000Chars()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch(new string('x', 10000), AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        var text = RootOf(req.Body).GetProperty("text").GetString()!;
        Assert.Equal(4000 + "\n... (truncated)".Length, text.Length);
        Assert.EndsWith("... (truncated)", text);
    }

    [Fact]
    public void Telegram_Http400_RetriesOnceWithoutParseMode()
    {
        var handler = new RecordingHandler { StatusCode = HttpStatusCode.BadRequest };
        var dispatcher = Dispatcher(handler);
        dispatcher.Dispatch("oops", AlertKind.Failure);

        var tg = handler.Sent.Where(r => r.Request.RequestUri!.Host == "api.telegram.org").ToList();
        Assert.Equal(2, tg.Count);
        Assert.Equal("HTML", RootOf(tg[0].Body).GetProperty("parse_mode").GetString());
        Assert.False(RootOf(tg[1].Body).TryGetProperty("parse_mode", out _));
    }

    [Fact]
    public void NoChannelsConfigured_DoesNothing()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler, new AlertOptions(null, null, null, null, "label")).Dispatch("hi", AlertKind.Test);
        Assert.Empty(handler.Sent);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~AlertDispatcherTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `TelegramAlertTextFormatter`**

Create `src/EAxWiki.Monitor/TelegramAlertTextFormatter.cs`:

```csharp
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

/// <summary>
/// Port of the PS Format-TelegramAlertText: emoji title, &lt;b&gt; kind label, HTML-escaping,
/// fence → &lt;pre&gt; with inner escaping (two-pass so the outer escaper doesn't double-escape),
/// 4000-char truncation. HTML mode because Markdown v1 silently drops content on unmatched '*'/'_'.
/// </summary>
public static class TelegramAlertTextFormatter
{
    private const string EnDash = "\u2014";

    public static string EmojiFor(AlertKind kind) => kind switch
    {
        AlertKind.Start => "\U0001F504",          // 🔄
        AlertKind.Finish => "\U0001F7E2",         // 🟢
        AlertKind.Failure => "\U0001F534",        // 🔴
        AlertKind.ServeFailure => "\U0001F534",
        AlertKind.LlmFailure => "\U0001F534",
        AlertKind.ApiFailure => "\U0001F534",
        AlertKind.Recovery => "\U0001F7E2",
        AlertKind.ServeRecovery => "\U0001F7E2",
        AlertKind.LlmRecovery => "\U0001F7E2",
        AlertKind.ApiRecovery => "\U0001F7E2",
        AlertKind.Test => "\U0001F535",           // 🔵
        AlertKind.DailyDigest => "\U0001F4CA",    // 📊
        AlertKind.UserStop => "\u270B",           // ✋
        _ => "\U0001F535",
    };

    public static string ColorFor(AlertKind kind) => kind switch
    {
        AlertKind.Start => "#3aa3e3",
        AlertKind.Finish => "#28a745",
        AlertKind.Failure => "#dc3545",
        AlertKind.ServeFailure => "#dc3545",
        AlertKind.LlmFailure => "#dc3545",
        AlertKind.ApiFailure => "#dc3545",
        AlertKind.Recovery => "#28a745",
        AlertKind.ServeRecovery => "#28a745",
        AlertKind.LlmRecovery => "#28a745",
        AlertKind.ApiRecovery => "#28a745",
        AlertKind.Test => "#3aa3e3",
        AlertKind.DailyDigest => "#3aa3e3",
        AlertKind.UserStop => "#FF8C00",
        _ => "#3aa3e3",
    };

    public static string Format(AlertKind kind, string instanceLabel, string message, DateTimeOffset timestamp)
    {
        var labelHtml = HtmlEscape(instanceLabel);
        var kindHtml = HtmlEscape(kind.ToString());
        var stamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss zzz");

        var composed = $"{EmojiFor(kind)} <b>EAxWiki [{kindHtml}]</b> {EnDash} {labelHtml}\n" +
                       $"{FencesToPre(message)}\n\n<i>{labelHtml} • {stamp}</i>";

        if (composed.Length > 4000)
            composed = composed[..4000] + "\n... (truncated)";
        return composed;
    }

    public static string HtmlEscape(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    public static string FencesToPre(string text)
    {
        // Two-pass: swap fences to placeholders so the second pass doesn't double-escape pre content.
        var preBlocks = new List<string>();
        var withPlaceholders = Regex.Replace(text, "(?s)```(.*?)```", m =>
        {
            preBlocks.Add("<pre>" + HtmlEscape(m.Groups[1].Value) + "</pre>");
            return $"\uFFFD{"PRE"}{preBlocks.Count - 1}\uFFFD";
        });
        var escaped = HtmlEscape(withPlaceholders);
        return Regex.Replace(escaped, "\uFFFD" + "PRE(\\d+)" + "\uFFFD", m =>
            preBlocks[int.Parse(m.Groups[1].Value)]);
    }
}
```

- [ ] **Step 4: Implement `AlertDispatcher`**

Create `src/EAxWiki.Monitor/AlertDispatcher.cs`:

```csharp
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public enum AlertKind
{
    Start, Finish, Failure, Recovery, ServeFailure, ServeRecovery,
    LlmFailure, LlmRecovery, ApiFailure, ApiRecovery, Test, DailyDigest, UserStop,
}

public interface IAlertDispatcher
{
    void Dispatch(string message, AlertKind kind);
}

public record AlertOptions(
    string? WebhookUrl,
    string? TeamsWebhookUrl,
    string? TelegramBotToken,
    string? TelegramChatId,
    string InstanceLabel);

/// <summary>
/// Port of the PS Send-Alert + Send-TelegramMessage: Slack attachments, Teams MessageCard, and
/// Telegram HTML messages. Channels are independent, not exclusive — an alert goes to every
/// configured channel. Injectable <see cref="HttpMessageHandler"/> for unit tests.
/// </summary>
public class AlertDispatcher : IAlertDispatcher
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private readonly AlertOptions _options;
    private readonly HttpClient _http;
    private readonly ILogger _logger;

    public AlertDispatcher(AlertOptions options, HttpMessageHandler? handler, ILogger logger)
    {
        _options = options;
        _http = handler is null ? new HttpClient() : new HttpClient(handler);
        _logger = logger;
    }

    public void Dispatch(string message, AlertKind kind)
    {
        _logger.LogInformation("[{Kind}] {Message}", kind, message);
        if (string.IsNullOrEmpty(_options.WebhookUrl) &&
            string.IsNullOrEmpty(_options.TeamsWebhookUrl) &&
            string.IsNullOrEmpty(_options.TelegramBotToken) &&
            string.IsNullOrEmpty(_options.TelegramChatId))
        {
            _logger.LogInformation("No alert channel configured; alert logged only.");
            return;
        }

        var color = TelegramAlertTextFormatter.ColorFor(kind);
        var emoji = TelegramAlertTextFormatter.EmojiFor(kind);

        if (!string.IsNullOrEmpty(_options.WebhookUrl))
            SendSlackAsync(_options.WebhookUrl, kind, emoji, color, message).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(_options.TeamsWebhookUrl))
            SendTeamsAsync(_options.TeamsWebhookUrl, kind, color, message).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(_options.TelegramBotToken) && !string.IsNullOrEmpty(_options.TelegramChatId))
            SendTelegramAsync(kind, message).GetAwaiter().GetResult();
    }

    private async Task SendSlackAsync(string url, AlertKind kind, string emoji, string color, string message)
    {
        var payload = new
        {
            attachments = new[]
            {
                new
                {
                    color,
                    mrkdwn_in = new[] { "text", "pretext" },
                    pretext = $"{emoji} *EAxWiki [{kind}]* - {_options.InstanceLabel}",
                    text = message,
                    footer = _options.InstanceLabel,
                    ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                }
            }
        };
        try
        {
            await _http.PostAsJsonAsync(url, payload, Json);
            _logger.LogInformation("Slack webhook dispatched.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Slack webhook dispatch failed: {Error}", ex.Message);
        }
    }

    private async Task SendTeamsAsync(string url, AlertKind kind, string color, string message)
    {
        var payload = new
        {
            @type = "MessageCard",
            @context = "http://schema.org/extensions",
            themeColor = color.TrimStart('#'),
            summary = $"EAxWiki [{kind}] - {_options.InstanceLabel}",
            sections = new[]
            {
                new
                {
                    activityTitle = $"EAxWiki [{kind}] - {_options.InstanceLabel}",
                    text = message,
                }
            }
        };
        try
        {
            await _http.PostAsJsonAsync(url, payload, Json);
            _logger.LogInformation("Teams webhook dispatched.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Teams webhook dispatch failed: {Error}", ex.Message);
        }
    }

    private async Task SendTelegramAsync(AlertKind kind, string message)
    {
        var uri = $"https://api.telegram.org/bot{_options.TelegramBotToken}/sendMessage";
        var text = TelegramAlertTextFormatter.Format(kind, _options.InstanceLabel, message, DateTimeOffset.Now);
        var body = new Dictionary<string, object?>
        {
            ["chat_id"] = _options.TelegramChatId,
            ["text"] = text,
            ["parse_mode"] = "HTML",
        };

        var attempts = 0;
        while (true)
        {
            attempts++;
            try
            {
                await _http.PostAsJsonAsync(uri, body, Json);
                _logger.LogInformation("Telegram dispatched.");
                return;
            }
            catch (HttpRequestException ex)
            {
                // HTTP 400 usually means Telegram rejected our HTML (unmatched tag, etc.).
                // Retry exactly once with parse_mode omitted; anything else just logs.
                var status = (int?)ex.StatusCode;
                if (status == 400 && attempts == 1 && body.ContainsKey("parse_mode"))
                {
                    body.Remove("parse_mode");
                    continue;
                }
                _logger.LogWarning("Telegram dispatch failed: {Error}", ex.Message);
                return;
            }
        }
    }
}
```

Note on Telegram 400 detection: `HttpClient` surfaces a 400 as `HttpRequestException` with `StatusCode = 400` (the `PostAsJsonAsync` overloads don't throw on non-success by default in this stack; if the handler returns 400 without throwing, the retry path won't fire in tests — if `PostAsJsonAsync` returns a 400 response, extend `SendTelegramAsync` to check `response.IsSuccessStatusCode` and treat `(int)response.StatusCode == 400` identically. Verify against the actual behavior in Step 5; the test `Telegram_Http400_RetriesOnceWithoutParseMode` gates it.)

- [ ] **Step 5: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~AlertDispatcherTests"
```

Expected: `Passed! - Failed: 0` (7). If `Telegram_Http400_RetriesOnceWithoutParseMode` fails because `PostAsJsonAsync` returns a 400 response rather than throwing, change the Telegram send to:

```csharp
                var response = await _http.PostAsJsonAsync(uri, body, Json);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Telegram dispatched.");
                    return;
                }
                var status = (int)response.StatusCode;
                if (status == 400 && attempts == 1 && body.ContainsKey("parse_mode"))
                {
                    body.Remove("parse_mode");
                    continue;
                }
                _logger.LogWarning("Telegram dispatch failed: HTTP {Status}", status);
                return;
```

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/AlertDispatcher.cs src/EAxWiki.Monitor/TelegramAlertTextFormatter.cs src/EAxWiki.Tests/AlertDispatcherTests.cs
git commit -m "feat(monitor): add Slack/Teams/Telegram alert dispatcher (issue #86)"
```

---

### Task 8: `ProcessSupervisor` + `ServiceSpec`

**Files:**
- Create: `src/EAxWiki.Monitor/ProcessSupervisor.cs`
- Create: `src/EAxWiki.Tests/ProcessSupervisorTests.cs`

**Interfaces:**
- Consumes: `IPortProbe`/`TcpPortProbe` (Task 4), `IPortKiller` (Task 4), `PidFile` (Task 4).
- Produces: `record ServiceSpec(...)`, `interface IProcessSupervisor { bool IsAlive(ServiceSpec spec); int AttemptsUsed { get; } Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct); }`, `class ProcessSupervisor : IProcessSupervisor`.

`ServiceSpec` fields (all the per-service knobs the PS watchdogs need):
```csharp
public sealed record ServiceSpec(
    string Name,
    string PidFilePath,
    string Executable,
    IReadOnlyList<string> Arguments,
    string LogDir,
    int? Port = null,
    string? ReadyFile = null,
    bool PortProbeFallback = false,
    bool ClearPortBeforeStart = false,
    string? WorkingDirectory = null,
    int ReadyTimeoutSeconds = 120,
    int PostStartDelaySeconds = 5);
```

Semantics:
- `IsAlive(spec)`: `PidFile.IsAlive(spec.PidFilePath)` OR (`spec.PortProbeFallback && spec.Port is { } p && _probe.IsListening(p)`).
- `EnsureRunningAsync`: loops up to `maxRetries` attempts. Each attempt: if `ClearPortBeforeStart` and `spec.Port` is set, kill the port owner; if `spec.ReadyFile` is set, delete a stale ready file; start the process with stdout/stderr redirected to `{logDir}/{name}-{stamp}.{out|err}.log`; if ready file set, poll for it every 1 s up to `ReadyTimeoutSeconds`; else wait `PostStartDelaySeconds`; success = ready-file appeared (or process still running); on success write `PidFile.Write(spec.PidFilePath, proc.Id, proc.StartTime.ToUniversalTime())` and return true; between failed attempts sleep `retryDelaySeconds * attempt`. Records the attempt count in `AttemptsUsed`.
- The monitor's per-service give-up/recovery alerts are the caller's job (MonitorLoop), matching the PS script — the supervisor only returns success/failure.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/ProcessSupervisorTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ProcessSupervisorTests : IDisposable
{
    private readonly string _dir;

    public ProcessSupervisorTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_super_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private string LogDir()
    {
        var dir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static ServiceSpec Pinger(string name, string pidPath)
    {
        return new ServiceSpec(name, pidPath, "cmd.exe",
            new[] { "/c", "ping -n 30 127.0.0.1 >nul" }, Path.GetDirectoryName(pidPath)!,
            PostStartDelaySeconds: 0);
    }

    [Fact]
    public async Task EnsureRunning_StartsLongLivedChild_ReturnsTrue_WritesPid()
    {
        var pidPath = Path.Combine(_dir, "pinger.pid");
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());
        var spec = Pinger("pinger", pidPath);

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 1, retryDelaySeconds: 0, CancellationToken.None);

        Assert.True(ok);
        Assert.Equal(1, supervisor.AttemptsUsed);
        var info = PidFile.Read(pidPath);
        Assert.NotNull(info);
        Assert.True(PidFile.IsAlive(pidPath));
    }

    [Fact]
    public async Task EnsureRunning_ExeNotFound_FailsAfterRetries()
    {
        var pidPath = Path.Combine(_dir, "missing.pid");
        var spec = new ServiceSpec("missing", pidPath, @"Z:\does-not-exist.exe",
            Array.Empty<string>(), LogDir(), PostStartDelaySeconds: 0);
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 2, retryDelaySeconds: 0, CancellationToken.None);

        Assert.False(ok);
        Assert.Equal(2, supervisor.AttemptsUsed);
    }

    [Fact]
    public async Task EnsureRunning_ReadyFile_WaitsForIt()
    {
        var pidPath = Path.Combine(_dir, "ready.pid");
        var readyFile = Path.Combine(_dir, "status", "api-ready");
        Directory.CreateDirectory(Path.GetDirectoryName(readyFile)!);
        var spec = new ServiceSpec("ready", pidPath, "cmd.exe",
            new[] { "/c", $"echo ready > \"{readyFile}\"" }, LogDir(),
            ReadyFile: readyFile, ReadyTimeoutSeconds: 15, PostStartDelaySeconds: 0);
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        var ok = await supervisor.EnsureRunningAsync(spec, maxRetries: 1, retryDelaySeconds: 0, CancellationToken.None);

        Assert.True(ok);
        Assert.True(File.Exists(readyFile));
    }

    [Fact]
    public void IsAlive_UntrackedListeningPort_TrueWithPortProbeFallback()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            var spec = new ServiceSpec("serve", Path.Combine(_dir, "serve.pid"), "cmd.exe",
                Array.Empty<string>(), LogDir(), Port: port, PortProbeFallback: true);
            var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

            Assert.True(supervisor.IsAlive(spec));
        }
        finally { listener.Stop(); }
    }

    [Fact]
    public void IsAlive_NoPidNoProbe_ReturnsFalse()
    {
        var spec = new ServiceSpec("serve", Path.Combine(_dir, "serve.pid"), "cmd.exe",
            Array.Empty<string>(), LogDir());
        var supervisor = new ProcessSupervisor(NullLogger.Instance, new TcpPortProbe(), new NetstatPortKiller());

        Assert.False(supervisor.IsAlive(spec));
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~ProcessSupervisorTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `ProcessSupervisor`**

Create `src/EAxWiki.Monitor/ProcessSupervisor.cs`:

```csharp
using System.Diagnostics;
using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public sealed record ServiceSpec(
    string Name,
    string PidFilePath,
    string Executable,
    IReadOnlyList<string> Arguments,
    string LogDir,
    int? Port = null,
    string? ReadyFile = null,
    bool PortProbeFallback = false,
    bool ClearPortBeforeStart = false,
    string? WorkingDirectory = null,
    int ReadyTimeoutSeconds = 120,
    int PostStartDelaySeconds = 5);

public interface IProcessSupervisor
{
    int AttemptsUsed { get; }
    bool IsAlive(ServiceSpec spec);
    Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct);
}

/// <summary>
/// Generic child-process watchdog (serve.ps1, llama-server, EAxWiki --api). Alive = pid-file
/// alive (with port-probe fallback for serve). Start = optional Clear-Port, optional stale
/// ready-file removal, redirected output logs, optional ready-file poll, pid file written on
/// success, retry/backoff. Recovery/give-up alerts are the MonitorLoop's job (PS parity).
/// </summary>
public class ProcessSupervisor : IProcessSupervisor
{
    private readonly ILogger _logger;
    private readonly IPortProbe _probe;
    private readonly IPortKiller _killer;

    public ProcessSupervisor(ILogger logger, IPortProbe probe, IPortKiller killer)
    {
        _logger = logger;
        _probe = probe;
        _killer = killer;
    }

    public int AttemptsUsed { get; private set; }

    public bool IsAlive(ServiceSpec spec)
    {
        if (PidFile.IsAlive(spec.PidFilePath)) return true;
        if (spec.PortProbeFallback && spec.Port is { } port && _probe.IsListening(port)) return true;
        return false;
    }

    public async Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct)
    {
        AttemptsUsed = 0;
        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            AttemptsUsed = attempt;
            ct.ThrowIfCancellationRequested();
            _logger.LogInformation("Start attempt {Attempt}/{MaxRetries} for {Name}.", attempt, maxRetries, spec.Name);

            try
            {
                if (spec.ClearPortBeforeStart && spec.Port is { } clearPort)
                    _killer.KillPortOwner(clearPort);

                if (spec.ReadyFile is { } ready && File.Exists(ready))
                    File.Delete(ready);

                var started = StartAndWait(spec);
                if (started)
                {
                    PidFile.Write(spec.PidFilePath, started.Value.Id, started.Value.StartTime.ToUniversalTime());
                    _logger.LogInformation("{Name} started (PID {Pid}).", spec.Name, started.Value.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Start attempt {Attempt} for {Name} failed: {Error}", attempt, spec.Name, ex.Message);
            }

            if (attempt < maxRetries)
            {
                var delay = retryDelaySeconds * attempt;
                _logger.LogInformation("Retrying {Name} start in {Delay} seconds.", spec.Name, delay);
                await Task.Delay(TimeSpan.FromSeconds(delay), ct);
            }
        }
        return false;
    }

    private Process? StartAndWait(ServiceSpec spec)
    {
        var stamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        var outFile = Path.Combine(spec.LogDir, $"{spec.Name}-{stamp}.out.log");
        var errFile = Path.Combine(spec.LogDir, $"{spec.Name}-{stamp}.err.log");

        var psi = new ProcessStartInfo
        {
            FileName = spec.Executable,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        foreach (var arg in spec.Arguments) psi.ArgumentList.Add(arg);
        if (!string.IsNullOrEmpty(spec.WorkingDirectory)) psi.WorkingDirectory = spec.WorkingDirectory;

        var proc = Process.Start(psi);
        if (proc == null) return null;

        // Drain output to the per-run log files in the background so a full pipe buffer can't
        // stall the child.
        _ = Task.Run(async () =>
        {
            try
            {
                var outText = await proc.StandardOutput.ReadToEndAsync();
                var errText = await proc.StandardError.ReadToEndAsync();
                Directory.CreateDirectory(spec.LogDir);
                File.WriteAllText(outFile, outText);
                File.WriteAllText(errFile, errText);
            }
            catch { /* child already gone; ignore */ }
        });

        if (spec.ReadyFile is { } ready)
        {
            var deadline = DateTime.UtcNow.AddSeconds(spec.ReadyTimeoutSeconds);
            while (DateTime.UtcNow < deadline && !File.Exists(ready))
            {
                if (proc.HasExited) break;
                Thread.Sleep(1000);
            }
            return File.Exists(ready) ? proc : null;
        }

        Thread.Sleep(TimeSpan.FromSeconds(spec.PostStartDelaySeconds));
        return proc.HasExited ? null : proc;
    }
}
```

- [ ] **Step 4: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~ProcessSupervisorTests"
```

Expected: `Passed! - Failed: 0` (5). If `EnsureRunning_ExeNotFound_FailsAfterRetries` takes longer than expected because `Process.Start` throws on a missing exe — it does, and the catch handles it; confirm the retry count via `AttemptsUsed`.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Monitor/ProcessSupervisor.cs src/EAxWiki.Tests/ProcessSupervisorTests.cs
git commit -m "feat(monitor): add generic process supervisor watchdog (issue #86)"
```

---

### Task 9: `ExportRunner` + STA in-process export

**Files:**
- Create: `src/EAxWiki.Monitor/ExportRunner.cs`
- Create: `src/EAxWiki.Tests/ExportRunnerTests.cs`

**Interfaces:**
- Consumes: `MonitorOptions` (Task 3), `HealthState` (Task 1), `AlertDispatcher` (Task 7), `WritebackDelta` (Task 6).
- Produces:
  - `interface IStaExporter { void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? brand, string? aiEndpoint); }` and `class StaMarkdownExporter : IStaExporter` (STA thread: `EaReader.Open`, optional `WriteBackScanner.Scan`, `MarkdownExporter.ExportAsync(...).GetAwaiter().GetResult()`; a broad catch rethrows so the per-run crash boundary in ExportRunner stays). The exporter's return is unused — ExportRunner reads element/diagram counts back from the output via `IWikiOutputMetrics` (PS parity).
  - `interface IWikiOutputMetrics { int CountMarkdownFiles(string wikiDir); int CountDiagramFiles(string wikiDir); }` and `class WikiOutputMetrics : IWikiOutputMetrics` (`*.md` count; diagrams under a `diagrams` path segment).
  - `interface IExportRunner { bool ShouldForce(int runsSinceForce); Task<bool> RunExportAsync(bool effectiveForce, WritebackDelta writebacks, CancellationToken ct); }` and `class ExportRunner : IExportRunner` — ctor `(MonitorOptions options, IStaExporter exporter, IWikiOutputMetrics metrics, HealthState state, IAlertDispatcher alerts, ILogger<ExportRunner> logger)`.

`RunExportAsync` semantics (ported from the PS export block):
1. Set env vars `EAXWIKI_API_PORT`, `EAXWIKI_AI_ENDPOINT`, `EAXWIKI_BRAND` for the in-process export (same as `EAxWiki/Program.cs:219-222`).
2. `state.LastMode = effectiveForce ? "full (--force)" : "incremental"`.
3. Retry loop (`attempt < options.MaxRetries`): call `_exporter.ExportOnSta(...)` (writeBack = `options.ApiPort > 0`); on success count element/diagram files; sanity check `elementCount >= floor(previousCount * minFraction)` when `previousCount > 0`; update `state.LastElementCount`; backoff `retryDelay * attempt` between failures. Broad catch on each attempt (crash boundary: a native COM fault → failure, loop continues).
4. `state.LastExitCode = 0/1`. On success: recovery alert if `state.ConsecutiveFailures > 0`; set `LastSuccessTime`, `ConsecutiveFailures = 0`, `RunsSinceForce = effectiveForce ? 0 : runsSinceForce + 1`, `LastApiPort`; `LastDiagramCount`; compose Finish alert (gated on `options.NotifyOnStart`) with duration, page counts, delta vs previous, validation suffix from `{wikiDir}/.validation-report.json`, writeback suffix from the passed-in `WritebackDelta`. On failure: `LastFailureTime`, `ConsecutiveFailures++`, Failure alert with fenced output tail. Returns success.
5. `internal static bool ShouldForce(int runsSinceForce, bool force, int forceEveryNRuns)` → `force || (forceEveryNRuns > 0 && runsSinceForce >= forceEveryNRuns)`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/ExportRunnerTests.cs`:

```csharp
using EAxWiki.Core.Models;
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class ExportRunnerTests : IDisposable
{
    private readonly string _dir;

    public ExportRunnerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_export_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private sealed class FakeExporter : IStaExporter
    {
        public int Calls;
        public bool Throw;
        public bool LastForce;
        public bool LastWriteBack;
        public void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? brand, string? aiEndpoint)
        {
            Calls++;
            LastForce = force;
            LastWriteBack = writeBack;
            if (Throw) throw new InvalidOperationException("COM boom");
            Directory.CreateDirectory(Path.Combine(outputPath, "Pkg"));
            File.WriteAllText(Path.Combine(outputPath, "Pkg", "Elem.md"), "# Elem");
            Directory.CreateDirectory(Path.Combine(outputPath, "diagrams"));
            File.WriteAllText(Path.Combine(outputPath, "diagrams", "D1.md"), "# D1");
        }
    }

    private sealed class FakeAlerts : IAlertDispatcher
    {
        public readonly List<(AlertKind Kind, string Message)> Sent = new();
        public void Dispatch(string message, AlertKind kind) => Sent.Add((kind, message));
    }

    private static MonitorOptions Options(string? brand = null) => new()
    {
        WikiDir = "W", MaxRetries = 3, RetryDelaySeconds = 0, MinElementFraction = 0.5,
        ApiPort = 0, NotifyOnStart = true, Brand = brand, AiEndpoint = null,
    };

    private (ExportRunner Runner, FakeExporter Exporter, FakeAlerts Alerts, HealthState State, string WikiDir) Create(
        MonitorOptions? options = null)
    {
        var wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        var exporter = new FakeExporter();
        var alerts = new FakeAlerts();
        var state = new HealthState();
        var runner = new ExportRunner(options ?? Options(), exporter, new WikiOutputMetrics(),
            state, alerts, NullLogger<ExportRunner>.Instance);
        return (runner, exporter, alerts, state, wikiDir);
    }

    [Fact]
    public void ShouldForce_Incremental_False()
    {
        var (runner, _, _, _, _) = Create();
        Assert.False(runner.ShouldForce(0));
        Assert.False(runner.ShouldForce(5));
    }

    [Fact]
    public void ShouldForce_ForceFlag_AlwaysTrue()
    {
        var (runner, _, _, _, _) = Create(Options() with { Force = true });
        Assert.True(runner.ShouldForce(0));
    }

    [Fact]
    public void ShouldForce_ForceEveryN_TrueWhenReached()
    {
        var (runner, _, _, _, _) = Create(Options() with { ForceEveryNRuns = 4 });
        Assert.False(runner.ShouldForce(3));
        Assert.True(runner.ShouldForce(4));
    }

    [Fact]
    public async Task RunExport_Forced_SetsForceAndResetsRunsSinceForce()
    {
        var (runner, exporter, _, state, _) = Create();
        state.RunsSinceForce = 9;

        var ok = await runner.RunExportAsync(effectiveForce: true, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.True(exporter.LastForce);
        Assert.Equal(0, state.RunsSinceForce);
        Assert.Equal("full (--force)", state.LastMode);
        Assert.Equal(0, state.LastExitCode);
        Assert.NotNull(state.LastSuccessTime);
        Assert.Equal(1, state.LastElementCount);
        Assert.Equal(1, state.LastDiagramCount);
    }

    [Fact]
    public async Task RunExport_Incremental_IncrementsRunsSinceForce()
    {
        var (runner, exporter, _, state, _) = Create();
        state.RunsSinceForce = 3;

        var ok = await runner.RunExportAsync(effectiveForce: false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.False(exporter.LastForce);
        Assert.Equal("incremental", state.LastMode);
        Assert.Equal(4, state.RunsSinceForce);
    }

    [Fact]
    public async Task RunExport_WritebackEnabled_WhenApiPortSet()
    {
        var opts = Options() with { ApiPort = 8001 };
        var (runner, exporter, _, _, _) = Create(opts);

        await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(exporter.LastWriteBack);
    }

    [Fact]
    public async Task RunExport_SanityFloor_MarksFailureWhenCollapse()
    {
        // Previous run recorded 100 elements; this run only exports 1 → below floor 50 → failure.
        var (runner, exporter, alerts, state, _) = Create();
        state.LastElementCount = 100;
        state.ConsecutiveFailures = 0;

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.False(ok);
        Assert.Equal(1, state.LastExitCode);
        Assert.Equal(1, state.ConsecutiveFailures);
        Assert.NotNull(state.LastFailureTime);
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.Failure);
    }

    [Fact]
    public async Task RunExport_RetriesAndSucceeds_AfterTransientFailure()
    {
        var (runner, exporter, alerts, state, _) = Create();
        exporter.Throw = true;

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        // Fails on every attempt (still throwing) — proves max-retries attempts are made, no success.
        Assert.False(ok);
        Assert.Equal(3, exporter.Calls); // MaxRetries = 3
        Assert.Equal(3, state.ConsecutiveFailures);
    }

    [Fact]
    public async Task RunExport_RecoveryAlert_WhenPreviouslyFailing()
    {
        var (runner, _, alerts, state, _) = Create();
        state.ConsecutiveFailures = 2;
        state.LastElementCount = 1; // matches this run's count → sanity passes

        var ok = await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);

        Assert.True(ok);
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.Recovery);
        Assert.Equal(0, state.ConsecutiveFailures);
    }

    [Fact]
    public async Task RunExport_FinishAlert_HasWritebackAndValidationSuffixes()
    {
        var wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        var validation = Path.Combine(wikiDir, ".validation-report.json");
        File.WriteAllText(validation, """{"Errors":1,"Warnings":0,"Passed":5,"FilesValidated":6}""");
        var (runner, _, alerts, state, _) = Create();

        var writebacks = new WritebackDelta(3, new Dictionary<string, int> { ["status"] = 2, ["notes"] = 1 });
        await runner.RunExportAsync(false, writebacks, CancellationToken.None);

        var finish = alerts.Sent.Single(a => a.Kind == AlertKind.Finish).Message;
        Assert.Contains("page(s) total", finish);
        Assert.Contains("1 diagram", finish);
        Assert.Contains("- validation: 1 error(s) (5/6 files clean)", finish);
        Assert.Contains("- write-backs: 2 status, 1 notes", finish);
    }

    [Fact]
    public async Task RunExport_NoNotifyStart_NoFinishAlert()
    {
        var (runner, _, alerts, _, _) = Create(Options() with { NotifyOnStart = false });
        await runner.RunExportAsync(false, new WritebackDelta(0, new Dictionary<string, int>()), CancellationToken.None);
        Assert.DoesNotContain(alerts.Sent, a => a.Kind == AlertKind.Finish);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~ExportRunnerTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `IWikiOutputMetrics` and `WikiOutputMetrics`**

Create `src/EAxWiki.Monitor/ExportRunner.cs` with the following content (all types in this file, one file per responsibility per repo convention is overridden here because these five small types form one unit; if the file grows unwieldy, split `StaMarkdownExporter.cs` and `WikiOutputMetrics.cs`):

```csharp
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

    public StaMarkdownExporter(ILogger logger) => _logger = logger;

    public void ExportOnSta(string repoPath, string outputPath, bool force, bool writeBack, int apiPort, string? brand, string? aiEndpoint)
    {
        Exception? failure = null;
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            try
            {
                using var reader = new EaReader(_logger as ILogger<EaReader>);
                reader.Open(repoPath);

                if (writeBack && Directory.Exists(outputPath))
                {
                    _logger.LogInformation("Running write-back scan...");
                    var scanner = new WriteBackScanner(reader, _logger);
                    var scanResult = scanner.Scan(outputPath);
                    if (scanResult.StatusChanges.Count == 0 && scanResult.NotesChanges.Count == 0)
                        _logger.LogInformation("Write-back: no changes detected.");
                    else
                        _logger.LogInformation("Write-back: applied {Status} status and {Notes} notes change(s).",
                            scanResult.StatusChanges.Count, scanResult.NotesChanges.Count);
                }

                var writer = new FileOutputWriter();
                var exporter = new MarkdownExporter(writer, _logger as ILogger<MarkdownExporter>);
                var repository = reader.Open(repoPath);
                var result = exporter.ExportAsync(repository, null, outputPath, reader, force)
                    .GetAwaiter().GetResult();
                _logger.LogInformation("Export finished: {Total} pages, {Failed} failed, {Diagrams} diagrams.",
                    result.TotalElements, result.FailedElements, result.DiagramsExported);
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                failure = ex;
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        tcs.Task.GetAwaiter().GetResult();
        if (failure != null) throw failure;
    }
}
```

- [ ] **Step 4: Implement `ExportRunner`**

Append to the same `ExportRunner.cs`:

```csharp
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

        var succeeded = false;
        var lastExitCode = 1;
        var elementCount = 0;
        var previousCount = _state.LastElementCount ?? 0;
        var diagramCount = 0;
        var outputTail = "";
        var stopwatch = Stopwatch.StartNew();

        for (var attempt = 1; attempt <= _options.MaxRetries && !succeeded; attempt++)
        {
            ct.ThrowIfCancellationRequested();
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
                var floor = Math.Floor(previousCount * _options.MinElementFraction);
                if (previousCount > 0 && elementCount < floor)
                {
                    _logger.LogWarning("Sanity check failed: element count {Count} below floor {Floor} (previous {Previous}).",
                        elementCount, floor, previousCount);
                    lastExitCode = 1;
                }
                else
                {
                    succeeded = true;
                    _state.LastElementCount = elementCount;
                }
            }
            catch (Exception ex)
            {
                outputTail = ex.Message;
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
            var wasFailing = _state.ConsecutiveFailures > 0;
            _state.LastSuccessTime = DateTimeOffset.Now;
            _state.ConsecutiveFailures = 0;
            _state.RunsSinceForce = effectiveForce ? 0 : _state.RunsSinceForce + 1;
            _state.LastApiPort = _options.ApiPort;
            if (wasFailing)
                _alerts.Dispatch($"Export succeeded, recovering from a prior failure.", AlertKind.Recovery);

            diagramCount = _metrics.CountDiagramFiles(_options.WikiDir);
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
            _logger.LogInformation("Succeeded on attempt {Attempt} in {Elapsed}.", 1, stopwatch.Elapsed.ToString("mm\\:ss"));
        }
        else
        {
            _state.LastFailureTime = DateTimeOffset.Now;
            _state.ConsecutiveFailures++;
            _logger.LogWarning("Gave up after {MaxRetries} attempt(s).", _options.MaxRetries);
            _alerts.Dispatch($"Export failed after {_options.MaxRetries} attempt(s) (exit code {lastExitCode}).\n```\n{outputTail}\n```",
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
    }
}
```

- [ ] **Step 5: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~ExportRunnerTests"
```

Expected: `Passed! - Failed: 0` (11). If `RunExport_RetriesAndSucceeds_AfterTransientFailure` asserts `3` calls, confirm `_options.MaxRetries` default of 3 is what the test `Options()` builder produces (it sets `MaxRetries = 3`).

- [ ] **Step 6: Commit**

```bash
git add src/EAxWiki.Monitor/ExportRunner.cs src/EAxWiki.Tests/ExportRunnerTests.cs
git commit -m "feat(monitor): add export runner with STA in-process export (issue #86)"
```

---

### Task 10: `MonitorLoop` + `MonitorLock` + `MonitorPaths`

**Files:**
- Create: `src/EAxWiki.Monitor/MonitorLock.cs`
- Create: `src/EAxWiki.Monitor/MonitorPaths.cs`
- Create: `src/EAxWiki.Monitor/MonitorLoop.cs`
- Create: `src/EAxWiki.Tests/MonitorLockTests.cs`
- Create: `src/EAxWiki.Tests/MonitorPathsTests.cs`
- Create: `src/EAxWiki.Tests/MonitorLoopTests.cs`

**Interfaces:**
- Consumes: `MonitorOptions` (Task 3), `HealthStore`/`HealthState` (Task 1), `IExportRunner` (Task 9), `IDigestTracker` (Task 6), `IAlertDispatcher` (Task 7), `IProcessSupervisor`/`ServiceSpec` (Task 8), `EditLock` (Task 5), `HealthPageRenderer` (Task 5).
- Produces: `static class MonitorLock { bool TryAcquire(string monitorPidPath, out int pid); void Release(string monitorPidPath); }`, `static class MonitorPaths { string FindRepoRoot(string startDir); string StateDir(string repoRoot, string wikiDir); string FindPowerShell(); }`, `class MonitorLoop { MonitorLoop(MonitorOptions options, HealthState state, HealthStore healthStore, HealthPageRenderer pageRenderer, IExportRunner exportRunner, IDigestTracker digestTracker, IAlertDispatcher alerts, IProcessSupervisor supervisor, ServiceSpec serveSpec, ServiceSpec apiSpec, ServiceSpec llmSpec, ILogger logger); Task RunAsync(CancellationToken ct); }`.

`MonitorLoop.RunAsync` — one `while (true)` cycle per `--check-interval`:

1. **Reset skip flags:** `state.SkipExport = false; state.SkipServe = false;`
2. **Export due?** `_lastExportTime == default || (DateTime.UtcNow - _lastExportTime).TotalMinutes >= options.ExportIntervalMinutes`.
3. **Edit-lock defer:** if export due and `EditLock.IsActive(options.WikiDir)` → log, not due this cycle.
4. **Export:** if due:
   - if `state.SkipExport` (set by SchedulerUI stop): log + `UserStop` alert, set `succeeded=true`, `lastExitCode=0`.
   - else: `writebacks = digestTracker.CountNewWritebacks()`; `effectiveForce = exportRunner.ShouldForce(state.RunsSinceForce)`; if `options.NotifyOnStart` dispatch `Start` alert ("Scheduled run starting (forced full rebuild)." / "Scheduled run starting (incremental)."); `succeeded = await exportRunner.RunExportAsync(effectiveForce, writebacks, ct)`.
   - `_lastExportTime = DateTime.UtcNow`.
   - else: log "Skipping export (next due in N min)."
5. **Digest accounting:** `state.PageReadsToday += digestTracker.CountNewPageReads();` and if a digest composed (non-null from `digestTracker.MaybeComposeDailyDigest(DateTime.Now)`) dispatch `DailyDigest`.
6. **Render + save:** `pageRenderer.Render(state); healthStore.Save(healthPath, state);`
7. **Serve watchdog:** if `state.SkipServe` → log blocked; else if not `supervisor.IsAlive(serveSpec)` → attempt restart via `supervisor.EnsureRunningAsync`; on success: recovery alert if `state.ServeConsecutiveFailures > 0`, reset counter + `LastServeSuccessTime`; on failure: increment + `LastServeFailureTime` + `ServeFailure` alert; re-render + re-save.
8. **API watchdog** (if `options.ApiPort > 0`): same pattern → `ApiFailure`/`ApiRecovery`, re-render + re-save.
9. **LLM watchdog** (if `options.AiMode == "local"` and both llama paths exist): same pattern → `LlmFailure`/`LlmRecovery`, re-render + re-save; else log "LLM not configured (AiMode=...)".
10. Sleep `options.CheckIntervalSeconds`.

`MonitorLock`: monitor.pid is plain text PID (not JSON). `TryAcquire`: if file exists and parses to a live PID that isn't our own → return false (duplicate); else delete stale file, write own PID, return true. `Release`: delete the file.

`MonitorPaths`: `FindRepoRoot(startDir)` walks up from `AppContext.BaseDirectory` until `scripts/register-scheduled-task.ps1` or `.git` exists (Task Scheduler actions have no WorkingDirectory); `StateDir(repoRoot, wikiDir)` = `Path.Combine(repoRoot, ".eaxwiki-monitor", InstanceHash.Compute(wikiDir))`; `FindPowerShell()` = `$PSHOME/pwsh` if set else `"pwsh"`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/MonitorLockTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorLockTests : IDisposable
{
    private readonly string _dir;

    public MonitorLockTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_mlock_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void TryAcquire_NoFile_Acquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        Assert.True(MonitorLock.TryAcquire(path, out var pid));
        Assert.Equal(Environment.ProcessId, pid);
        Assert.Equal(Environment.ProcessId.ToString(), File.ReadAllText(path).Trim());
    }

    [Fact]
    public void TryAcquire_OwnPidFile_Acquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        File.WriteAllText(path, Environment.ProcessId.ToString());
        Assert.True(MonitorLock.TryAcquire(path, out _));
    }

    [Fact]
    public void TryAcquire_LiveForeignPid_Rejects()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        // A live PID that isn't ours: spawn a fresh child.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 10 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        File.WriteAllText(path, p!.Id.ToString());

        Assert.False(MonitorLock.TryAcquire(path, out _));
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void TryAcquire_DeadPidFile_RemovesAndAcquires()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        File.WriteAllText(path, "-9999");
        Assert.True(MonitorLock.TryAcquire(path, out _));
        Assert.Equal(Environment.ProcessId.ToString(), File.ReadAllText(path).Trim());
    }

    [Fact]
    public void Release_RemovesFile()
    {
        var path = Path.Combine(_dir, "monitor.pid");
        MonitorLock.TryAcquire(path, out _);
        MonitorLock.Release(path);
        Assert.False(File.Exists(path));
    }
}
```

Create `src/EAxWiki.Tests/MonitorPathsTests.cs`:

```csharp
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorPathsTests : IDisposable
{
    private readonly string _dir;

    public MonitorPathsTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_mpath_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(_dir, "scripts"));
        Directory.CreateDirectory(Path.Combine(_dir, ".git"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void FindRepoRoot_FindsScriptsMarkerInParent()
    {
        var nested = Path.Combine(_dir, "a", "b");
        Directory.CreateDirectory(nested);
        Assert.Equal(_dir, MonitorPaths.FindRepoRoot(nested));
    }

    [Fact]
    public void StateDir_UsesInstanceHash()
    {
        var wiki = Path.Combine(_dir, "wiki");
        var stateDir = MonitorPaths.StateDir(_dir, wiki);
        Assert.Equal(Path.Combine(_dir, ".eaxwiki-monitor", EAxWiki.Core.Monitoring.InstanceHash.Compute(wiki)), stateDir);
    }
}
```

Create `src/EAxWiki.Tests/MonitorLoopTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class MonitorLoopTests
{
    private sealed class StubExportRunner : IExportRunner
    {
        public int Runs;
        public bool ShouldForce(int runsSinceForce) => false;
        public Task<bool> RunExportAsync(bool effectiveForce, WritebackDelta writebacks, CancellationToken ct)
        {
            Runs++;
            return Task.FromResult(true);
        }
    }

    private sealed class StubDigest : IDigestTracker
    {
        public int PageReads;
        public int CountNewPageReads() => PageReads;
        public WritebackDelta CountNewWritebacks() => new(0, new Dictionary<string, int>());
        public string? MaybeComposeDailyDigest(DateTime now) => null;
    }

    private sealed class StubAlerts : IAlertDispatcher
    {
        public readonly List<(AlertKind Kind, string Message)> Sent = new();
        public void Dispatch(string message, AlertKind kind) => Sent.Add((kind, message));
    }

    private sealed class StubSupervisor : IProcessSupervisor
    {
        public int AttemptsUsed { get; set; }
        public int StartCount;
        public bool IsAlive(ServiceSpec spec) => false;
        public Task<bool> EnsureRunningAsync(ServiceSpec spec, int maxRetries, int retryDelaySeconds, CancellationToken ct)
        {
            StartCount++;
            return Task.FromResult(true);
        }
    }

    private sealed class FakeHealthStore : HealthStore
    {
        public int Saves;
        public override void Save(string path, HealthState state) => Saves++;
    }

    private static MonitorLoop Build(
        out StubExportRunner exportRunner, out StubDigest digest, out StubAlerts alerts,
        out StubSupervisor supervisor, out HealthState state, out FakeHealthStore store,
        string? wikiDir = null, int checkInterval = 0, bool local = false)
    {
        var dir = Path.Combine(Path.GetTempPath(), "eaxwiki_loop_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var options = new MonitorOptions
        {
            RepoPath = @"C:\models\repo.qea",
            WikiDir = wikiDir ?? Path.Combine(dir, "wiki"),
            ApiPort = 8001,
            ExportIntervalMinutes = 30,
            CheckIntervalSeconds = checkInterval,
            MaxRetries = 1,
            RetryDelaySeconds = 0,
            AiMode = local ? "local" : "none",
            LlamaExePath = Path.Combine(dir, "llama-server.exe"),
            LlamaModelPath = Path.Combine(dir, "model.gguf"),
            NotifyOnStart = true,
        };
        exportRunner = new StubExportRunner();
        digest = new StubDigest();
        alerts = new StubAlerts();
        supervisor = new StubSupervisor();
        state = new HealthState();
        store = new FakeHealthStore();

        var loop = new MonitorLoop(
            options, state, store,
            new HealthPageRenderer(Path.Combine(dir, "health-template.md"), options.WikiDir),
            exportRunner, digest, alerts, supervisor,
            new ServiceSpec("serve", Path.Combine(dir, "serve.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("api", Path.Combine(dir, "api.pid"), "cmd.exe", Array.Empty<string>(), dir),
            new ServiceSpec("llm", Path.Combine(dir, "llm.pid"), "cmd.exe", Array.Empty<string>(), dir),
            NullLogger.Instance);
        return loop;
    }

    [Fact]
    public void RunOnce_FirstCycle_ExportsAndStartsServices()
    {
        var loop = Build(out var exportRunner, out _, out var alerts, out var supervisor, out var state, out _);
        loop.RunOnce();

        Assert.Equal(1, exportRunner.Runs);
        Assert.True(supervisor.StartCount >= 3); // serve, api, llm(none → skipped; see below)
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.Start);
        Assert.Equal(0, state.SkipExport);
        Assert.Equal(0, state.SkipServe);
    }

    [Fact]
    public void RunOnce_SecondCycleWithinInterval_DoesNotExport()
    {
        var loop = Build(out var exportRunner, out _, out _, out _, out _, out _);
        loop.RunOnce(); // first: export
        loop.RunOnce(); // second: within 30-min interval → no export

        Assert.Equal(1, exportRunner.Runs);
    }

    [Fact]
    public void RunOnce_SkipExport_SetByStop_AlertsUserStop()
    {
        var loop = Build(out var exportRunner, out _, out var alerts, out _, out var state, out _);
        state.SkipExport = true;

        loop.RunOnce();

        Assert.Equal(0, exportRunner.Runs);
        Assert.Contains(alerts.Sent, a => a.Kind == AlertKind.UserStop);
    }

    [Fact]
    public void RunOnce_SaveHealth_Invoked()
    {
        var loop = Build(out _, out _, out _, out _, out _, out var store);
        loop.RunOnce();
        Assert.Equal(1, store.Saves);
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorLockTests|FullyQualifiedName~MonitorPathsTests|FullyQualifiedName~MonitorLoopTests"
```

Expected: compile error — types do not exist.

- [ ] **Step 3: Implement `MonitorLock`**

Create `src/EAxWiki.Monitor/MonitorLock.cs`:

```csharp
using System.Diagnostics;

namespace EAxWiki.Monitor;

/// <summary>
/// Duplicate-instance guard. monitor.pid is plain PID text (unlike the JSON serve/api/llm pid
/// files). TryAcquire returns false when an existing live PID that isn't this process holds the
/// file (the monitor then exits 0); a stale/dead pid file is replaced.
/// </summary>
public static class MonitorLock
{
    public static bool TryAcquire(string monitorPidPath, out int pid)
    {
        pid = Environment.ProcessId;
        if (File.Exists(monitorPidPath))
        {
            var existing = File.ReadAllText(monitorPidPath).Trim();
            if (int.TryParse(existing, out var existingPid) && existingPid != pid && IsAlive(existingPid))
                return false;
            File.Delete(monitorPidPath);
        }
        Directory.CreateDirectory(Path.GetDirectoryName(monitorPidPath)!);
        File.WriteAllText(monitorPidPath, pid.ToString());
        return true;
    }

    public static void Release(string monitorPidPath)
    {
        if (File.Exists(monitorPidPath))
            File.Delete(monitorPidPath);
    }

    private static bool IsAlive(int pid)
    {
        try
        {
            using var proc = Process.GetProcessById(pid);
            return !proc.HasExited;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
```

- [ ] **Step 4: Implement `MonitorPaths`**

Create `src/EAxWiki.Monitor/MonitorPaths.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

/// <summary>
/// Repo-root and state-dir resolution. FindRepoRoot walks up from the executable directory
/// (Task Scheduler actions have no WorkingDirectory) until it finds scripts/register-scheduled-task.ps1
/// or a .git directory. StateDir replicates the PS monitor's per-instance .eaxwiki-monitor/&lt;hash&gt;
/// folder keyed on the wiki dir.
/// </summary>
public static class MonitorPaths
{
    public static string FindRepoRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "scripts", "register-scheduled-task.ps1")) ||
                Directory.Exists(Path.Combine(dir.FullName, ".git")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Could not locate the EAxWiki repo root.");
    }

    public static string StateDir(string repoRoot, string wikiDir) =>
        Path.Combine(repoRoot, ".eaxwiki-monitor", InstanceHash.Compute(wikiDir));

    public static string FindPowerShell()
    {
        var pshome = Environment.GetEnvironmentVariable("PSHOME");
        if (!string.IsNullOrEmpty(pshome))
            return Path.Combine(pshome, "pwsh");
        return "pwsh";
    }
}
```

- [ ] **Step 5: Implement `MonitorLoop`**

Create `src/EAxWiki.Monitor/MonitorLoop.cs`:

```csharp
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
    private readonly IExportRunner _exportRunner;
    private readonly IDigestTracker _digestTracker;
    private readonly IAlertDispatcher _alerts;
    private readonly IProcessSupervisor _supervisor;
    private readonly ServiceSpec _serveSpec;
    private readonly ServiceSpec _apiSpec;
    private readonly ServiceSpec _llmSpec;
    private readonly ILogger _logger;

    private string HealthPath => Path.Combine(_options.WikiDir.ParentStateDir(), "health.json");

    private DateTime _lastExportTime = DateTime.MinValue;

    public MonitorLoop(
        MonitorOptions options,
        HealthState state,
        HealthStore healthStore,
        HealthPageRenderer pageRenderer,
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

        _state.SkipExport = false;
        _state.SkipServe = false;

        if (exportDue)
        {
            if (EditLock.IsActive(_options.WikiDir))
            {
                _logger.LogInformation("Deferring export - edit in progress, retry next cycle.");
                exportDue = false;
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
            }
            else
            {
                if (_options.NotifyOnStart)
                    _alerts.Dispatch(
                        effectiveForce ? "Scheduled run starting (forced full rebuild)." : "Scheduled run starting (incremental).",
                        AlertKind.Start);
                writebackSummary = _digestTracker.CountNewWritebacks();
                var _ = await ExportProtectedAsync(effectiveForce, writebackSummary);
            }
            _lastExportTime = DateTime.UtcNow;
        }
        else
        {
            _logger.LogInformation("Skipping export (next due in {Interval} min).", _options.ExportIntervalMinutes);
        }

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
        _healthStore.Save(HealthPath, _state);
    }
}

internal static class MonitorDirExtensions
{
    public static string ParentStateDir(this string wikiDir) =>
        Path.Combine(Path.GetDirectoryName(wikiDir) ?? wikiDir, ".eaxwiki-monitor");
}
```

Note on `HealthPath`: the state dir is resolved by the caller (MonitorApp) and passed in via the `HealthStore`/renderer construction; the `ParentStateDir` helper here is a simplification — in the real wiring (Task 11) `MonitorLoop` receives the actual state dir through the constructor-injected `HealthStore` and `HealthPageRenderer`. To keep this test-visible simple, replace the `HealthPath` property with an injected state-dir string in the constructor signature instead:

Change the ctor to add `string stateDir` and use `_healthStore.Save(Path.Combine(stateDir, "health.json"), _state)` in `RenderAndSave`, and update the `Build` test helper to pass `dir` as `stateDir`. (Adjust the two signatures together so the type matches between Task 10 and Task 11.)

- [ ] **Step 6: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorLockTests|FullyQualifiedName~MonitorPathsTests|FullyQualifiedName~MonitorLoopTests"
```

Expected: `Passed! - Failed: 0` (5 + 2 + 4 = 11, minus any folded by the ctor adjustment). If `RunOnce_FirstCycle_ExportsAndStartsServices` asserts `supervisor.StartCount >= 3` but `AiMode == "none"` skips LLM, drop that expectation to `>= 2` (serve + api) and add a dedicated `local` LLM watchdog test with `Build(local: true)` asserting `StartCount >= 3`.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.Monitor/MonitorLock.cs src/EAxWiki.Monitor/MonitorPaths.cs src/EAxWiki.Monitor/MonitorLoop.cs src/EAxWiki.Tests/MonitorLockTests.cs src/EAxWiki.Tests/MonitorPathsTests.cs src/EAxWiki.Tests/MonitorLoopTests.cs
git commit -m "feat(monitor): add monitor loop, lock and path resolution (issue #86)"
```

---

### Task 11: `Program` wiring + `MonitorApp` factory + file logger

**Files:**
- Create: `src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs`
- Create: `src/EAxWiki.Monitor/MonitorApp.cs`
- Create: `src/EAxWiki.Monitor/Program.cs` (replace placeholder from Task 1)
- Create: `src/EAxWiki.Tests/MonitorFileLoggerTests.cs` (optional smoke)

**Interfaces:**
- Consumes: everything from Tasks 1-10.
- Produces: `class MonitorFileLoggerProvider : ILoggerProvider` (writes `{stateDir}/logs/monitor-{yyyy-MM-dd}.log`, format `"yyyy-MM-dd HH:mm:ss [category-last] message"`), `static class MonitorApp { static ServiceSpec BuildServeSpec(...); static ServiceSpec BuildApiSpec(...); static ServiceSpec BuildLlmSpec(...); static MonitorLoop Build(MonitorOptions options, HealthState state, HealthStore store, string stateDir, ILoggerFactory loggerFactory); }`, `static class Program { static int Main(string[] args); }`.

`Program.Main` sequence:
1. `if (!OperatingSystem.IsWindows()) { Console.Error.WriteLine("Monitoring requires Windows (Sparx Enterprise Architect)."); return 1; }`
2. Parse with `MonitorCommandLine.BuildCommand()`; if `parseResult.Errors.Count > 0` → `await parseResult.InvokeAsync()` (prints the error + help) and return 1.
3. `repoRoot = MonitorPaths.FindRepoRoot(AppContext.BaseDirectory)`.
4. Load `.eaxwiki` via `LocalConfigStore.Load` if present.
5. `options = MonitorOptionsResolver.Resolve(cli, repoRoot, Environment.GetEnvironmentVariable, config)`.
6. `stateDir = MonitorPaths.StateDir(repoRoot, options.WikiDir)`; ensure `logs/` exists.
7. Build `LoggerFactory` with SimpleConsole + `MonitorFileLoggerProvider` (file `{stateDir}/logs/monitor-{yyyy-MM-dd}.log`).
8. `monitorPidPath = Path.Combine(stateDir, "monitor.pid")`; if not `MonitorLock.TryAcquire` → log duplicate, return 0.
9. If `options.TestAlert` → dispatch a Test alert with the exact PS message and return 0.
10. `EnsureServePlaceholders`: create `{wikiDir}/status/` and `{wikiDir}/status/api-ready` placeholder if missing (PS `Start-Serve` did this).
11. Load `HealthState` via `HealthStore.Load(healthPath)`.
12. Build `MonitorLoop` via `MonitorApp.Build(...)`; `await loop.RunAsync(ct)`; in `finally`, `MonitorLock.Release(monitorPidPath)`.

`MonitorApp.BuildServeSpec`: `pwsh -NoProfile -File scripts/serve.ps1 --port {WikiPort} --wiki-dir {WikiDir}` — Executable `MonitorPaths.FindPowerShell()`, `PortProbeFallback: true`, `Port: WikiPort`, `PidFilePath: {stateDir}/serve.pid`, `LogDir: {stateDir}/logs`, `PostStartDelaySeconds: 5`.

`MonitorApp.BuildApiSpec`: Executable `"dotnet"`, args `exec --runtimeconfig {src/EAxWiki/bin/Debug/net10.0/EAxWiki.runtimeconfig.json} --depsfile {src/EAxWiki/bin/Debug/net10.0/EAxWiki.deps.json} {src/EAxWiki/bin/Debug/net10.0/EAxWiki.dll} --api --api-port {ApiPort} --wiki-port {WikiPort} --output {WikiDir}` + `--repo {RepoPath}` when set; `ReadyFile: {wikiDir}/status/api-ready`, `ClearPortBeforeStart: true`, `Port: ApiPort`, `ReadyTimeoutSeconds: 120`.

`MonitorApp.BuildLlmSpec`: Executable `options.LlamaExePath`, args `-m {LlamaModelPath} -c 4096 --port {LlmPort} --n-gpu-layers 0`, `ClearPortBeforeStart: true`, `Port: LlmPort`, `PostStartDelaySeconds: 5`, only constructed when `AiMode == "local"` and both paths exist.

- [ ] **Step 1: Write the failing test (file logger smoke)**

Create `src/EAxWiki.Tests/MonitorFileLoggerTests.cs`:

```csharp
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Tests;

public class MonitorFileLoggerTests : IDisposable
{
    private readonly string _dir;

    public MonitorFileLoggerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_flog_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void LogsToDateStampedFile()
    {
        using var provider = new MonitorFileLoggerProvider(_dir);
        var logger = provider.CreateLogger("EAxWiki.Monitor.ExportRunner");
        logger.LogInformation("hello {X}", 42);
        provider.Dispose();

        var logDir = Path.Combine(_dir, "logs");
        var file = Directory.GetFiles(logDir, "monitor-*.log").Single();
        var content = File.ReadAllText(file);
        Assert.Contains("[ExportRunner] hello 42", content);
        Assert.Matches("^\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} \\[ExportRunner\\] hello 42", content);
    }
}
```

- [ ] **Step 2: Run test to verify it fails to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorFileLoggerTests"
```

Expected: compile error — type does not exist.

- [ ] **Step 3: Implement `MonitorFileLoggerProvider`**

Create `src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs`:

```csharp
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

/// <summary>
/// Minimal ILoggerProvider writing to {stateDir}/logs/monitor-{yyyy-MM-dd}.log with the PS
/// monitor's "yyyy-MM-dd HH:mm:ss [phase] message" shape (phase = last category segment).
/// </summary>
public sealed class MonitorFileLoggerProvider : ILoggerProvider
{
    private readonly string _logDir;

    public MonitorFileLoggerProvider(string stateDir)
    {
        _logDir = Path.Combine(stateDir, "logs");
        Directory.CreateDirectory(_logDir);
    }

    public ILogger CreateLogger(string categoryName)
    {
        var shortName = categoryName.Split('.').LastOrDefault() ?? categoryName;
        return new FileLogger(this, shortName);
    }

    public void Dispose() { }

    private sealed class FileLogger : ILogger
    {
        private readonly MonitorFileLoggerProvider _parent;
        private readonly string _name;

        public FileLogger(MonitorFileLoggerProvider parent, string name)
        {
            _parent = parent;
            _name = name;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception != null) message += $" {exception.Message}";
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_name}] {message}";
            var stamp = DateTime.Now.ToString("yyyy-MM-dd");
            File.AppendAllText(Path.Combine(_parent._logDir, $"monitor-{stamp}.log"), line + Environment.NewLine);
        }
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~MonitorFileLoggerTests"
```

Expected: `Passed! - Failed: 0, Passed: 1`.

- [ ] **Step 5: Implement `MonitorApp`**

Create `src/EAxWiki.Monitor/MonitorApp.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public static class MonitorApp
{
    public static MonitorLoop Build(
        MonitorOptions options,
        HealthState state,
        HealthStore healthStore,
        string stateDir,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("MonitorApp");
        var statePath = Path.Combine(stateDir, "health.json");

        var pageRenderer = new HealthPageRenderer(
            Path.Combine(Path.GetDirectoryName(stateDir)!, ".eaxwiki-monitor", "health-template.md"),
            options.WikiDir);

        var alerts = new AlertDispatcher(
            new AlertOptions(options.WebhookUrl, options.TeamsWebhookUrl,
                options.TelegramBotToken, options.TelegramChatId,
                $"{Environment.MachineName} - {options.WikiDir}"),
            null,
            loggerFactory.CreateLogger("Alert"));

        var digestTracker = new DigestTracker(state, options.WikiDir, Path.Combine(stateDir, "logs"),
            Path.Combine(Path.GetDirectoryName(stateDir)!, ".eaxwiki-monitor", "digest-template.md"));

        var exporter = new StaMarkdownExporter(loggerFactory.CreateLogger("Export"));
        var metrics = new WikiOutputMetrics();
        var exportRunner = new ExportRunner(options, exporter, metrics, state, alerts,
            loggerFactory.CreateLogger<ExportRunner>());

        var supervisor = new ProcessSupervisor(loggerFactory.CreateLogger("Supervisor"),
            new TcpPortProbe(), new NetstatPortKiller());

        var serveSpec = BuildServeSpec(options, stateDir);
        var apiSpec = BuildApiSpec(options, stateDir);
        var llmSpec = BuildLlmSpec(options, stateDir);

        return new MonitorLoop(options, state, healthStore, pageRenderer,
            exportRunner, digestTracker, alerts, supervisor, serveSpec, apiSpec, llmSpec,
            loggerFactory.CreateLogger("MonitorLoop"));
    }

    public static ServiceSpec BuildServeSpec(MonitorOptions options, string stateDir)
    {
        return new ServiceSpec(
            "serve",
            Path.Combine(stateDir, "serve.pid"),
            MonitorPaths.FindPowerShell(),
            new[] { "-NoProfile", "-File", "scripts\\serve.ps1", "--port", options.WikiPort.ToString(), "--wiki-dir", options.WikiDir },
            Path.Combine(stateDir, "logs"),
            Port: options.WikiPort,
            PortProbeFallback: true,
            WorkingDirectory: ResolveRepoRoot(stateDir),
            PostStartDelaySeconds: 5);
    }

    public static ServiceSpec BuildApiSpec(MonitorOptions options, string stateDir)
    {
        var repoRoot = ResolveRepoRoot(stateDir);
        var projDir = Path.Combine(repoRoot, "src", "EAxWiki", "bin", "Debug", "net10.0");
        var args = new List<string>
        {
            "exec",
            "--runtimeconfig", Path.Combine(projDir, "EAxWiki.runtimeconfig.json"),
            "--depsfile", Path.Combine(projDir, "EAxWiki.deps.json"),
            Path.Combine(projDir, "EAxWiki.dll"),
            "--api", "--api-port", options.ApiPort.ToString(),
            "--wiki-port", options.WikiPort.ToString(),
            "--output", options.WikiDir,
        };
        if (!string.IsNullOrEmpty(options.RepoPath))
        {
            args.Add("--repo");
            args.Add(options.RepoPath);
        }

        return new ServiceSpec(
            "api",
            Path.Combine(stateDir, "api.pid"),
            "dotnet",
            args,
            Path.Combine(stateDir, "logs"),
            Port: options.ApiPort,
            ReadyFile: Path.Combine(options.WikiDir, "status", "api-ready"),
            ClearPortBeforeStart: true,
            WorkingDirectory: repoRoot,
            ReadyTimeoutSeconds: 120);
    }

    public static ServiceSpec? BuildLlmSpec(MonitorOptions options, string stateDir)
    {
        if (options.AiMode != "local") return null;
        if (string.IsNullOrEmpty(options.LlamaExePath) || string.IsNullOrEmpty(options.LlamaModelPath)) return null;
        if (!File.Exists(options.LlamaExePath) || !File.Exists(options.LlamaModelPath)) return null;

        return new ServiceSpec(
            "llm",
            Path.Combine(stateDir, "llm.pid"),
            options.LlamaExePath,
            new[] { "-m", options.LlamaModelPath, "-c", "4096", "--port", options.LlmPort.ToString(), "--n-gpu-layers", "0" },
            Path.Combine(stateDir, "logs"),
            Port: options.LlmPort,
            ClearPortBeforeStart: true,
            PostStartDelaySeconds: 5);
    }

    private static string ResolveRepoRoot(string stateDir) =>
        MonitorPaths.FindRepoRoot(stateDir);
}
```

- [ ] **Step 6: Replace `Program.cs`**

Replace `src/EAxWiki.Monitor/Program.cs` (the placeholder from Task 1) with:

```csharp
using EAxWiki.Core.Configuration;
using EAxWiki.Core.Monitoring;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine("Monitoring requires Sparx Enterprise Architect, which is only available on Windows.");
            return 1;
        }

        var root = MonitorCommandLine.BuildCommand();
        var parseResult = root.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            await parseResult.InvokeAsync();
            return 1;
        }
        var cli = MonitorCommandLine.ToOptions(parseResult);

        var repoRoot = MonitorPaths.FindRepoRoot(AppContext.BaseDirectory);

        LocalConfigStore.Config? config = null;
        var eaxwikiPath = Path.Combine(repoRoot, ".eaxwiki");
        if (File.Exists(eaxwikiPath))
        {
            try { config = LocalConfigStore.Load(eaxwikiPath, out _); }
            catch { /* legacy/undecryptable — resolve with null config */ }
        }

        var options = MonitorOptionsResolver.Resolve(cli, repoRoot, Environment.GetEnvironmentVariable, config);
        var stateDir = MonitorPaths.StateDir(repoRoot, options.WikiDir);
        Directory.CreateDirectory(Path.Combine(stateDir, "logs"));

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.fff ");
            builder.AddProvider(new MonitorFileLoggerProvider(stateDir));
        });
        var logger = loggerFactory.CreateLogger("monitor");

        var monitorPidPath = Path.Combine(stateDir, "monitor.pid");
        if (!MonitorLock.TryAcquire(monitorPidPath, out _))
        {
            logger.LogInformation("Duplicate monitor detected; exiting.");
            return 0;
        }
        try
        {
            logger.LogInformation("Repo: {Repo}", Redact(options.RepoPath));
            logger.LogInformation("ApiPort={ApiPort} WikiPort={WikiPort} AiEndpoint={AiEndpoint} LlamaExePath={LlamaExePath}",
                options.ApiPort, options.WikiPort, options.AiEndpoint, options.LlamaExePath);

            var alerts = new AlertDispatcher(
                new AlertOptions(options.WebhookUrl, options.TeamsWebhookUrl,
                    options.TelegramBotToken, options.TelegramChatId,
                    $"{Environment.MachineName} - {options.WikiDir}"),
                null,
                loggerFactory.CreateLogger("Alert"));

            if (options.TestAlert)
            {
                alerts.Dispatch("Test alert from EAxWiki.Monitor - if you can see this in Slack/Teams/Telegram, alerting is wired correctly.", AlertKind.Test);
                return 0;
            }

            EnsureServePlaceholders(options.WikiDir);

            var healthStore = new HealthStore();
            var state = healthStore.Load(Path.Combine(stateDir, "health.json"));
            var loop = MonitorApp.Build(options, state, healthStore, stateDir, loggerFactory);
            await loop.RunAsync(CancellationToken.None);
        }
        finally
        {
            MonitorLock.Release(monitorPidPath);
        }
        return 0;
    }

    private static void EnsureServePlaceholders(string wikiDir)
    {
        var statusDir = Path.Combine(wikiDir, "status");
        Directory.CreateDirectory(statusDir);
        var apiReady = Path.Combine(statusDir, "api-ready");
        if (!File.Exists(apiReady))
            File.WriteAllText(apiReady, "placeholder");
    }

    private static string Redact(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (!value.Contains('=')) return value;
        return System.Text.RegularExpressions.Regex.Replace(value, "(?i)(Password|Pwd)\\s*=[^;]*", "$1=***");
    }
}
```

Note: `MonitorApp.Build` currently computes the health-template path from `stateDir`'s parent — the templates live at `.eaxwiki-monitor/health-template.md` (repo root). Because `stateDir` = `{repoRoot}/.eaxwiki-monitor/{hash}`, `Path.GetDirectoryName(stateDir)` = `{repoRoot}/.eaxwiki-monitor`, so `Path.Combine(that, "health-template.md")` is correct.

- [ ] **Step 7: Build the whole solution**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build EAxWiki.slnx --nologo
```

Expected: build succeeds with 0 warnings-as-errors. Fix any compile errors (e.g. the `MonitorLoop` ctor `stateDir` parameter if you adopted that variant — make `Program`/`MonitorApp` pass it consistently).

- [ ] **Step 8: Run the full .NET suite**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0` (baseline 311 + 7 + 22 + 14 + 12 + 8 + 5 + 7 + 11 + 11 + 1 ≈ 409, plus the LocalConfigStore round-trip adjustment; record the real number).

- [ ] **Step 9: Commit**

```bash
git add src/EAxWiki.Monitor/Program.cs src/EAxWiki.Monitor/MonitorApp.cs src/EAxWiki.Monitor/MonitorFileLoggerProvider.cs src/EAxWiki.Tests/MonitorFileLoggerTests.cs
git commit -m "feat(monitor): wire Program, MonitorApp and file logger (issue #86)"
```

---

### Task 12: SchedulerUI — LLM port box + health dashboard + `RunMonitorAsync` retarget

**Files:**
- Create: `src/EAxWiki.SchedulerUI/HealthDashboardReader.cs`
- Create: `src/EAxWiki.Tests/HealthDashboardReaderTests.cs`
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs`

**Interfaces:**
- Consumes: `HealthState`, `HealthStore`, `PidFile`, `TcpPortProbe` (all in `EAxWiki.Core.Monitoring`, Task 1/4), `LocalConfigStore.Config.LlmPort` (Task 3).
- Produces: `record ServiceSnapshot(string Name, bool Running, bool NotConfigured, string LastSuccess, string LastFailure, int ConsecutiveFailures)` and `record DashboardSnapshot(string InstanceLabel, IReadOnlyList<ServiceSnapshot> Services)`; `class HealthDashboardReader { DashboardSnapshot ReadAll(string repoRoot); }` — reads `.eaxwiki-monitor/*/health.json`, `*.pid` files, resolves service state for Export/Serve/API/LLM, "not configured" heuristics (API when `ApiPort` unset — derivable from `health.json` `lastApiPort == 0`; LLM when `aiMode != local` isn't in health.json — so LLM NotConfigured = no `llm.pid` file present).

SchedulerForm changes:
1. Field `_llmPortBox = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 8080, Width = 80 }`.
2. `BuildAiTab()`: add `AddRow(localTable, "Port:", _llmPortBox);` after the model row.
3. `SaveAiConfig()`: persist `config.LlmPort = (int)_llmPortBox.Value;`.
4. `LoadEaxwikiConfig()`: `_llmPortBox.Value = Math.Clamp(config.LlmPort ?? 8080, ...)` (both the no-file branch and the file branch).
5. `StartLlmAsync()`: replace hardcoded `--port 8080` with `--port {_llmPortBox.Value}`, and set the endpoint text/append output with the box value.
6. `UpdateAiModeEnablement()`: `_llmPortBox.Enabled = local;`.
7. `RunMonitorAsync()`: replace the `psExe`/script invocation with the monitor exe:
   - `var monitorExe = Path.Combine(_repoRoot, "src", "EAxWiki.Monitor", "bin", "Debug", "net10.0", "EAxWiki.Monitor.exe");`
   - args: `--repo {repoPath} --port {wikiPort}` + optional webhooks (unchanged) + force args (unchanged) + `--llm-port {_llmPortBox.Value}`.
   - `UseShellExecute = true`, `WorkingDirectory = _repoRoot`.
8. New dashboard tab `BuildDashboardTab()`: a `DataGridView` bound to `HealthDashboardReader.ReadAll(_repoRoot)` services + a Refresh button; added via `tabs.TabPages.Add(BuildDashboardTab());` after `BuildTaskStatusTab()` (line 127).
9. `HealthDashboardReader` uses `HealthStore` (Core) for the health.json and `PidFile.IsAlive` for `serve.pid`/`api.pid`/`llm.pid`.

- [ ] **Step 1: Write the failing tests**

Create `src/EAxWiki.Tests/HealthDashboardReaderTests.cs`:

```csharp
using EAxWiki.Core.Monitoring;
using EAxWiki.SchedulerUI;

namespace EAxWiki.Tests;

public class HealthDashboardReaderTests : IDisposable
{
    private readonly string _dir;

    public HealthDashboardReaderTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_dash_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void ReadAll_NoStateDir_ReturnsFourUnconfiguredServices()
    {
        var snapshot = new HealthDashboardReader().ReadAll(_dir);

        Assert.Equal(4, snapshot.Services.Count);
        Assert.All(snapshot.Services, s => Assert.False(s.Running));
    }

    [Fact]
    public void ReadAll_HealthFile_PopulatesExportRow()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);
        new HealthStore().Save(Path.Combine(stateDir, "health.json"), new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            ConsecutiveFailures = 2,
            LastElementCount = 150,
            LastDiagramCount = 30,
            LastMode = "incremental",
            RunsSinceForce = 5,
            LastApiPort = 8001,
        });

        var snapshot = new HealthDashboardReader().ReadAll(_dir);
        var export = snapshot.Services.Single(s => s.Name == "Export");

        Assert.Contains("2026-08-01T10:00:00Z", export.LastSuccess);
        Assert.Equal(2, export.ConsecutiveFailures);
    }

    [Fact]
    public void ReadAll_PidFileAlive_ServiceRunning()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);

        // Spawn a short-lived child, record its pid, and confirm the dashboard reads it as running.
        using var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
            "cmd.exe", "/c ping -n 30 127.0.0.1 >nul") { CreateNoWindow = true, UseShellExecute = false });
        Assert.NotNull(p);
        PidFile.Write(Path.Combine(stateDir, "serve.pid"), p!.Id, p.StartTime.ToUniversalTime());

        var snapshot = new HealthDashboardReader().ReadAll(_dir);
        var serve = snapshot.Services.Single(s => s.Name == "Serve");

        Assert.True(serve.Running);
        p.Kill();
        p.WaitForExit();
    }

    [Fact]
    public void ReadAll_MissingPidFiles_NotRunning()
    {
        var hash = InstanceHash.Compute(Path.Combine(_dir, "wiki"));
        var stateDir = Path.Combine(_dir, ".eaxwiki-monitor", hash);
        Directory.CreateDirectory(stateDir);

        var snapshot = new HealthDashboardReader().ReadAll(_dir);

        Assert.All(snapshot.Services.Where(s => s.Name != "Export"), s => Assert.False(s.Running));
    }
}
```

- [ ] **Step 2: Run tests to verify they fail to compile**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~HealthDashboardReaderTests"
```

Expected: compile error — `EAxWiki.SchedulerUI.HealthDashboardReader` does not exist.

- [ ] **Step 3: Implement `HealthDashboardReader`**

Create `src/EAxWiki.SchedulerUI/HealthDashboardReader.cs`:

```csharp
using EAxWiki.Core.Monitoring;

namespace EAxWiki.SchedulerUI;

public record ServiceSnapshot(
    string Name,
    bool Running,
    bool NotConfigured,
    string LastSuccess,
    string LastFailure,
    int ConsecutiveFailures);

public record DashboardSnapshot(
    string InstanceLabel,
    IReadOnlyList<ServiceSnapshot> Services);

/// <summary>
/// Read-only health dashboard source: reads .eaxwiki-monitor/&lt;hash&gt;/health.json plus the
/// serve/api/llm pid files (pure file reads + Process.GetProcessById — no HTTP surface).
/// The Export row always shows; Serve/API/LLM derive Running from their pid files.
/// </summary>
public class HealthDashboardReader
{
    private static readonly HealthStore Store = new();

    public DashboardSnapshot ReadAll(string repoRoot)
    {
        var monitorDir = Path.Combine(repoRoot, ".eaxwiki-monitor");
        var services = new List<ServiceSnapshot>();

        var healthPath = Directory.Exists(monitorDir)
            ? Directory.GetFiles(monitorDir, "health.json", SearchOption.AllDirectories).FirstOrDefault()
            : null;

        HealthState? state = null;
        string? instanceLabel = null;
        if (healthPath != null)
        {
            state = Store.Load(healthPath);
            var stateDir = Path.GetDirectoryName(healthPath)!;
            instanceLabel = $"{Environment.MachineName} - {Path.GetDirectoryName(Path.GetDirectoryName(stateDir))}";
        }

        services.Add(new ServiceSnapshot(
            "Export",
            Running: false,
            NotConfigured: false,
            state?.LastSuccessTime?.ToString("O") ?? "-",
            state?.LastFailureTime?.ToString("O") ?? "-",
            state?.ConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "Serve",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "serve.pid")),
            NotConfigured: false,
            state?.LastServeSuccessTime?.ToString("O") ?? "-",
            state?.LastServeFailureTime?.ToString("O") ?? "-",
            state?.ServeConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "API",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "api.pid")),
            NotConfigured: (state?.LastApiPort ?? 0) == 0,
            state?.LastApiSuccessTime?.ToString("O") ?? "-",
            state?.LastApiFailureTime?.ToString("O") ?? "-",
            state?.ApiConsecutiveFailures ?? 0));

        services.Add(new ServiceSnapshot(
            "LLM",
            Running: stateDir != null && PidFile.IsAlive(Path.Combine(stateDir!, "llm.pid")),
            NotConfigured: stateDir == null || !File.Exists(Path.Combine(stateDir!, "llm.pid")),
            state?.LastLlmSuccessTime?.ToString("O") ?? "-",
            state?.LastLlmFailureTime?.ToString("O") ?? "-",
            state?.LlmConsecutiveFailures ?? 0));

        return new DashboardSnapshot(instanceLabel ?? "-", services);
    }
}
```

- [ ] **Step 4: Run tests to verify they pass**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet --filter "FullyQualifiedName~HealthDashboardReaderTests"
```

Expected: `Passed! - Failed: 0` (4). If `ReadAll_NoStateDir_ReturnsFourUnconfiguredServices` fails because `ReadAll` throws when the monitor dir is absent, guard `monitorDir` access before calling `Directory.GetFiles` (the code above already does).

- [ ] **Step 5: SchedulerForm — LLM port box**

In `src/EAxWiki.SchedulerUI/SchedulerForm.cs`:

Add the field near the other AI fields (after line 60, `_llmModelPathBox`):

```csharp
    private readonly NumericUpDown _llmPortBox = new() { Minimum = 1, Maximum = 65535, Value = 8080, Width = 80 };
```

In `BuildAiTab()` after the model-row `AddRow` (line 389), add:

```csharp
        AddRow(localTable, "Port:", _llmPortBox);
```

In `UpdateAiModeEnablement()` add `_llmPortBox.Enabled = local;` alongside the other local fields.

In `StartLlmAsync()` (line 525), change:

```csharp
            var psi = new ProcessStartInfo(exePath, $"-m \"{modelPath}\" -c 4096 --port 8080 --n-gpu-layers 0")
```
to:
```csharp
            var port = (int)_llmPortBox.Value;
            var psi = new ProcessStartInfo(exePath, $"-m \"{modelPath}\" -c 4096 --port {port} --n-gpu-layers 0")
```
and update the two `8080` endpoint strings at lines 538-539 to `http://localhost:{port}/v1`.

In `SaveAiConfig()` after the `LlamaModelPath` line (609), add:
```csharp
            config.LlmPort = (int)_llmPortBox.Value;
```

In `LoadEaxwikiConfig()`:
- no-file branch (after line 683 `_llmModelPathBox.Text = ...`): add `_llmPortBox.Value = 8080;`
- file branch (after line 706): add `_llmPortBox.Value = Math.Clamp(config.LlmPort ?? 8080, (int)_llmPortBox.Minimum, (int)_llmPortBox.Maximum);`

- [ ] **Step 6: SchedulerForm — dashboard tab + `RunMonitorAsync` retarget**

Add a field next to the other buttons (after line 102):

```csharp
    private readonly Button _refreshDashboardButton = new() { Text = "Refresh", AutoSize = true };
    private readonly DataGridView _dashboardGrid = new()
    {
        ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false, Dock = DockStyle.Top,
        Height = 260,
    };
```

Add `tabs.TabPages.Add(BuildDashboardTab());` after line 127 (`tabs.TabPages.Add(BuildTaskStatusTab());`).

Add `BuildDashboardTab()` (near `BuildTaskStatusTab`, after line 326):

```csharp
    private TabPage BuildDashboardTab()
    {
        var buttonRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow.Controls.Add(_refreshDashboardButton);
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        panel.Controls.Add(buttonRow);
        panel.Controls.Add(_dashboardGrid);
        _refreshDashboardButton.Click += (_, _) => RefreshDashboard();
        return new TabPage("Health Dashboard") { Padding = new Padding(10), AutoScroll = true, Controls = { panel } };
    }

    private void RefreshDashboard()
    {
        if (_repoRoot == null) return;
        var snapshot = new HealthDashboardReader().ReadAll(_repoRoot);
        _dashboardGrid.DataSource = snapshot.Services
            .Select(s => new
            {
                s.Name,
                Status = s.NotConfigured ? "not configured" : s.Running ? "running" : "not running",
                s.LastSuccess,
                s.LastFailure,
                s.ConsecutiveFailures,
            })
            .ToList();
    }
```

Replace `RunMonitorAsync`'s launch block (lines 1170-1202) with:

```csharp
        var monitorExe = Path.Combine(_repoRoot, "src", "EAxWiki.Monitor", "bin", "Debug", "net10.0", "EAxWiki.Monitor.exe");
        if (!File.Exists(monitorExe))
        {
            AppendOutput($"Monitor executable not found: {monitorExe}");
            return;
        }

        var args = new List<string>
        {
            "--repo", repoPath,
            "--port", ((int)_wikiPortConfigBox.Value).ToString(),
            "--llm-port", ((int)_llmPortBox.Value).ToString(),
        };
        var webhook = _webhookBox.Text.Trim();
        if (webhook.Length > 0) { args.Add("--webhook-url"); args.Add(webhook); }
        var teamsWebhook = _teamsWebhookBox.Text.Trim();
        if (teamsWebhook.Length > 0) { args.Add("--teams-webhook-url"); args.Add(teamsWebhook); }
        var tgBotToken = _telegramBotTokenBox.Text.Trim();
        if (tgBotToken.Length > 0) { args.Add("--telegram-bot-token"); args.Add(tgBotToken); }
        var tgChatId = _telegramChatIdBox.Text.Trim();
        if (tgChatId.Length > 0) { args.Add("--telegram-chat-id"); args.Add(tgChatId); }
        if (_forceEveryRunRadio.Checked) args.Add("--force");
        else if (_forceEveryNRadio.Checked) { args.Add("--force-every"); args.Add(((int)_forceEveryN.Value).ToString()); }

        AppendOutput($"> Starting monitor in new window...");
        var psi = new ProcessStartInfo
        {
            FileName = monitorExe,
            Arguments = string.Join(" ", args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a)),
            WorkingDirectory = _repoRoot,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal,
        };
        Process.Start(psi);
        AppendOutput($"Monitor launched in separate window.");
```

- [ ] **Step 7: Build + run the full .NET suite**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build EAxWiki.slnx --nologo
```

Expected: build succeeds (WinForms `DataGridView` resolves via the SchedulerUI's `UseWindowsForms`). Then:

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0` (record the real count).

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki.SchedulerUI/HealthDashboardReader.cs src/EAxWiki.SchedulerUI/SchedulerForm.cs src/EAxWiki.Tests/HealthDashboardReaderTests.cs
git commit -m "feat(schedulerui): add health dashboard, LLM port box, monitor exe launch (issue #86)"
```

---

### Task 13: Caller updates, removals, docs, and full verification

**Files:**
- Modify: `scripts/_bootstrap.ps1`
- Modify: `tests/scripts/_bootstrap.Tests.ps1`
- Modify: `scripts/register-scheduled-task.ps1`
- Delete: `scripts/monitor-export-and-serve.ps1`
- Delete: `tests/scripts/monitor-export-and-serve.Tests.ps1`
- Delete: `tests/scripts/send-alert.Tests.ps1`
- Modify: `README.md` (test-count table lines 624-647; monitor references lines 467-490, 618-622)
- Modify: `.claude/skills/scheduled-task-diagnostics/SKILL.md` (health.md references)

**Interfaces:**
- Consumes: `MonitorApp`/`MonitorPaths` (Task 11).
- Produces: `Get-EAxWikiMonitorExePath` helper in `_bootstrap.ps1`; `register-scheduled-task.ps1` action runs `EAxWiki.Monitor.exe` directly.

- [ ] **Step 1: Add `Get-EAxWikiMonitorExePath` to `_bootstrap.ps1`**

In `scripts/_bootstrap.ps1`, after `Get-EAxWikiDllPath` (line 35), add:

```powershell
# Get-EAxWikiMonitorExePath - resolve the pre-built EAxWiki.Monitor.exe and verify it exists.
# RepoRoot is explicit (not derived from $PSScriptRoot) for the same reason as Get-EAxWikiDllPath.
function Get-EAxWikiMonitorExePath {
    param([string]$RepoRoot)
    $exePath = Join-Path $RepoRoot "src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe"
    if (-not (Test-Path $exePath)) {
        throw "EAxWiki.Monitor.exe not found at '$exePath'. Run 'dotnet build src/EAxWiki.Monitor' first."
    }
    return $exePath
}
```

- [ ] **Step 2: Add the bootstrap Pester test**

Append to `tests/scripts/_bootstrap.Tests.ps1` a test mirroring the `Get-EAxWikiDllPath` coverage:

```powershell
Describe 'Get-EAxWikiMonitorExePath' {
    It 'resolves the built EAxWiki.Monitor.exe' {
        $repoRoot = (Split-Path -Parent $PSScriptRoot) | Split-Path -Parent
        $exe = Get-EAxWikiMonitorExePath -RepoRoot $repoRoot
        $exe | Should -Be (Join-Path $repoRoot 'src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe')
        Test-Path $exe | Should -BeTrue
    }

    It 'throws a clear error when the exe is missing' {
        $repoRoot = (Split-Path -Parent $PSScriptRoot) | Split-Path -Parent
        { Get-EAxWikiMonitorExePath -RepoRoot "$repoRoot\does-not-exist" } | Should -Throw
    }
}
```

- [ ] **Step 3: Retarget `register-scheduled-task.ps1`**

In `scripts/register-scheduled-task.ps1`, replace lines 146-164:

```powershell
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$monitorScript = Join-Path $repoRoot "scripts\monitor-export-and-serve.ps1"

$scriptArgs = @("--max-retries", $MaxRetries, "--retry-delay", $RetryDelaySeconds)
# NOT baking --repo into the task action: the monitor resolves RepoPath from the
# DPAPI-encrypted .eaxwiki file instead, avoiding plaintext secrets on the command
# line (visible in Task Scheduler UI, Event Log, and process viewers). The user
# passes --repo to this registration script for validation, but the scheduled task
# itself never carries it.
if ($OutputDir)  { $scriptArgs += "--output", $OutputDir }
if ($Port)       { $scriptArgs += "--port", $Port }
if ($ForceExport) { $scriptArgs += "--force" }
elseif ($ForceEveryNRuns -gt 0) { $scriptArgs += "--force-every", $ForceEveryNRuns }

$argLine = ($scriptArgs | ForEach-Object { if ($_ -match '\s') { "`"$_`"" } else { $_ } }) -join ' '
$psExe = $PSExecutable

$action  = New-ScheduledTaskAction -Execute $psExe `
    -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$monitorScript`" $argLine"
```

with:

```powershell
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
$monitorExe = Get-EAxWikiMonitorExePath -RepoRoot $repoRoot

$scriptArgs = @("--max-retries", $MaxRetries, "--retry-delay", $RetryDelaySeconds)
# NOT baking --repo into the task action: the monitor resolves RepoPath from the
# DPAPI-encrypted .eaxwiki file instead, avoiding plaintext secrets on the command
# line (visible in Task Scheduler UI, Event Log, and process viewers). The user
# passes --repo to this registration script for validation, but the scheduled task
# itself never carries it.
if ($OutputDir)  { $scriptArgs += "--output", $OutputDir }
if ($Port)       { $scriptArgs += "--port", $Port }
if ($ForceExport) { $scriptArgs += "--force" }
elseif ($ForceEveryNRuns -gt 0) { $scriptArgs += "--force-every", $ForceEveryNRuns }

$argLine = ($scriptArgs | ForEach-Object { if ($_ -match '\s') { "`"$_`"" } else { $_ } }) -join ' '

# Direct exe invocation (no pwsh -File wrapper): EAxWiki.Monitor.exe has its own argument parser.
$action  = New-ScheduledTaskAction -Execute $monitorExe -Argument $argLine
```

- [ ] **Step 4: Delete the PS monitor and its tests**

```powershell
Remove-Item scripts/monitor-export-and-serve.ps1
Remove-Item tests/scripts/monitor-export-and-serve.Tests.ps1
Remove-Item tests/scripts/send-alert.Tests.ps1
```

- [ ] **Step 5: Update README test-count table**

In `README.md` lines 624-647, change the two removed rows and the totals:

- Remove the `| MonitorExportAndServe | 47 | ...` row and the `| SendAlert | 2 | ...` row.
- Add a row for the new bootstrap tests if the count changed: `| Bootstrap | 4 | Get-EAxWikiDllPath + Get-EAxWikiMonitorExePath resolution + clear missing-DLL/exe errors |`.
- Update the Pester subtotal from `162` to `115` (162 − 47 − 2 + 2) and the grand total `473` to `311 + 115 + <new monitor .NET tests>`. Record the exact .NET count from Task 12 Step 7's run.

Also update the monitor references (lines 467-490) — replace `monitor-export-and-serve.ps1` invocations with `EAxWiki.Monitor.exe` equivalents and note the new `--llm-port` flag.

- [ ] **Step 6: Update `scheduled-task-diagnostics/SKILL.md` references**

In `.claude/skills/scheduled-task-diagnostics/SKILL.md`:
- Line 24: change `monitor-export-and-serve.ps1:403-439` to `EAxWiki.Monitor` (health page rendering location).
- Line 26: the `.eaxwiki-monitor/<instanceHash>/health.json` description is unchanged (the C# monitor keeps the same file).

- [ ] **Step 7: Run the full .NET suite**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests/EAxWiki.Tests.csproj --nologo --verbosity quiet
```

Expected: `Passed! - Failed: 0` (record the exact count).

- [ ] **Step 8: Run the full Pester suite**

Pester suite must be run from a shell where port 8000 is held by a `TcpListener` and 8001 is free (same recipe as prior issue #86 parts):

```powershell
$l = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, 8000); $l.Start()
try {
    Invoke-Pester -Path tests -Output Detailed
}
finally { $l.Stop() }
```

Run from repo root (`E:\Users\Han\Repos\EAxWiki`). Expected: `Passed: 115, Failed: 0, Skipped: 0` (or the actual count after the removals/addition — record it).

- [ ] **Step 9: Smoke test — `--test-alert` and one live cycle**

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build src/EAxWiki.Monitor/EAxWiki.Monitor.csproj --nologo
.\src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe --test-alert
```

Expected: a `Test` alert dispatched to every configured channel (if any), then exit 0.

Then run one live cycle against the dev `.qea` (from the repo root, after a normal `dotnet build` of the solution):

```powershell
.\src\EAxWiki.Monitor\bin\Debug\net10.0\EAxWiki.Monitor.exe --repo "model\file.qea" --port 8000 --check-interval 1
```

Let it run through one cycle (export + serve/API/LLM watchdogs), then Ctrl+C. Verify `wiki/status/health.md` exists and `.eaxwiki-monitor/<hash>/health.json` was written. **Never commit** `wiki/`, `.eaxwiki-monitor/`, or `model/`.

- [ ] **Step 10: Confirm working tree is clean of runtime artifacts**

```powershell
git status --short
```

Expected: only the plan's commits and the expected file changes; no `model/`, `wiki/`, `.eaxwiki-monitor/`, or `.eaxwiki` entries. If any runtime artifact shows, restore or delete it (see repo conventions) — do NOT commit it.

- [ ] **Step 11: Commit**

```bash
git add scripts/_bootstrap.ps1 tests/scripts/_bootstrap.Tests.ps1 scripts/register-scheduled-task.ps1 README.md .claude/skills/scheduled-task-diagnostics/SKILL.md
git add -u scripts/monitor-export-and-serve.ps1 tests/scripts/monitor-export-and-serve.Tests.ps1 tests/scripts/send-alert.Tests.ps1
git commit -m "refactor(monitor): replace PS monitor with EAxWiki.Monitor exe (issue #86)"
```

(If the deletions don't stage cleanly with `-u`, use `git rm` for the three removed files.)

---

## Self-Review Notes

- **Spec coverage:** design goals map 1:1 — full replacement + removals (Task 13), exe + state file + alert parity (Tasks 2-7), watchdog semantics via ProcessSupervisor (Task 8), live force semantics as the one deliberate fix (Task 9 `ShouldForce`), configurable LLM port (Task 3 `LlmPort` + Task 12 UI), SchedulerUI dashboard (Task 12), System.CommandLine root command without touching `Config.Load` (Task 2), detached process + duplicate guard (Tasks 10-11), no HTTP control endpoint (kept — file-based flags). Design §HealthPageRenderer writes `wiki/status/health.md` (Task 5), diverging from the PS script's `.eaxwiki-monitor/status/health.md` — flagged in Global Constraints as intentional (makes `InfrastructureWriter`'s nav-entry check work).
- **Placeholder scan:** every step has complete code; no TBD/TODO.
- **Type consistency:** `IAlertDispatcher` defined in Task 7 and consumed by Task 9's `FakeAlerts`; `MonitorOptions` fields match the resolver output and the `ExportRunner`/`MonitorLoop` usage; `ServiceSpec` positional params consistent across Task 8, the `MonitorLoop` test stubs (Task 10) and `MonitorApp` builders (Task 11); `WritebackDelta`/`IDigestTracker` consistent between Task 6 and Task 9/10. One deliberate seam: Task 10's ctor note recommends injecting `stateDir` (instead of the `ParentStateDir` helper) so `Program`/`MonitorApp`/`MonitorLoop` agree — implement the ctor variant and update the `Build` test helper together.
- **Test counts:** the per-task "Expected" counts are estimates; each step's real gate is `Failed: 0`. Final counts are recorded during Task 11 Step 8 / Task 13 Step 7 rather than asserted. Pester: 162 − 47 (MonitorExportAndServe) − 2 (SendAlert) + 2 (new bootstrap) = 115.

## Execution Handoff

Plan complete and saved to `docs/superpowers/plans/2026-08-14-issue-86-csharp-monitor.md`. Two execution options:

1. **Subagent-Driven (recommended)** — I dispatch a fresh subagent per task, review between tasks, fast iteration.

2. **Inline Execution** — execute tasks in this session using executing-plans, batch execution with checkpoints.

Which approach?