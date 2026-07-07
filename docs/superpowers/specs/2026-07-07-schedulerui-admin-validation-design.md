# SchedulerUI: Admin Elevation Check and Input Validation

- **Issue:** [#59](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/59)
- **Date:** 2026-07-07
- **Status:** Approved

## Problem

`SchedulerForm` shells out to `pwsh.exe` to run `register-scheduled-task.ps1` and other Task Scheduler cmdlets without checking:
1. If the user has admin privileges (Task Scheduler registration requires elevation)
2. If the input values are valid (webhook URLs, repo path connectivity)

Without admin, cmdlets throw an unhandled `UnauthorizedAccessException`.

## Scope

1. Admin elevation check at form load; disable all task-operation buttons with a clear message.
2. EA repository connection test via a dedicated button, using `IEaReader.TestConnection`.
3. Webhook URL validation (absolute URI or empty) on Save.
4. Button enablement logic driven by admin status + connection validity.

## Design

### 1. IEaReader.TestConnection

**Interface** (`src/EAxWiki.Core/Interfaces/IEaReader.cs`):

```csharp
bool TestConnection(string connectionString, out string? error);
```

**Implementation** (`src/EAxWiki.EA/EaReader.cs`):

```csharp
public bool TestConnection(string connectionString, out string? error)
{
    try
    {
        var repo = new EA.Repository();
        repo.OpenFile(connectionString);
        repo.CloseFile();
        error = null;
        return true;
    }
    catch (Exception ex)
    {
        error = ex.Message;
        return false;
    }
}
```

**Test double** (`src/EAxWiki.Tests/TestDoubles/FakeEaReader.cs`): returns `(true, null)` always.

**STA threading:** The Test Connection button dispatches to a background STA thread (manual `Thread` + `TaskCompletionSource`) since EA COM requires STA. The form awaits the result and updates the Output pane on the UI thread.

### 2. Admin Elevation Check

In `SchedulerForm` constructor, after existing `_repoRoot` null check:

- Query `WindowsIdentity.GetCurrent().IsInRole(WindowsBuiltInRole.Administrator)`
- Store `_isAdmin` field
- If not admin:
  - Disable: Register, Enable, Disable, Unregister, Refresh Status buttons
  - Append "Run as Administrator to manage scheduled tasks." to Output pane
  - Add an info label on the Task Status tab (banner-style)

Config editing/saving remains enabled regardless of admin status.

### 3. Test Connection Button

Added to the Configuration tab, next to Save/Refresh. On click:

1. Read repo path via `BuildRepoPath()`
2. If empty: "Enter repository details first."
3. Append "Testing repository connection..."
4. Disable button, show "Testing..." text
5. Spawn background STA thread → `new EaReader().TestConnection(path, out error)`
6. Re-enable button, show result in Output pane
7. If successful, set `_connectionValid = true`

### 4. Webhook URL Validation

In `SaveEaxwikiConfig()`, before saving:
- If Slack Webhook is non-empty: `Uri.TryCreate(url, UriKind.Absolute, out _)` — fail + return on invalid
- If Teams Webhook is non-empty: same check
- Error appended to Output pane, save aborted

### 5. Button Enablement Logic

| Button | Enabled when |
|---|---|
| Register | `_isAdmin && _connectionValid` |
| Enable/Disable/Unregister/Refresh Status | `_isAdmin` |
| Test Connection | Always (no prereq) |
| Save Configuration | Always (no prereq) |

All buttons disable themselves during their async operation to prevent double-clicks.

**Note:** `_connectionValid` is set to `true` only by a successful Test Connection click. It is never auto-reset — the user is responsible for re-testing after changing repo fields. The Output pane shows each test result as a clear success/failure message.

### 6. Project References

`EAxWiki.SchedulerUI` gains a project reference to `EAxWiki.EA` (for `EaReader`).

## Files Changed

| File | Change |
|---|---|
| `src/EAxWiki.Core/Interfaces/IEaReader.cs` | Add `TestConnection` method |
| `src/EAxWiki.EA/EaReader.cs` | Implement `TestConnection` |
| `src/EAxWiki.Tests/TestDoubles/FakeEaReader.cs` | Add `TestConnection` stub |
| `src/EAxWiki.SchedulerUI/EAxWiki.SchedulerUI.csproj` | Add project reference to `EAxWiki.EA` |
| `src/EAxWiki.SchedulerUI/SchedulerForm.cs` | Admin check, Test Connection button, URI validation, button enablement |

## Testing

- `FakeEaReader.TestConnection` returns success always (existing test pattern)
- Admin check is environment-dependent (always admin in CI) — tested manually
- URI validation is inline in Save — existing save tests cover this implicitly
- 232 existing tests must still pass
