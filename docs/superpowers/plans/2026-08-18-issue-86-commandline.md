# Plan: Part 4 — System.CommandLine arg parser + PowerShell wrapper overhaul (issue #86, item #4)

Status: ready to execute.
Spec: `docs/superpowers/specs/2026-08-18-issue-86-commandline-design.md` (approved).

## Goal

Replace the hand-rolled `Config.Load` switch in `src/EAxWiki/Config.cs` (lines 25–129) with System.CommandLine 2.0.11 (same version already used by `EAxWiki.Monitor`), and slim the four wrapper scripts (`export.ps1`, `writeback.ps1`, `export-and-serve.ps1`, `serve-api.ps1`) so they stop re-parsing and re-building the flag list. `serve.ps1` is untouched. Unknown/typo'd flags become hard parse errors (exit 1). `--help` becomes SCL auto-generated help with the connection-string examples moved into the root command description.

## Conventions (apply to every step)

- **Line endings:** LF, UTF-8 no BOM, no trailing whitespace.
- **dotnet commands** must carry the EA COM interop path inline:
  `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet ...`
  (Some EAxWiki.Tests tests reference `Interop.EA` via `$(EAPath)`.)
- **Commit messages:** lowercase conventional commits, exact, one per task, e.g. `refactor(parser): replace Config.Load switch with System.CommandLine (issue #86)`.
- **Never stage** `model/`, `wiki/`, `.eaxwiki-monitor/`, `.eaxwiki`, `**/bin/`, `**/obj/`.
- Working tree is already on `master`; `git status` must be clean before and after each task (except the task's own changes).
- All PowerShell code in the plan is deliberately written so Pester tests can `Contains(...)`-match the exact substrings shown.

## Verified environment facts (do not rediscover)

- System.CommandLine 2.0.11 API (confirmed by reflection against the local NuGet cache):
  - `Option<T>` ctor `(string name, params string[] aliases)`; settable `Description`; mutable `Validators` (`List<Action<OptionResult>>`).
  - `OptionResult.GetValueOrDefault<T>()`, `OptionResult.AddError(string)` (inherited from `SymbolResult`).
  - `ParseResult.Errors`, `ParseResult.Action`, `ParseResult.GetValue<T>(Option<T>)`, `ParseResult.GetValue<T>(Argument<T>)`.
  - `Command.Parse(IReadOnlyList<string>)` → `ParseResult`; `Command.TreatUnmatchedTokensAsErrors` is settable.
  - `Command.Add(Option)` / `Command.Add(Argument)`; `Command` collection-initializer works for `{ Argument<string?>, Option<string?>, ... }`.
  - `RootCommand(string description)` auto-registers help aliases `--help`, `-h`, `/h`, `-?`, `/?`.
  - `HelpAction` lives in `System.CommandLine.Help` and `ParseResult.Action is HelpAction` detects `--help`.
  - `ParseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None)` prints parse errors (stderr) or help (stdout) exactly as `EAxWiki.Monitor/Program.cs` already does.
- SCL 2.0.11 runtime behaviors (all empirically verified with a throwaway console app against 2.0.11):
  - A bare `Argument<string?>` defaults to `ExactlyOne` arity — calling `ParseResult.GetValue(argument)` when the token is absent **throws** "Required argument missing". It must be constructed with `{ Arity = ArgumentArity.ZeroOrOne }` so absent → `null`, and that also avoids a "required argument" parse error for no-arg invocations.
  - **Unknown option-like tokens are NOT errors and NOT unmatched when a bare argument exists — the argument swallows them** (`--froce` → repo=`--froce`, zero errors, even with `TreatUnmatchedTokensAsErrors = true`). To honor decision #2 (unknown/typo'd flags → hard error), the repo argument needs its own validator rejecting values that start with `-` → produces `Unknown option '--froce'.` / `Unknown option '-x'.`.
  - Repeated single-valued options are **parse errors**, not last-wins: `--output a --output b` → `Option '--output' expects a single argument but 2 were provided.` (same for `--api-port`). This is a deliberate behavior change vs. the old last-wins `Config.Load`; document it in the deviations note.
  - Calling `OptionResult.GetValueOrDefault<int?>()` inside a validator **throws** (does not add an error) when the token is non-numeric (`--api-port abc` → `Cannot parse argument 'abc' ...`). Port range validation must therefore parse the raw token: `result.Tokens` / `SymbolResult.Tokens` are public, so use `result.Tokens[^1].Value` + `int.TryParse` and only `AddError` for parseable-but-out-of-range values. Non-numeric values already produce a clean SCL conversion error in `ParseResult.Errors`.
  - Validators only run when the option was matched, and only add errors — no throw — as long as they don't call `GetValueOrDefault`.
- `EaRepository.Redact` (in `EAxWiki.Core`) redacts `Password`, `Pwd`, `User Id`, `Uid`, `User Name`, `Username` (case-insensitive) plus Oracle parenthesized forms — strictly richer than the old `writeback.ps1` display regex.
- The C# monitor never invokes `export.ps1` (exports run in-process via `StaMarkdownExporter`); `MonitorApp.BuildApiSpec` runs the exe directly with only exe flags (`--api --api-port --wiki-port --output [--repo]`). Nothing passes `--force-every` or other monitor-only flags to `export.ps1`. The Part 4 wrapper changes cannot break the monitor.
- `register-scheduled-task.ps1` feeds `EAxWiki.Monitor.exe` (not `export.ps1`) and normalizes `-RepoPath`/`-OutputDir`/`-Port` to canonical `--output`/`--port`; it is out of scope and unaffected. `serve.ps1`, `Validate-WikiOutput.ps1` also out of scope.
- `Config.Load` is referenced only by `Program.cs` and `ConfigTests.cs`. `HealthStore.Load` / `LocalConfigStore.Load` are unrelated.
- Pester recipe: hold port 8000 with a `TcpListener` bound to `[System.Net.IPAddress]::Any` (`0.0.0.0:8000`) — `Loopback`/`127.0.0.1` does NOT stop mkdocs on this box. Trailing mkdocs bind noise is expected.
- Known flakes (pass on rerun): `PropertyBasedTests.EscapeCell_LengthAtLeastInputLength` (CRLF seeds); `Export_StatusEditorScript` (parallel interference).

## Testing approach

- Task 1 is strict TDD: write `CommandLineTests.cs` first (fails to compile → RED), implement `CommandLine.cs` (GREEN), then delete `Config.Load`/`ParsePort`/`HelpRequested` and rewire `Program.cs`.
- Tasks 2–3: scripts + their Pester files are rewritten together; run the affected Pester files in isolation before the full suite.
- Every task ends with the relevant verification command and a commit.

---

## Task 1 — System.CommandLine in EAxWiki (C#)

Files:
- `src/EAxWiki/EAxWiki.csproj` (add package ref)
- `src/EAxWiki.Tests/CommandLineTests.cs` (new — replaces `ConfigTests.cs`)
- `src/EAxWiki.Tests/ConfigTests.cs` (delete)
- `src/EAxWiki/CommandLine.cs` (new)
- `src/EAxWiki/Config.cs` (slim)
- `src/EAxWiki/Program.cs` (rewire + delete `ShowUsage`)

### Step 1 — add the package reference (RED setup)

Edit `src/EAxWiki/EAxWiki.csproj` so the existing `PackageReference` item group becomes:

```xml
  <ItemGroup>
    <!-- DPAPI wrapper used by LocalConfigStore to encrypt the saved .eaxwiki connection string at rest -->
    <PackageReference Include="System.Security.Cryptography.ProtectedData" Version="10.0.11" />
    <PackageReference Include="System.CommandLine" Version="2.0.11" />
  </ItemGroup>
```

### Step 2 — write the failing tests (RED)

Delete `src/EAxWiki.Tests/ConfigTests.cs` and create `src/EAxWiki.Tests/CommandLineTests.cs`:

```csharp
using System.CommandLine;
using System.CommandLine.Help;
using EAxWiki;

namespace EAxWiki.Tests;

public class CommandLineTests
{
    private static Config Parse(params string[] args) =>
        CommandLine.ToConfig(CommandLine.BuildCommand().Parse(args));

    private static ParseResult ParseResult(params string[] args) =>
        CommandLine.BuildCommand().Parse(args);

    [Fact]
    public void NoArgs_AllDefaults()
    {
        var cfg = Parse();
        Assert.Equal("", cfg.RepositoryPath);
        Assert.Equal("wiki", cfg.OutputPath);
        Assert.Null(cfg.RepositoryName);
        Assert.Null(cfg.PackageFilter);
        Assert.False(cfg.Verbose);
        Assert.False(cfg.Force);
        Assert.False(cfg.JsonExport);
        Assert.False(cfg.WriteBack);
        Assert.False(cfg.ApiMode);
        Assert.Equal(0, cfg.ApiPort);
        Assert.Equal(0, cfg.WikiPort);
        Assert.Equal(60, cfg.ApiRateLimitPerMinute);
        Assert.Equal("", cfg.Brand);
        Assert.Equal("", cfg.AiEndpoint);
        Assert.Equal("llama-3.2-3b", cfg.AiModel);
        Assert.Equal("", cfg.AiKey);
        Assert.Null(cfg.CertPath);
        Assert.Null(cfg.CertPassword);
    }

    [Theory]
    [InlineData("-r")]
    [InlineData("--repo")]
    public void RepoFlag_SetsRepositoryPath(string flag)
    {
        Assert.Equal(@"C:\model.qea", Parse(flag, @"C:\model.qea").RepositoryPath);
    }

    [Fact]
    public void BarePositionalRepo_SetsRepositoryPath()
    {
        Assert.Equal("model.qea", Parse("model.qea").RepositoryPath);
    }

    [Fact]
    public void BarePositionalConnectionString_SetsRepositoryPath()
    {
        Assert.Equal("DBType=postgresql;Database=foo", Parse("DBType=postgresql;Database=foo").RepositoryPath);
    }

    [Fact]
    public void RepoFlag_WinsOverBarePositional()
    {
        Assert.Equal("a.qea", Parse("b.qea", "--repo", "a.qea").RepositoryPath);
    }

    [Fact]
    public void EmptyRepoValue_StaysEmpty()
    {
        Assert.Equal("", Parse("--repo", "").RepositoryPath);
    }

    [Theory]
    [InlineData("-o")]
    [InlineData("--output")]
    public void OutputFlag_SetsOutputPath(string flag)
    {
        Assert.Equal("output", Parse(flag, "output").OutputPath);
    }

    [Theory]
    [InlineData("-n")]
    [InlineData("--name")]
    public void NameFlag_SetsRepositoryName(string flag)
    {
        Assert.Equal("MyRepo", Parse(flag, "MyRepo").RepositoryName);
    }

    [Theory]
    [InlineData("-p")]
    [InlineData("--package")]
    public void PackageFlag_SetsPackageFilter(string flag)
    {
        Assert.Equal("ArchiMate", Parse(flag, "ArchiMate").PackageFilter);
    }

    [Theory]
    [InlineData("-f")]
    [InlineData("--force")]
    public void ForceFlags_SetForce(string flag) => Assert.True(Parse(flag).Force);

    [Theory]
    [InlineData("-v")]
    [InlineData("--verbose")]
    public void VerboseFlags_SetVerbose(string flag) => Assert.True(Parse(flag).Verbose);

    [Theory]
    [InlineData("-j")]
    [InlineData("--json")]
    public void JsonFlags_SetJsonExport(string flag) => Assert.True(Parse(flag).JsonExport);

    [Theory]
    [InlineData("-w")]
    [InlineData("--writeback")]
    public void WritebackFlags_SetWriteBack(string flag) => Assert.True(Parse(flag).WriteBack);

    [Fact]
    public void MissingValue_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--repo").Errors);
    }

    [Fact]
    public void UnknownFlag_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--unknown-flag").Errors);
    }

    [Theory]
    [InlineData("--froce")]
    [InlineData("-x")]
    public void TypoFlag_IsParseError(string flag)
    {
        Assert.NotEmpty(ParseResult(flag).Errors);
    }

    [Fact]
    public void ApiFlag_SetsApiModeWithDefaultPort()
    {
        var cfg = Parse("--api");
        Assert.True(cfg.ApiMode);
        Assert.Equal(8001, cfg.ApiPort);
    }

    [Fact]
    public void ApiPortWithoutApi_NoAutoApiMode()
    {
        var cfg = Parse("--api-port", "9000");
        Assert.Equal(9000, cfg.ApiPort);
        Assert.False(cfg.ApiMode);
    }

    [Fact]
    public void ApiWithApiPort_SetsBoth()
    {
        var cfg = Parse("--api", "--api-port", "9000");
        Assert.True(cfg.ApiMode);
        Assert.Equal(9000, cfg.ApiPort);
    }

    [Fact]
    public void WikiPortFlag_SetsWikiPort()
    {
        Assert.Equal(8080, Parse("--wiki-port", "8080").WikiPort);
    }

    [Theory]
    [InlineData("--api-port", "0")]
    [InlineData("--api-port", "65536")]
    [InlineData("--api-port", "abc")]
    [InlineData("--wiki-port", "0")]
    [InlineData("--wiki-port", "99999")]
    public void InvalidPorts_AreParseErrors(string flag, string value)
    {
        Assert.NotEmpty(ParseResult(flag, value).Errors);
    }

    [Fact]
    public void BrandFlag_SetsBrand()
    {
        Assert.Equal("eursura", Parse("--brand", "eursura").Brand);
    }

    [Fact]
    public void CertAndAiFlags_SetValues()
    {
        var cfg = Parse(
            "--cert", "cert.pfx", "--cert-password", "secret",
            "--ai-endpoint", "http://localhost:11434/v1", "--ai-model", "gpt-x", "--ai-key", "k");
        Assert.Equal("cert.pfx", cfg.CertPath);
        Assert.Equal("secret", cfg.CertPassword);
        Assert.Equal("http://localhost:11434/v1", cfg.AiEndpoint);
        Assert.Equal("gpt-x", cfg.AiModel);
        Assert.Equal("k", cfg.AiKey);
    }

    [Fact]
    public void AllFlagsTogether_ParsesCorrectly()
    {
        var cfg = Parse("--repo", "r", "--output", "out", "-f", "-v", "--json",
            "--writeback", "--api", "--api-port", "9001", "--wiki-port", "8080",
            "--package", "pkg1", "--name", "MyRepo", "--brand", "eursura");
        Assert.Equal("r", cfg.RepositoryPath);
        Assert.Equal("out", cfg.OutputPath);
        Assert.Equal("pkg1", cfg.PackageFilter);
        Assert.Equal("MyRepo", cfg.RepositoryName);
        Assert.True(cfg.Force);
        Assert.True(cfg.Verbose);
        Assert.True(cfg.JsonExport);
        Assert.True(cfg.WriteBack);
        Assert.True(cfg.ApiMode);
        Assert.Equal(9001, cfg.ApiPort);
        Assert.Equal(8080, cfg.WikiPort);
        Assert.Equal("eursura", cfg.Brand);
    }

    [Fact]
    public void DuplicateOutput_IsParseError()
    {
        // SCL 2.0.11 rejects repeated single-valued options (old Config.Load silently took the last).
        Assert.NotEmpty(ParseResult("--output", "wiki1", "--output", "wiki2").Errors);
    }

    [Fact]
    public void DuplicateApiPort_IsParseError()
    {
        Assert.NotEmpty(ParseResult("--api-port", "8000", "--api-port", "9000").Errors);
    }

    [Fact]
    public void DuplicateBareRepo_IsParseError()
    {
        Assert.NotEmpty(ParseResult("a.qea", "b.qea").Errors);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    [InlineData("-?")]
    [InlineData("/?")]
    public void HelpFlags_AreHelpActions(string flag)
    {
        var r = ParseResult(flag);
        Assert.Empty(r.Errors);
        Assert.IsType<HelpAction>(r.Action);
    }
}
```

Run the .NET build + tests to confirm RED (compile failure — `CommandLine` does not exist yet):

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build src/EAxWiki.Tests
```

### Step 3 — implement `src/EAxWiki/CommandLine.cs` (GREEN)

```csharp
using System.CommandLine;

namespace EAxWiki;

public static class CommandLine
{
    private static readonly Argument<string?> RepoArg = new("repo")
    {
        Description = "Path to a .qea file, or a DB connection string. Omit to enter interactive connection builder.",
        // Default arity for string? is ExactlyOne (GetValue throws "Required argument missing" when absent);
        // ZeroOrOne lets no-arg invocations parse cleanly and keeps GetValue -> null.
        Arity = ArgumentArity.ZeroOrOne,
    };

    private static readonly Option<string?> Repo = new("--repo", "-r")
    {
        Description = "Path to a .qea file, or a DB connection string (takes precedence over a bare positional repo).",
    };
    private static readonly Option<string?> Name = new("--name", "-n")
    {
        Description = "Display name for the repository.",
    };
    private static readonly Option<string?> Output = new("--output", "-o")
    {
        Description = "Output directory for the wiki (default: wiki).",
    };
    private static readonly Option<string?> Package = new("--package", "-p")
    {
        Description = "Only export a specific package (by name).",
    };
    private static readonly Option<bool> Verbose = new("--verbose", "-v")
    {
        Description = "Enable verbose logging per-element timing.",
    };
    private static readonly Option<bool> Force = new("--force", "-f")
    {
        Description = "Force full regeneration (rebuild all files).",
    };
    private static readonly Option<bool> Json = new("--json", "-j")
    {
        Description = "Also export model.json alongside markdown.",
    };
    private static readonly Option<bool> WriteBack = new("--writeback", "-w")
    {
        Description = "Scan wiki for status changes and write them back to EA via COM.",
    };
    private static readonly Option<bool> Api = new("--api")
    {
        Description = "Start wiki write-back server for in-wiki status editing.",
    };
    private static readonly Option<int?> ApiPort = new("--api-port")
    {
        Description = "Port for the wiki write-back server (default: 8001 when --api is given).",
    };
    private static readonly Option<int?> WikiPort = new("--wiki-port")
    {
        Description = "Port the paired 'mkdocs serve' uses (default: 8000); --api only accepts requests whose Origin matches this port.",
    };
    private static readonly Option<string?> Cert = new("--cert")
    {
        Description = "Path to PFX certificate for HTTPS.",
    };
    private static readonly Option<string?> CertPassword = new("--cert-password")
    {
        Description = "PFX certificate password.",
    };
    private static readonly Option<string?> AiEndpoint = new("--ai-endpoint")
    {
        Description = "OpenAI-compatible API base URL (empty = AI suggestions disabled).",
    };
    private static readonly Option<string?> AiModel = new("--ai-model")
    {
        Description = "Model name sent to AI endpoint (default: llama-3.2-3b).",
    };
    private static readonly Option<string?> AiKey = new("--ai-key")
    {
        Description = "API key for AI endpoint (optional for local LLMs).",
    };
    private static readonly Option<string?> Brand = new("--brand")
    {
        Description = "Brand theme to emit (eursura); default: none.",
    };

    private const string Description = """
        EAxWiki - Sparx EA Repository to Wiki Generator

        Connection string examples:
          SQL Server:  DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=SERVER;Initial Catalog=EA;Integrated Security=SSPI;
          MySQL:       DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;
          MariaDB:     DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;
          Oracle:      DBType=2;Connect=Data Source=TNSNAME;User Id=user;Password=pass;
          PostgreSQL:  DBType=7;Connect=Server=localhost;Database=EA;User Id=user;Password=pass;
        """;

    public static RootCommand BuildCommand()
    {
        AddPortValidation(ApiPort);
        AddPortValidation(WikiPort);
        AddRepoArgValidation(RepoArg);

        var root = new RootCommand(Description)
        {
            RepoArg, Repo, Name, Output, Package, Verbose, Force, Json, WriteBack,
            Api, ApiPort, WikiPort, Cert, CertPassword, AiEndpoint, AiModel, AiKey, Brand,
        };
        // A bare positional repo is the only legitimate non-option token; anything else is a typo.
        root.TreatUnmatchedTokensAsErrors = true;
        return root;
    }

    public static Config ToConfig(ParseResult r)
    {
        var api = r.GetValue(Api);
        var apiPort = r.GetValue(ApiPort);
        return new Config
        {
            RepositoryPath = r.GetValue(Repo) ?? r.GetValue(RepoArg) ?? "",
            RepositoryName = r.GetValue(Name),
            OutputPath = r.GetValue(Output) ?? "wiki",
            PackageFilter = r.GetValue(Package),
            Verbose = r.GetValue(Verbose),
            Force = r.GetValue(Force),
            JsonExport = r.GetValue(Json),
            WriteBack = r.GetValue(WriteBack),
            ApiMode = api,
            ApiPort = apiPort ?? (api ? 8001 : 0),
            WikiPort = r.GetValue(WikiPort) ?? 0,
            CertPath = r.GetValue(Cert),
            CertPassword = r.GetValue(CertPassword),
            AiEndpoint = r.GetValue(AiEndpoint) ?? "",
            AiModel = r.GetValue(AiModel) ?? "llama-3.2-3b",
            AiKey = r.GetValue(AiKey) ?? "",
            Brand = r.GetValue(Brand) ?? "",
        };
    }

    private static void AddPortValidation(Option<int?> opt)
    {
        opt.Validators.Add(result =>
        {
            // Read the raw token, not GetValueOrDefault: the latter THROWS on non-numeric input,
            // while SCL already records a clean "Cannot parse argument" error for that case.
            var token = result.Tokens.Count > 0 ? result.Tokens[^1].Value : null;
            if (token is not null && int.TryParse(token, out var value) && value is < 1 or > 65535)
                result.AddError($"Option {opt.Name} requires an integer port in 1-65535 (got '{value}').");
        });
    }

    private static void AddRepoArgValidation(Argument<string?> arg)
    {
        // Without this, an unknown option-like token is swallowed by the bare argument as its value
        // (verified: '--froce' parsed with zero errors and repo='--froce'), defeating decision #2.
        arg.Validators.Add(result =>
        {
            var token = result.Tokens.Count > 0 ? result.Tokens[^1].Value : null;
            if (token is not null && token.StartsWith("-", StringComparison.Ordinal))
                result.AddError($"Unknown option '{token}'.");
        });
    }
}
```

### Step 4 — slim `src/EAxWiki/Config.cs`

Delete `HelpRequested` (line 9), `Load(...)` (lines 25–122), and `ParsePort(...)` (lines 124–129). The file becomes:

```csharp
namespace EAxWiki;

public class Config
{
    public string RepositoryPath { get; set; } = string.Empty;
    public string? RepositoryName { get; set; }
    public string OutputPath { get; set; } = "wiki";
    public string? PackageFilter { get; set; }
    public bool Verbose { get; set; }
    public bool Force { get; set; }
    public bool JsonExport { get; set; }
    public bool WriteBack { get; set; }
    public bool ApiMode { get; set; }
    public int ApiPort { get; set; } = 0;
    public int WikiPort { get; set; } = 0;
    public int ApiRateLimitPerMinute { get; set; } = 60;
    public string? CertPath { get; set; }
    public string? CertPassword { get; set; }
    public string AiEndpoint { get; set; } = "";
    public string AiModel { get; set; } = "llama-3.2-3b";
    public string AiKey { get; set; } = "";
    public string Brand { get; set; } = "";
}
```

### Step 5 — rewire `src/EAxWiki/Program.cs`

Replace the top of the file (current lines 1–20) with:

```csharp
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using EAxWiki;
using EAxWiki.Core.Configuration;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.EA;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;

Console.WriteLine("EAxWiki - Sparx EA Repository to Wiki Generator");
Console.WriteLine();

var root = CommandLine.BuildCommand();
var parseResult = root.Parse(args);
if (parseResult.Errors.Count > 0)
{
    await parseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None);
    return 1;
}

