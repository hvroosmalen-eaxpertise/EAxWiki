using EAxWiki.EA;
using EAxWiki.Export.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EAxWiki;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal static class WikiWritebackServer
{
    internal record StatusChangeRequest(int ElementId, string NewStatus, string FilePath);
    internal record NotesChangeRequest(int ElementId, string NewNotes, string FilePath);
    internal record DiagramNotesChangeRequest(int DiagramId, string NewNotes, string FilePath);

    public static async Task RunAsync(Config config, string outputPath, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("WikiWritebackServer");

        var reader = new EaReader(loggerFactory.CreateLogger<EaReader>());
        reader.Open(config.RepositoryPath);
        logger.LogInformation("EA repository opened for write-back server");

        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        var app = builder.Build();

        app.UseCors();

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

            // Resolve file path relative to wiki output dir; guard against path traversal
            var filePath = Path.GetFullPath(Path.Combine(outputPath, req.FilePath));
            if (!filePath.StartsWith(Path.GetFullPath(outputPath), StringComparison.OrdinalIgnoreCase))
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

            var filePath = Path.GetFullPath(Path.Combine(outputPath, req.FilePath));
            if (!filePath.StartsWith(Path.GetFullPath(outputPath), StringComparison.OrdinalIgnoreCase))
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

            var filePath = Path.GetFullPath(Path.Combine(outputPath, req.FilePath));
            if (!filePath.StartsWith(Path.GetFullPath(outputPath), StringComparison.OrdinalIgnoreCase))
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

        var port = config.ApiPort > 0 ? config.ApiPort : 8001;
        logger.LogInformation("Wiki write-back server listening on http://localhost:{Port}", port);
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
