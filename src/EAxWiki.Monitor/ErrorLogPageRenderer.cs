using System.Globalization;
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

/// <summary>
/// Renders errors-template.md → {wikiDir}/status/errors.md. Reads the instance's own
/// {logsDir}/monitor-*.log files (Task 1 severity format), keeps [WRN]/[ERR] lines within a
/// 7-day window (newest first, capped at 100), redacts the instance's secrets and
/// connection-string passwords, and fills @@GENERATED_AT@@ / @@ERRORS@@ / @@RECENT@@.
/// </summary>
public class ErrorLogPageRenderer
{
    private static readonly Regex SeverityRegex = new(@"\[(INF|WRN|ERR)\]", RegexOptions.Compiled);
    private static readonly Regex ConnectionStringRegex = new(@"(?i)(Password|Pwd)\s*=[^;]*", RegexOptions.Compiled);

    private readonly string _templatePath;
    private readonly string _outputPath;
    private readonly string _logsDir;
    private readonly string[] _secrets;

    public ErrorLogPageRenderer(string templatePath, string wikiDir, string logsDir, string[] secrets)
    {
        _templatePath = templatePath;
        _outputPath = Path.Combine(wikiDir, "status", "errors.md");
        _logsDir = logsDir;
        _secrets = secrets ?? [];
    }

    public void Render(DateTime now)
    {
        var lines = ReadLogLines(now);

        var errors = lines
            .Where(l => l.Severity is "WRN" or "ERR")
            .Take(100)
            .Select(l => Redact(l.Text))
            .ToList();

        var recent = lines.Take(20).Select(l => Redact(l.Text)).ToList();

        var template = File.ReadAllText(_templatePath);
        template = template.Replace("@@GENERATED_AT@@", now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        template = template.Replace("@@ERRORS@@", errors.Count == 0
            ? "No issues found in the last 7 days."
            : string.Join(Environment.NewLine, errors.Select(e => "- `" + e + "`")));
        template = template.Replace("@@RECENT@@", recent.Count == 0
            ? "(no log lines yet)"
            : string.Join(Environment.NewLine, recent.Select(e => "- `" + e + "`")));

        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);
        File.WriteAllText(_outputPath, template);
    }

    private List<(string Severity, string Text)> ReadLogLines(DateTime now)
    {
        var result = new List<(string Severity, string Text)>();
        if (!Directory.Exists(_logsDir)) return result;

        var files = Directory.GetFiles(_logsDir, "monitor-*.log")
            .Where(f =>
            {
                var name = Path.GetFileName(f);
                if (name.Length < 19) return false;
                return DateTime.TryParseExact(name.Substring(8, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var d)
                    && d.Date >= now.Date.AddDays(-6) && d.Date <= now.Date;
            })
            .OrderByDescending(File.GetLastWriteTime)
            .ToArray();

        foreach (var file in files)
        {
            foreach (var line in File.ReadLines(file).Reverse())
            {
                var m = SeverityRegex.Match(line);
                var severity = m.Success ? m.Groups[1].Value : "INF"; // pre-Task-1 lines count as INF
                result.Add((severity, line));
            }
        }
        return result;
    }

    private string Redact(string line)
    {
        var result = line;
        foreach (var secret in _secrets)
        {
            if (!string.IsNullOrEmpty(secret) && secret.Length >= 3)
                result = result.Replace(secret, "***");
        }
        return ConnectionStringRegex.Replace(result, "$1=***");
    }
}
