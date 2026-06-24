# Config flag bounds checking

## Problem

`Config.Load()` in `src/EAxWiki/Config.cs` increments `i` via `args[++i]` for four value-taking flags (`--repo`, `--name`, `--output`, `--package`) without checking whether the next argument exists. Running `EAxWiki --repo` without a following value throws an unhandled `IndexOutOfRangeException`.

## Fix

Add a bounds guard before each `args[++i]`:

```csharp
if (i + 1 >= args.Length)
    throw new ArgumentException($"Option {args[i]} requires a value");
```

This applies to all four flags that consume a following argument. Boolean flags (`--verbose`, `--force`, `--json`, `--help`) are unaffected — they don't use `++i`.

## Affected file

- `src/EAxWiki/Config.cs` — 4 guards added, ~12 lines total