if (parseResult.Action is HelpAction)
{
    await parseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None);
    return 0;
}

var config = CommandLine.ToConfig(parseResult);
```

Everything downstream (`config` through the rest of the file) is untouched. Delete the `ShowUsage()` static method (current lines 299–331).

### Step 6 — verify Task 1

1. `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build src/EAxWiki` — 0 warnings / 0 errors.
2. `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests --filter "FullyQualifiedName~CommandLineTests"` — all pass.
3. Grep for dangling references:
   `Select-String -Path src\EAxWiki\*.cs,src\EAxWiki.Tests\*.cs -Pattern '\.Load\(|HelpRequested|ShowUsage'`
   — only unrelated `HealthStore`/`LocalConfigStore` `.Load(` hits may remain; no `HelpRequested` / `ShowUsage`.
4. Smoke (help + typo + exit codes):
   ```powershell
   dotnet run --project src/EAxWiki -- --help
   ```
   prints the options table plus the connection-string examples; then
   ```powershell
   dotnet run --project src/EAxWiki -- --froce; echo "exit=$LASTEXITCODE"
   ```
   prints a parse error to stderr and `exit=1`.
5. Full .NET suite: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests`
   — record the actual count (ConfigTests 20 replaced by CommandLineTests ~44 → expect roughly 451; README update happens in Task 4).

### Step 7 — commit

```
git add src/EAxWiki/EAxWiki.csproj src/EAxWiki/CommandLine.cs src/EAxWiki/Config.cs src/EAxWiki/Program.cs src/EAxWiki.Tests/CommandLineTests.cs src/EAxWiki.Tests/ConfigTests.cs
git commit -m "refactor(parser): replace Config.Load switch with System.CommandLine (issue #86)"
```

---

## Task 2 — slim `export.ps1` and `writeback.ps1` to verbatim forwarders

Files:
- `scripts/export.ps1` (rewrite)
- `scripts/writeback.ps1` (rewrite)
- `tests/scripts/export.Tests.ps1` (rewrite)
- `tests/scripts/writeback.Tests.ps1` (rewrite)

### Step 1 — rewrite `scripts/export.ps1`

Delete `Get-ExportArgs` and the parsing/extraction block (current lines 9–55) and the `$runArgs` construction (current lines 87–99). The new file:

```powershell
. $PSScriptRoot\_bootstrap.ps1

# $PSNativeCommandUseErrorActionPreference (PowerShell 7.3+) defaults to $true in a fresh
# -NoProfile session (e.g. launched by EAxWiki.Monitor.exe or Task Scheduler). When
# set, dotnet's own warn-level log lines on stderr are enough to corrupt the $LASTEXITCODE
# check below even on a fully successful run. Scoped to this script only.
$PSNativeCommandUseErrorActionPreference = $false

if (-not $IsWindowsOS) {
    Write-Error "Export requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })

function Cleanup-EAProcesses {
    $eaProcesses = Get-Process EA -ErrorAction SilentlyContinue
    $orphans = $eaProcesses | Where-Object { $_.Id -notin $eaPidsBefore }
    if ($orphans) {
        $orphans | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-Host "Cleaned up $($orphans.Count) orphaned EA process(es)." -ForegroundColor DarkYellow
    }
}

# User args are forwarded verbatim: relative --repo / --output / bare repo resolve against
# $repoRoot because EAxWiki.dll resolves them against its working directory (we Push-Location'd).
Write-Host "=== Exporting wiki from EA model ===" -ForegroundColor Cyan

try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $args
    $code = if ($null -eq $LASTEXITCODE) { 0 } else { $LASTEXITCODE }
    Write-Output "EAXWIKI_EXIT_CODE=$code"
    if ($code -ne 0) {
        Write-Error "Export failed (exit code $code)."
        Cleanup-EAProcesses
        Pop-Location
        exit $code
    }
    Write-Host "Export complete." -ForegroundColor Green
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
```

### Step 2 — rewrite `scripts/writeback.ps1`

Delete `Get-WritebackArgs` (lines 16–34), the parsing block (lines 36–38), and the repository display block (lines 61–69). The new file:

```powershell
. $PSScriptRoot\_bootstrap.ps1

# Scan the local wiki for status changes made by users and write them back to the EA model via COM.
#
# Production workflow:
#   1. A user edits the 'status:' field in the YAML frontmatter of an element page (wiki/*.md).
#   2. Run this script to detect changes and push them back to the EA repository via the EA COM API.
#   3. Re-run export.ps1 to regenerate the wiki from the updated EA model.
#
# Requirements: Windows + Sparx Enterprise Architect installed (same as export).
#
# Args (--verbose, --repo/-r, or a bare repo path) are forwarded verbatim to EAxWiki.dll, which
# always prints the (redacted) repository and applies --writeback for us.

# See export.ps1 for why this is needed: dotnet's own stderr log lines can otherwise corrupt
# $LASTEXITCODE under $PSNativeCommandUseErrorActionPreference's default in a -NoProfile session.
$PSNativeCommandUseErrorActionPreference = $false

if (-not $IsWindowsOS) {
    Write-Error "Write-back requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

$eaPidsBefore = @(Get-Process EA -ErrorAction SilentlyContinue | ForEach-Object { $_.Id })

function Cleanup-EAProcesses {
    $eaProcesses = Get-Process EA -ErrorAction SilentlyContinue
    $orphans = $eaProcesses | Where-Object { $_.Id -notin $eaPidsBefore }
    if ($orphans) {
        $orphans | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-Host "Cleaned up $($orphans.Count) orphaned EA process(es)." -ForegroundColor DarkYellow
    }
}

Write-Host "=== Writing wiki status changes back to EA model ===" -ForegroundColor Cyan

$runArgs = @('--writeback') + $args

try {
    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    dotnet exec $dll $runArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Write-back failed (exit code $LASTEXITCODE)."
        Cleanup-EAProcesses
        Pop-Location
        exit $LASTEXITCODE
    }
    Write-Host "Write-back complete." -ForegroundColor Green
    Write-Host "Run export.ps1 to regenerate the wiki from the updated EA model." -ForegroundColor DarkCyan
}
finally {
    Cleanup-EAProcesses
}

Pop-Location
```

Note (deliberate deviation from spec line 76 "keep repository display"): the PS-side `Repository:` display is removed. EAxWiki.dll always prints `Repository: <redacted>` itself (Program.cs:190) and `EaRepository.Redact` covers strictly more credential fields than the old regex, so redaction is preserved and no re-parsing is reintroduced.

### Step 3 — rewrite `tests/scripts/export.Tests.ps1`

Keep the `BeforeAll` dot-source (it still runs a real export as today's live smoke; requires the DLL built). Replace the `Get-ExportArgs` describe (26 It-blocks) with forwarder assertions:

```powershell
BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export.ps1"
}

Describe 'export.ps1 forwarder' {
    It 'no longer hand-rolls its own arg parser' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('function Get-ExportArgs') | Should -Be $false
    }

    It 'forwards the user args ($args) to dotnet exec verbatim' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('dotnet exec $dll $args') | Should -Be $true
    }

    It 'still emits the EAXWIKI_EXIT_CODE= protocol' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('EAXWIKI_EXIT_CODE') | Should -Be $true
    }

    It 'still cleans up orphaned EA processes' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('function Cleanup-EAProcesses') | Should -Be $true
    }

    It 'still sets $PSNativeCommandUseErrorActionPreference = $false' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content.Contains('$PSNativeCommandUseErrorActionPreference = $false') | Should -Be $true
    }
}

Describe 'export.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

### Step 4 — rewrite `tests/scripts/writeback.Tests.ps1`

```powershell
BeforeAll {
    . "$PSScriptRoot\..\..\scripts\writeback.ps1"
}

Describe 'writeback.ps1 forwarder' {
    It 'no longer hand-rolls its own arg parser' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('function Get-WritebackArgs') | Should -Be $false
    }

    It 'prepends --writeback to the raw user args' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('$runArgs = @(''--writeback'') + $args') | Should -Be $true
    }

    It 'still cleans up orphaned EA processes' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('function Cleanup-EAProcesses') | Should -Be $true
    }

    It 'still sets $PSNativeCommandUseErrorActionPreference = $false' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content.Contains('$PSNativeCommandUseErrorActionPreference = $false') | Should -Be $true
    }
}

