---
name: export-cycle
description: Use when code changes are ready and need to go through the build-export-commit-push-close cycle with a human partner
---

# Export Cycle

## Overview

Code changes happen in the IDE, but the EA COM export only runs on the user's Windows machine. This skill defines the handoff protocol.

## The Cycle

```mermaid
flowchart LR
    A[I write code] --> B[You build & export]
    B --> C[I review wiki diff]
    C --> D[I commit wiki + push]
    D --> E[I close issue]
```

### Step 1: Write Code (me)

I make changes to the C# code, markdown files, etc. I will tell you when changes are ready for export.

### Step 2: Build & Export (you)

```powershell
dotnet build
.\scripts\export-and-serve.ps1 -RepoPath "model/EurSuRA.qea"
```

Use `-Verbose` for debug-level logging during export.
Use `-Force` for full regeneration (required after template/markdown structure changes — otherwise only data-changed elements are updated).

Review the served wiki in the browser at `http://localhost:8000` to verify the output looks correct.

### Step 3: Commit Wiki Diff (me)

After you confirm the export looks good and you've committed the wiki changes, I review the diff, commit the code changes and push everything.

### Step 4: Close Issue (me)

I close the issue with a summary comment describing what was implemented.

## When NOT to use

- **Read-only exploration** — no export needed, just code review
- **CI/MkDocs-only changes** — no EA export needed (e.g., theme config, README)
- **Wiki-only changes** — you can edit `wiki/` directly; no export cycle needed

## Rationalization Table

| Excuse | Reality |
|--------|---------|
| "The change is trivial, skip export" | Untested exports break in unpredictable ways (EA schema quirks, COM nulls). Always export. |
| "I can just commit the code, you export later" | Committing unverified code risks pushing broken wiki. Code + wiki must be in sync. |
| "The export takes too long" | It takes ~30s. Export after every meaningful change — not after every line. Batch related changes into one cycle. |
