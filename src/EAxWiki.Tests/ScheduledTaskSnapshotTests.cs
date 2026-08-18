using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class ScheduledTaskSnapshotTests
{
    private static readonly string DailyJson = """
        {
          "TaskName": "EAxWiki-Monitor",
          "State": "Ready",
          "WakeToRun": false,
          "ExecutionTimeLimit": "PT72H",
          "MultipleInstances": 1,
          "Triggers": [
            { "Kind": "MSFT_TaskDailyTrigger", "StartBoundary": "2026-08-01T00:00:00",
              "RepetitionInterval": "PT4H", "RepetitionDuration": "PT8H", "DaysInterval": 1, "DaysOfWeek": 0 }
          ]
        }
        """;

    private static readonly string WeeklyJson = """
        {
          "TaskName": "EAxWiki-Monitor",
          "State": "Ready",
          "WakeToRun": true,
          "ExecutionTimeLimit": "PT72H",
          "MultipleInstances": 1,
          "Triggers": [
            { "Kind": "MSFT_TaskWeeklyTrigger", "StartBoundary": "2026-08-03T08:00:00",
              "RepetitionInterval": "PT10M", "RepetitionDuration": "PT10H", "DaysInterval": 1, "DaysOfWeek": 62 }
          ]
        }
        """;

    [Fact]
    public void Parse_DailyTrigger_FormatsDescription()
    {
        var info = ScheduledTaskJsonParser.Parse(DailyJson);
        Assert.NotNull(info);
        Assert.Equal("EAxWiki-Monitor", info.TaskName);
        Assert.Equal("Ready", info.State);
        Assert.Equal("IgnoreNew", info.MultipleInstances);
        Assert.Equal("PT72H", info.ExecutionTimeLimit);
        Assert.Single(info.Triggers);
        Assert.Contains("Daily at 00:00", info.Triggers[0]);
        Assert.Contains("every 4 h (for 8 h)", info.Triggers[0]);
    }

    [Fact]
    public void Parse_WeeklyTrigger_ListsWeekdaysAndInterval()
    {
        var info = ScheduledTaskJsonParser.Parse(WeeklyJson);
        Assert.NotNull(info);
        Assert.True(info.WakeToRun);
        Assert.Single(info.Triggers);
        Assert.Contains("Mon, Tue, Wed, Thu, Fri at 08:00", info.Triggers[0]);
        Assert.Contains("every 10 min (for 10 h)", info.Triggers[0]);
    }

    [Fact]
    public void Parse_NullJson_ReturnsNull()
    {
        Assert.Null(ScheduledTaskJsonParser.Parse(null));
        Assert.Null(ScheduledTaskJsonParser.Parse("null"));
        Assert.Null(ScheduledTaskJsonParser.Parse("   "));
    }

    [Fact]
    public void Parse_NotAnObject_ReturnsNull()
    {
        Assert.Null(ScheduledTaskJsonParser.Parse("[]"));
        Assert.Null(ScheduledTaskJsonParser.Parse("{}"));
    }

    [Fact]
    public void Parse_UnknownTriggerKind_FallsBack()
    {
        var json = """
            { "TaskName": "EAxWiki-Monitor", "State": "Ready", "WakeToRun": false,
              "ExecutionTimeLimit": "PT72H", "MultipleInstances": 0,
              "Triggers": [ { "Kind": "MSFT_TaskLogonTrigger", "StartBoundary": null,
                              "RepetitionInterval": null, "RepetitionDuration": null,
                              "DaysInterval": 0, "DaysOfWeek": 0 } ] }
            """;
        var info = ScheduledTaskJsonParser.Parse(json);
        Assert.NotNull(info);
        Assert.Equal("Parallel", info.MultipleInstances);
        Assert.Contains("Logon at , single run", info.Triggers[0]);
    }

    [Fact]
    public void FormatIsoDuration_Variants()
    {
        Assert.Equal("4 h", ScheduledTaskJsonParser.FormatIsoDuration("PT4H"));
        Assert.Equal("10 min", ScheduledTaskJsonParser.FormatIsoDuration("PT10M"));
        Assert.Equal("10 h 30 min", ScheduledTaskJsonParser.FormatIsoDuration("PT10H30M"));
        Assert.Equal("1 d", ScheduledTaskJsonParser.FormatIsoDuration("P1D"));
        Assert.Null(ScheduledTaskJsonParser.FormatIsoDuration(null));
        Assert.Null(ScheduledTaskJsonParser.FormatIsoDuration("PT0S"));
    }

    [Fact]
    public void Get_CachesWithinTtl()
    {
        var calls = 0;
        string? Query() { calls++; return DailyJson; }
        var snapshot = new ScheduledTaskSnapshot(Query);

        snapshot.Get();
        snapshot.Get();

        Assert.Equal(1, calls);
    }

    [Fact]
    public void Get_ReQueriesAfterTtl()
    {
        var calls = 0;
        string? Query() { calls++; return DailyJson; }
        var snapshot = new ScheduledTaskSnapshot(Query, TimeSpan.FromMilliseconds(10));

        snapshot.Get();
        System.Threading.Thread.Sleep(30);
        snapshot.Get();

        Assert.Equal(2, calls);
    }

    [Fact]
    public void Get_QueryReturnsNull_ReturnsNull()
    {
        var snapshot = new ScheduledTaskSnapshot(() => null);
        Assert.Null(snapshot.Get());
    }
}