Describe 'writeback.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\writeback.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

### Step 5 — verify Task 2

Build first (the tests' live smoke needs the DLL):

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build src/EAxWiki
```

Run the two affected files in isolation (takes a while — each dot-source runs a real export):

```powershell
Invoke-Pester -Path tests/scripts/export.Tests.ps1, tests/scripts/writeback.Tests.ps1
```

### Step 6 — commit

```
git add scripts/export.ps1 scripts/writeback.ps1 tests/scripts/export.Tests.ps1 tests/scripts/writeback.Tests.ps1
git commit -m "refactor(scripts): slim export.ps1 and writeback.ps1 to forward args verbatim (issue #86)"
```

---

## Task 3 — reduce `export-and-serve.ps1` and `serve-api.ps1` orchestration parsing

Files:
- `scripts/export-and-serve.ps1` (rewrite)
- `scripts/serve-api.ps1` (rewrite)
- `tests/scripts/export-and-serve.Tests.ps1` (rewrite)
- `tests/scripts/serve-api.Tests.ps1` (rewrite)

Design (resolves spec line 77's literal "forward $args unchanged" — impossible because `--port` is serve-only and unknown to EAxWiki.dll): the reduced `Get-*Args` parses ONLY the four orchestration values (`--port`/`-p`/`-Port`, `--api-port`/`-ApiPort`, `--repo`/`-r`/`-RepoPath`, `--output`/`-o`/`-OutputDir`) plus the bare positional; it returns a `Forward` array of the remaining tokens. Serve-only tokens (`--port` value, bare numeric port) are stripped; `--api-port` is stripped and re-appended as the parsed value; `--repo`/`--output` (incl. their legacy aliases) are normalized to canonical `--repo`/`--output` when forwarded. All pass-through flags (`--force`, `--verbose`, `--json`, `--writeback`, `--brand`, `--name`, `--package`, `--cert*`, `--ai-*`) are forwarded verbatim, untouched.

### Step 1 — rewrite `scripts/export-and-serve.ps1`

```powershell
. $PSScriptRoot\_bootstrap.ps1

# Export the wiki and start MkDocs. If --api-port is given, also starts the
# local wiki write-back server so the status-editor widget can write back to EA.
#
# Usage:
#   .\scripts\export-and-serve.ps1
#   .\scripts\export-and-serve.ps1 --repo "model/file.qea" --output "wiki" --port 8000 --api-port 8001
#
# Only the orchestration values are parsed here (--port, --api-port, --repo, --output, bare repo).
# Everything else in $args is forwarded verbatim to export.ps1 and from there to EAxWiki.dll, so
# typo'd flags fail fast in the parser (exit 1) instead of being swallowed. Serve-only tokens
# (--port and a bare numeric port) are stripped because EAxWiki.dll doesn't know --port; --api-port
# is stripped and re-appended as the parsed value so the status-editor widget is embedded with the
# correct port. Legacy wrapper aliases (-RepoPath, -OutputDir, -ApiPort) are normalized to the
# canonical flags the exe accepts.

function Get-ExportAndServeArgs {
    param([string[]]$Arguments)
    $RepoPath  = ""
    $OutputDir = ""
    $Port      = 8000
    $ApiPort   = 8001
    $Forward   = [System.Collections.Generic.List[string]]::new()

    $i = 0
    while ($i -lt $Arguments.Count) {
        $arg = $Arguments[$i]
        switch -Regex ($arg) {
            '^(-p|--port|-Port)$' {
                $i++
                if ($i -lt $Arguments.Count) { $Port = [int]$Arguments[$i] }
            }
            '^(--api-port|-ApiPort)$' {
                $i++
                if ($i -lt $Arguments.Count) { $ApiPort = [int]$Arguments[$i] }
            }
            '^(-r|--repo|-RepoPath)$' {
                $Forward.Add('--repo')
                $i++
                if ($i -lt $Arguments.Count) { $RepoPath = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            '^(-o|--output|-OutputDir)$' {
                $Forward.Add('--output')
                $i++
                if ($i -lt $Arguments.Count) { $OutputDir = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            default {
                if (-not "$arg".StartsWith('-')) { $RepoPath = $arg }
                $Forward.Add($arg)
            }
        }
        $i++
    }
    return [PSCustomObject]@{
        RepoPath  = $RepoPath
        OutputDir = $OutputDir
        Port      = $Port
        ApiPort   = $ApiPort
        Forward   = $Forward.ToArray()
    }
}

$parsed = Get-ExportAndServeArgs -Arguments $args
$RepoPath  = $parsed.RepoPath
$OutputDir = $parsed.OutputDir
$Port      = $parsed.Port
$ApiPort   = $parsed.ApiPort

if ($ApiPort -gt 0 -and -not $IsWindowsOS) {
    Write-Error "The wiki write-back server requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve wiki output directory to an absolute path once so both the export and
# the write-back server refer to the same directory.
$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

# --- Export ---
# Forward the user's (serve-only-stripped) args, then inject the resolved API port so the
# status-editor widget embeds the correct URL. A relative --output resolves against $repoRoot
# inside EAxWiki.dll, so it matches $wikiDir by construction.
$exportArgs = @($parsed.Forward)
$exportArgs += '--api-port', $ApiPort

& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) { Pop-Location; exit $LASTEXITCODE }

# --- wiki write-back server (optional background job) ---
$apiJob = $null
if ($ApiPort -gt 0) {
    Write-Host ""
    Write-Host "Starting wiki write-back server on port $ApiPort..." -ForegroundColor Cyan

    $apiArgs = @("--api", "--api-port", $ApiPort, "--wiki-port", $Port, "--output", $wikiDir)
    if ($RepoPath) {
        $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                        elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                        else { Join-Path $repoRoot $RepoPath }
        $apiArgs += "--repo", $resolvedRepo
    }

    $dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
    $apiJob = Start-Job -ScriptBlock {
        param($root, $dllPath, $argList)
        Set-Location $root
        dotnet exec $dllPath $argList
    } -ArgumentList $repoRoot, $dll, $apiArgs

    Start-Sleep -Seconds 3

    if ($apiJob.State -eq 'Failed') {
        Write-Error "wiki write-back server failed to start."
        Receive-Job $apiJob
        Pop-Location
        exit 1
    }

    Write-Host "wiki write-back server started (job $($apiJob.Id))." -ForegroundColor Green
}

# --- MkDocs ---
try {
    & $PSScriptRoot\serve.ps1 --port $Port --wiki-dir $wikiDir
} finally {
    if ($apiJob) {
        Write-Host ""
        Write-Host "Stopping wiki write-back server..." -ForegroundColor DarkYellow
        Stop-Job  $apiJob -ErrorAction SilentlyContinue
        Remove-Job $apiJob -Force -ErrorAction SilentlyContinue
    }
}

Pop-Location
```

### Step 2 — rewrite `scripts/serve-api.ps1`

Same pattern, but a bare numeric token is a serve-only port (stripped) and any other bare token is forwarded (becomes the exe's bare positional repo — a small improvement over the old script, which silently dropped it).

```powershell
. $PSScriptRoot\_bootstrap.ps1

# Start the EAxWiki wiki write-back server alongside MkDocs.
#
# The write-back server listens on --api-port and handles status changes
# from the status-editor widget on element pages. MkDocs serves on --port.
#
# Usage:
#   .\scripts\serve-api.ps1
#   .\scripts\serve-api.ps1 --repo "path/to/model.qea" --output "wiki" --port 8000 --api-port 8001
#
# Only the orchestration values are parsed here (--port, --api-port, --repo, --output, bare repo).
# Everything else in $args is forwarded verbatim to export.ps1 and from there to EAxWiki.dll, so
# typo'd flags fail fast in the parser (exit 1) instead of being swallowed. Serve-only tokens
# (--port and a bare numeric port) are stripped; --api-port is stripped and re-appended as the
# parsed value; legacy wrapper aliases (-RepoPath, -OutputDir, -ApiPort) are normalized to the
# canonical flags the exe accepts.

function Get-ServeApiArgs {
    param([string[]]$Arguments)
    $RepoPath  = ""
    $OutputDir = ""
    $Port      = 8000
    $ApiPort   = 8001
    $Forward   = [System.Collections.Generic.List[string]]::new()

    $i = 0
    while ($i -lt $Arguments.Count) {
        $arg = $Arguments[$i]
        switch -Regex ($arg) {
            '^(-p|--port|-Port)$' {
                $i++
                if ($i -lt $Arguments.Count) { $Port = [int]$Arguments[$i] }
            }
            '^(--api-port|-ApiPort)$' {
                $i++
                if ($i -lt $Arguments.Count) { $ApiPort = [int]$Arguments[$i] }
            }
            '^(-r|--repo|-RepoPath)$' {
                $Forward.Add('--repo')
                $i++
                if ($i -lt $Arguments.Count) { $RepoPath = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            '^(-o|--output|-OutputDir)$' {
                $Forward.Add('--output')
                $i++
                if ($i -lt $Arguments.Count) { $OutputDir = $Arguments[$i]; $Forward.Add($Arguments[$i]) }
            }
            default {
                if ($arg -match '^\d+$') { $Port = [int]$arg }
                else { $Forward.Add($arg) }
            }
        }
        $i++
    }
    return [PSCustomObject]@{
        RepoPath  = $RepoPath
        OutputDir = $OutputDir
        Port      = $Port
        ApiPort   = $ApiPort
        Forward   = $Forward.ToArray()
    }
}

$parsed = Get-ServeApiArgs -Arguments $args
$RepoPath  = $parsed.RepoPath
$OutputDir = $parsed.OutputDir
$Port      = $parsed.Port
$ApiPort   = $parsed.ApiPort

if (-not $IsWindowsOS) {
    Write-Error "The wiki write-back server requires Sparx Enterprise Architect, which is only available on Windows."
    exit 1
}

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition | Split-Path -Parent
Push-Location $repoRoot

# Resolve wiki output directory to an absolute path once.
$wikiDir = if ($OutputDir) {
    if ([System.IO.Path]::IsPathRooted($OutputDir)) { $OutputDir }
    else { Join-Path $repoRoot $OutputDir }
} else {
    Join-Path $repoRoot "wiki"
}

Write-Host "=== EAxWiki - wiki write-back server + Wiki ===" -ForegroundColor Cyan
Write-Host "Write-back server : http://localhost:$ApiPort"
Write-Host "Wiki              : http://localhost:$Port"
Write-Host "Output            : $wikiDir"
Write-Host ""

# Export first so the widget is embedded with the correct API port.
Write-Host "Exporting wiki (--api-port $ApiPort embeds the status-editor widget)..." -ForegroundColor Cyan
$exportArgs = @($parsed.Forward)
$exportArgs += '--api-port', $ApiPort
& $PSScriptRoot\export.ps1 @exportArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "Export failed - cannot start write-back server + wiki."
    Pop-Location
    exit $LASTEXITCODE
}
Write-Host ""
Write-Host "Starting wiki write-back server in background..."

$apiArgs = @("--api", "--api-port", $ApiPort, "--wiki-port", $Port, "--output", $wikiDir)
if ($RepoPath) {
    $resolvedRepo = if ($RepoPath -match '=') { $RepoPath }
                    elseif ([System.IO.Path]::IsPathRooted($RepoPath)) { $RepoPath }
                    else { Join-Path $repoRoot $RepoPath }
    $apiArgs += "--repo", $resolvedRepo
}

$dll = Get-EAxWikiDllPath -RepoRoot $repoRoot
$apiJob = Start-Job -ScriptBlock {
    param($root, $dllPath, $argList)
    Set-Location $root
    dotnet exec $dllPath $argList
} -ArgumentList $repoRoot, $dll, $apiArgs

Start-Sleep -Seconds 3

if ($apiJob.State -eq 'Failed') {
    Write-Error "wiki write-back server failed to start."
    Receive-Job $apiJob
    Pop-Location
    exit 1
}

Write-Host "wiki write-back server started (job $($apiJob.Id))." -ForegroundColor Green
Write-Host ""

# Set up MkDocs environment
$mkdocsTemp = Join-Path $repoRoot ".mkdocs_temp"
if (-not (Test-Path $mkdocsTemp)) { New-Item -ItemType Directory -Path $mkdocsTemp | Out-Null }
$env:TEMP = $mkdocsTemp
$env:TMP  = $mkdocsTemp

$pipCache = Join-Path $repoRoot ".pip_cache"
if (-not (Test-Path $pipCache)) { New-Item -ItemType Directory -Path $pipCache | Out-Null }
$env:PIP_CACHE_DIR = $pipCache

$venvDir = Join-Path $repoRoot ".venv"
if (-not (Test-Path $venvDir)) {
    Write-Host "Creating virtual environment..."
    python3 -m venv $venvDir 2>$null
    if ($LASTEXITCODE -ne 0) { python -m venv $venvDir }
}

$activate = if ($IsWindowsOS) {
    Join-Path $venvDir "Scripts\Activate.ps1"
} else {
    Join-Path $venvDir "bin/Activate.ps1"
}

. $activate
python -m pip install --upgrade pip --quiet
python -m pip install -r (Join-Path $repoRoot "requirements.txt") --quiet

Write-Host "Starting MkDocs (Ctrl+C to stop both)..."
Write-Host ""

try {
    mkdocs serve --dev-addr "0.0.0.0:$Port" --dirty
}
finally {
    Write-Host ""
    Write-Host "Stopping wiki write-back server..." -ForegroundColor DarkYellow
    Stop-Job $apiJob -ErrorAction SilentlyContinue
    Remove-Job $apiJob -Force -ErrorAction SilentlyContinue
}

Pop-Location
```

### Step 3 — rewrite `tests/scripts/export-and-serve.Tests.ps1`

```powershell
BeforeAll {
    . "$PSScriptRoot\..\..\scripts\export-and-serve.ps1"
}

Describe 'Get-ExportAndServeArgs (reduced orchestration parser)' {
    It 'returns defaults with no arguments' {
        $r = Get-ExportAndServeArgs
        $r.RepoPath  | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port      | Should -Be 8000
        $r.ApiPort   | Should -Be 8001
        $r.Forward   | Should -BeNullOrEmpty
    }

    It 'parses --port with value' {
        $r = Get-ExportAndServeArgs -Arguments @('--port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -p shorthand' {
        $r = Get-ExportAndServeArgs -Arguments @('-p', '8080')
        $r.Port | Should -Be 8080
    }

    It 'strips serve-only --port from the forwarded args' {
        $r = Get-ExportAndServeArgs -Arguments @('--port', '8080', '--force')
        $r.Forward | Should -Be @('--force')
    }

    It 'parses --repo with value and forwards it as --repo' {
        $r = Get-ExportAndServeArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'normalizes legacy -RepoPath to --repo when forwarding' {
        $r = Get-ExportAndServeArgs -Arguments @('-RepoPath', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'parses --output with value and forwards it as --output' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
        $r.Forward   | Should -Be @('--output', 'mywiki')
    }

    It 'parses --api-port with value and does not forward it (re-appended parsed)' {
        $r = Get-ExportAndServeArgs -Arguments @('--api-port', '9090')
        $r.ApiPort | Should -Be 9090
        $r.Forward  | Should -BeNullOrEmpty
    }

    It 'accepts bare repo path' {
        $r = Get-ExportAndServeArgs -Arguments @('model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('model.qea')
    }

    It 'accepts connection string as repo' {
        $r = Get-ExportAndServeArgs -Arguments @('DBType=postgresql;Database=foo')
        $r.RepoPath | Should -Be 'DBType=postgresql;Database=foo'
        $r.Forward  | Should -Be @('DBType=postgresql;Database=foo')
    }

    It 'handles Unicode output dir' {
        $r = Get-ExportAndServeArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
        $r.Forward   | Should -Be @('--output', 'héllo-wörld')
    }

    It 'passes unknown flags through verbatim' {
        $r = Get-ExportAndServeArgs -Arguments @('--brand', 'eursura', '--force')
        $r.Forward | Should -Be @('--brand', 'eursura', '--force')
    }

    It 'all flags combined: orchestration parsed, pass-through forwarded' {
        $r = Get-ExportAndServeArgs -Arguments @('--force', '--verbose', '--json', '--writeback', '--port', '8080', '--repo', 'model.qea', '--output', 'wiki', '--api-port', '9090')
        $r.Port      | Should -Be 8080
        $r.ApiPort   | Should -Be 9090
        $r.RepoPath  | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.Forward   | Should -Be @('--force', '--verbose', '--json', '--writeback', '--repo', 'model.qea', '--output', 'wiki')
    }
}

Describe 'export-and-serve.ps1 forwarder' {
    It 'builds the export call from the reduced parser Forward list' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content.Contains('$exportArgs = @($parsed.Forward)') | Should -Be $true
        $content.Contains('$exportArgs += ''--api-port'', $ApiPort') | Should -Be $true
    }

    It 'no longer parses pass-through flags' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content.Contains('$Force = $true') | Should -Be $false
        $content.Contains('$Verbose = $true') | Should -Be $false
    }
}

Describe 'export-and-serve.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\export-and-serve.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

### Step 4 — rewrite `tests/scripts/serve-api.Tests.ps1`

```powershell
BeforeAll {
    . "$PSScriptRoot\..\..\scripts\serve-api.ps1"
}

Describe 'Get-ServeApiArgs (reduced orchestration parser)' {
    It 'returns defaults with no arguments' {
        $r = Get-ServeApiArgs
        $r.RepoPath  | Should -Be ""
        $r.OutputDir | Should -Be ""
        $r.Port      | Should -Be 8000
        $r.ApiPort   | Should -Be 8001
        $r.Forward   | Should -BeNullOrEmpty
    }

    It 'parses --port with value' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -Port with value' {
        $r = Get-ServeApiArgs -Arguments @('-Port', '8080')
        $r.Port | Should -Be 8080
    }

    It 'parses -p shorthand' {
        $r = Get-ServeApiArgs -Arguments @('-p', '8080')
        $r.Port | Should -Be 8080
    }

    It 'accepts bare numeric port and strips it from Forward' {
        $r = Get-ServeApiArgs -Arguments @('9000')
        $r.Port | Should -Be 9000
        $r.Forward | Should -BeNullOrEmpty
    }

    It 'bare port overrides --port flag value (last wins)' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '9000')
        $r.Port | Should -Be 9000
    }

    It 'parses --api-port with value' {
        $r = Get-ServeApiArgs -Arguments @('--api-port', '9090')
        $r.ApiPort | Should -Be 9090
    }

    It 'parses -ApiPort with value' {
        $r = Get-ServeApiArgs -Arguments @('-ApiPort', '9090')
        $r.ApiPort | Should -Be 9090
    }

    It 'parses --repo with value and forwards it as --repo' {
        $r = Get-ServeApiArgs -Arguments @('--repo', 'model.qea')
        $r.RepoPath | Should -Be 'model.qea'
        $r.Forward  | Should -Be @('--repo', 'model.qea')
    }

    It 'parses --output with value and forwards it as --output' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'mywiki')
        $r.OutputDir | Should -Be 'mywiki'
        $r.Forward   | Should -Be @('--output', 'mywiki')
    }

    It 'handles Unicode output dir' {
        $r = Get-ServeApiArgs -Arguments @('--output', 'héllo-wörld')
        $r.OutputDir | Should -Be 'héllo-wörld'
    }

    It 'forwards a bare non-numeric token (becomes the exe bare positional repo)' {
        $r = Get-ServeApiArgs -Arguments @('model.qea')
        $r.Forward | Should -Be @('model.qea')
    }

    It 'all flags combined: orchestration parsed, pass-through forwarded' {
        $r = Get-ServeApiArgs -Arguments @('--port', '8080', '--api-port', '9090', '--repo', 'model.qea', '--output', 'wiki', '--force')
        $r.Port      | Should -Be 8080
        $r.ApiPort   | Should -Be 9090
        $r.RepoPath  | Should -Be 'model.qea'
        $r.OutputDir | Should -Be 'wiki'
        $r.Forward   | Should -Be @('--repo', 'model.qea', '--output', 'wiki', '--force')
    }
}

Describe 'serve-api.ps1 forwarder' {
    It 'builds the export call from the reduced parser Forward list' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\serve-api.ps1" -Raw
        $content.Contains('$exportArgs = @($parsed.Forward)') | Should -Be $true
        $content.Contains('$exportArgs += ''--api-port'', $ApiPort') | Should -Be $true
    }
}

Describe 'serve-api.ps1 runs the pre-built DLL' {
    It 'does not use dotnet run (which rebuilds and locks the API DLL)' {
        $content = Get-Content "$PSScriptRoot\..\..\scripts\serve-api.ps1" -Raw
        $content | Should -Not -Match 'dotnet run --project'
        $content | Should -Match 'Get-EAxWikiDllPath'
        $content | Should -Match 'dotnet exec'
    }
}
```

### Step 5 — verify Task 3

Hold port 8000 on `0.0.0.0` (the verified recipe) while running the two affected files. The dot-source live smoke starts an API job on 8001 and mkdocs on 8000 (bind error — expected noise):

```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet build src/EAxWiki
$listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, 8000)
$listener.Start()
try { Invoke-Pester -Path tests/scripts/export-and-serve.Tests.ps1, tests/scripts/serve-api.Tests.ps1 } finally { $listener.Stop() }
```

### Step 6 — commit

```
git add scripts/export-and-serve.ps1 scripts/serve-api.ps1 tests/scripts/export-and-serve.Tests.ps1 tests/scripts/serve-api.Tests.ps1
git commit -m "refactor(scripts): reduce export-and-serve and serve-api orchestration parsing (issue #86)"
```

---

## Task 4 — full verification + README counts + ledger

### Step 1 — full suites

1. Full .NET: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src/EAxWiki.Tests`
   - If `PropertyBasedTests.EscapeCell_LengthAtLeastInputLength` or `Export_StatusEditorScript` fails, rerun once (known flakes); record final counts.
2. Full Pester (hold 8000, expect ~80+ tests, ~3 min):
   ```powershell
   $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, 8000)
   $listener.Start()
   try { .\tests\run-tests.ps1 } finally { $listener.Stop() }
   ```

### Step 2 — README test counts

Update the "Tests" section (README lines ~625–647): the .NET subtotal row and Pester subtotal row, the "Other" row, and line 647 `**<N> tests total** (<A> .NET + <P> Pester), all pass.` to the ACTUAL counts from Step 1. Verify the table arithmetic matches.

### Step 3 — commit docs

```
git add README.md
git commit -m "docs(readme): update test counts after parser overhaul (issue #86)"
```

### Step 4 — ledger

Append a dated Part 4 section to `.git/sdd/progress.md` recording: what changed (CommandLine.cs + wrapper slimming), the deliberate deviations below, and the recorded test counts. Commit:
```
git add .git/sdd/progress.md
git commit -m "docs(sdd): record part 4 command-line overhaul completion (issue #86)"
```

### Step 5 — end-to-end smoke (manual)

- `.\scripts\export.ps1 --repo "model\EurSuRA.qea" --force` — succeeds (real export).
- `.\scripts\writeback.ps1 --repo "model\EurSuRA.qea"` — succeeds (prints `Repository: model\EurSuRA.qea` from the exe).
- `.\scripts\export-and-serve.ps1 --repo "model\EurSuRA.qea" --force` — exports, starts API on 8001, mkdocs on 8000 (port may need the TcpListener recipe if another process holds it).
- `dotnet exec src\EAxWiki\bin\Debug\net10.0\EAxWiki.dll --froce` → parse error, exit 1.
- `git status` clean of runtime artifacts (never stage `model/`, `wiki/`, `.eaxwiki-monitor/`, `.eaxwiki`, bin/obj).

---

## Deliberate deviations from the spec (flag for reviewer)

1. **`writeback.ps1` drops its `Repository:` display** (spec line 76 said "keep repository display"). Rationale: EAxWiki.dll always prints `Repository: <redacted>` (Program.cs:190) and `EaRepository.Redact` covers strictly more fields than the old regex; keeping the PS display would require re-parsing the very args we removed.
2. **Orchestration scripts cannot forward `$args` 100% verbatim** (spec line 77). `--port` is serve-only and unknown to EAxWiki.dll, so the reduced parser strips serve-only tokens (`--port` + value, bare numeric port) and `--api-port` (re-appended as the parsed value), and normalizes legacy `-RepoPath`/`-OutputDir` aliases to canonical `--repo`/`--output` when forwarding. All pass-through flags are forwarded verbatim with zero parsing — matching spec line 77's pass-through list exactly.
3. **Legacy wrapper-only aliases are retired** at the exe boundary: `-RepoPath`, `-OutputDir`, `-ApiPort`, `-Brand`, `-Force`, `-Verbose`, `-Json`, `-WriteBack`, `-Port` are not part of the spec's exe option surface and now fail as unknown flags. Orchestration scripts still accept `-RepoPath`/`-OutputDir`/`-ApiPort` (normalizing them); `register-scheduled-task.ps1`, `serve.ps1`, `Validate-WikiOutput.ps1` are untouched and unaffected (they normalize to canonical flags themselves). Pester tests asserting these aliases are deleted in the rewrites.
4. **Repeated single-valued options become parse errors** (SCL 2.0.11 behavior; the old `Config.Load` silently took the last value). `--output a --output b` and `--api-port 8000 --api-port 9000` now exit 1 with `Option '--output' expects a single argument but 2 were provided.` This is fail-fast on a user mistake, consistent with decision #2; no wrapper passes duplicate single-valued flags in normal use, so nothing in the repo relies on last-wins.

## Self-review checklist (run before committing the plan)

- [ ] Every file listed has complete, ready-to-paste code.
- [ ] No placeholder paths/versions.
- [ ] TDD ordering is explicit for Task 1 (RED before implementation).
- [ ] Verification commands include the `$env:EAPath` inline and the `0.0.0.0:8000` TcpListener recipe.
- [ ] `serve.ps1`, `serve.Tests.ps1`, `_bootstrap.Tests.ps1`, `install.Tests.ps1`, `Validate-WikiOutput.Tests.ps1` are untouched (not listed for modification).
- [ ] Commit messages are lowercase conventional with `(issue #86)`.
- [ ] Deviations are listed and justified.
