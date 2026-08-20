# Dashboard FillWeight Defaults Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make the Health Dashboard grid's column `FillWeight`s a grid-level default applied at column creation time, instead of a post-`DataSource` loop in `RefreshDashboard`.

**Architecture:** A layout-only change confined to `src/EAxWiki.SchedulerUI/SchedulerForm.cs`. The `foreach` FillWeight block is removed from `RefreshDashboard`; a `ColumnAdded` event handler wired once in the constructor applies the weights to each column as it is auto-generated. No data, logic, or other-tab changes.

**Tech Stack:** C# / WinForms (.NET 10, `net10.0-windows`).

## Global Constraints

- LF line endings + UTF-8 no BOM for changed files (do not re-encode the whole file; only edit targeted locations matching surrounding style).
- Exact lowercase conventional commit message `refactor(schedulerui): apply dashboard fillweights on column add (issue #86)`.
- `dotnet build` of `src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj` must succeed with 0 errors.
- Do NOT change: `HealthDashboardReader`, refresh logic, the SplitContainer layout, other four tabs, form `Size`/`MinimumSize`.
- Do NOT stage `bin/`, `obj/`, or `.eaxwiki-monitor/*/`.
- Do NOT push; do NOT run the full test suite or the GUI.

---

### Task 1: Move FillWeights into a ColumnAdded handler

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:363-375` (`RefreshDashboard`, remove the foreach block)
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:113-116` (constructor, wire the handler)
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` (add a private `OnDashboardColumnAdded` method near `RefreshDashboard`)

**Interfaces:**
- Consumes: existing `_dashboardGrid` field (`Dock = DockStyle.Fill`, `AutoSizeColumnsMode = Fill`).
- Produces: `_dashboardGrid.ColumnAdded` wired to a handler that assigns `FillWeight` by column name at column creation time.

- [ ] **Step 1: Remove the foreach block from `RefreshDashboard`**

In `RefreshDashboard` (`SchedulerForm.cs`), locate the end of the method. Current code:

```csharp
            })
            .ToList();

        foreach (DataGridViewColumn col in _dashboardGrid.Columns)
        {
            col.FillWeight = col.Name switch
            {
                "LastSuccess" or "LastFailure" => 35,
                "Name" or "Status" or "ConsecutiveFailures" => 10,
                _ => 10,
            };
        }
    }
```

Replace with:

```csharp
            })
            .ToList();
    }
```

- [ ] **Step 2: Wire `ColumnAdded` in the constructor**

In `SchedulerForm()` (`SchedulerForm.cs:113-116`), right after the existing `_repoRoot = RepoLocator.FindRepoRoot();` line, add:

```csharp
        _repoRoot = RepoLocator.FindRepoRoot();
        _dashboardGrid.ColumnAdded += OnDashboardColumnAdded;
```

- [ ] **Step 3: Add the handler method**

Add this method immediately after `RefreshDashboard`:

```csharp
    private void OnDashboardColumnAdded(object? sender, DataGridViewColumnEventArgs e)
    {
        e.Column.FillWeight = e.Column.Name switch
        {
            "LastSuccess" or "LastFailure" => 35,
            "Name" or "Status" or "ConsecutiveFailures" => 10,
            _ => 10,
        };
    }
```

- [ ] **Step 4: Build and verify it compiles**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug --nologo -v q
```
Expected: `0 Error(s)`.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "refactor(schedulerui): apply dashboard fillweights on column add (issue #86)"
```

- [ ] **Step 6: Verify the diff**

Run `git show HEAD -- src/EAxWiki.SchedulerUI/SchedulerForm.cs` and confirm the only changes are: (a) the foreach block removed from `RefreshDashboard`, (b) the one-line `ColumnAdded +=` wiring in the constructor, (c) the new `OnDashboardColumnAdded` method.

---

### Task 2: Whole-branch verification

**Files:**
- None (verification only)

**Interfaces:**
- Consumes: Task 1 result.

- [ ] **Step 1: Build the full solution**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build EAxWiki.slnx --configuration Debug --nologo -v q
```
Expected: `0 Error(s)`.

- [ ] **Step 2: Run the .NET test suite**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet test src\EAxWiki.Tests\EAxWiki.Tests.csproj --configuration Debug --nologo -v q
```
Expected: all pass, 0 failed (480 tests at baseline; no tests touch the SchedulerUI layout).

- [ ] **Step 3: Push to origin/master**

```bash
git push origin master
```
