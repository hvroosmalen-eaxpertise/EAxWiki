using System.Net;
using System.Text.Json;
using EAxWiki.Monitor;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class AlertDispatcherTests
{
    private static readonly AlertOptions Options = new(
        WebhookUrl: "https://hooks.slack.com/ABC",
        TeamsWebhookUrl: "https://outlook.office.com/DEF",
        TelegramBotToken: "123456789:AAbbCCddEeffGGhhIIjj",
        TelegramChatId: "-1001234567890",
        InstanceLabel: "MYPC - C:\\repo\\wiki");

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public readonly List<(HttpRequestMessage Request, string Body)> Sent = new();
        public HttpStatusCode StatusCode = HttpStatusCode.OK;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var body = request.Content!.ReadAsStringAsync(ct).GetAwaiter().GetResult();
            Sent.Add((request, body));
            var response = new HttpResponseMessage(StatusCode) { Content = new StringContent("{}") };
            return Task.FromResult(response);
        }
    }

    private static AlertDispatcher Dispatcher(RecordingHandler handler, AlertOptions? options = null) =>
        new(options ?? Options, handler, NullLogger.Instance);

    private static JsonElement RootOf(string body) =>
        JsonDocument.Parse(body).RootElement.Clone();

    [Fact]
    public void Slack_Payload_HasAttachmentWithPretextAndFooter()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Export failed", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "hooks.slack.com");
        var root = RootOf(req.Body);
        var attachment = root.GetProperty("attachments")[0];
        Assert.Equal("#dc3545", attachment.GetProperty("color").GetString());
        Assert.Equal(":red_circle: *EAxWiki [Failure]* - MYPC - C:\\repo\\wiki",
            attachment.GetProperty("pretext").GetString());
        Assert.Equal("Export failed", attachment.GetProperty("text").GetString());
        Assert.Equal("MYPC - C:\\repo\\wiki", attachment.GetProperty("footer").GetString());
        Assert.True(attachment.GetProperty("ts").GetInt64() > 0);
    }

    [Fact]
    public void Teams_Payload_IsMessageCardWithThemeColor()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Serve down", AlertKind.ServeFailure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "outlook.office.com");
        var root = RootOf(req.Body);
        Assert.Equal("MessageCard", root.GetProperty("@type").GetString());
        Assert.Equal("dc3545", root.GetProperty("themeColor").GetString()); // no '#'
        Assert.Equal("EAxWiki [ServeFailure] - MYPC - C:\\repo\\wiki", root.GetProperty("summary").GetString());
        Assert.Equal("Serve down", root.GetProperty("sections")[0].GetProperty("text").GetString());
    }

    [Fact]
    public void Telegram_Text_HasEmojiTitleFooterAndHtmlEscaping()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("a <b>boom</b>", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        Assert.Equal("https://api.telegram.org/bot123456789:AAbbCCddEeffGGhhIIjj/sendMessage", req.Request!.RequestUri!.ToString());
        var root = RootOf(req.Body);
        var text = root.GetProperty("text").GetString()!;
        Assert.StartsWith("🔴 <b>EAxWiki [Failure]</b> — MYPC - C:\\repo\\wiki", text);
        Assert.Contains("a &lt;b&gt;boom&lt;/b&gt;", text); // label + body escaped, label en-dash
        Assert.Contains("<i>MYPC - C:\\repo\\wiki • ", text);
        Assert.Equal("HTML", root.GetProperty("parse_mode").GetString());
        Assert.Equal("-1001234567890", root.GetProperty("chat_id").GetString());
    }

    [Fact]
    public void Telegram_Fences_BecomePre_WithInnerEscaping()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch("Export failed.\n```\nline with <tag> & stuff\n```\nDone.", AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        var text = RootOf(req.Body).GetProperty("text").GetString()!;
        Assert.Contains("Export failed.\n<pre>line with &lt;tag&gt; &amp; stuff</pre>\nDone.", text);
    }

    [Fact]
    public void Telegram_TruncatesAt4000Chars()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler).Dispatch(new string('x', 10000), AlertKind.Failure);

        var req = handler.Sent.Single(r => r.Request.RequestUri!.Host == "api.telegram.org");
        var text = RootOf(req.Body).GetProperty("text").GetString()!;
        Assert.Equal(4000 + "\n... (truncated)".Length, text.Length);
        Assert.EndsWith("... (truncated)", text);
    }

    [Fact]
    public void Telegram_Http400_RetriesOnceWithoutParseMode()
    {
        var handler = new RecordingHandler { StatusCode = HttpStatusCode.BadRequest };
        var dispatcher = Dispatcher(handler);
        dispatcher.Dispatch("oops", AlertKind.Failure);

        var tg = handler.Sent.Where(r => r.Request.RequestUri!.Host == "api.telegram.org").ToList();
        Assert.Equal(2, tg.Count);
        Assert.Equal("HTML", RootOf(tg[0].Body).GetProperty("parse_mode").GetString());
        Assert.False(RootOf(tg[1].Body).TryGetProperty("parse_mode", out _));
    }

    [Fact]
    public void NoChannelsConfigured_DoesNothing()
    {
        var handler = new RecordingHandler();
        Dispatcher(handler, new AlertOptions(null, null, null, null, "label")).Dispatch("hi", AlertKind.Test);
        Assert.Empty(handler.Sent);
    }
}