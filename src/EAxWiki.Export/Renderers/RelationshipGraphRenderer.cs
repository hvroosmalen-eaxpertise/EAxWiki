using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Renderers;

internal static class RelationshipGraphRenderer
{
    internal static string Render(EaElement focal, string focalPkgDir, ExportContext ctx)
    {
        var hop1 = new HashSet<int>();
        foreach (var conn in focal.Connectors)
        {
            var neighborId = conn.SourceId == focal.Id ? conn.TargetId : conn.SourceId;
            if (neighborId != focal.Id && ctx.ElementLookup.ContainsKey(neighborId))
                hop1.Add(neighborId);
        }

        if (hop1.Count == 0) return string.Empty;

        var allIds = new HashSet<int>(hop1) { focal.Id };
        foreach (var h1Id in hop1)
        {
            var (h1Elem, _) = ctx.ElementLookup[h1Id];
            foreach (var conn in h1Elem.Connectors)
            {
                var neighborId = conn.SourceId == h1Id ? conn.TargetId : conn.SourceId;
                if (neighborId != h1Id && ctx.ElementLookup.ContainsKey(neighborId))
                    allIds.Add(neighborId);
            }
        }

        var nodes = new System.Text.StringBuilder();
        var nodeLayerMap = new Dictionary<int, string>();
        var firstNode = true;
        foreach (var id in allIds)
        {
            var (elem, pkgDir) = ctx.ElementLookup[id];
            var isFocal = id == focal.Id;
            var label = JsonEscape(elem.Name.Length > 24 ? elem.Name[..23] + "…" : elem.Name);
            var fullName = JsonEscape(elem.Name);
            var pkgName = ctx.PackageLookup.TryGetValue(elem.PackageId, out var pkg) ? JsonEscape(pkg.Name) : "";
            var layer = MarkdownHelpers.GetLayer(elem);
            nodeLayerMap[id] = layer;
            string url;
            if (isFocal)
            {
                url = "";
            }
            else
            {
                var targetFile = MarkdownHelpers.SanitizeName(elem.Name) + ".html";
                var fromFolder = Path.GetFileName(focalPkgDir);
                var toFolder = Path.GetFileName(pkgDir);
                url = fromFolder.Equals(toFolder, StringComparison.OrdinalIgnoreCase)
                    ? targetFile
                    : $"../{toFolder}/{targetFile}";
                url = JsonEscape(url);
            }
            if (!firstNode) nodes.Append(',');
            firstNode = false;
            nodes.Append($"{{\"id\":\"e{id}\",\"label\":\"{label}\",\"fullName\":\"{fullName}\",\"packageName\":\"{pkgName}\",\"layer\":\"{layer}\",\"isFocal\":{(isFocal ? "true" : "false")},\"hasUrl\":{(!isFocal ? "true" : "false")},\"url\":\"{url}\"}}");
        }

        var edges = new System.Text.StringBuilder();
        var seenEdgeIds = new HashSet<int>();
        var firstEdge = true;
        foreach (var id in allIds)
        {
            var (elem, _) = ctx.ElementLookup[id];
            foreach (var conn in elem.Connectors)
            {
                if (!seenEdgeIds.Add(conn.Id)) continue;
                if (!allIds.Contains(conn.SourceId) || !allIds.Contains(conn.TargetId)) continue;
                var edgeLabel = JsonEscape(!string.IsNullOrEmpty(conn.Name) ? conn.Name : conn.Type);
                var sourceLayer = nodeLayerMap.TryGetValue(conn.SourceId, out var sl) ? sl : "uml";
                if (!firstEdge) edges.Append(',');
                firstEdge = false;
                edges.Append($"{{\"id\":\"c{conn.Id}\",\"source\":\"e{conn.SourceId}\",\"target\":\"e{conn.TargetId}\",\"label\":\"{edgeLabel}\",\"sourceLayer\":\"{sourceLayer}\"}}");
            }
        }

        var json = HtmlHelpers.HtmlEscape($"{{\"nodes\":[{nodes}],\"edges\":[{edges}]}}");
        return
            "<div id=\"ea-graph-container\"></div>\n" +
            $"<div id=\"ea-graph-data\" style=\"display:none\">{json}</div>";
    }

    private static string JsonEscape(string s) =>
        (s ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", " ").Replace("\t", " ");
}
