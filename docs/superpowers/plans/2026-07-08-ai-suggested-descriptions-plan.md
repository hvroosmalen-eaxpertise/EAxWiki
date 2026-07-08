# AI-Suggested Descriptions Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax.

**Goal:** Add a "Suggest a description" button to empty notes widgets that calls a local or cloud LLM and populates the textarea with a draft.

**Architecture:** Browser `ai-suggest.js` → `POST /api/ai-suggest` → `IEaReader.GetElementSummary` fetches context from EA COM → builds prompt → calls OpenAI-compatible LLM endpoint → returns draft text. User reviews/edits/saves via existing notes-editor flow.

**Tech Stack:** .NET 10, ASP.NET Minimal API, EA COM interop, OpenAI-compatible HTTP API (any provider), vanilla JS

**LLM server:** `llama-server` (llama.cpp) running locally at `http://localhost:8080/v1` with `Llama-3.2-3B-Instruct-Q4_K_M.gguf`

## Global Constraints

- Use existing patterns: inline JS strings in `InfrastructureWriter.cs`, dataset attributes for config, `X-EAxWiki-Token` auth
- `--ai-endpoint` empty = AI feature disabled entirely (no Suggest button rendered, no endpoint registered)
- Minimal-context elements (no relationships AND no tagged values) return 204 without calling LLM
- `ai-suggest.js` is a separate file from `notes-editor.js` (decision A)
- The Spec is at `docs/superpowers/specs/2026-07-08-ai-suggested-descriptions-design.md`
- All 244 existing tests must continue passing
- No new NuGet dependencies — use `System.Net.Http.HttpClient` for LLM calls

---

### Task 1: Model + Interface + `ExportContext.AiConfigured`

**Files:**
- Create: `src/EAxWiki.Core/Models/EaElementSummary.cs`
- Modify: `src/EAxWiki.Core/Interfaces/IEaReader.cs` (add method)
- Modify: `src/EAxWiki.Export/ExportContext.cs` (add AiConfigured)

**Interfaces:**
- Consumes: nothing
- Produces: `EaElementSummary` record, `IEaReader.GetElementSummary(int)`, `ExportContext.AiConfigured`

- [ ] **Step 1: Create `EaElementSummary.cs`**

```csharp
using EAxWiki.Core.Models;

namespace EAxWiki.Core.Models;

public record EaElementSummary
{
    public int ElementId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Stereotype { get; init; } = string.Empty;
    public string PackagePath { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public List<AttributeInfo> Attributes { get; init; } = [];
    public List<MethodInfo> Methods { get; init; } = [];
    public List<TaggedValueInfo> TaggedValues { get; init; } = [];
    public List<RelationshipInfo> Relationships { get; init; } = [];
}

public record AttributeInfo(string Name, string Type);
public record MethodInfo(string Name, string ReturnType, bool IsStatic);
public record TaggedValueInfo(string Name, string Value);
public record RelationshipInfo(string Type, string Direction, string TargetName, string TargetType);
```

- [ ] **Step 2: Add method to `IEaReader.cs`**

After `ExportDiagramImage` line, add:

```csharp
EaElementSummary? GetElementSummary(int elementId);
```

- [ ] **Step 3: Add `AiConfigured` to `ExportContext`**

In `src/EAxWiki.Export/ExportContext.cs`, after `ApiToken` line, add:

```csharp
public bool AiConfigured { get; init; } = false;
```

- [ ] **Step 4: Build and test**

```bash
dotnet build
```
Expected: 0 errors.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Core/Models/EaElementSummary.cs
git add src/EAxWiki.Core/Interfaces/IEaReader.cs
git add src/EAxWiki.Export/ExportContext.cs
git commit -m "feat: add EaElementSummary model, IEaReader.GetElementSummary, ExportContext.AiConfigured"
```

---

### Task 2: EA Reader Implementation

**Files:**
- Modify: `src/EAxWiki.EA/EaReader.cs` (implement `GetElementSummary`)
- Modify: `src/EAxWiki.EA/EaReaderStaDispatcher.cs` (add dispatch wrapper)

**Interfaces:**
- Consumes: `EaElementSummary` from Task 1
- Produces: working `GetElementSummary` on both `EaReader` and `EaReaderStaDispatcher`

- [ ] **Step 1: Add mapper helpers to `EaReader.cs`**

Add private mapper methods before `ExportDiagramImage`:

```csharp
private static List<AttributeInfo> MapAttributesForSummary(EA.Element element)
{
    var result = new List<AttributeInfo>();
    if (element.Attributes is EA.Collection attrs)
        for (short i = 0; i < attrs.Count; i++)
            if (attrs.GetAt(i) is EA.Attribute attr)
                result.Add(new AttributeInfo(attr.Name, attr.Type));
    return result;
}

