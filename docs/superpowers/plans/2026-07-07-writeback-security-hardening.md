# Write-Back Server Security Hardening Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add HTTPS (`--cert`/`--cert-password`), request body limit (1MB), rate limiting (60/min per token), structured audit logging (`.eaxwiki-monitor/audit.log`), and health endpoints (`/healthz`, `/readyz`) to `WikiWritebackServer.cs`.

**Architecture:** All features added inline to `WikiWritebackServer.cs` with one extraction (`AuditLogger.cs`) for the structured audit log. Rate limiter is `PartitionedRateLimiter` middleware. Health endpoints registered before auth middleware. HTTPS binding uses Kestrel `ConfigureHttpsDefaults` when `--cert` is provided.

**Tech Stack:** .NET 10, Kestrel, `System.Threading.RateLimiting` NuGet, JSON-lines audit file

## Global Constraints

- Target framework: `net10.0`
- `System.Threading.RateLimiting` version: `9.0.0`
- All audit log writes are best-effort (never block write-back)
- Rate limit: 60 requests/minute per token, sliding window with 6 segments
- HTTPS-only when cert provided; fall back to HTTP otherwise

---

### Task 1: CLI flags for HTTPS (`--cert`, `--cert-password`)

**Files:**
- Modify: `src/EAxWiki/Config.cs` (add `CertPath`, `CertPassword` properties + parse `--cert`, `--cert-password`)
- Modify: `src/EAxWiki/Program.cs` (add to `ShowUsage()`)

**Interfaces:**
- Produces: `Config.CertPath` (string?), `Config.CertPassword` (string?)

- [ ] **Step 1: Add properties to Config.cs**

Add after `WikiPort`:
```csharp
public string? CertPath { get; set; }
public string? CertPassword { get; set; }
```

Add cases to the `switch` in `Load()`:
```csharp
case "--cert":
    if (i + 1 >= args.Length)
        throw new ArgumentException($"Option {args[i]} requires a value");
    CertPath = args[++i];
    break;
case "--cert-password":
    if (i + 1 >= args.Length)
        throw new ArgumentException($"Option {args[i]} requires a value");
    CertPassword = args[++i];
    break;
```

- [ ] **Step 2: Add to ShowUsage() in Program.cs**

```csharp
Console.WriteLine("  --cert <path>         Path to PFX certificate for HTTPS");
Console.WriteLine("  --cert-password <pw>  PFX certificate password");
```

- [ ] **Step 3: Build to verify**

Run: `dotnet build src/EAxWiki`
Expected: Build succeeds

- [ ] **Step 4: Commit**

```bash
git add src/EAxWiki/Config.cs src/EAxWiki/Program.cs
git commit -m "feat: add --cert / --cert-password CLI flags for HTTPS"
```

---

### Task 2: AuditLogger (extracted class)

**Files:**
- Create: `src/EAxWiki/AuditLogger.cs`

**Interfaces:**
- Produces: `static class AuditLogger` with `LogAsync(string outputPath, string endpoint, int? elementId, string? field, int statusCode, string? message, string? tokenPrefix)`

- [ ] **Step 1: Create AuditLogger.cs**

```csharp
using System.Text.Json;

namespace EAxWiki;

internal static class AuditLogger
{
    private static readonly string LogDir = Path.Combine(
        Directory.GetCurrentDirectory(), ".eaxwiki-monitor");

    public static async Task LogAsync(
        string outputPath,
        string endpoint,
        int? elementId,
        string? field,
        int statusCode,
        string? message,
        string? tokenPrefix)
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            var entry = new
            {
                timestamp = DateTime.UtcNow.ToString("O"),
                tokenPrefix = (tokenPrefix ?? "").Length > 8 ? tokenPrefix[..8] : (tokenPrefix ?? ""),
                endpoint,
                elementId,
                field,
                statusCode,
                message
            };
            var line = JsonSerializer.Serialize(entry);
            var logPath = Path.Combine(LogDir, "audit.log");
            await File.AppendAllTextAsync(logPath, line + Environment.NewLine);
        }
        catch
        {
            // Best-effort — never let audit logging failure block a write-back
        }
    }
}
```

