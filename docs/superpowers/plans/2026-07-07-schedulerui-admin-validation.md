# SchedulerUI: Admin Elevation Check and Input Validation — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add admin elevation check, EA COM connection test button, webhook URI validation, and button enablement logic to the Scheduler GUI.

**Architecture:** Adds `TestConnection` method to `IEaReader` interface (implemented in `EaReader` via `EA.Repository.OpenFile`/`CloseFile`); SchedulerUI gains a project reference to `EAxWiki.EA` and uses `EaReader` on a background STA thread. Admin check runs at form load via `WindowsIdentity.GetCurrent().IsInRole(Administrator)`.

**Tech Stack:** .NET 10 WinForms, EA COM Interop, xUnit + Moq

## Global Constraints

- All existing tests must pass after each task
- Must compile with `dotnet build` on Windows
- `EAxWiki.SchedulerUI` targets `$(WindowsTargetFramework)` (defaults to `net10.0-windows`)
- The `_repoRoot == null` guard at form construction already disables all task buttons — admin check adds on top of this, not replacing it

---

### Task 1: IEaReader.TestConnection — Interface, Implementation, Test Double

**Files:**
- Modify: `src/EAxWiki.Core/Interfaces/IEaReader.cs:7`
- Modify: `src/EAxWiki.EA/EaReader.cs:30`
- Modify: `src/EAxWiki.Tests/TestDoubles/FakeEaReader.cs:15`

**Interfaces:**
- Consumes: `IEaReader` (existing interface in `EAxWiki.Core`)
- Produces: `bool TestConnection(string connectionString, out string? error)` on `IEaReader`

- [ ] **Step 1: Add `TestConnection` to `IEaReader` interface**

```csharp
// In IEaReader.cs, after the Open(…) declaration:
bool TestConnection(string connectionString, out string? error);
```

- [ ] **Step 2: Add stub in `FakeEaReader`**

```csharp
// In FakeEaReader.cs, after the Open override:
public bool TestConnection(string connectionString, out string? error)
{
    error = null;
    return true;
}
```

- [ ] **Step 3: Implement `TestConnection` in `EaReader`**

```csharp
// In EaReader.cs, after the Open method:
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

- [ ] **Step 4: Build and verify**

Run: `dotnet build src/EAxWiki.sln` (or equivalent build command)
Expected: Build succeeds with no warnings.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Core/Interfaces/IEaReader.cs src/EAxWiki.EA/EaReader.cs src/EAxWiki.Tests/TestDoubles/FakeEaReader.cs
git commit -m "feat: add TestConnection to IEaReader interface and implementation"
```

---

### Task 2: SchedulerUI — Project Reference, Admin Check, Button Enablement

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/EAxWiki.SchedulerUI.csproj`
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:75` (constructor area)
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` (fields area around line 16)

**Interfaces:**
- Consumes: `IEaReader.TestConnection`, `EaReader` class from `EAxWiki.EA`
- Produces: `_isAdmin` field, disabled task buttons when not admin, `_connectionValid` field

- [ ] **Step 1: Add project reference to `EAxWiki.EA` in `.csproj`**

```xml
<!-- In EAxWiki.SchedulerUI.csproj, after the EAxWiki.Core reference: -->
<ProjectReference Include="..\EAxWiki.EA\EAxWiki.EA.csproj" />
```

- [ ] **Step 2: Add `_isAdmin` and `_connectionValid` fields to SchedulerForm**

```csharp
// In SchedulerForm.cs, after _repoRoot field (~line 16):
private readonly bool _isAdmin;
private bool _connectionValid;
```

- [ ] **Step 3: Add admin check in constructor**

After `if (_repoRoot == null)` block (and its `else` branch), add:

```csharp
// After line 143 (close of UpdateRepoTypeEnablement call in ctor):
using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
{
    var principal = new System.Security.Principal.WindowsPrincipal(identity);
    _isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
}

