using Microsoft.Extensions.Logging;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export.Exporters;

internal class ElementPageWriter(IOutputWriter writer, ILogger logger)
{
    public async Task WriteAsync(EaElement element, string dir, ExportContext ctx)
    {
        var outputDir = ctx.OutputPath;
        var filePath = Path.Combine(dir, $"{MarkdownHelpers.SanitizeName(element.Name)}.md");

        if (element.ModifiedDate != DateTime.MinValue && File.Exists(filePath))
        {
            var fileTime = File.GetLastWriteTimeUtc(filePath);
            var elementTime = element.ModifiedDate.Kind == DateTimeKind.Utc
                ? element.ModifiedDate
                : element.ModifiedDate.ToUniversalTime();
            if (fileTime >= elementTime)
            {
                logger.LogDebug("Skipped {ElementName}", element.Name);
                return;
            }
        }

        var isNew = !File.Exists(filePath);
        logger.LogInformation("{Action} {ElementName}", isNew ? "Created" : "Updated", element.Name);

        var createdStr = element.CreatedDate?.ToString("yyyy-MM-dd") ?? "-";
        var modifiedStr = element.ModifiedDate == DateTime.MinValue ? "-" : element.ModifiedDate.ToString("yyyy-MM-dd");

        var lines = new List<string>
        {
            $"# {element.Name}",
            string.Empty,
            $"**Type:** {element.Type}  **Stereotype:** {element.Stereotype}  " +
            (string.IsNullOrEmpty(element.Status) ? "" : $"**Status:** {element.Status}  "),
            $"**Created:** {createdStr}  **Modified:** {modifiedStr}",
            string.Empty,
            string.Empty,
            MarkdownHelpers.BuildBreadcrumb(element.PackageId, dir, outputDir, ctx.PackageLookup),
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
        }

        if (ctx.DiagramIndex.TryGetValue(element.Id, out var elementDiagrams))
        {
            lines.Add("### Appears on Diagrams");
            lines.Add(string.Empty);

            foreach (var (diagram, pkgDir) in elementDiagrams)
            {
                var diagDir = Path.Combine(pkgDir, "diagrams");
                var diagLink = Path.GetRelativePath(dir, Path.Combine(diagDir, $"{MarkdownHelpers.SanitizeName(diagram.Name)}.md")).Replace('\\', '/');
                lines.Add($"- [{diagram.Name}]({diagLink})");
            }

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
        }

        lines.Add(MarkdownHelpers.FormatTimestamp());
        await writer.WriteFileAsync(filePath, string.Join(Environment.NewLine, lines));
    }
}
