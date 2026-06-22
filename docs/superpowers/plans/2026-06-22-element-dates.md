# Element Dates (Issue #8) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add `CreatedDate` to `EaElement`, read from EA COM, display on element pages alongside existing `ModifiedDate`.

**Architecture:** 3-layer: Model property → COM reader mapping → exporter rendering. Follows the established pattern from `ModifiedDate`, `StereotypeEx`, `FQStereotype`.

**Tech Stack:** .NET 10, C#, EA COM Interop

## Global Constraints

- All user-derived filenames sanitized via `SanitizeName()`
- Relative links use `Path.GetRelativePath` + `.Replace('\\', '/')`
- Target framework: net10.0

---

### Task 1: Add CreatedDate to model, reader, and exporter

**Files:**
- Modify: `src/EAxWiki.Core/Models/EaElement.cs:5-18`
- Modify: `src/EAxWiki.EA/EaReader.cs:80-93`
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs:465-472`

**Interfaces:**
- Consumes: (none)
- Produces: `EaElement.CreatedDate` (DateTime?), visible on element pages

- [ ] **Step 1: Add `CreatedDate` to `EaElement`**

In `src/EAxWiki.Core/Models/EaElement.cs`, add after `ModifiedDate`:

```csharp
public DateTime? CreatedDate { get; set; }
```

- [ ] **Step 2: Read `CreatedDate` in `MapElement`**

In `src/EAxWiki.EA/EaReader.cs`, inside `MapElement`, add after `ModifiedDate`:

```csharp
CreatedDate = eaElement.CreatedDate as DateTime?,
```

Keep existing comma on the line above.

- [ ] **Step 3: Render dates on element pages**

In `src/EAxWiki.Export/MarkdownExporter.cs`, in `WriteElementAsync`, change the metadata block from:

```csharp
var lines = new List<string>
{
    $"# {element.Name}",
    string.Empty,
    $"**Type:** {element.Type}  ",
    $"**Stereotype:** {element.Stereotype}  ",
    string.Empty
};
```

to:

```csharp
var createdStr = element.CreatedDate?.ToString("yyyy-MM-dd") ?? "-";
var modifiedStr = element.ModifiedDate == DateTime.MinValue ? "-" : element.ModifiedDate.ToString("yyyy-MM-dd");
var lines = new List<string>
{
    $"# {element.Name}",
    string.Empty,
    $"**Type:** {element.Type}  ",
    $"**Stereotype:** {element.Stereotype}  ",
    $"**Created:** {createdStr}  **Modified:** {modifiedStr}",
    string.Empty
};
```

- [ ] **Step 4: Build to verify**

```
dotnet build src/EAxWiki.Export/
```

Expected: Build succeeded, 0 warnings, 0 errors.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Core/Models/EaElement.cs src/EAxWiki.EA/EaReader.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: show element CreatedDate and ModifiedDate on element pages (Issue #8)"
```