if (!_isAdmin)
{
    AppendOutput("This tool requires Administrator privileges to manage scheduled tasks. Please restart as Administrator.");
    _registerButton.Enabled = false;
    _enableButton.Enabled = false;
    _disableButton.Enabled = false;
    _unregisterButton.Enabled = false;
    _refreshStatusButton.Enabled = false;
}
```

- [ ] **Step 4: Update `RegisterAsync` to also check `_connectionValid`**

In `RegisterAsync()`, after the `if (_repoRoot == null) return;` guard:

```csharp
if (!_connectionValid)
{
    AppendOutput("Test the repository connection on the Configuration tab first.");
    return;
}
```

- [ ] **Step 5: Ensure `_connectionValid` doesn't gate admin-restricted operations**

The Enable/Disable/Unregister/Refresh buttons use the existing `RunTaskCommandAsync` — they don't call `RegisterAsync`, so no change needed. They're already gated by the admin check disabling them in the constructor.

- [ ] **Step 6: Build and verify**

Run: `dotnet build src/EAxWiki.SchedulerUI`
Expected: Build succeeds.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki.SchedulerUI/EAxWiki.SchedulerUI.csproj src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat: add admin elevation check and project reference to EAxWiki.EA"
```

---

### Task 3: Test Connection Button + URI Validation

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs`

**Interfaces:**
- Consumes: `EaReader.TestConnection` (from `EAxWiki.EA`)
- Produces: Test Connection button on Configuration tab, URI validation in Save method

- [ ] **Step 1: Add Test Connection button field**

```csharp
// In SchedulerForm.cs field declarations, near the existing config buttons (~line 40):
private readonly Button _testConnectionButton = new() { Text = "Test Connection", AutoSize = true };
```

- [ ] **Step 2: Add the button to the Configuration tab**

In `BuildConfigTab()`, in the buttons panel (`_saveConfigButton` / `_refreshConfigButton` area, line 163-165), add the test button between Refresh and Save:

```csharp
// After _refreshConfigButton:
buttons.Controls.Add(_testConnectionButton);
```

- [ ] **Step 3: Wire up the Test Connection click handler**

In the constructor, after existing `_saveConfigButton.Click += ...`:

```csharp
_testConnectionButton.Click += async (_, _) => await TestConnectionAsync();
```

- [ ] **Step 4: Add `TestConnectionAsync` method**

```csharp
private async Task TestConnectionAsync()
{
    var repoPath = BuildRepoPath();
    if (repoPath.Length == 0)
    {
        AppendOutput("Enter repository details first.");
        return;
    }

    _testConnectionButton.Enabled = false;
    _testConnectionButton.Text = "Testing...";
    AppendOutput("Testing repository connection...");

    try
    {
        var tcs = new TaskCompletionSource<(bool ok, string? error)>();
        var thread = new Thread(() =>
        {
            try
            {
                var reader = new EaReader();
                var ok = reader.TestConnection(repoPath, out var error);
                reader.Dispose();
                tcs.SetResult((ok, error));
            }
            catch (Exception ex)
            {
                tcs.SetResult((false, ex.Message));
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        var (ok, error) = await tcs.Task;
        _connectionValid = ok;

        if (ok)
            AppendOutput("Connection successful.");
        else
            AppendOutput($"Connection failed: {error}");
    }
    finally
    {
        _testConnectionButton.Enabled = true;
        _testConnectionButton.Text = "Test Connection";
    }
}
```

- [ ] **Step 5: Add URI validation in `SaveEaxwikiConfig`**

In `SaveEaxwikiConfig()`, before the `var config = new LocalConfigStore.Config` block (around line 352), add:

```csharp
if (_webhookBox.Text.Trim() is { Length: > 0 } slackUrl &&
    !Uri.TryCreate(slackUrl, UriKind.Absolute, out _))
{
    AppendOutput($"Invalid Slack webhook URL: {slackUrl}");
    return;
}
if (_teamsWebhookBox.Text.Trim() is { Length: > 0 } teamsUrl &&
    !Uri.TryCreate(teamsUrl, UriKind.Absolute, out _))
{
    AppendOutput($"Invalid Teams webhook URL: {teamsUrl}");
    return;
}
```

- [ ] **Step 6: Build and verify**

Run: `dotnet build src/EAxWiki.SchedulerUI`
Expected: Build succeeds.

- [ ] **Step 7: Run all .NET tests**

Run: `$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'; dotnet test src\EAxWiki.Tests`
Expected: All 232+ tests pass.

- [ ] **Step 8: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat: add Test Connection button, STA threading, and URI validation"
```

---

### Self-Review Checklist

1. **Spec coverage:** All spec items covered — Task 1 (TestConnection interface), Task 2 (admin check, project reference), Task 3 (button, threading, URI validation, enablement).
2. **Placeholder scan:** No TBD, TODO, or vague steps. No "add appropriate error handling" without showing code.
3. **Type consistency:** `TestConnection` signature `(string, out string?) -> bool` is consistent across interface, implementation, fake, and caller.
