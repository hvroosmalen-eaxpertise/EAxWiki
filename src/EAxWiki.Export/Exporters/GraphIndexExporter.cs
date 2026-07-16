using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class GraphIndexExporter(IOutputWriter writer, ILogger logger)
{
    public async Task ExportAsync(ExportContext ctx, CancellationToken ct = default)
    {
        var nodes = new List<Dictionary<string, object?>>(ctx.Elements.Count);
        var nodeLayers = new Dictionary<int, string>(ctx.Elements.Count);

        foreach (var (elem, pkgDir) in ctx.Elements)
        {
            var layer = MarkdownHelpers.GetLayer(elem);
            nodeLayers[elem.Id] = layer;

            var relPath = Path.GetRelativePath(ctx.OutputPath, Path.Combine(pkgDir, MarkdownHelpers.SanitizeName(elem.Name) + ".md"));
            var url = Path.ChangeExtension(relPath, ".html").Replace('\\', '/');

            nodes.Add(new Dictionary<string, object?>
            {
                ["id"] = elem.Id,
                ["label"] = elem.Name.Length > 24 ? elem.Name[..23] + "…" : elem.Name,
                ["fullName"] = elem.Name,
                ["packageName"] = ctx.PackageLookup.TryGetValue(elem.PackageId, out var pkg) ? pkg.Name : "",
                ["layer"] = layer,
                ["url"] = url,
            });
        }

        var edges = new List<Dictionary<string, object?>>();
        var seenEdges = new HashSet<int>();

        foreach (var (elem, _) in ctx.Elements)
        {
            foreach (var conn in elem.Connectors)
            {
                if (!seenEdges.Add(conn.Id)) continue;
                if (!ctx.ElementLookup.ContainsKey(conn.SourceId) || !ctx.ElementLookup.ContainsKey(conn.TargetId)) continue;

                var edgeLabel = !string.IsNullOrEmpty(conn.Name) ? conn.Name : conn.Type;
                var sourceLayer = nodeLayers.TryGetValue(conn.SourceId, out var sl) ? sl : "uml";

                edges.Add(new Dictionary<string, object?>
                {
                    ["id"] = conn.Id,
                    ["source"] = conn.SourceId,
                    ["target"] = conn.TargetId,
                    ["label"] = edgeLabel,
                    ["sourceLayer"] = sourceLayer,
                });
            }
        }

        var root = new Dictionary<string, object?>
        {
            ["nodes"] = nodes,
            ["edges"] = edges,
        };

        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = false });
        await writer.WriteFileAsync(Path.Combine(ctx.OutputPath, "graph-index.json"), json, ct);
    }
}
