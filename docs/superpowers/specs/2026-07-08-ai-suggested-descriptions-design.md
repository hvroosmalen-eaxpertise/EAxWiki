# AI-Suggested First-Draft Descriptions for Empty Notes

- **Issue:** [#74](https://github.com/hvroosmalen-eaxpertise/EAxWiki/issues/74)
- **Date:** 2026-07-08
- **Status:** Approved

## Problem

Many EA elements ship with empty Notes. An author currently writes a description from scratch even though the element's type, stereotype, relationships, and tagged values often imply an obvious first draft.

## Scope

1. **Suggest button** in the notes-editor widget, shown only when Notes are empty.
2. **`POST /api/ai-suggest`** endpoint on the write-back server that reads element context from EA COM, sends it to a configurable LLM, and returns a draft.
3. **CLI flags + Scheduler UI config** for the AI endpoint, model, and optional API key.
4. **Local LLM** runs via `llama-server` as the default provider; any OpenAI-compatible API works via config.

## LLM Choice and Installation

### Local Hardware Profile (this machine)

| Component | Spec |
|-----------|------|
| CPU | Intel i5-10310U @ 1.70GHz (4 cores, 8 threads) |
| RAM | ~24 GB |
| GPU | Integrated Intel UHD Graphics (1 GB VRAM, no CUDA) |
| OS | Windows |
| Free space | E:\ 942 GB, C:\ 0 GB |

No NVIDIA GPU — all inference runs on CPU via llama.cpp.

### Models Tested

| Model | Size | File | Speed (this machine) | Quality |
|-------|------|------|---------------------|---------|
| **Phi-3-mini** (3.8B) Q4_K | 2.23 GB | `phi-3-mini-q4.gguf` | ~17s | Good, factual |
| **Llama 3.2 3B** Q4_K_M | 2.02 GB | `llama-3.2-3b-q4.gguf` | ~17s | Slightly better, more detail |

Both generate a usable 2-3 sentence description in ~17 seconds. The user sees a loading spinner during generation — acceptable for a "click, wait, review, save" flow.

**Recommendation: Llama 3.2 3B.** Smaller file, slightly better output quality, same speed.

### llama.cpp Installation (done, documented for reproducibility)

```powershell
# 1. Create directories on E: (942 GB free)
New-Item -ItemType Directory -Path "E:\llama-cpp" -Force
New-Item -ItemType Directory -Path "E:\models" -Force

# 2. Download llama.cpp Windows CPU-only bundle (17 MB)
$url = "https://github.com/ggml-org/llama.cpp/releases/download/b9911/llama-b9911-bin-win-cpu-x64.zip"
Invoke-WebRequest -Uri $url -OutFile "E:\llama-cpp\llama-cpp.zip"
Expand-Archive -Path "E:\llama-cpp\llama-cpp.zip" -DestinationPath "E:\llama-cpp" -Force

# 3. Download model GGUF (2.0 GB)
$url = "https://huggingface.co/bartowski/Llama-3.2-3B-Instruct-GGUF/resolve/main/Llama-3.2-3B-Instruct-Q4_K_M.gguf"
$wc = New-Object System.Net.WebClient
$wc.DownloadFile($url, "E:\models\llama-3.2-3b-q4.gguf")

# 4. Start the server (can be configured as a Windows service)
.\llama-server.exe -m E:\models\llama-3.2-3b-q4.gguf -c 4096 --port 8080 --n-gpu-layers 0
```

`llama-server` listens on `http://127.0.0.1:8080` and exposes an OpenAI-compatible API (`/v1/chat/completions`, `/v1/models`).

### Alternative: Cloud LLM

The same `--ai-endpoint` config works with any OpenAI-compatible provider:

| Provider | Endpoint | Model | Speed |
|----------|----------|-------|-------|
| OpenAI | `https://api.openai.com/v1` | `gpt-4o-mini` | ~1-2s |
| Claude (Anthropic) | via LiteLLM proxy | `claude-3-haiku` | ~1-3s |
| Azure OpenAI | `https://{resource}.openai.azure.com/v1` | `gpt-4o-mini` | ~1-2s |
| Local (llama-server) | `http://localhost:8080/v1` | `llama-3.2-3b` | ~17s |

## Design

### 1. `IEaReader.GetElementSummary`

**Interface** (`src/EAxWiki.Core/Interfaces/IEaReader.cs`):

```csharp
EaElementSummary? GetElementSummary(int elementId);
```

**Model** (`src/EAxWiki.Core/Models/EaElementSummary.cs`):

```csharp
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

**Implementation** (`src/EAxWiki.EA/EaReader.cs`):

```csharp
public EaElementSummary? GetElementSummary(int elementId)
{
    var element = _repository?.GetElementByID(elementId);
    if (element == null) return null;

    // Resolve package path by walking parent chain
    var path = new List<string>();
    var pkg = _repository?.GetPackageByID(element.PackageID);
    while (pkg != null)
    {
        path.Add(pkg.Name);
        pkg = pkg.ParentID != 0
            ? _repository?.GetPackageByID(pkg.ParentID)
            : null;
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

**EA COM calls involved:**
- `GetElementByID(elementId)` — 1 call
- `GetPackageByID(pkg.ParentID)` — N calls (depth of package hierarchy, typically 1-4)

All calls are sub-millisecond to low-millisecond. No full-model scans.

**Dispatcher** (`src/EAxWiki.EA/EaReaderStaDispatcher.cs`):

```csharp
public EaElementSummary? GetElementSummary(int elementId) =>
    Dispatch(r => r.GetElementSummary(elementId));
```

### 2. New API Endpoint

**`POST /api/ai-suggest`** in `src/EAxWiki/WikiWritebackServer.cs`:

Request:
```json
{ "elementId": 1234 }
```

Handler flow:
1. Authenticate via `X-EAxWiki-Token` (same as all other endpoints).
2. Call `reader.GetElementSummary(elementId)`.
3. If null, return 404.
4. Build a prompt from the summary (see section 5).
5. POST to `{AI_ENDPOINT}/v1/chat/completions` with OpenAI-compatible schema.
6. Return `{ "suggestion": "..." }`.

#### Prompt Template

**System message:**
```
You are a technical writer for enterprise architecture documentation.
Write a concise 2-3 sentence description for the following element.
Use the context provided (type, stereotype, package, relationships, tagged values).
Be factual, precise, and write in plain English. Do not use markdown.
```

**User message:**
```
Name: {Name}
Type: {Type}
Stereotype: {Stereotype}
Package: {PackagePath}
Status: {Status}

Attributes:
{Attributes}

TaggedValues:
{TaggedValues}

Relationships:
{Relationships}
```

Empty sections (no attributes, no relationships, etc.) are omitted.

#### HTTP Call to LLM

```http
POST {AI_ENDPOINT}/chat/completions
Content-Type: application/json
Authorization: Bearer {AI_KEY}  (if set)

{
  "model": "{AI_MODEL}",
  "messages": [
    { "role": "system", "content": "..." },
    { "role": "user", "content": "..." }
  ],
  "max_tokens": 300,
  "temperature": 0.3,
  "stream": false
}
```

### 3. Config & CLI Flags

**`src/EAxWiki/Config.cs`** additions:

| Flag | Env | Default | Purpose |
|------|-----|---------|---------|
| `--ai-endpoint` | `AI_ENDPOINT` | `http://localhost:8080/v1` | OpenAI-compatible API base URL |
| `--ai-model` | `AI_MODEL` | `llama-3.2-3b` | Model name sent in API requests |
| `--ai-key` | `AI_KEY` | `""` | API key (empty = no auth header) |

`ShowUsage()` updated with the new flags.

**Scheduler UI** (`SchedulerForm.cs`): new text fields in the Settings tab for the three values, saved to `.eaxwiki-config.json`.

### 4. Frontend: Suggest Button

**New file:** `src/EAxWiki.Export/wwwroot/ai-suggest.js`

- When the notes widget loads with an empty textarea, inject a "Suggest a description" button below it.
- On click:
  1. Disable button, show spinner/ellipsis.
  2. `POST /api/ai-suggest { elementId }` using the existing `apiBase` and `X-EAxWiki-Token`.
  3. On success: populate the textarea with the suggestion, enable the save button.
  4. On error: show inline error message, re-enable the button.
- User edits the draft freely, then saves normally — the existing `POST /api/notes` flow is untouched.

**In `InfrastructureWriter`** (`src/EAxWiki.Export/Exporters/InfrastructureWriter.cs`):
- Embed the new JS alongside `notes-editor.js`.
- Only include when `ApiPort > 0` (same guard).

### 5. Prompt Context Example

The LLM receives this context for a typical element:

```
Name: ESG Score
Type: Class
Stereotype: Assessment
Package: Sustainability/ESG Framework/Scorecards
Status: Approved

Attributes:
  - scoreValue: decimal
  - calculationDate: datetime

TaggedValues:
  - frequency: quarterly
  - owner: sustainability-team

Relationships:
  - measures → Sustainability Performance (Goal)
  - tracked_by → ESG Dashboard (ApplicationComponent)
```

### 6. Error Handling

| Scenario | HTTP | Browser UX |
|----------|------|------------|
| Element not found | 404 | "Element not found" error, button re-enabled |
| LLM endpoint unreachable | 502 | "AI service unavailable, try again" |
| LLM timeout (>30s) | 504 | "Request timed out, try again" |
| LLM returns empty | 422 | "AI returned no suggestion" |
| Rate limited | 429 | "Too many requests, wait a moment" |
| Invalid/expired token | 401 | Handled by existing auth middleware |

### 7. Files Changed

| File | Change |
|------|--------|
| `src/EAxWiki.Core/Interfaces/IEaReader.cs` | Add `GetElementSummary(int)` |
| `src/EAxWiki.Core/Models/EaElementSummary.cs` | New file (record types) |
| `src/EAxWiki.EA/EaReader.cs` | Implement `GetElementSummary` and mapper helpers |
| `src/EAxWiki.EA/EaReaderStaDispatcher.cs` | Add dispatch wrapper |
| `src/EAxWiki/Config.cs` | Add `--ai-endpoint`, `--ai-model`, `--ai-key` |
| `src/EAxWiki/Program.cs` | Pass AI config to write-back server |
| `src/EAxWiki/WikiWritebackServer.cs` | Add `POST /api/ai-suggest` endpoint |
| `src/EAxWiki.Export/Exporters/InfrastructureWriter.cs` | Embed `ai-suggest.js` |
| `src/EAxWiki.Export/wwwroot/ai-suggest.js` | New file (Suggest button widget) |
| `src/EAxWiki.SchedulerUI/SchedulerForm.cs` | AI config fields in Settings tab |

### 8. Out of Scope (for now)

- Per-element user feedback ("this suggestion was helpful / not helpful").
- Streaming suggestions token-by-token.
- AI-generated descriptions for diagrams.
- Batch AI-suggest for all elements at export time.
