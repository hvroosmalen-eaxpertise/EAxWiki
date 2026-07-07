using System.Text.RegularExpressions;
using System.Text.Json;
using EAxWiki.Export.Helpers;

namespace EAxWiki.Export;

internal record ValidationIssue(string File, string Type, string Message);

internal record ValidationReport(
    int FilesValidated,
    int Passed,
    int Warnings,
    int Errors,
    List<ValidationIssue> Issues
);

internal static class ExportValidator
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static ValidationReport Validate(IEnumerable<string> files, string outputPath)
    {
        var issues = new List<ValidationIssue>();
        var passed = 0;
        var validated = 0;

        foreach (var file in files)
        {
            if (!File.Exists(file)) continue;
            validated++;
            var fileIssues = ValidateFile(file, outputPath);
            if (fileIssues.Count == 0)
                passed++;
            else
                issues.AddRange(fileIssues);
        }

        var warnings = issues.Count(i => i.Type == "warning");
        var errors = issues.Count(i => i.Type == "error");
        return new ValidationReport(validated, passed, warnings, errors, issues);
    }

    private static List<ValidationIssue> ValidateFile(string filePath, string outputPath)
    {
        var issues = new List<ValidationIssue>();
        var relativePath = Path.GetRelativePath(outputPath, filePath);
        var content = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(content))
        {
            issues.Add(new ValidationIssue(relativePath, "error", "File is empty"));
            return issues;
        }

        var fm = FrontmatterParser.Parse(filePath);
        if (fm.Count == 0 && content.Contains("---"))
        {
            issues.Add(new ValidationIssue(relativePath, "error", "YAML frontmatter present but could not be parsed"));
        }

        var body = StripFrontmatter(content);

        foreach (var tag in FindUnclosedTags(body))
            issues.Add(new ValidationIssue(relativePath, "error", $"Unclosed <{tag}> tag"));

        foreach (Match match in Regex.Matches(body, @"\]\(([^)]+)\)"))
        {
            var link = match.Groups[1].Value;
            if (IsExternalUrl(link) || link.StartsWith('#')) continue;
            var target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath)!, link));
            if (!File.Exists(target))
                issues.Add(new ValidationIssue(relativePath, "warning", $"Broken link: {link}"));
        }

        foreach (Match match in Regex.Matches(body, @"<img\s+[^>]*src=""([^""]+)"""))
        {
            var src = match.Groups[1].Value;
            if (IsExternalUrl(src)) continue;
            var imgPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath)!, src));
            if (!File.Exists(imgPath))
                issues.Add(new ValidationIssue(relativePath, "warning", $"Missing image: {src}"));
        }

        return issues;
    }

    private static bool IsExternalUrl(string value) =>
        value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

    private static string StripFrontmatter(string content)
    {
        var match = Regex.Match(content, @"\A---\s*\n.*?\n---\s*\n", RegexOptions.Singleline);
        return match.Success ? content[match.Length..] : content;
    }

    private static List<string> FindUnclosedTags(string html)
    {
        var tagsToCheck = new[] { "div", "span", "details", "summary", "table", "tr", "td", "th", "a", "p", "ul", "ol", "li", "pre", "code", "select", "option" };
        var unclosed = new List<string>();
        foreach (var tag in tagsToCheck)
        {
            var opens = Regex.Matches(html, $"<{tag}(\\s[^>]*)?>", RegexOptions.IgnoreCase).Count;
            var closes = Regex.Matches(html, $"</{tag}>", RegexOptions.IgnoreCase).Count;
            if (opens > closes)
                unclosed.Add(tag);
        }
        return unclosed;
    }

    public static string ToJson(ValidationReport report) =>
        JsonSerializer.Serialize(report, JsonOptions);
}
