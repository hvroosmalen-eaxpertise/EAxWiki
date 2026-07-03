# Configurable scheduling for unattended runs (Issue #38)

## Problem

`register-scheduled-task.ps1` (built for #37) registers a single fixed-interval Task Scheduler
trigger — the same cadence runs 24/7, every day. The right cadence isn't actually constant: a
weekday work-hours window benefits from a fast interval (fresh data while people are actively
editing the model), while nights and weekends don't need — and shouldn't pay the EA COM cost of —
the same frequency. #37 shipped the fixed interval as a documented stopgap; this issue is about
replacing it with something that reflects that real day/night difference, without touching the
monitoring/alerting/retry logic #37 already built.

## Decisions

- **This is not timezone-aware scheduling for a global team**, despite the issue's title. EA COM
  automation only runs on one Windows machine in one timezone — "day vs night" can only ever mean
  *that machine's own local clock*, not per-reader adaptation. Confirmed during the design
  brainstorm that the real need is narrower than the title implies: faster cadence during weekday
  work hours, slower cadence at night and on weekends, all on the exporting machine's own clock.
  Per-timezone and per-weekday granularity (both listed in the issue's original draft scope) are
  explicitly **not** being built — a weekday/weekend split is enough.
- **Mechanism: two native Task Scheduler triggers on one task, not a config file read by the
  wrapper.** Windows Task Scheduler already supports multiple triggers per task, each independently
  scoped by day-of-week and time-of-day, each with its own repetition interval. That native
  capability does the entire job:
  - **Baseline trigger** — every day, all 24 hours, slow interval (e.g. every 4 hours). This is the
    "always-alive" heartbeat: Slack Start notifications and the health page keep updating even when
    nothing else fires, so a real failure at 2am on a Saturday doesn't look identical to "it's just
    paused."
  - **Boost trigger** — weekdays only, within the work-hours window, fast interval (e.g. every 10
    minutes). Layered on top of the baseline.
  - Weekday daytime therefore has both triggers active simultaneously. That's fine and deliberately
    not deduplicated at the trigger level: `MultipleInstances: IgnoreNew` (already set by the #37
    work) silently drops whichever trigger's fire is redundant at any given moment, so overlap
    between the baseline and boost triggers costs nothing.
  - This means **`monitor-export-and-serve.ps1` (the #37 wrapper) requires zero changes.** It has
    no idea day/night scheduling exists — it just gets invoked whenever Windows decides to invoke
    it, exactly as before. This satisfies the issue's own stated constraint: replace the fixed-
    interval trigger "without requiring a redesign of the monitoring/alerting/retry logic."
- **Configuration surface: CLI flags on `register-scheduled-task.ps1`, not a config file or wizard.**
  Confirmed during the brainstorm that "configurable without touching code" was the actual
  requirement — flags satisfy that; a new file format or interactive wizard would be solving a
  problem nobody described. The existing flat `--interval-minutes`/`--interval-hours` mode is kept
  unchanged as the default (single trigger, current #37 behavior) for anyone who doesn't want
  day/night differentiation. New flags — `--work-start`, `--work-end`, `--work-interval-minutes`,
  `--off-hours-interval-minutes` — opt into the two-trigger mode instead.
- **Re-registration, not live config.** Changing the work-hours window later means re-running
  `register-scheduled-task.ps1` with new flags. Confirmed acceptable during the brainstorm — this
  is a rarely-changed setting, and avoiding a live-reloaded config file keeps the wrapper untouched
  (per the point above) and avoids inventing a persistence format for values that live entirely in
  Task Scheduler's own trigger definitions once registered.
- **A settings GUI was raised and deliberately split out.** The brainstorm surfaced a desire for
  "configuration via a UI" for *all* EAxWiki config (repo path, ports, webhook, and eventually
  schedule) — a standalone desktop app, not something this project has a tech stack for today. That
  is a materially larger, separate effort and does not block this issue; tracked as **#40**. Once
  built, it would simply become another way to set the same flags this issue introduces.

## Architecture

- **`scripts/register-scheduled-task.ps1`** (modified): gains a second registration mode.
  - Existing mode (unchanged): `--interval-minutes` / `--interval-hours` → one
    `New-ScheduledTaskTrigger -Once -RepetitionInterval ... -RepetitionDuration (10 years)`, as today.
  - New mode: presence of `--work-start`/`--work-end` switches to two-trigger registration:
    - Baseline: `New-ScheduledTaskTrigger -Daily -At <midnight>`, registered every day.
    - Boost: `New-ScheduledTaskTrigger -Weekly -DaysOfWeek Mon,Tue,Wed,Thu,Fri -At <work-start>`.
    - Both triggers passed to `Register-ScheduledTask -Trigger @($baseline, $boost) ...`, same action/settings (`MultipleInstances IgnoreNew`, execution time limit) as today.
  - **Implementation gotcha (verified via `Get-Command New-ScheduledTaskTrigger`'s own
    `ParameterSets`, not assumed): `-RepetitionInterval`/`-RepetitionDuration` only exist in the
    `-Once` parameter set.** `-Daily` and `-Weekly` don't expose sub-day repetition through the
    cmdlet at all. The underlying CIM trigger object supports a `.Repetition` sub-object regardless
    of which parameter set created it, though — the cmdlet just doesn't surface it for Daily/Weekly.
    Workaround: build a throwaway `-Once` trigger purely to get a correctly-populated `.Repetition`
    CimInstance (`(New-ScheduledTaskTrigger -Once -At ... -RepetitionInterval ... -RepetitionDuration ...).Repetition`),
    then assign that object directly onto the real Daily/Weekly trigger's `.Repetition` property
    before registering. No direct CIM/XML construction needed.
  - Validation: `--work-end` must be after `--work-start` on the same day (no overnight-spanning
    work window — if that's ever needed, it's a new ask, not this issue's scope); 5-minute floor on
    both `--work-interval-minutes` and `--off-hours-interval-minutes`, same rationale as the
    existing interval floor. All four day/night flags are required together — no partial
    combination is accepted. Time values parsed via `[DateTime]::TryParseExact` with explicit
    `InvariantCulture` (not `$null` — passing `$null` for the `IFormatProvider` argument fails
    overload resolution) and a pre-typed `[ref]` output variable (`[DateTime]::MinValue`, not
    `$null` — an untyped `$null` also fails overload resolution on the `[ref]` parameter).
  - Post-registration check (already added for the #37/original-#38 duration bug) extends
    naturally: confirm the task exists *and* has the expected trigger count after registration.
- **`monitor-export-and-serve.ps1`**: unchanged, per the design decision above.
- **No new files.** No config file, no wizard, no `.eaxwiki` schema changes.

## Open questions / deferred

- Exact default values for work-start/work-end/work-interval/off-hours-interval — left as required
  flags with no baked-in defaults for the day/night mode (unlike the existing single-interval mode's
  4-hour default), since "work hours" is inherently deployment-specific and a wrong silent default
  is worse than requiring an explicit choice.
- The settings GUI (#40) that would eventually make these flags editable without touching a
  terminal at all.
- Per-weekday (not just weekday/weekend) and true timezone-adaptive scheduling remain explicitly
  out of scope — revisit only if a concrete need surfaces, not preemptively.

## Related

- Builds on: #37 (monitoring/alerting/retry — `scripts/monitor-export-and-serve.ps1`,
  `scripts/register-scheduled-task.ps1`), left untouched by this design.
- Split out: #40 (unified desktop settings GUI), deliberately not part of this issue.
- Design doc for #37: `docs/superpowers/specs/2026-07-02-issue-37-monitoring-alerting-design.md`.
