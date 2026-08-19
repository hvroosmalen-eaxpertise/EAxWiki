# Health Dashboard Table Sizing — Design

**Date:** 2026-08-19

## Problem

The Health Dashboard tab in EAxWiki.SchedulerUI shows a 5-column `DataGridView`
(`_dashboardGrid`) that is small and cannot be resized:

- It is docked `Top` with a fixed `Height = 260` (`SchedulerForm.cs:106-111`), so it
  always occupies exactly 260px and offers no drag affordance to grow taller.
- `AutoSizeColumnsMode = Fill` splits the window width equally across all 5 columns
  (~140px each at the default 700px window). The two ISO-8601 timestamp columns
  (`LastSuccess` / `LastFailure`, values like `2026-08-19T13:22:45+02:00`) need ~180px
  and are truncated.

## Goal

Make the dashboard table taller by default, resizable by dragging, and give the
timestamp columns enough width to show their values. Layout-only change; no logic or
data changes.

## Design

All changes are in `src/EAxWiki.SchedulerUI/SchedulerForm.cs`.

### Layout (`BuildDashboardTab`, line ~337)

Replace the `FlowLayoutPanel { Dock = Fill, FlowDirection = TopDown }` that holds the
Refresh button row + grid with a horizontal `SplitContainer`:

- `Panel1`: the existing Refresh button row (a `FlowLayoutPanel { AutoSize = true }`).
- `Panel2`: `_dashboardGrid` with `Dock = Fill`.
- `Orientation = Horizontal` (splitter drags up/down).
- Initial `SplitterDistance` chosen so the grid gets the majority of the tab height
  (the grid area starts generous; the user can drag it larger or smaller).
- `IsSplitterFixed` stays `false` (the default) so the splitter is draggable.
- Keep the tab page's `Padding` and `AutoScroll`.

### Column widths (`RefreshDashboard`, line ~348)

Columns are auto-generated from the anonymous type in `RefreshDashboard`, so widths are
assigned after `_dashboardGrid.DataSource = ...` is set. Keep
`AutoSizeColumnsMode = Fill` and set per-column `FillWeight`:

| Column | FillWeight |
|---|---|
| Name | 10 |
| Status | 10 |
| LastSuccess | 35 |
| LastFailure | 35 |
| ConsecutiveFailures | 10 |

This gives the two ISO-date columns roughly 3.5x the room of the others (~35/100 of the
width each instead of ~20/100).

Optionally set `AutoSizeRowsMode = DisplayedCells` so rows fit their content height.

### Out of scope

- `HealthDashboardReader` / data source (unchanged).
- Refresh logic (unchanged).
- The other four tabs and the form size (unchanged).

## Testing

No new automated tests — this is a layout-only change with no logic. Verification is a
`dotnet build` (0 errors) plus a manual run of `scripts/start-scheduler-ui.ps1` to
confirm: the grid starts tall, the splitter drags, and timestamp columns show full
values.