private static List<MethodInfo> MapMethodsForSummary(EA.Element element)
{
    var result = new List<MethodInfo>();
    if (element.Methods is EA.Collection methods)
        for (short i = 0; i < methods.Count; i++)
            if (methods.GetAt(i) is EA.Method method)
                result.Add(new MethodInfo(method.Name, method.ReturnType, method.IsStatic));
    return result;
}

private static List<TaggedValueInfo> MapTaggedValuesForSummary(EA.Element element)
{
    var result = new List<TaggedValueInfo>();
    if (element.TaggedValues is EA.Collection tvs)
        for (short i = 0; i < tvs.Count; i++)
            if (tvs.GetAt(i) is EA.TaggedValue tv)
                result.Add(new TaggedValueInfo(tv.Name, tv.Value));
    return result;
}

private List<RelationshipInfo> MapRelationshipsForSummary(EA.Element element)
{
    var result = new List<RelationshipInfo>();
    if (element.Connectors is EA.Collection connectors)
    {
        for (short i = 0; i < connectors.Count; i++)
        {
            if (connectors.GetAt(i) is not EA.Connector conn) continue;
            var isSource = conn.ClientID == element.ElementID;
            var targetId = isSource ? conn.SupplierID : conn.ClientID;
            var target = _repository?.GetElementByID(targetId);
            result.Add(new RelationshipInfo(
                conn.Type,
                isSource ? "source→target" : "target→source",
                target?.Name ?? "(deleted)",
                target?.Type ?? "Unknown"));
        }
    }
    return result;
}
```

- [ ] **Step 2: Add `GetElementSummary` method to `EaReader.cs`**

Add after `GetElementStatus` and before `UpdateElementStatus`:

```csharp
public EaElementSummary? GetElementSummary(int elementId)
{
    if (_repository == null)
        throw new InvalidOperationException("Repository is not open.");
    var element = _repository.GetElementByID(elementId);
    if (element == null) return null;

    var path = new List<string>();
    var pkg = _repository.GetPackageByID(element.PackageID);
    while (pkg != null)
    {
        path.Add(pkg.Name);
        pkg = pkg.ParentID != 0 ? _repository.GetPackageByID(pkg.ParentID) : null;
    }
    path.Reverse();

    return new EaElementSummary
    {
        ElementId = element.ElementID,
        Name = element.Name,
        Type = element.Type,
        Stereotype = element.Stereotype ?? element.FQStereotype ?? string.Empty,
        PackagePath = string.Join("/", path),
        Status = element.Status ?? string.Empty,
        Attributes = MapAttributesForSummary(element),
        Methods = MapMethodsForSummary(element),
        TaggedValues = MapTaggedValuesForSummary(element),
        Relationships = MapRelationshipsForSummary(element)
    };
}
```

- [ ] **Step 3: Add dispatch wrapper to `EaReaderStaDispatcher.cs`**

Add after the existing dispatch methods:

```csharp
public EaElementSummary? GetElementSummary(int elementId) =>
    Dispatch(r => r.GetElementSummary(elementId));
```

- [ ] **Step 4: Build and test**

```bash
dotnet build
dotnet test --no-build
```
Expected: 0 errors, 244 passed.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.EA/EaReader.cs src/EAxWiki.EA/EaReaderStaDispatcher.cs
git commit -m "feat: implement GetElementSummary in EaReader + dispatcher"
```

---

### Task 3: Config, CLI Flags, and Env Var Wiring

