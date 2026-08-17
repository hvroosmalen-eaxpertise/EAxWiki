using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

/// <summary>
/// Port of the PS Format-TelegramAlertText: emoji title, &lt;b&gt; kind label, HTML-escaping,
/// fence → &lt;pre&gt; with inner escaping (two-pass so the outer escaper doesn't double-escape),
/// 4000-char truncation. HTML mode because Markdown v1 silently drops content on unmatched '*'/'_'.
/// </summary>
public static class TelegramAlertTextFormatter
{
    private const string EnDash = "\u2014";

    public static string EmojiFor(AlertKind kind) => kind switch
    {
        AlertKind.Start => "\U0001F504",          // 🔄
        AlertKind.Finish => "\U0001F7E2",         // 🟢
        AlertKind.Failure => "\U0001F534",        // 🔴
        AlertKind.ServeFailure => "\U0001F534",
        AlertKind.LlmFailure => "\U0001F534",
        AlertKind.ApiFailure => "\U0001F534",
        AlertKind.Recovery => "\U0001F7E2",
        AlertKind.ServeRecovery => "\U0001F7E2",
        AlertKind.LlmRecovery => "\U0001F7E2",
        AlertKind.ApiRecovery => "\U0001F7E2",
        AlertKind.Test => "\U0001F535",           // 🔵
        AlertKind.DailyDigest => "\U0001F4CA",    // 📊
        AlertKind.UserStop => "\u270B",           // ✋
        _ => "\U0001F535",
    };

    public static string ColorFor(AlertKind kind) => kind switch
    {
        AlertKind.Start => "#3aa3e3",
        AlertKind.Finish => "#28a745",
        AlertKind.Failure => "#dc3545",
        AlertKind.ServeFailure => "#dc3545",
        AlertKind.LlmFailure => "#dc3545",
        AlertKind.ApiFailure => "#dc3545",
        AlertKind.Recovery => "#28a745",
        AlertKind.ServeRecovery => "#28a745",
        AlertKind.LlmRecovery => "#28a745",
        AlertKind.ApiRecovery => "#28a745",
        AlertKind.Test => "#3aa3e3",
        AlertKind.DailyDigest => "#3aa3e3",
        AlertKind.UserStop => "#FF8C00",
        _ => "#3aa3e3",
    };

    public static string Format(AlertKind kind, string instanceLabel, string message, DateTimeOffset timestamp)
    {
        var labelHtml = HtmlEscape(instanceLabel);
        var kindHtml = HtmlEscape(kind.ToString());
        var stamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss zzz");

        var composed = $"{EmojiFor(kind)} <b>EAxWiki [{kindHtml}]</b> {EnDash} {labelHtml}\n" +
                       $"{FencesToPre(message)}\n\n<i>{labelHtml} • {stamp}</i>";

        if (composed.Length > 4000)
            composed = composed[..4000] + "\n... (truncated)";
        return composed;
    }

    public static string HtmlEscape(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    public static string FencesToPre(string text)
    {
        // Two-pass: swap fences to placeholders so the second pass doesn't double-escape pre content.
        var preBlocks = new List<string>();
        var withPlaceholders = Regex.Replace(text, "(?s)```(.*?)```", m =>
        {
            preBlocks.Add("<pre>" + HtmlEscape(m.Groups[1].Value.Trim('\r', '\n')) + "</pre>");
            return $"\uFFFD{"PRE"}{preBlocks.Count - 1}\uFFFD";
        });
        var escaped = HtmlEscape(withPlaceholders);
        return Regex.Replace(escaped, "\uFFFD" + "PRE(\\d+)" + "\uFFFD", m =>
            preBlocks[int.Parse(m.Groups[1].Value)]);
    }
}