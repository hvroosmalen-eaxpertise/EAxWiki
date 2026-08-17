using System.Text.RegularExpressions;
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

public record WritebackDelta(int Total, IReadOnlyDictionary<string, int> Kinds);

public interface IDigestTracker
{
    /// <summary>Count page-read lines in the newest serve-*.err.log since the last scan.</summary>
    int CountNewPageReads();

    /// <summary>Count write-back lines in wiki/status/writeback.log since the last scan.</summary>
    WritebackDelta CountNewWritebacks();

    /// <summary>Return a DailyDigest message on a calendar-day boundary (and reset counters), else null.</summary>
    string? MaybeComposeDailyDigest(DateTime now);
}

/// <summary>
/// Offset-based incremental counters over append-only logs. Both counters use a file+offset pair
/// in HealthState so a frequently-run monitor never re-counts already-seen lines; a log that was
/// rotated/truncated (length &lt; offset) resets to 0.
/// </summary>
public class DigestTracker : IDigestTracker
{
    private static readonly Regex ReloadRegex = new(@"\[(\d{2}):(\d{2}):(\d{2})\]\s+Reloading browsers");
    private static readonly Regex ConnectRegex = new(@"\[(\d{2}):(\d{2}):(\d{2})\]\s+Browser connected:");

    private readonly HealthState _state;
    private readonly string _wikiDir;
    private readonly string _logDir;
    private readonly string _digestTemplatePath;

    public DigestTracker(HealthState state, string wikiDir, string logDir, string digestTemplatePath)
    {
        _state = state;
        _wikiDir = wikiDir;
        _logDir = logDir;
        _digestTemplatePath = digestTemplatePath;
    }

    public int CountNewPageReads()
    {
        var files = Directory.Exists(_logDir)
            ? Directory.GetFiles(_logDir, "serve-*.err.log").OrderBy(File.GetLastWriteTime).ToArray()
            : [];
        if (files.Length == 0) return 0;

        var currentFile = files[^1];
        if (_state.PageReadLogFile != currentFile)
        {
            _state.PageReadLogFile = currentFile;
            _state.PageReadLogOffset = 0;
        }

        var newText = ReadNewText(currentFile, () => _state.PageReadLogOffset,
            v => _state.PageReadLogOffset = v);
        if (newText == null) return 0;

        int? lastReloadSeconds = null;
        var count = 0;
        foreach (var line in newText.Split('\n'))
        {
            var m = ReloadRegex.Match(line);
            if (m.Success)
            {
                lastReloadSeconds = ToSeconds(m);
                continue;
            }
            m = ConnectRegex.Match(line);
            if (m.Success)
            {
                var seconds = ToSeconds(m);
                if (lastReloadSeconds is { } reload && seconds - reload is >= 0 and <= 10)
                    continue;
                count++;
            }
        }
        return count;
    }

    public WritebackDelta CountNewWritebacks()
    {
        var logPath = Path.Combine(_wikiDir, "status", "writeback.log");
        if (_state.WritebackLogFile != logPath)
        {
            _state.WritebackLogFile = logPath;
            _state.WritebackLogOffset = 0;
        }

        var newText = ReadNewText(logPath, () => _state.WritebackLogOffset,
            v => _state.WritebackLogOffset = v);
        if (newText == null) return new WritebackDelta(0, new Dictionary<string, int>());

        var kinds = new Dictionary<string, int>();
        var total = 0;
        foreach (var line in newText.Split('\n'))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0) continue;
            total++;
            var parts = trimmed.Split((char[]?)null, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
                kinds[parts[2]] = kinds.GetValueOrDefault(parts[2]) + 1;
        }
        return new WritebackDelta(total, kinds);
    }

    public string? MaybeComposeDailyDigest(DateTime now)
    {
        var today = now.ToString("yyyy-MM-dd");
        if (_state.LastDigestDate is { Length: > 0 } last && last != today)
        {
            var template = File.ReadAllText(_digestTemplatePath);
            var message = template
                .Replace("@@DIGEST_DATE@@", last)
                .Replace("@@PAGE_READS_TODAY@@", _state.PageReadsToday.ToString())
                .Replace("@@WRITEBACKS_TODAY@@", _state.WritebacksToday.ToString());
            _state.PageReadsToday = 0;
            _state.WritebacksToday = 0;
            _state.LastDigestDate = today;
            return message;
        }
        _state.LastDigestDate = today;
        return null;
    }

    private static int ToSeconds(Match m) =>
        int.Parse(m.Groups[1].Value) * 3600 + int.Parse(m.Groups[2].Value) * 60 + int.Parse(m.Groups[3].Value);

    private static string? ReadNewText(string path, Func<long> getOffset, Action<long> setOffset)
    {
        if (!File.Exists(path)) return null;
        var length = new FileInfo(path).Length;
        var offset = getOffset();
        if (length < offset) offset = 0;
        if (length == offset) return null;

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        stream.Seek(offset, SeekOrigin.Begin);
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        setOffset(stream.Position);
        return text;
    }
}