**Files:**
- Modify: `src/EAxWiki/Config.cs` (add properties + argument parsing)
- Modify: `src/EAxWiki/Program.cs` (ShowUsage + env var for export)

**Interfaces:**
- Consumes: nothing new
- Produces: `Config.AiEndpoint`, `Config.AiModel`, `Config.AiKey` — used by Task 4 for the API endpoint and Task 5 for the export-time env var

- [ ] **Step 1: Add AI properties to `Config.cs`**

After `CertPassword` line, add:

```csharp
public string AiEndpoint { get; set; } = "";
public string AiModel { get; set; } = "llama-3.2-3b";
public string AiKey { get; set; } = "";
```

- [ ] **Step 2: Add argument parsing to `Config.Load`**

After `--cert-password` case, add:

```csharp
case "--ai-endpoint":
    if (i + 1 >= args.Length)
        throw new ArgumentException($"Option {args[i]} requires a value");
    AiEndpoint = args[++i];
    break;
case "--ai-model":
    if (i + 1 >= args.Length)
        throw new ArgumentException($"Option {args[i]} requires a value");
    AiModel = args[++i];
    break;
case "--ai-key":
    if (i + 1 >= args.Length)
        throw new ArgumentException($"Option {args[i]} requires a value");
    AiKey = args[++i];
    break;
```

- [ ] **Step 3: Add `ShowUsage` lines in `Program.cs`**

After the `--cert-password` line, add:

```csharp
Console.WriteLine("  --ai-endpoint <url>    OpenAI-compatible API base URL (default: empty = disabled)");
Console.WriteLine("  --ai-model <name>      Model name sent to AI endpoint (default: llama-3.2-3b)");
Console.WriteLine("  --ai-key <key>         API key for AI endpoint (optional for local LLMs)");
```

- [ ] **Step 4: Set env var for export mode in `Program.cs`**

After the `EAXWIKI_API_PORT` env var line (line 179), add:

```csharp
if (!string.IsNullOrEmpty(config.AiEndpoint))
    Environment.SetEnvironmentVariable("EAXWIKI_AI_ENDPOINT", config.AiEndpoint);
```

- [ ] **Step 5: Wire `AiConfigured` in `MarkdownExporter.cs`**

After `ApiToken` line (line 48), add:

```csharp
var aiEndpoint = Environment.GetEnvironmentVariable("EAXWIKI_AI_ENDPOINT");
var aiConfigured = !string.IsNullOrEmpty(aiEndpoint);
```

And add `AiConfigured = aiConfigured` to the `with` expression:

```csharp
var ctx = ContextBuilder.Build(packages, outputPath, force) with
{
    StatusTypes = statusTypes,
    ApiPort = apiPort,
    ApiToken = apiToken,
    AiConfigured = aiConfigured,
};
```

- [ ] **Step 6: Build and test**

```bash
dotnet build
dotnet test --no-build
```
Expected: 0 errors, 244 passed.

- [ ] **Step 7: Commit**

```bash
git add src/EAxWiki/Config.cs src/EAxWiki/Program.cs src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add --ai-endpoint/--ai-model/--ai-key CLI flags + export env var wiring"
```

---

### Task 4: `POST /api/ai-suggest` Endpoint

**Files:**
- Modify: `src/EAxWiki/WikiWritebackServer.cs` (new endpoint + request model + prompt builder)

**Interfaces:**
- Consumes: `Config.AiEndpoint`, `Config.AiModel`, `Config.AiKey`, `IEaReader.GetElementSummary`
- Produces: registered `POST /api/ai-suggest` endpoint

- [ ] **Step 1: Add request model near the top of `WikiWritebackServer.cs`**

After `RowNotesChangeRequest`, add:

```csharp
internal record AiSuggestRequest(int ElementId);
```

- [ ] **Step 2: Add the endpoint before the `app.Run()` line**

Find the last `app.Map*` call (around line 350) and add before `app.Run()`:

