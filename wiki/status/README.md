# Runtime-generated wiki status files

This directory contains small runtime-generated files used by the exporter/monitor and the write-back server. They are instance-specific and intended to be transient — they are ignored in the repository by default and will be recreated/updated by local runs of the exporter or monitor.

Common files

- `config.md` — rendered configuration/status information for the running instance.
- `errors.md` — recent errors / validation output rendered as a page.

Why they are ignored

These files vary per machine and run and therefore cause a lot of noisy commits and merge conflicts if tracked. The repository's `.gitignore` purposefully prevents them from being committed.

If you really want to track them

It's usually better to commit a template or example file under `docs/` rather than the live runtime files. If you still want to start tracking these exact files, stop Git from ignoring them by removing them from the index (this preserves the files on disk):

```powershell
# stop tracking while keeping local copies
git rm --cached wiki/status/config.md wiki/status/errors.md
# commit the change
git commit -m "chore: track runtime wiki status files"
# push if desired
git push
```

Note: doing the above will make these files tracked for everyone. That will likely increase merge conflicts and should be coordinated with your team.

Recommended alternative

Create a committed template in `docs/` that documents the expected structure or contains example content, and keep the runtime files ignored.
