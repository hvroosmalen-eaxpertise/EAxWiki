# Dashboard FillWeight Defaults via ColumnAdded

**Date:** 2026-08-19

## Problem

The Health Dashboard grid applies per-column `FillWeight`s only in `RefreshDashboard`, after each `DataSource` assignment. The weights are set by a `foreach` loop over `_dashboardGrid.Columns`. This couples column layout to refresh logic: every future column creation must remember to run the loop.

## Goal

Make the column `FillWeight`s a property of the grid itself — applied automatically whenever a column is created — instead of a post-assignment step.

## Design

Confined to `src/EAxWiki.SchedulerUI/SchedulerForm.cs`.

1. Remove the `foreach (DataGridViewColumn col in _dashboardGrid.Columns)` block from `RefreshDashboard`.
2. Wire `_dashboardGrid.ColumnAdded += OnDashboardColumnAdded;` once in the constructor (after `_repoRoot = RepoLocator.FindRepoRoot();`).
3. Add a private handler that assigns the weight to the newly created column:

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

## Behavior

The grid auto-generates its columns from the anonymous type in `RefreshDashboard` when `DataSource` is assigned. Each auto-generation fires `ColumnAdded` per column, so the handler applies the weight at column creation time. Resulting weights are identical to today: `LastSuccess`/`LastFailure` = 35 each, `Name`/`Status`/`ConsecutiveFailures` = 10 each. No visible change; this is a structural simplification.

## Non-goals

- No data, logic, or other-tab changes.
- No change to the SplitContainer layout from the previous design.
- No change to `HealthDashboardReader`.

## Verification

- `dotnet build src\EAxWiki.SchedulerUI\EAxWiki.SchedulerUI.csproj --configuration Debug` → 0 errors.
- No new tests (layout-only; no automated WinForms coverage in the repo).
