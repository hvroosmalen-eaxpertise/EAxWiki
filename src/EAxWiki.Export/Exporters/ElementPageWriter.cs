using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class ElementPageWriter(IOutputWriter writer, ILogger logger)
{
    public async Task WriteAsync(EaElement element, string dir, ExportContext ctx, string? fileNameOverride = null)
    {
        var outputDir = ctx.OutputPath;
        var baseName = fileNameOverride ?? MarkdownHelpers.SanitizeName(element.Name);
        var filePath = Path.Combine(dir, $"{baseName}.md");

        var isNew = true;
        if (!ctx.Force && element.ModifiedDate != DateTime.MinValue)
        {
            try
            {
                var fileTime = File.GetLastWriteTimeUtc(filePath);
                // GetLastWriteTimeUtc returns 1601-01-01 when the file does not exist — treat that as "needs write"
                if (fileTime > DateTime.UnixEpoch)
                {
                    isNew = false;
                    var elementTime = element.ModifiedDate.Kind == DateTimeKind.Utc
                        ? element.ModifiedDate
                        : element.ModifiedDate.ToUniversalTime();
                    if (fileTime >= elementTime)
                    {
                        logger.LogDebug("Skipped {ElementName}", element.Name);
                        return;
                    }
                }
            }
            catch (IOException)
            {
                // File was removed between our check and the read — proceed with writing
            }
        }
        logger.LogInformation("{Action} {ElementName}", isNew ? "Created" : "Updated", element.Name);

        var createdStr = element.CreatedDate?.ToString("yyyy-MM-dd") ?? "-";
        var modifiedStr = element.ModifiedDate == DateTime.MinValue ? "-" : element.ModifiedDate.ToString("yyyy-MM-dd");

        var lines = new List<string>
        {
            $"# {MarkdownHelpers.GetStereotypeLabel(element)} {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  " +
            $"**Stereotype:** {MarkdownHelpers.EscapeCell(element.Stereotype)}  " +
            (string.IsNullOrWhiteSpace(element.StereotypeEx) ? "" : $"**StereotypeEx:** {MarkdownHelpers.EscapeCell(element.StereotypeEx)}  ") +
            (string.IsNullOrWhiteSpace(element.FQStereotype) ? "" : $"**FQStereotype:** {MarkdownHelpers.EscapeCell(element.FQStereotype)}  ") +
            (string.IsNullOrEmpty(element.Status) ? "" : $"**Status:** <span class=\"status-badge status-{element.Status.ToLowerInvariant()}\">{MarkdownHelpers.EscapeCell(element.Status)}</span>  "),
            $"**Created:** {createdStr}  **Modified:** {modifiedStr}",
            string.Empty,
            string.Empty,
            MarkdownHelpers.BuildBreadcrumb(element.PackageId, dir, outputDir, ctx.PackageLookup,
                msg => logger.LogWarning("{Message} (element '{Name}')", msg, element.Name)),
            string.Empty,
        };

        if (!string.IsNullOrWhiteSpace(element.Notes))
        {
            lines.Add(element.Notes);
            lines.Add(string.Empty);
        }

        if (element.Attributes.Count > 0)
        {
            lines.Add("## Attributes");
            lines.Add(string.Empty);
            lines.Add("| Name | Type | Default | Description |");
            lines.Add("|------|------|---------|-------------|");

            foreach (var attr in element.Attributes)
            {
                var desc = (attr.Notes ?? "").Replace("|", "\\|").Replace("\n", "<br/>");
                lines.Add($"| {MarkdownHelpers.EscapeCell(attr.Name)} | {MarkdownHelpers.EscapeCell(attr.Type)} | {MarkdownHelpers.EscapeCell(attr.DefaultValue ?? "")} | {desc} |");
            }

            lines.Add(string.Empty);
            lines.Add("[↑ Back to top](#)");
            lines.Add(string.Empty);
        }

        if (element.Methods.Count > 0)
        {
            lines.Add("## Methods");
            lines.Add(string.Empty);

            foreach (var method in element.Methods)
            {
                var staticTag = method.IsStatic ? " *(static)*" : "";
                lines.Add($"### {method.Name}{staticTag}");
                lines.Add(string.Empty);
                lines.Add($"**Returns:** `{method.Type}`");
                lines.Add(string.Empty);
                if (!string.IsNullOrWhiteSpace(method.Notes))
                {
                    lines.Add(method.Notes);
                    lines.Add(string.Empty);
                }
            }
        }

        if (element.TaggedValues.Count > 0)
        {
            lines.Add("## Tagged Values");
            lines.Add(string.Empty);
            lines.Add("| Name | Value | Notes |");
            lines.Add("|------|-------|-------|");

            foreach (var tv in element.TaggedValues)
                lines.Add($"| {MarkdownHelpers.EscapeCell(tv.Name)} | {MarkdownHelpers.EscapeCell(tv.Value)} | {MarkdownHelpers.EscapeCell(tv.Notes ?? "")} |");

            lines.Add(string.Empty);
            lines.Add("[↑ Back to top](#)");
            lines.Add(string.Empty);
        }

        if (element.Connectors.Count > 0)
        {
            lines.Add("## Relationships");
            lines.Add(string.Empty);
            lines.Add("| Type | Stereotype | Connected To |");
            lines.Add("|------|------------|-------------|");

            foreach (var conn in element.Connectors)
            {
                var otherId = conn.SourceId == element.Id ? conn.TargetId
                    : conn.TargetId == element.Id ? conn.SourceId
                    : -1;

                if (otherId <= 0) continue;

                string connectedTo;
                if (ctx.ElementLookup.TryGetValue(otherId, out var other))
                {
                    var otherName = MarkdownHelpers.SanitizeName(other.Element.Name);
                    var relativePath = Path.GetRelativePath(dir, Path.Combine(other.PackageDir, $"{otherName}.md")).Replace('\\', '/');
                    connectedTo = $"[{MarkdownHelpers.EscapeCell(other.Element.Name)}]({relativePath})";
                }
                else
                {
                    connectedTo = $"Element ID {otherId} (not in export)";
                }

                lines.Add($"| {MarkdownHelpers.EscapeCell(conn.Type)} | {MarkdownHelpers.EscapeCell(conn.Stereotype)} | {connectedTo} |");
            }

            lines.Add(string.Empty);
            lines.Add("[↑ Back to top](#)");
            lines.Add(string.Empty);
        }

        if (ctx.DiagramIndex.TryGetValue(element.Id, out var elementDiagrams))
        {
            lines.Add("### Appears on Diagrams");
            lines.Add(string.Empty);
            lines.Add("<div class=\"diagram-thumbs\">");

            foreach (var (diagram, pkgDir) in elementDiagrams)
            {
                var diagDir = Path.Combine(pkgDir, "diagrams");
                var sanitized = MarkdownHelpers.SanitizeName(diagram.Name);
                var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.md")).Replace('\\', '/');
                var diagLinkHtml = diagLink.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                    ? diagLink[..^3] + ".html" : diagLink;

                var pngRelPath = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{sanitized}.png")).Replace('\\', '/');
                var pngAbsPath = Path.Combine(diagDir, $"{sanitized}.png");

                if (File.Exists(pngAbsPath))
                    lines.Add($"  <a href=\"{diagLinkHtml}\" class=\"diagram-thumb\"><img src=\"{pngRelPath}\" alt=\"{diagram.Name}\" loading=\"lazy\"><span>{MarkdownHelpers.EscapeCell(diagram.Name)}</span></a>");
                else
                    lines.Add($"  <a href=\"{diagLinkHtml}\" class=\"diagram-thumb diagram-thumb--noimg\"><span>{MarkdownHelpers.EscapeCell(diagram.Name)}</span></a>");
            }

            lines.Add("</div>");
            lines.Add(string.Empty);
            lines.Add("[↑ Back to top](#)");
            lines.Add(string.Empty);
        }

        if (ctx.IncomingIndex.TryGetValue(element.Id, out var incomingConns))
        {
            lines.Add("### Referenced By");
            lines.Add(string.Empty);
            lines.Add("| Type | Stereotype | Source |");
            lines.Add("|------|------------|--------|");

            foreach (var (conn, sourceId) in incomingConns)
            {
                string source;
                if (ctx.ElementLookup.TryGetValue(sourceId, out var srcElem))
                {
                    var srcName = MarkdownHelpers.SanitizeName(srcElem.Element.Name);
                    var relativePath = Path.GetRelativePath(dir, Path.Combine(srcElem.PackageDir, $"{srcName}.md")).Replace('\\', '/');
                    source = $"[{MarkdownHelpers.EscapeCell(srcElem.Element.Name)}]({relativePath})";
                }
                else
                {
                    source = $"Element ID {sourceId} (not in export)";
                }

                lines.Add($"| {MarkdownHelpers.EscapeCell(conn.Type)} | {MarkdownHelpers.EscapeCell(conn.Stereotype)} | {source} |");
            }

            lines.Add(string.Empty);
            lines.Add("[↑ Back to top](#)");
            lines.Add(string.Empty);
        }

        var graphHtml = BuildGraphHtml(element, dir, ctx);
        if (graphHtml.Length > 0)
            lines.AddRange(["---", string.Empty, "## Relationship Graph", string.Empty, graphHtml, string.Empty]);

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines));
    }

    private static string BuildGraphHtml(EaElement focal, string focalPkgDir, ExportContext ctx)
    {
        // 1-hop: all elements directly connected to focal (both directions)
        var hop1 = new HashSet<int>();
        foreach (var conn in focal.Connectors)
        {
            var neighborId = conn.SourceId == focal.Id ? conn.TargetId : conn.SourceId;
            if (neighborId != focal.Id && ctx.ElementLookup.ContainsKey(neighborId))
                hop1.Add(neighborId);
        }

        if (hop1.Count == 0) return string.Empty;

        // 2-hop: neighbors of neighbors not already in the set
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

        // Nodes JSON
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

        // Edges JSON — deduplicate by connector ID, both endpoints must be in allIds
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

        var json = HtmlEscape($"{{\"nodes\":[{nodes}],\"edges\":[{edges}]}}");
        return
            "<div id=\"ea-graph-container\"></div>\n" +
            $"<div id=\"ea-graph-data\" style=\"display:none\">{json}</div>";
    }

    private static string JsonEscape(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", " ").Replace("\t", " ");

    private static string HtmlEscape(string s) =>
        s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
