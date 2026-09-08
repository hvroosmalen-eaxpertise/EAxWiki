Tracking runtime-generated wiki status files

The `wiki/status/` folder contains runtime artifacts (e.g., `config.md`, `errors.md`) created by the exporter/monitor and write-back server. These files are ignored by default because they are instance-specific and change frequently.

If you want to track them in source control, consider the tradeoffs:

- Pros: your repository contains a snapshot of the last-run status pages.
- Cons: these files will change often, creating noisy commits and merge conflicts among developers and CI.

Two safe approaches

1) Keep runtime files ignored and commit templates/examples

- Create committed example files under `docs/` (e.g. `docs/wiki-status-config-example.md`) and document how they are generated. This preserves a canonical example for reviewers while avoiding noise.

2) Track the exact runtime files (not recommended without team agreement)

If you decide to track `wiki/status/config.md` and `wiki/status/errors.md`, perform these steps in the repository root:

```powershell
# 1) Ensure these files are present on disk and ready to be tracked
# 2) Remove them from the index without deleting local copies
git rm --cached wiki/status/config.md wiki/status/errors.md
# 3) Add or edit .gitignore if needed (the repo already contains entries; remove conflicting ignore lines)
# 4) Commit the change
git commit -m "chore: start tracking wiki/status config and errors"
# 5) Push to remote
git push
```

Alternative: change .gitignore to explicitly "unignore" these files and then add them. Example:

Add a negation to `.gitignore` (not recommended unless you understand other rules):

```
# re-allow these specific files (insert above the rule that ignores the wiki folder)
!/wiki/status/config.md
!/wiki/status/errors.md
```

Then add and commit the files as usual. Note that `.gitignore` rules can be subtle: a negation only has effect if an ancestor path is not excluded by an earlier pattern. Always verify with `git check-ignore -v <path>`.

Recommendation

Create a documentation example under `docs/` and continue ignoring the live `wiki/status/*` files. If you must track them, coordinate with your team and use the `git rm --cached` approach to avoid deleting local copies.