```csharp
app.MapPost("/api/ai-suggest", async (AiSuggestRequest req, HttpContext context) =>
{
    try
    {
        if (string.IsNullOrEmpty(config.AiEndpoint))
            return Results.Json(new { message = "AI suggestions are not configured." }, statusCode: 501);

        var summary = reader.GetElementSummary(req.ElementId);
        if (summary == null)
            return Results.Json(new { message = "Element not found." }, statusCode: 404);

        if (summary.Relationships.Count == 0 && summary.TaggedValues.Count == 0)
            return Results.Json(new { message = "Not enough context to suggest a description." }, statusCode: 204);

        var prompt = BuildSuggestPrompt(summary);

        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        var llmBody = new
        {
            model = config.AiModel,
            messages = new[]
            {
                new { role = "system", content = "You are a technical writer for enterprise architecture documentation. Write a concise 2-3 sentence description for the following element. Use the context provided (type, stereotype, package, relationships, tagged values). Be factual, precise, and write in plain English. Do not use markdown." },
                new { role = "user", content = prompt }
            },
            max_tokens = 300,
            temperature = 0.3,
            stream = false
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"{config.AiEndpoint.TrimEnd('/')}/chat/completions")
        {
            Content = JsonContent.Create(llmBody)
        };
        if (!string.IsNullOrEmpty(config.AiKey))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AiKey);

        var response = await httpClient.SendAsync(request, context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            logger.LogWarning("AI endpoint returned {StatusCode}: {Error}", (int)response.StatusCode, errorBody);
            return Results.Json(new { message = "AI service returned an error." }, statusCode: 502);
        }

        var result = await response.Content.ReadFromJsonAsync<LlmChatResponse>();
        var suggestion = result?.choices?.Length > 0 ? result.choices[0].message.content?.Trim() : null;

        if (string.IsNullOrEmpty(suggestion))
            return Results.Json(new { message = "AI returned no suggestion." }, statusCode: 422);

        AuditLogger.Log(outputPath, "ai-suggest", $"element {req.ElementId}", "suggested");
        return Results.Json(new { suggestion });
    }
    catch (TaskCanceledException)
    {
        return Results.Json(new { message = "AI request timed out." }, statusCode: 504);
    }
    catch (HttpRequestException ex)
    {
        logger.LogWarning(ex, "AI endpoint unreachable");
        return Results.Json(new { message = "AI service unavailable." }, statusCode: 502);
    }
});
```

- [ ] **Step 3: Add the prompt builder and response model classes**

Before `RunAsync`, add:

```csharp
private static string BuildSuggestPrompt(EaElementSummary el)
{
    var sb = new StringBuilder();
    sb.AppendLine($"Name: {el.Name}");
    sb.AppendLine($"Type: {el.Type}");
    sb.AppendLine($"Stereotype: {el.Stereotype}");
    sb.AppendLine($"Package: {el.PackagePath}");
    sb.AppendLine($"Status: {el.Status}");

    if (el.Attributes.Count > 0)
    {
        sb.AppendLine();
        sb.AppendLine("Attributes:");
        foreach (var a in el.Attributes)
            sb.AppendLine($"  - {a.Name}: {a.Type}");
    }

    if (el.TaggedValues.Count > 0)
    {
        sb.AppendLine();
        sb.AppendLine("TaggedValues:");
        foreach (var t in el.TaggedValues)
            sb.AppendLine($"  - {t.Name}: {t.Value}");
    }

    if (el.Relationships.Count > 0)
    {
        sb.AppendLine();
        sb.AppendLine("Relationships:");
        foreach (var r in el.Relationships)
            sb.AppendLine($"  - {r.Direction} {r.Type} → {r.TargetName} ({r.TargetType})");
    }

    return sb.ToString();
}

private record LlmChatMessage(string role, string content);
private record LlmChatChoice(LlmChatMessage message);
private record LlmChatResponse(LlmChatChoice[]? choices);
```

- [ ] **Step 4: Build and test**

