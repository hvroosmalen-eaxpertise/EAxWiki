using System.Text.Json;

namespace EAxWiki.Monitor;

public record ScheduledTaskInfo(string TaskName, string State, bool WakeToRun,
    string ExecutionTimeLimit, string MultipleInstances, IReadOnlyList<string> Triggers);

/// <summary>
/// Parses the JSON emitted by ScheduledTaskSnapshot's pwsh query into ScheduledTaskInfo.
/// Pure and unit-testable; the pwsh side lives only in ScheduledTaskSnapshot.
/// </summary>
public static class ScheduledTaskJsonParser
{
    public static ScheduledTaskInfo? Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        if (json.Trim() == "null") return null;

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        if (root.ValueKind == JsonValueKind.Array)
            root = root.EnumerateArray().FirstOrDefault();
        if (root.ValueKind != JsonValueKind.Object) return null;
        if (!root.TryGetProperty("TaskName", out var taskNameProp)) return null;
        if (taskNameProp.ValueKind != JsonValueKind.String) return null;

        var triggers = new List<string>();
        if (root.TryGetProperty("Triggers", out var trig) && trig.ValueKind == JsonValueKind.Array)
        {
            foreach (var t in trig.EnumerateArray())
                triggers.Add(FormatTrigger(t));
        }

        return new ScheduledTaskInfo(
            taskNameProp.GetString() ?? "",
            AsString(root, "State") ?? "",
            root.TryGetProperty("WakeToRun", out var w) && w.ValueKind == JsonValueKind.True,
            AsString(root, "ExecutionTimeLimit") ?? "",
            AsString(root, "MultipleInstances") ?? AsInt(root, "MultipleInstances") switch
            {
                1 => "IgnoreNew",
                2 => "Queue",
                _ => "Parallel",
            },
            triggers);
    }

    private static string FormatTrigger(JsonElement t)
    {
        var kind = AsString(t, "Kind") ?? "";
        var start = AsString(t, "StartBoundary") ?? "";
        var interval = AsString(t, "RepetitionInterval");
        var duration = AsString(t, "RepetitionDuration");
        var daysInterval = AsInt(t, "DaysInterval");
        var daysOfWeek = AsInt(t, "DaysOfWeek");

        var when = interval == null
            ? "single run"
            : $"every {FormatIsoDuration(interval)}" + (duration == null ? "" : $" (for {FormatIsoDuration(duration)})");

        var clock = start.Length >= 16 ? start.Substring(11, 5) : "";
        return kind switch
        {
            "MSFT_TaskDailyTrigger" => $"Daily at {clock}" + (daysInterval > 1 ? $" (every {daysInterval} days)" : "") + $", {when}",
            "MSFT_TaskWeeklyTrigger" => $"{WeekdayNames(daysOfWeek)} at {clock}" + (daysInterval > 1 ? $" (every {daysInterval} weeks)" : "") + $", {when}",
            _ => $"{kind.Replace("MSFT_Task", "").Replace("Trigger", "")} at {clock}, {when}",
        };
    }

    private static string WeekdayNames(int mask)
    {
        var names = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        var hits = new List<string>();
        for (var i = 0; i < 7; i++)
            if ((mask & (1 << i)) != 0) hits.Add(names[i]);
        return hits.Count == 0 ? "(no weekdays)" : string.Join(", ", hits);
    }

    public static string? FormatIsoDuration(string? iso)
    {
        if (string.IsNullOrEmpty(iso)) return null;
        var text = iso.Trim();
        if (text == "PT0S") return null;
        if (text.StartsWith("P", StringComparison.Ordinal))
        {
            var daysPart = text.Substring(1).Split('T', 2)[0];
            if (daysPart.Length > 0 && int.TryParse(daysPart.TrimEnd('D'), out var days) && days > 0)
            {
                var timePart = text.Contains('T') ? text.Split('T', 2)[1] : "";
                var h = 0; var m = 0;
                ParseTimePart(timePart, ref h, ref m);
                return h > 0 ? $"{days} d {h} h" + (m > 0 ? $" {m} min" : "") : $"{days} d";
            }
        }
        if (text.StartsWith("PT", StringComparison.Ordinal))
        {
            var h = 0; var m = 0;
            ParseTimePart(text.Substring(2), ref h, ref m);
            if (h > 0 && m > 0) return $"{h} h {m} min";
            if (h > 0) return $"{h} h";
            if (m > 0) return $"{m} min";
        }
        return iso;
    }

    private static void ParseTimePart(string part, ref int h, ref int m)
    {
        var hIndex = part.IndexOf('H');
        var mIndex = part.IndexOf('M');
        if (hIndex >= 0 && int.TryParse(part.Substring(0, hIndex), out var hv))
            h = hv;
        if (mIndex >= 0)
        {
            var start = Math.Max(0, hIndex + 1);
            var mText = part.Substring(start, mIndex - start);
            if (mText.Length > 0 && int.TryParse(mText, out var mv))
                m = mv;
        }
    }

    private static string? AsString(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;

    private static int AsInt(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.Number ? p.GetInt32() : 0;
}
