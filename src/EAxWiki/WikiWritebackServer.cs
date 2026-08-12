using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.RateLimiting;
using EAxWiki.Core.Models;
using EAxWiki.EA;
using EAxWiki.Export.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EAxWiki;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal static class WikiWritebackServer
{
    internal record StatusChangeRequest(int? ElementId, string NewStatus, string FilePath);
    internal record NotesChangeRequest(int? ElementId, int? PackageId, string NewNotes, string FilePath);
    internal record DiagramNotesChangeRequest(int DiagramId, string NewNotes, string FilePath);
    internal record AiSuggestRequest(int ElementId);
    internal record AiSuggestDiagramRequest(int DiagramId);

    internal record RowNotesChangeRequest(
        string Kind, int ElementId, string RowId, string NewNotes, string FilePath,
        string? AttributeName, string? AttributeType,
        string? MethodName, string? ReturnType, bool? IsStatic,
        string? TagName, string? TagValue);

    internal record EditLockRequest(string Action, int? ElementId = null);
    internal record EditLockState(bool Active, int? ElementId, DateTime AcquiredAt, DateTime ExpiresAt);

    /// <summary>
    /// Resolves <paramref name="relativePath"/> against <paramref name="outputPath"/> and rejects it
    /// unless the result stays strictly inside that directory (with a trailing separator, so a sibling
    /// directory sharing the same prefix — e.g. "wiki" vs "wiki-archive" — cannot pass) and ends in
    /// ".md", since that is the only file type write-back ever touches.
    /// </summary>
    private static bool TryResolveWikiFilePath(string outputPath, string relativePath, out string filePath)
    {
        var root = Path.GetFullPath(outputPath);
        var rootWithSeparator = root.EndsWith(Path.DirectorySeparatorChar) ? root : root + Path.DirectorySeparatorChar;
        filePath = Path.GetFullPath(Path.Combine(root, relativePath));

        if (!filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
            return false;
        return filePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Appends one line per successful write-back to status/writeback.log (issue #41's "daily
    /// number of writebacks" alert reads this). Written under status/ specifically because
    /// InfrastructureWriter.CleanupOrphanedFilesAsync treats that directory as special and never
    /// recurses into it — anywhere else under outputPath, the next export would delete this file
    /// as an unrecognized artifact.
    /// </summary>
    private static void LogWriteback(string outputPath, string kind)
    {
        try
        {
            var statusDir = Path.Combine(outputPath, "status");
            Directory.CreateDirectory(statusDir);
            File.AppendAllText(Path.Combine(statusDir, "writeback.log"), $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} {kind}{Environment.NewLine}");
        }
        catch
        {
            // Best-effort activity counter — never let logging failure block a write-back that otherwise succeeded.
        }
    }

    private static readonly TimeSpan EditLockDuration = TimeSpan.FromMinutes(5);

    private static string GetEditLockPath(string outputPath)
    {
        var dataDir = Path.Combine(Path.GetDirectoryName(outputPath)!, ".data");
        Directory.CreateDirectory(dataDir);
        return Path.Combine(dataDir, "edit-lock.json");
    }

    private static IResult HandleEditLock(string outputPath, EditLockRequest req)
    {
        var lockPath = GetEditLockPath(outputPath);
        var dir = Path.GetDirectoryName(lockPath)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        if (string.Equals(req.Action, "acquire", StringComparison.OrdinalIgnoreCase))
        {
            var now = DateTime.UtcNow;
            var state = new EditLockState(true, req.ElementId, now, now.Add(EditLockDuration));
            File.WriteAllText(lockPath, System.Text.Json.JsonSerializer.Serialize(state));
            return Results.Ok(state);
        }
        else if (string.Equals(req.Action, "release", StringComparison.OrdinalIgnoreCase))
        {
            var state = new EditLockState(false, null, default, default);
            File.WriteAllText(lockPath, System.Text.Json.JsonSerializer.Serialize(state));
            return Results.Ok(state);
        }

        return Results.BadRequest(new { message = "Action must be 'acquire' or 'release'." });
    }

    private static string BuildSuggestPrompt(EaElementSummary el)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Name: {el.Name}");

        if (!string.IsNullOrWhiteSpace(el.Notes))
        {
            sb.AppendLine();
            sb.AppendLine("Existing Description:");
            sb.AppendLine(el.Notes);
        }

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
            sb.AppendLine("Related Elements:");
            foreach (var r in el.Relationships)
            {
                var line = $"  - {r.TargetName} (connected via {r.Type}";
                if (!string.IsNullOrEmpty(r.ConnectorStereotype))
                    line += $", {r.ConnectorStereotype}";
                line += ")";
                if (!string.IsNullOrEmpty(r.TargetNotes))
                {
                    var snippet = r.TargetNotes.Length > 120
                        ? r.TargetNotes[..120] + "..."
                        : r.TargetNotes;
                    line += $" — \"{snippet}\"";
                }
                sb.AppendLine(line);
            }
        }

        return sb.ToString();
    }

    private static string BuildDiagramSuggestPrompt(EaDiagramSummary diagram)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Diagram Name: {diagram.Name}");
        sb.AppendLine($"Diagram Type: {diagram.Type}");

        if (!string.IsNullOrWhiteSpace(diagram.Notes))
        {
            sb.AppendLine();
            sb.AppendLine("Existing Description:");
            sb.AppendLine(diagram.Notes);
        }

        if (diagram.Elements.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Elements on this diagram:");
            foreach (var el in diagram.Elements)
            {
                var line = $"  - {el.Name} ({el.Type}";
                if (!string.IsNullOrEmpty(el.Stereotype))
                    line += $", {el.Stereotype}";
                line += ")";
                if (!string.IsNullOrEmpty(el.Notes))
                {
                    var snippet = el.Notes.Length > 120
                        ? el.Notes[..120] + "..."
                        : el.Notes;
                    line += $" — \"{snippet}\"";
                }
                sb.AppendLine(line);
            }
        }

        return sb.ToString();
    }

    private record LlmChatMessage(string role, string content);
    private record LlmChatChoice(LlmChatMessage message);
    private record LlmChatResponse(LlmChatChoice[]? choices);

    // Shared client to avoid socket exhaustion. Timeout is the ceiling for a single AI request;
    // per-request cancellation still flows via HttpContext.RequestAborted.
    private static readonly HttpClient AiHttpClient = new() { Timeout = TimeSpan.FromSeconds(300) };

    public static async Task RunAsync(Config config, string outputPath, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("WikiWritebackServer");

        var reader = new EaReaderStaDispatcher(loggerFactory.CreateLogger<EaReader>(), config.RepositoryPath);
        logger.LogInformation("EA repository opened for write-back server (STA dispatch)");

        // Ensure token file exists (call once at startup, but re-read on each request).
        ApiTokenStore.GetOrCreate(outputPath, loggerFactory.CreateLogger("ApiTokenStore"));

        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.WebHost.UseKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = 1_048_576; // 1 MB

            if (!string.IsNullOrEmpty(config.CertPath))
            {
                options.ConfigureHttpsDefaults(adaptOptions =>
                {
                    adaptOptions.ServerCertificate = X509CertificateLoader.LoadPkcs12(File.ReadAllBytes(config.CertPath), config.CertPassword ?? "", X509KeyStorageFlags.DefaultKeySet);
                });
            }
        });

        try
        {
            var statusTypes = reader.GetStatusTypes();
            logger.LogInformation("EA health check OK — {StatusCount} status types read from repository", statusTypes.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EA health check FAILED — repository not responding");
        }

        // /readyz needs to stay truthful across the server's lifetime (issue #83 gates the browser
        // edit pencils on it). Two signals combine:
        //   * dispatcher.IsHealthy — flipped false the moment a work item hits a COMException,
        //     back to true on the next successful work item. Zero probe traffic; reflects real
        //     user activity.
        //   * a cached fallback probe (GetStatusTypes) — runs at most once per ReadyProbeCacheTtl
        //     to catch the "server idle for hours, EA quietly died" case. Cache stops browser
        //     refresh spam from hammering EA COM.
        var readyProbeGate = new SemaphoreSlim(1, 1);
        var readyProbeCacheTtl = TimeSpan.FromSeconds(15);
        DateTime lastProbeAt = DateTime.MinValue;
        bool lastProbeOk = true;

        async Task<bool> ProbeEaAsync()
        {
            if (!reader.IsHealthy) return false;
            if (DateTime.UtcNow - lastProbeAt < readyProbeCacheTtl) return lastProbeOk;
            await readyProbeGate.WaitAsync();
            try
            {
                if (DateTime.UtcNow - lastProbeAt < readyProbeCacheTtl) return lastProbeOk;
                try { _ = reader.GetStatusTypes(); lastProbeOk = true; }
                catch (Exception ex) { logger.LogWarning(ex, "EA readiness probe failed"); lastProbeOk = false; }
                lastProbeAt = DateTime.UtcNow;
                return lastProbeOk;
            }
            finally { readyProbeGate.Release(); }
        }

        var app = builder.Build();

        app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", ea = reader.IsHealthy }));
        app.MapGet("/readyz", async () =>
        {
            var ok = await ProbeEaAsync();
            return ok
                ? Results.Ok(new { status = "ready", ea = true })
                : Results.Json(new { status = "not ready", ea = false }, statusCode: 503);
        });

        // This server is paired 1:1 with one `mkdocs serve` instance. Rather than a global
        // AllowAnyOrigin() (which would let a page from *any* origin — including sibling
        // EAxWiki instances on the same machine, or the public GitHub Pages export — call
        // this write-back API), only accept requests whose Origin hostname matches this
        // request's own Host header (so it still works under any LAN name/IP the server is
        // reached by) AND whose Origin port matches the configured --wiki-port. This keeps
        // trust scoped to exactly the one wiki instance this server was started for.
        var wikiPort = config.WikiPort > 0 ? config.WikiPort : 8000;
        app.Use(async (context, next) =>
        {
            var origin = context.Request.Headers.Origin.ToString();
            if (!string.IsNullOrEmpty(origin) &&
                Uri.TryCreate(origin, UriKind.Absolute, out var originUri) &&
                string.Equals(originUri.Host, context.Request.Host.Host, StringComparison.OrdinalIgnoreCase) &&
                originUri.Port == wikiPort)
            {
                context.Response.Headers.AccessControlAllowOrigin = origin;
                context.Response.Headers.AccessControlAllowHeaders = "Content-Type, X-EAxWiki-Token";
                context.Response.Headers.AccessControlAllowMethods = "GET, POST";
            }

            if (HttpMethods.IsOptions(context.Request.Method))
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            // Origin/port matching above only restricts browser-mediated cross-origin calls — a
            // raw HTTP client (curl, a LAN port scan) can set any Origin header it likes, so it is
            // not authentication. This shared secret is: generated once per wiki output directory
            // (ApiTokenStore), embedded into every exported page's widgets as data-api-token, and
            // required here on every /api request. It is visible to anyone who can view a wiki page
            // (view-source), so it does not protect against someone with legitimate view access to
            // this instance — it protects against everyone else (LAN scanning, unrelated sites).
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var provided = context.Request.Headers["X-EAxWiki-Token"].ToString();
                var tokenPath = Path.Combine(outputPath, ".eaxwiki-token");
                var expectedToken = File.Exists(tokenPath) ? File.ReadAllText(tokenPath).Trim() : "";
                if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(provided), Encoding.UTF8.GetBytes(expectedToken)))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { success = false, message = "Missing or invalid API token." });
                    return;
                }
            }

            await next();
        });

        // Per-token rate limiter (60 requests/minute, sliding window)
        var rateLimiter = PartitionedRateLimiter.Create<string, string>(key =>
            RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromSeconds(60),
                SegmentsPerWindow = 6,
                AutoReplenishment = true
            }));

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

        app.MapGet("/api/status-types", () =>
        {
            try
            {
                var types = reader.GetStatusTypes();
                return Results.Ok(types);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve status types");
                return Results.Problem("Failed to retrieve status types from EA.");
            }
        });

        app.MapPost("/api/status", (StatusChangeRequest req, HttpContext context) =>
        {
            if (string.IsNullOrWhiteSpace(req.NewStatus))
                return Results.BadRequest(new { success = false, message = "newStatus is required." });

            var allowed = reader.GetStatusTypes();
            if (!allowed.Contains(req.NewStatus, StringComparer.OrdinalIgnoreCase))
                return Results.BadRequest(new { success = false, message = $"'{req.NewStatus}' is not a valid status. Allowed: {string.Join(", ", allowed)}" });

            if (!TryResolveWikiFilePath(outputPath, req.FilePath, out var filePath))
                return Results.BadRequest(new { success = false, message = "Invalid file path." });

            if (!File.Exists(filePath))
                return Results.NotFound(new { success = false, message = $"File not found: {req.FilePath}" });

            try
            {
                if (req.ElementId.HasValue)
                {
                    reader.UpdateElementStatus(req.ElementId.Value, req.NewStatus);
                    FrontmatterParser.UpdateStatus(filePath, req.NewStatus);
                    logger.LogInformation("Status change: element {Id} → {Status} ({File})", req.ElementId.Value, req.NewStatus, req.FilePath);
                }
                else
                {
                    return Results.BadRequest(new { success = false, message = "elementId is required." });
                }

                LogWriteback(outputPath, "status");
                _ = AuditLogger.LogAsync(outputPath, "POST /api/status", req.ElementId ?? 0, "status",
                    context.Response.StatusCode, "Write-back completed",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Ok(new { success = true, message = $"Status updated to '{req.NewStatus}'." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Write-back failed for element {Id}", req.ElementId);
                _ = AuditLogger.LogAsync(outputPath, "POST /api/status", req.ElementId ?? 0, "status",
                    StatusCodes.Status500InternalServerError, $"Write-back failed: {ex.Message}",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/notes", (NotesChangeRequest req, HttpContext context) =>
        {
            if (req.NewNotes == null)
                return Results.BadRequest(new { success = false, message = "newNotes is required." });

            if (!TryResolveWikiFilePath(outputPath, req.FilePath, out var filePath))
                return Results.BadRequest(new { success = false, message = "Invalid file path." });

            if (!File.Exists(filePath))
                return Results.NotFound(new { success = false, message = $"File not found: {req.FilePath}" });

            try
            {
                var normalized = FrontmatterParser.NormalizeNotesHtml(req.NewNotes);

                if (req.PackageId.HasValue)
                {
                    reader.UpdatePackageNotes(req.PackageId.Value, normalized);
                    FrontmatterParser.UpdatePackageNotes(filePath, normalized);
                    logger.LogInformation("Notes updated for package {Id} ({File})", req.PackageId.Value, req.FilePath);
                }
                else if (req.ElementId.HasValue)
                {
                    reader.UpdateElementNotes(req.ElementId.Value, normalized);
                    FrontmatterParser.UpdateNotes(filePath, normalized);
                    logger.LogInformation("Notes updated for element {Id} ({File})", req.ElementId.Value, req.FilePath);
                }
                else
                {
                    return Results.BadRequest(new { success = false, message = "Either elementId or packageId is required." });
                }

                LogWriteback(outputPath, "notes");
                _ = AuditLogger.LogAsync(outputPath, "POST /api/notes", req.PackageId ?? req.ElementId ?? 0, "notes",
                    context.Response.StatusCode, "Write-back completed",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Ok(new { success = true, message = "Notes updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Notes write-back failed for {Entity}", req.PackageId.HasValue ? $"package {req.PackageId}" : $"element {req.ElementId}");
                _ = AuditLogger.LogAsync(outputPath, "POST /api/notes", req.PackageId ?? req.ElementId ?? 0, "notes",
                    StatusCodes.Status500InternalServerError, $"Write-back failed: {ex.Message}",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/diagram-notes", (DiagramNotesChangeRequest req, HttpContext context) =>
        {
            if (req.NewNotes == null)
                return Results.BadRequest(new { success = false, message = "newNotes is required." });

            if (!TryResolveWikiFilePath(outputPath, req.FilePath, out var filePath))
                return Results.BadRequest(new { success = false, message = "Invalid file path." });

            if (!File.Exists(filePath))
                return Results.NotFound(new { success = false, message = $"File not found: {req.FilePath}" });

            try
            {
                var normalized = FrontmatterParser.NormalizeNotesHtml(req.NewNotes);
                reader.UpdateDiagramNotes(req.DiagramId, normalized);
                FrontmatterParser.UpdateNotes(filePath, normalized);
                logger.LogInformation("Notes updated for diagram {Id} ({File})", req.DiagramId, req.FilePath);
                LogWriteback(outputPath, "diagram-notes");
                _ = AuditLogger.LogAsync(outputPath, "POST /api/diagram-notes", req.DiagramId, "diagram-notes",
                    context.Response.StatusCode, "Write-back completed",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Ok(new { success = true, message = "Description updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Notes write-back failed for diagram {Id}", req.DiagramId);
                _ = AuditLogger.LogAsync(outputPath, "POST /api/diagram-notes", req.DiagramId, "diagram-notes",
                    StatusCodes.Status500InternalServerError, $"Write-back failed: {ex.Message}",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/row-notes", (RowNotesChangeRequest req, HttpContext context) =>
        {
            if (req.NewNotes == null)
                return Results.BadRequest(new { success = false, message = "newNotes is required." });

            if (!TryResolveWikiFilePath(outputPath, req.FilePath, out var filePath))
                return Results.BadRequest(new { success = false, message = "Invalid file path." });

            if (!File.Exists(filePath))
                return Results.NotFound(new { success = false, message = $"File not found: {req.FilePath}" });

            try
            {
                var normalized = FrontmatterParser.NormalizeNotesHtml(req.NewNotes);

                switch (req.Kind)
                {
                    case "attribute":
                        if (req.AttributeName == null || req.AttributeType == null)
                            return Results.BadRequest(new { success = false, message = "attributeName and attributeType are required." });
                        reader.UpdateAttributeNotes(req.ElementId, req.AttributeName, req.AttributeType, normalized);
                        break;
                    case "method":
                        if (req.MethodName == null || req.ReturnType == null || req.IsStatic == null)
                            return Results.BadRequest(new { success = false, message = "methodName, returnType, and isStatic are required." });
                        reader.UpdateMethodNotes(req.ElementId, req.MethodName, req.ReturnType, req.IsStatic.Value, normalized);
                        break;
                    case "tagged-value":
                        if (req.TagName == null || req.TagValue == null)
                            return Results.BadRequest(new { success = false, message = "tagName and tagValue are required." });
                        reader.UpdateTaggedValueNotes(req.ElementId, req.TagName, req.TagValue, normalized);
                        break;
                    default:
                        return Results.BadRequest(new { success = false, message = $"Unknown kind '{req.Kind}'." });
                }

                FrontmatterParser.UpdateRowNotes(filePath, req.RowId, normalized);
                logger.LogInformation("Row notes updated: {Kind} on element {Id}, row {RowId} ({File})", req.Kind, req.ElementId, req.RowId, req.FilePath);
                LogWriteback(outputPath, $"row-notes:{req.Kind}");
                _ = AuditLogger.LogAsync(outputPath, "POST /api/row-notes", req.ElementId, $"row-notes:{req.Kind}",
                    context.Response.StatusCode, "Write-back completed",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Ok(new { success = true, message = "Description updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Row notes write-back failed: {Kind} on element {Id}, row {RowId}", req.Kind, req.ElementId, req.RowId);
                _ = AuditLogger.LogAsync(outputPath, "POST /api/row-notes", req.ElementId, $"row-notes:{req.Kind}",
                    StatusCodes.Status500InternalServerError, $"Write-back failed: {ex.Message}",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

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

                var llmBody = new
                {
                    model = config.AiModel,
                    messages = new[]
                    {
                        new { role = "system", content = "You are a technical writer for enterprise architecture documentation. Write a concise 2-3 sentence description for the element below. Focus on what the element does, its purpose, and its business significance based on attributes, tagged values, and related elements. Do not mention the element's type, stereotype, status, package, or relationship types by name — the reader can already see these in the diagram. Avoid listing or enumerating data. Write in plain English. Do not use markdown." },
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

                var response = await AiHttpClient.SendAsync(request, context.RequestAborted);
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

                _ = AuditLogger.LogAsync(outputPath, "POST /api/ai-suggest", req.ElementId, "suggested",
                    StatusCodes.Status200OK, "AI suggestion generated",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Json(new { suggestion });
            }
            catch (OperationCanceledException)
            {
                return Results.Json(new { message = "AI request timed out." }, statusCode: 504);
            }
            catch (HttpRequestException ex)
            {
                logger.LogWarning(ex, "AI endpoint unreachable");
                return Results.Json(new { message = "AI service unavailable." }, statusCode: 502);
            }
        });

        app.MapPost("/api/ai-suggest-diagram", async (AiSuggestDiagramRequest req, HttpContext context) =>
        {
            try
            {
                if (string.IsNullOrEmpty(config.AiEndpoint))
                    return Results.Json(new { message = "AI suggestions are not configured." }, statusCode: 501);

                var summary = reader.GetDiagramSummary(req.DiagramId);
                if (summary == null)
                    return Results.Json(new { message = "Diagram not found." }, statusCode: 404);

                if (summary.Elements.Count == 0)
                    return Results.Json(new { message = "Not enough context to suggest a description." }, statusCode: 204);

                var prompt = BuildDiagramSuggestPrompt(summary);

                var llmBody = new
                {
                    model = config.AiModel,
                    messages = new[]
                    {
                        new { role = "system", content = "You are a technical writer for enterprise architecture documentation. Write a concise 1-3 sentence description for the diagram below. Focus on what the diagram shows, its purpose, and the business domain it covers based on the elements it contains. Do not mention the diagram type or list element names. Write in plain English. Do not use markdown." },
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

                var response = await AiHttpClient.SendAsync(request, context.RequestAborted);
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

                _ = AuditLogger.LogAsync(outputPath, "POST /api/ai-suggest-diagram", req.DiagramId, "suggested",
                    StatusCodes.Status200OK, "AI suggestion generated for diagram",
                    context.Request.Headers["X-EAxWiki-Token"].ToString());
                return Results.Json(new { suggestion });
            }
            catch (OperationCanceledException)
            {
                return Results.Json(new { message = "AI request timed out." }, statusCode: 504);
            }
            catch (HttpRequestException ex)
            {
                logger.LogWarning(ex, "AI endpoint unreachable");
                return Results.Json(new { message = "AI service unavailable." }, statusCode: 502);
            }
        });

        app.MapPost("/api/edit-lock", (EditLockRequest req) =>
        {
            return HandleEditLock(outputPath, req);
        });

        // Graceful shutdown for the monitor (issue #81): token-authenticated (it's under /api),
        // so Stop-ApiServer can ask the API to dispose its EA COM connection and exit 0 instead
        // of force-killing it and orphaning an EA.exe -Embedding instance per export cycle.
        app.MapPost("/api/shutdown", async (HttpContext context, Microsoft.Extensions.Hosting.IApplicationLifetime lifetime) =>
        {
            await AuditLogger.LogAsync(outputPath, "POST /api/shutdown", 0, "shutdown",
                StatusCodes.Status200OK, "Graceful shutdown requested",
                context.Request.Headers["X-EAxWiki-Token"].ToString());
            logger.LogInformation("Shutdown requested via /api/shutdown; stopping host.");
            // Respond first, then stop after a short delay so the client reliably sees the 200.
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                lifetime.StopApplication();
            });
            return Results.Ok(new { success = true, message = "Shutting down." });
        });

        var port = config.ApiPort > 0 ? config.ApiPort : 8001;
        logger.LogInformation("Wiki write-back server listening on port {Port}", port);
        logger.LogInformation("Accepting requests only from origins on port {WikiPort} (pass --wiki-port to override)", wikiPort);
        logger.LogInformation("Press Ctrl+C to stop.");

        // Signal readiness to the monitor by writing a "ready" file in the status dir.
        var readyDir = Path.Combine(outputPath, "status");
        Directory.CreateDirectory(readyDir);
        var readyFile = Path.Combine(readyDir, "api-ready");
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            File.WriteAllText(readyFile, $"{Environment.ProcessId}");
        });
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("API server shutting down; closing EA repository.");
            reader.Dispose();
        });

        // Bind both IPv4 and IPv6 so browsers resolving localhost to either
        // 127.0.0.1 or ::1 can reach the server.
        var scheme = string.IsNullOrEmpty(config.CertPath) ? "http" : "https";
        app.Urls.Add($"{scheme}://0.0.0.0:{port}");
        app.Urls.Add($"{scheme}://[::]:{port}");
        try
        {
            await app.RunAsync();
        }
        finally
        {
            if (File.Exists(readyFile)) File.Delete(readyFile);
            reader.Dispose();
        }
    }
}
