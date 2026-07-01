using System.Security.Cryptography;
using System.Text;
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
    internal record StatusChangeRequest(int ElementId, string NewStatus, string FilePath);
    internal record NotesChangeRequest(int ElementId, string NewNotes, string FilePath);
    internal record DiagramNotesChangeRequest(int DiagramId, string NewNotes, string FilePath);
    internal record RowNotesChangeRequest(
        string Kind, int ElementId, string RowId, string NewNotes, string FilePath,
        string? AttributeName, string? AttributeType,
        string? MethodName, string? ReturnType, bool? IsStatic,
        string? TagName, string? TagValue);

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

    public static async Task RunAsync(Config config, string outputPath, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("WikiWritebackServer");

        var reader = new EaReader(loggerFactory.CreateLogger<EaReader>());
        reader.Open(config.RepositoryPath);
        logger.LogInformation("EA repository opened for write-back server");

        var apiToken = ApiTokenStore.GetOrCreate(outputPath);
        var apiTokenBytes = Encoding.UTF8.GetBytes(apiToken);

        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var app = builder.Build();

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
                if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(provided), apiTokenBytes))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { success = false, message = "Missing or invalid API token." });
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

        app.MapPost("/api/status", ([FromBody] StatusChangeRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.NewStatus))
                return Results.BadRequest(new { success = false, message = "newStatus is required." });

            // Validate against live t_statustypes
            var allowed = reader.GetStatusTypes();
            if (!allowed.Contains(req.NewStatus, StringComparer.OrdinalIgnoreCase))
                return Results.BadRequest(new { success = false, message = $"'{req.NewStatus}' is not a valid status. Allowed: {string.Join(", ", allowed)}" });

            if (!TryResolveWikiFilePath(outputPath, req.FilePath, out var filePath))
                return Results.BadRequest(new { success = false, message = "Invalid file path." });

            if (!File.Exists(filePath))
                return Results.NotFound(new { success = false, message = $"File not found: {req.FilePath}" });

            try
            {
                reader.UpdateElementStatus(req.ElementId, req.NewStatus);
                FrontmatterParser.UpdateStatus(filePath, req.NewStatus);
                logger.LogInformation("Status change: element {Id} → {Status} ({File})", req.ElementId, req.NewStatus, req.FilePath);
                return Results.Ok(new { success = true, message = $"Status updated to '{req.NewStatus}'." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Write-back failed for element {Id}", req.ElementId);
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/notes", ([FromBody] NotesChangeRequest req) =>
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
                reader.UpdateElementNotes(req.ElementId, normalized);
                FrontmatterParser.UpdateNotes(filePath, normalized);
                logger.LogInformation("Notes updated for element {Id} ({File})", req.ElementId, req.FilePath);
                return Results.Ok(new { success = true, message = "Notes updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Notes write-back failed for element {Id}", req.ElementId);
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/diagram-notes", ([FromBody] DiagramNotesChangeRequest req) =>
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
                return Results.Ok(new { success = true, message = "Description updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Notes write-back failed for diagram {Id}", req.DiagramId);
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        app.MapPost("/api/row-notes", ([FromBody] RowNotesChangeRequest req) =>
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
                return Results.Ok(new { success = true, message = "Description updated.", html = normalized });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Row notes write-back failed: {Kind} on element {Id}, row {RowId}", req.Kind, req.ElementId, req.RowId);
                return Results.Problem($"Write-back failed: {ex.Message}");
            }
        });

        var port = config.ApiPort > 0 ? config.ApiPort : 8001;
        logger.LogInformation("Wiki write-back server listening on port {Port}", port);
        logger.LogInformation("Accepting requests only from origins on port {WikiPort} (pass --wiki-port to override)", wikiPort);
        logger.LogInformation("Press Ctrl+C to stop.");

        // Bind both IPv4 and IPv6 so browsers resolving localhost to either
        // 127.0.0.1 or ::1 can reach the server.
        app.Urls.Add($"http://0.0.0.0:{port}");
        app.Urls.Add($"http://[::]:{port}");
        try
        {
            await app.RunAsync();
        }
        finally
        {
            reader.Dispose();
        }
    }
}
