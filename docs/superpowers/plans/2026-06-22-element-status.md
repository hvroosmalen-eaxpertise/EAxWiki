# Element Status (Issue #9) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add `Status` to `EaElement`, read from EA COM, display on element pages inline with Type and Stereotype.

**Architecture:** 3-layer: Model property → COM reader mapping → exporter rendering.

**Tech Stack:** .NET 10, C#, EA COM Interop

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Relative links use `Path.GetRelativePath` + `.Replace('\\', '/')`
- Target framework: net10.0

---

### Task 1: Add Status to model, reader, and exporter

**Files:**
- Modify: `src/EAxWiki.Core/Models/EaElement.cs:5-18`
- Modify: `src/EAxWiki.EA/EaReader.cs:80-93`
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:465-472`

**Interfaces:**
- Consumes: (none)
- Produces: `EaElement.Status` (string), visible on element pages

- [ ] **Step 1: Add `Status` to `EaElement`**

In `src/EAxWiki.Core/Models/EaElement.cs`, add after `PackageId`:

```csharp
public string Status { get; set; } = string.Empty;
```

- [ ] **Step 2: Read `Status` in `MapElement`**

In `src/EAxWiki.EA/EaReader.cs`, inside `MapElement`, add after `PackageId`:

```csharp
Status = eaElement.Status ?? string.Empty,
```

Keep existing comma on the line above.

- [ ] **Step 3: Render Status on element pages**

In `src/EAxWiki.Export/MarkdownExporter.cs`, in `WriteElementAsync`, change the Type line from:

```csharp
$"**Type:** {element.Type}  ",
```

to:

```csharp
string.IsNullOrEmpty(element.Status)
    ? $"**Type:** {element.Type}  **Stereotype:** {element.Stereotype}  "
    : $"**Type:** {element.Type}  **Stereotype:** {element.Stereotype}  **Status:** {element.Status}",
```

- [ ] **Step 4: Build to verify**

```
dotnet build src/EAxWiki.Export/
```

Expected: Build succeeded, 0 warnings, 0 errors.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Core/Models/EaElement.cs src/EAxWiki.EA/EaReader.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: show element Status on element pages (Issue #9)"
```