```bash
dotnet build
dotnet test --no-build
```
Expected: 0 errors, 244 passed.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki/WikiWritebackServer.cs
git commit -m "feat: add POST /api/ai-suggest endpoint"
```

---

### Task 5: Frontend — `ai-suggest.js` + `NotesWidgetRenderer` + `InfrastructureWriter`

**Files:**
- Modify: `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` (add `WriteAiSuggestScriptAsync`)
- Modify: `src/EAxWiki.Export/Renderers/NotesWidgetRenderer.cs` (add `data-ai-configured`)
- Modify: `src/EAxWiki.Export/MarkdownExporter.cs` (call the new writer method)

**Interfaces:**
- Consumes: `ExportContext.AiConfigured` from Task 1
- Produces: rendered `data-ai-configured` attribute on notes widget, `ai-suggest.js` added to output

- [ ] **Step 1: Add `data-ai-configured` to `NotesWidgetRenderer.cs`**

Change line 16 from:
```csharp
$" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\">";
```
to:
```csharp
$" data-api-token=\"{HtmlHelpers.HtmlEscape(ctx.ApiToken)}\"" +
$" data-ai-configured=\"{ctx.ApiPort > 0 && ctx.AiConfigured ? "true" : "false"}\">";
```

- [ ] **Step 2: Add `WriteAiSuggestScriptAsync` to `InfrastructureWriter.cs`**

After `WriteRowNotesEditorScriptAsync` (after line 714), add:

```csharp
public async Task WriteAiSuggestScriptAsync(string outputDir, CancellationToken ct = default)
{
    const string js = """
(function () {
  'use strict';

  function initAiSuggest() {
    var widget = document.getElementById('ea-notes-editor');
    if (!widget || widget.dataset.aiConfigured !== 'true') return;
    if (widget.querySelector('.ea-suggest-btn')) return;

    var contentDiv = widget.querySelector('.ea-notes-content');
    if (!contentDiv) return;
    var isPlaceholder = !!contentDiv.querySelector('.ea-notes-placeholder');
    if (!isPlaceholder) return;

    var eaId  = parseInt(widget.dataset.eaId, 10);
    var port  = widget.dataset.apiPort || '8001';
    var token = widget.dataset.apiToken || '';
    var apiBase = window.location.protocol + '//' + window.location.hostname + ':' + port;

    var btn = document.createElement('button');
    btn.className = 'ea-suggest-btn';
    btn.textContent = 'Suggest a description';
    btn.type = 'button';

    var msg = document.createElement('span');
    msg.className = 'ea-suggest-msg';
    msg.style.marginLeft = '8px';

    var container = document.createElement('div');
    container.style.marginTop = '8px';
    container.appendChild(btn);
    container.appendChild(msg);

    var editBtn = document.getElementById('ea-notes-edit-btn');
    if (editBtn && editBtn.parentNode) {
      editBtn.parentNode.insertBefore(container, editBtn.nextSibling);
    }

    btn.addEventListener('click', function () {
      btn.disabled = true;
      btn.textContent = 'Generating...';
      msg.textContent = '';
      msg.style.color = '';

      fetch(apiBase + '/api/ai-suggest', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'X-EAxWiki-Token': token },
        body: JSON.stringify({ elementId: eaId })
      })
      .then(function (r) {
        return r.json().then(function (d) { return { ok: r.ok, status: r.status, data: d }; });
      })
      .then(function (res) {
        if (res.ok) {
          var textarea = widget.querySelector('.ea-notes-textarea');
          if (textarea) {
            textarea.value = res.data.suggestion;
            msg.textContent = 'Draft loaded — review and save.';
            msg.style.color = '#2e7d32';
          }
        } else if (res.status === 204) {
          msg.textContent = res.data.message || 'Not enough context to suggest a description.';
          msg.style.color = '#666';
        } else {
          msg.textContent = 'Error: ' + (res.data.message || 'Unknown error');
          msg.style.color = '#c62828';
        }
        btn.disabled = false;
        btn.textContent = 'Suggest a description';
      })
      .catch(function (e) {
        msg.textContent = 'Could not reach AI service.';
        msg.style.color = '#c62828';
        btn.disabled = false;
        btn.textContent = 'Suggest a description';
        console.error('EAxWiki ai-suggest error:', e);
      });
    });
  }

  if (typeof document$ !== 'undefined') {
    document$.subscribe(function () { initAiSuggest(); });
  } else {
    document.addEventListener('DOMContentLoaded', initAiSuggest);
  }
})();
""";
    await writer.WriteFileAsync(Path.Combine(outputDir, "ai-suggest.js"), js, ct);
}
```

- [ ] **Step 3: Add the call in `MarkdownExporter.cs`**

In the `viewTasks` list in `MarkdownExporter.cs`, after `WriteRowNotesEditorScriptAsync` line, add:

```csharp
infrastructure.WriteAiSuggestScriptAsync(outputPath, cancellationToken),
```

- [ ] **Step 4: Build and test**

```bash
dotnet build
dotnet test --no-build
```
Expected: 0 errors, 244 passed.

- [ ] **Step 5: Commit**

```bash
git add src/EAxWiki.Export/Exporters/InfrastructureWriter.cs
git add src/EAxWiki.Export/Renderers/NotesWidgetRenderer.cs
git add src/EAxWiki.Export/MarkdownExporter.cs
git commit -m "feat: add ai-suggest.js widget + NotesWidgetRenderer data-ai-configured flag"
```

---

### Task 6: Scheduler UI AI Config Fields

**Files:**
- Modify: `src/EAxWiki.SchedulerUI/SchedulerForm.cs` (add AI config fields to Settings tab)
- The AI values save to `.eaxwiki-config.json` alongside existing settings

- [ ] **Step 1: Add AI fields in `SchedulerForm.cs`**

In the Settings tab (find the existing textbox creation pattern), add three new fields after the existing webhook text fields:

```csharp
var lblAiEndpoint = new Label { Text = "AI Endpoint:", Location = new Point(15, nextY), Size = new Size(120, 23) };
var txtAiEndpoint = new TextBox { Text = savedConfig.AiEndpoint ?? "", Location = new Point(140, nextY), Width = 350 };
Controls.Add(lblAiEndpoint); Controls.Add(txtAiEndpoint);
nextY += 30;

