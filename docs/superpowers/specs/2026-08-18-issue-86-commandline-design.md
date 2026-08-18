# Design: System.CommandLine arg parser + PowerShell wrapper overhaul (issue #86, part 4)

Status: approved design.

## Goal

Replace the hand-rolled `Config.Load` argument parser (`src/EAxWiki/Config.cs:25-122`, ~100 lines of `switch`) with System.CommandLine (typed parsing, range validation, auto-generated `--help`, hard errors on unknown flags), and slim the PowerShell wrappers so they stop re-parsing and re-building the flag list before forwarding to the exe.

## Problem statement (from issue #86 item #4)

`Config.cs` is a hand-rolled `switch` with no consistency (some flags have short forms, some don't), no validation, no bounds checks, and no auto-generated `--help`. The PowerShell wrappers (`export.ps1`, `export-and-serve.ps1`, `serve-api.ps1`, `writeback.ps1`) each re-parse the same flags with their own `Get-*Args` functions before forwarding. This duplication is the source of drift (each wrapper has different defaults and slightly different behaviors).

## Non-goals

- No new CLI surface: every flag keeps its current name, short form, and default.
- No behavior change to export / write-back / serve / API server logic itself.
- `serve.ps1` is untouched — it never invokes the exe (pure mkdocs) and keeps its tiny parser.

## Architecture

### C# side: new `src/EAxWiki/CommandLine.cs`

A new `public static class CommandLine` mirrors the established `EAxWiki.Monitor.MonitorCommandLine` pattern:

- `BuildCommand()` returns a `RootCommand`.
- `ToConfig(ParseResult)` returns the existing `Config` DTO.
- `Config` stays a pure data holder (unchanged); `Config.Load` and `ShowUsage()` are deleted.

**Dependencies:** add `System.CommandLine` 2.0.11 to `EAxWiki.csproj` (same version already used by `EAxWiki.Monitor`).

**Root command shape:**
- One `Argument<string?>` accepting the bare positional repo path / connection string (so `EAxWiki model.qea` works — this behavior moves from the wrappers into C#).
- Options, preserving current names + short forms:
  - `--repo` / `-r` (string) — repository path or connection string
  - `--name` / `-n` (string) — display name
  - `--output` / `-o` (string) — output directory (default `wiki`)
  - `--package` / `-p` (string) — package filter
  - `--verbose` / `-v` (bool flag)
  - `--force` / `-f` (bool flag)
  - `--json` / `-j` (bool flag)
  - `--writeback` / `-w` (bool flag)
  - `--api` (bool flag)
  - `--api-port` (int, 1–65535)
  - `--wiki-port` (int, 1–65535)
  - `--cert` (string)
  - `--cert-password` (string)
  - `--ai-endpoint` (string)
  - `--ai-model` (string)
  - `--ai-key` (string)
  - `--brand` (string)

**Validation:**
- Ports validated 1–65535 by System.CommandLine (replaces the manual `ParsePort`).
- Unknown flags are parse errors (SCL default; `TreatUnmatchedTokensAsErrors` stays `true`). A typo like `--froce` fails with a clear message + exit 1.
- The bare positional repo is the one legitimate non-option token (root `Argument`).

**Post-parse logic in `ToConfig` (preserves current asserted behavior):**
- `--api` → `ApiMode = true`; if no `--api-port` given → `ApiPort = 8001`.
- `--api-port 9000` without `--api` → `ApiMode = false`, `ApiPort = 9000`.

**Help:** `--help` / `-h` / `/?` → SCL auto-generated help. The connection-string examples from `ShowUsage()` move into the root command's `Description` so they still print. `ShowUsage()` is deleted.

### Program.cs changes

- Replace `config.Load(args)` + `if (config.HelpRequested) { ShowUsage(); return 0; }` with:
  - parse via `CommandLine.BuildCommand()`;
  - on parse errors → write to stderr, return 1;
  - on help → SCL prints help, return 0;
  - else `var config = CommandLine.ToConfig(result)`.
- Everything downstream (`.eaxwiki` discovery, interactive fallback, `.eaxwiki` merge, env-var wiring, export/api paths) is untouched.
- Relative `--output` / bare repo resolve against the working directory, which under the wrappers is the repo root (`Push-Location`), matching current behavior.

### PowerShell wrappers

- **`export.ps1`**: delete `Get-ExportArgs`; invoke `dotnet exec $dll @args` forwarding the user's `$args` unchanged. Keep the `$PSNativeCommandUseErrorActionPreference` guard, EA-process cleanup, and `EAXWIKI_EXIT_CODE=$code` echo.
- **`writeback.ps1`**: delete `Get-WritebackArgs`; forward `@('--writeback') + $args` unchanged. Keep EA cleanup and repository display.
- **`export-and-serve.ps1`** and **`serve-api.ps1`**: reduce `Get-*Args` to parse only the orchestration values they need programmatically: `--port`, `--api-port`, `--repo`, `--output`. Forward the user's original `$args` unchanged to `export.ps1` / the exe, appending only the orchestration-injected flags they compute (`--api-port`, `--wiki-port`, resolved `--output`). Pass-through flags (`--force`, `--verbose`, `--json`, `--writeback`, `--brand`, `--name`, `--package`, `--cert*`, `--ai-*`) are never re-parsed or rebuilt by the orchestration scripts.
- **`serve.ps1`**: unchanged.

## Error handling

- Unknown flag / bad port / missing value → System.CommandLine parse error, printed to stderr, process exit 1.
- `--help` → help to stdout, exit 0.
- All other runtime error handling (COM failures, missing repo, unreachable output dir) is unchanged.

## Testing

### C# tests

`ConfigTests.cs` is rewritten to drive `CommandLine.ToConfig` via `CommandLine.BuildCommand().Parse(args)`:
- All current field assertions preserved (defaults, short/long forms, combined flags).
- `Load_UnknownFlag_Ignored` flips to assert the new hard-error behavior (parse fails with an error).
- New cases:
  - bare positional repo (`EAxWiki model.qea` and connection-string form);
  - `--api` sets `ApiMode` + default `ApiPort` 8001;
  - `--api-port` without `--api` does not set `ApiMode`;
  - port range validation (0 and 65536 rejected);
  - help request handled (SCL).

### Pester tests

- `export.Tests.ps1` (26 It-blocks): rewrite the `Get-ExportArgs` describe — the parser is gone; assert the script forwards `$args` unchanged to the exe (mock/verify the invocation) and keep the "runs the pre-built DLL" invariant.
- `writeback.Tests.ps1` (14): same — `Get-WritebackArgs` describe replaced with forward-unchanged assertions (`--writeback` prepended).
- `export-and-serve.Tests.ps1` (23) and `serve-api.Tests.ps1` (13): reduce the `Get-*Args` describes to the four orchestration values; keep the "runs the pre-built DLL" invariant.
- `serve.Tests.ps1` (12): unchanged.
- `_bootstrap.Tests.ps1`, `install.Tests.ps1`, `Validate-WikiOutput.Tests.ps1`: unchanged.

## Verification

- Full .NET suite: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test` — all pass (expected count recorded at implementation time).
- Full Pester suite from repo root with port 8000 held by a `TcpListener` bound to `0.0.0.0` (the verified recipe; 127.0.0.1 does not stop mkdocs on this box) — all pass.
- Smoke: `EAxWiki.exe --help` prints options + connection-string examples; `EAxWiki.exe --froce` exits 1 with a parse error; a real export via `export.ps1` and a write-back via `writeback.ps1` still succeed end-to-end.
- `git status` clean of runtime artifacts (never stage `model/`, `wiki/`, `.eaxwiki-monitor/`, `.eaxwiki`, bin/obj).