- [ ] **Step 2: Build to verify**

Run: `dotnet build src/EAxWiki`
Expected: Build succeeds

- [ ] **Step 3: Commit**

```bash
git add src/EAxWiki/AuditLogger.cs
git commit -m "feat: add structured audit log (AuditLogger)"
```

---

### Task 3: Wire HTTPS, MaxRequestBodySize, rate limiting, health endpoints, and audit calls into WikiWritebackServer

**Files:**
- Modify: `src/EAxWiki/WikiWritebackServer.cs`
- Modify: `src/EAxWiki/EAxWiki.csproj` (add `System.Threading.RateLimiting` package reference)

- [ ] **Step 1: Add NuGet package reference to EAxWiki.csproj**

Add inside an `<ItemGroup>`:
```xml
<PackageReference Include="System.Threading.RateLimiting" Version="9.0.0" />
```

- [ ] **Step 2: Add using directives to WikiWritebackServer.cs**

Add to the using block:
```csharp
using System.Threading.RateLimiting;
using System.Security.Cryptography.X509Certificates;
```

- [ ] **Step 3: Configure Kestrel (MaxRequestBodySize + HTTPS)**

After `var builder = WebApplication.CreateBuilder();`, add:
```csharp
builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_048_576; // 1 MB

    if (!string.IsNullOrEmpty(config.CertPath))
    {
        options.ConfigureHttpsDefaults(adaptOptions =>
        {
            adaptOptions.ServerCertificate = new X509Certificate2(config.CertPath, config.CertPassword ?? "");
        });
    }
});
```

- [ ] **Step 4: Add health endpoints before middleware**

After `var app = builder.Build();`:
```csharp
app.MapGet("/healthz", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/readyz", () => Results.Ok(new { status = "ready" }));
```

- [ ] **Step 5: Add rate limiting middleware after CORS but before token auth**

Create a `PartitionedRateLimiter` and middleware after the CORS block (after `await next();` in the CORS middleware):
```csharp
// Per-token rate limiter (60 requests/minute, sliding window)
var rateLimiter = PartitionedRateLimiter.Create<string, string>(key =>
    RateLimitPartition.CreateSlidingWindow(key, _ => new SlidingWindowRateLimiterOptions
    {
        PermitLimit = 60,
        Window = TimeSpan.FromSeconds(60),
        SegmentsPerWindow = 6,
        AutoReplenish = true
    }));
```

Add the rate limiting middleware as a separate `app.Use` before the token-check block:
```csharp
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        var token = context.Request.Headers["X-EAxWiki-Token"].ToString();
        var partitionKey = string.IsNullOrEmpty(token) ? "anonymous" : token;

        using var lease = await rateLimiter.AcquireAsync(partitionKey);
        if (!lease.IsAcquired)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers.RetryAfter = "60";
            await context.Response.WriteAsJsonAsync(new { success = false, message = "Rate limit exceeded. Try again in 60 seconds." });
            return;
        }
    }

    await next();
});
```

- [ ] **Step 6: Change URL binding to use HTTPS when cert is provided**

Replace:
```csharp
app.Urls.Add($"http://0.0.0.0:{port}");
app.Urls.Add($"http://[::]:{port}");
```
With:
```csharp
var scheme = string.IsNullOrEmpty(config.CertPath) ? "http" : "https";
app.Urls.Add($"{scheme}://0.0.0.0:{port}");
app.Urls.Add($"{scheme}://[::]:{port}");
```

- [ ] **Step 7: Add audit logging calls to each write-back endpoint**