var lblAiModel = new Label { Text = "AI Model:", Location = new Point(15, nextY), Size = new Size(120, 23) };
var txtAiModel = new TextBox { Text = savedConfig.AiModel ?? "llama-3.2-3b", Location = new Point(140, nextY), Width = 350 };
Controls.Add(lblAiModel); Controls.Add(txtAiModel);
nextY += 30;

var lblAiKey = new Label { Text = "AI Key:", Location = new Point(15, nextY), Size = new Size(120, 23) };
var txtAiKey = new TextBox { Text = savedConfig.AiKey ?? "", Location = new Point(140, nextY), Width = 350, PasswordChar = '*' };
Controls.Add(lblAiKey); Controls.Add(txtAiKey);
nextY += 30;
```

Also save these values in the save handler:
```csharp
savedConfig.AiEndpoint = txtAiEndpoint.Text;
savedConfig.AiModel = txtAiModel.Text;
savedConfig.AiKey = txtAiKey.Text;
```

- [ ] **Step 2: Build the scheduler UI project**

```bash
dotnet build src/EAxWiki.SchedulerUI
```
Expected: 0 errors.

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki.SchedulerUI/SchedulerForm.cs
git commit -m "feat: add AI endpoint/model/key fields to SchedulerUI Settings tab"
```

---

### Task 7: Integration Smoke Test

- [ ] **Step 1: Full build + test**

```bash
dotnet build
dotnet test --no-build
dotnet build src/EAxWiki.SchedulerUI
```

Expected: 0 errors, 244 passed.

- [ ] **Step 2: Verify end-to-end**

Start `llama-server`:
```powershell
Start-Process -FilePath "E:\llama-cpp\llama-server.exe" -ArgumentList "-m E:\models\llama-3.2-3b-q4.gguf -c 4096 --port 8080 --n-gpu-layers 0" -NoNewWindow -PassThru
```

Export with AI config:
```powershell
dotnet run --project src/EAxWiki --repo "..." --ai-endpoint "http://localhost:8080/v1"
```

Start the API server:
```powershell
dotnet run --project src/EAxWiki --api --ai-endpoint "http://localhost:8080/v1"
```

Open a wiki page with empty Notes — verify the "Suggest a description" button appears, click it, verify a draft populates the textarea within ~20s.

- [ ] **Step 3: Push**

```bash
git push
```
