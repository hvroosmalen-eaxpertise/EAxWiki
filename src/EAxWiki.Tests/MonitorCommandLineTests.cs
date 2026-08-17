using System.CommandLine;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class MonitorCommandLineTests
{
    private static CliOptions Parse(params string[] args) =>
        MonitorCommandLine.ToOptions(MonitorCommandLine.BuildCommand().Parse(args));

    [Fact]
    public void NoArgs_ReturnsDefaults()
    {
        var o = Parse();
        Assert.Null(o.Repo);
        Assert.Null(o.OutputDir);
        Assert.Null(o.Port);
        Assert.Null(o.MaxRetries);
        Assert.Null(o.RetryDelaySeconds);
        Assert.Null(o.MinElementFraction);
        Assert.Null(o.WebhookUrl);
        Assert.Null(o.TeamsWebhookUrl);
        Assert.Null(o.TelegramBotToken);
        Assert.Null(o.TelegramChatId);
        Assert.Null(o.Brand);
        Assert.False(o.TestAlert);
        Assert.Null(o.NotifyOnStart);
        Assert.False(o.Force);
        Assert.Null(o.ForceEveryNRuns);
        Assert.Null(o.ExportIntervalMinutes);
        Assert.Null(o.CheckIntervalSeconds);
        Assert.Null(o.LlmPort);
    }

    [Theory]
    [InlineData("-r", "model.qea")]
    [InlineData("--repo", "model.qea")]
    public void Repo_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(value, Parse(flag, value).Repo);
    }

    [Fact]
    public void Repo_ConnectionStringAsBarePositional()
    {
        // System.CommandLine: unmatched tokens are collected; the monitor treats a bare
        // non-flag argument as the repo path (PS accepted a bare connection string too).
        var o = Parse("DBType=postgresql;Database=foo");
        Assert.Equal("DBType=postgresql;Database=foo", o.Repo);
    }

    [Theory]
    [InlineData("-o", "wiki")]
    [InlineData("--output", "wiki")]
    public void OutputDir_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(value, Parse(flag, value).OutputDir);
    }

    [Theory]
    [InlineData("-p", "8080")]
    [InlineData("--port", "8080")]
    public void Port_ParsesShortAndLong(string flag, string value)
    {
        Assert.Equal(8080, Parse(flag, value).Port);
    }

    [Fact]
    public void MaxRetries_Parses()
    {
        Assert.Equal(5, Parse("--max-retries", "5").MaxRetries);
    }

    [Fact]
    public void RetryDelay_Parses()
    {
        Assert.Equal(60, Parse("--retry-delay", "60").RetryDelaySeconds);
    }

    [Fact]
    public void MinElementFraction_ParsesDouble()
    {
        Assert.Equal(0.25, Parse("--min-element-fraction", "0.25").MinElementFraction);
    }

    [Fact]
    public void Webhooks_Parse()
    {
        var o = Parse("--webhook-url", "https://hooks.slack.com/ABC", "--teams-webhook-url", "https://outlook.office.com/DEF");
        Assert.Equal("https://hooks.slack.com/ABC", o.WebhookUrl);
        Assert.Equal("https://outlook.office.com/DEF", o.TeamsWebhookUrl);
    }

    [Fact]
    public void Telegram_Parses()
    {
        var o = Parse("--telegram-bot-token", "123:ABC", "--telegram-chat-id", "-100123");
        Assert.Equal("123:ABC", o.TelegramBotToken);
        Assert.Equal("-100123", o.TelegramChatId);
    }

    [Fact]
    public void Brand_Parses()
    {
        Assert.Equal("eursura", Parse("--brand", "eursura").Brand);
    }

    [Fact]
    public void TestAlert_IsSet()
    {
        Assert.True(Parse("--test-alert").TestAlert);
    }

    [Fact]
    public void NoNotifyStart_IsSet()
    {
        Assert.False(Parse("--no-notify-start").NotifyOnStart);
    }

    [Theory]
    [InlineData("-f")]
    [InlineData("--force")]
    public void Force_ParsesShortAndLong(string flag)
    {
        Assert.True(Parse(flag).Force);
    }

    [Fact]
    public void ForceEvery_Parses()
    {
        Assert.Equal(48, Parse("--force-every", "48").ForceEveryNRuns);
    }

    [Fact]
    public void ExportInterval_Parses()
    {
        Assert.Equal(60, Parse("--export-interval", "60").ExportIntervalMinutes);
    }

    [Fact]
    public void CheckInterval_Parses()
    {
        Assert.Equal(15, Parse("--check-interval", "15").CheckIntervalSeconds);
    }

    [Fact]
    public void LlmPort_Parses()
    {
        Assert.Equal(9090, Parse("--llm-port", "9090").LlmPort);
    }
}