In `POST /api/status` handler, after the success `return Results.Ok(...)` and after the error `logger.LogError`, add:
```csharp
_ = AuditLogger.LogAsync(outputPath, "POST /api/status", req.ElementId, "status",
    context.Response.StatusCode, "Write-back completed",
    context.Request.Headers["X-EAxWiki-Token"].ToString());
```

In `POST /api/notes` handler, add:
```csharp
_ = AuditLogger.LogAsync(outputPath, "POST /api/notes", req.ElementId, "notes",
    context.Response.StatusCode, "Notes updated",
    context.Request.Headers["X-EAxWiki-Token"].ToString());
```

In `POST /api/diagram-notes` handler, add:
```csharp
_ = AuditLogger.LogAsync(outputPath, "POST /api/diagram-notes", req.DiagramId, "diagram-notes",
    context.Response.StatusCode, "Diagram notes updated",
    context.Request.Headers["X-EAxWiki-Token"].ToString());
```

In `POST /api/row-notes` handler, add:
```csharp
_ = AuditLogger.LogAsync(outputPath, "POST /api/row-notes", req.ElementId, $"row-notes:{req.Kind}",
    context.Response.StatusCode, "Row notes updated",
    context.Request.Headers["X-EAxWiki-Token"].ToString());
```

Note: `context` is not directly available in the minimal API handlers since they use `[FromBody]` parameters. The handlers are `Func<...>`, not middleware. I need to get the `HttpContext` from the endpoint. For minimal APIs, I can use `HttpContext` via the `context` parameter in the handler delegate.

Actually, looking at the existing code, the handlers use `Results.Ok/badrequest`, so `HttpContext` isn't directly available. I'll need to pass the token from the request headers. Let me capture it before the handler:

Actually, looking more carefully, the simplest approach is to just log the token prefix from the request at the point where it's validated. The token is already read in the middleware at line 114. I can store it in `HttpContext.Items` and retrieve it in the audit log calls.

Or, more simply: the audit log calls can be after the response is sent (fire-and-forget), and the handlers already have access to `context` via `HttpContext` parameter injection in minimal APIs.

Wait, in ASP.NET Core minimal APIs, handlers can access `HttpContext` by adding it as a parameter:
```csharp
app.MapPost("/api/status", (StatusChangeRequest req, HttpContext context) => ...)
```

Let me adjust the handlers to accept `HttpContext` as a parameter. But that's a bigger change to each handler's signature. 

Simpler approach: just read the `X-EAxWiki-Token` header again in the audit call. It's available via `context.Request.Headers`.

The cleanest approach: add `HttpContext context` as a parameter to each minimal API handler. Then I can access `context.Request.Headers["X-EAxWiki-Token"]` and `context.Response.StatusCode`.

Actually, let me look at the existing code more carefully. The handlers are like:
```csharp
app.MapPost("/api/status", ([FromBody] StatusChangeRequest req) => { ... });
```

These don't have access to `HttpContext`. I need to add it:
```csharp
app.MapPost("/api/status", (StatusChangeRequest req, HttpContext context) => { ... });
```

But then using `Results.Ok/BadRequest` won't automatically set `context.Response.StatusCode` — though they do, since `Results.Ok` returns an `IResult` that writes to the response.

Actually, for the audit log, the simplest approach is to read the token from the incoming request before the handler runs, and store it and the status code after. But since we're going fire-and-forget, we can just re-read the headers.

Let me just use the `HttpContext` parameter approach. It's minimal and clean.

- [ ] **Step 8: Build to verify**

Run: `dotnet build src/EAxWiki`
Expected: Build succeeds

- [ ] **Step 9: Commit**

```bash
git add src/EAxWiki/WikiWritebackServer.cs src/EAxWiki/EAxWiki.csproj
git commit -m "feat: HTTPS, rate limiting (60/min), MaxRequestBodySize (1MB), health endpoints, audit logging"
```

---

### Task 4: Full test pass

- [ ] Run all tests: `dotnet test src/EAxWiki.Tests`
- [ ] Verify all 244 tests pass
