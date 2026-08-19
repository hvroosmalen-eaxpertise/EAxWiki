# Health Dashboard Table Sizing Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make the Health Dashboard table in EAxWiki.SchedulerUI taller by default, resizable via a drag splitter, and give its two ISO-timestamp columns enough width to show full values.

**Architecture:** A layout-only change confined to `src/EAxWiki.SchedulerUI/SchedulerForm.cs`. The dashboard tab's `FlowLayoutPanel` is replaced with a horizontal `SplitContainer` (Refresh row on top, grid fills the bottom), and `RefreshDashboard` assigns per-column `FillWeight`s so the two date columns get ~3.5x the room of the others. No data, logic, or other tab changes.

**Tech Stack:** C# / WinForms (.NET 10, `net10.0-windows`).

## Global Constraints

- LF line endings + UTF-8 no BOM for changed files.
- Exact lowercase conventional commit messages (this change is part of the SchedulerUI, tracked under issue #86).
- `dotnet build` of `src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj` must succeed with 0 errors.
- Do NOT change: `HealthDashboardReader`, refresh logic, other four tabs, form `Size`/`MinimumSize`.
- Do NOT stage `bin/`, `obj/`, or `.eaxwiki-monitor/*/`.

---

### Task 1: Add a SplitContainer layout to the dashboard tab

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:106-111` (grid field, make it fill)
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:337-346` (`BuildDashboardTab`)

**Interfaces:**
- Consumes: existing `_dashboardGrid` field, existing `_refreshDashboardButton` field.
- Produces: a draggable horizontal splitter; `_dashboardGrid` docked `Fill` in `SplitContainer.Panel2` (later tasks set its column weights).

- [ ] **Step 1: Change `_dashboardGrid` to dock Fill**

In `SchedulerForm.cs:106-111`, replace the field initializer so the grid fills its
container instead of being a fixed-height top-docked control:

```csharp
    private readonly DataGridView _dashboardGrid = new()
    {
        ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false, Dock = DockStyle.Fill,
        AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells,
    };
```

- [ ] **Step 2: Replace the dashboard tab layout with a SplitContainer**

In `BuildDashboardTab` (`SchedulerForm.cs:337-346`), replace the `FlowLayoutPanel`
construction with a horizontal `SplitContainer`. Current code:

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
```

Replace with:

```csharp
    private TabPage BuildDashboardTab()
    {
        var buttonRow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        buttonRow.Controls.Add(_refreshDashboardButton);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterWidth = 6,
            Panel1MinSize = 28, // Refresh button row
        };
        split.Panel1.Controls.Add(buttonRow);
        split.Panel1.AutoScroll = true;
        split.Panel2.Controls.Add(_dashboardGrid);
        split.SplitterDistance = 40; // button row only; the grid gets the rest

        _refreshDashboardButton.Click += (_, _) => RefreshDashboard();
        return new TabPage("Health Dashboard") { Padding = new Padding(10), Controls = { split } };
    }
```

- [ ] **Step 3: Build and verify it compiles**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug --nologo -v q
```
Expected: `0 Error(s)`.

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "refactor(schedulerui): make dashboard grid resizable via split container (issue #86)"
```

---

### Task 2: Weight the dashboard columns so timestamps have room

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs:348-362` (`RefreshDashboard`)

**Interfaces:**
- Consumes: the grid columns auto-generated from the anonymous type in `RefreshDashboard`
  (`Name`, `Status`, `LastSuccess`, `LastFailure`, `ConsecutiveFailures`).
- Produces: a grid where `LastSuccess` and `LastFailure` each get ~35% of width.

- [ ] **Step 1: Add per-column FillWeights after DataSource assignment**

In `RefreshDashboard` (`SchedulerForm.cs:348-362`), after the existing
`_dashboardGrid.DataSource = ...` assignment, assign weights keyed by column name.
Current method ends with:

```csharp
            .ToList();
    }
```

Replace with:

```csharp
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

- [ ] **Step 2: Build and verify it compiles**

Run:
```powershell
$env:EAPath = 'E:\Program Files\Sparx Systems\EA\'
dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug --nologo -v q
```
Expected: `0 Error(s)`.

- [ ] **Step 3: Manual verification (optional, user runs the GUI)**

Run `.\scripts\start-scheduler-ui.ps1`, open the Health Dashboard tab, click Refresh.
Confirm: the grid fills the tab below the Refresh row, the splitter drags to grow/shrink
the table, and `LastSuccess`/`LastFailure` columns show full ISO timestamps.

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat(schedulerui): weight dashboard date columns for full timestamps (issue #86)"
```

---

### Task 3: Whole-branch verification

**Files:**
- None (verification only)

**Interfaces:**
- Consumes: Tasks 1-2 results.

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

- [ ] **Step 4: Comment on issue #86**

Post a short comment noting the Health Dashboard table is now splitter-resizable with
weighted date columns.
