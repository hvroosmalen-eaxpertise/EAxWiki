using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class HealthStoreTests : IDisposable
{
    private readonly string _dir;

    public HealthStoreTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_health_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void Load_MissingFile_ReturnsDefaults()
    {
        var state = new HealthStore().Load(Path.Combine(_dir, "health.json"));
        Assert.Equal(0, state.ConsecutiveFailures);
        Assert.Null(state.LastSuccessTime);
        Assert.False(state.SkipExport);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesAllFields()
    {
        var path = Path.Combine(_dir, "health.json");
        var store = new HealthStore();
        var state = new HealthState
        {
            LastSuccessTime = DateTimeOffset.Parse("2026-08-01T10:00:00Z"),
            ConsecutiveFailures = 2,
            LastElementCount = 150,
            RunsSinceForce = 7,
            SkipExport = true,
            SkipServe = true,
            PageReadsToday = 12,
            WritebacksToday = 3,
            PageReadLogFile = @"C:\logs\serve-1.err.log",
            PageReadLogOffset = 4096,
        };

        store.Save(path, state);
        var loaded = store.Load(path);

        Assert.Equal(state.LastSuccessTime, loaded.LastSuccessTime);
        Assert.Equal(state.ConsecutiveFailures, loaded.ConsecutiveFailures);
        Assert.Equal(state.LastElementCount, loaded.LastElementCount);
        Assert.Equal(state.RunsSinceForce, loaded.RunsSinceForce);
        Assert.True(loaded.SkipExport);
        Assert.True(loaded.SkipServe);
        Assert.Equal(state.PageReadsToday, loaded.PageReadsToday);
        Assert.Equal(state.WritebacksToday, loaded.WritebacksToday);
        Assert.Equal(state.PageReadLogFile, loaded.PageReadLogFile);
        Assert.Equal(state.PageReadLogOffset, loaded.PageReadLogOffset);
    }

    [Fact]
    public void Load_OlderFile_MissingFieldsBackfillToDefaults()
    {
        // Simulates a health.json written by an older monitor that lacked skipExport.
        var path = Path.Combine(_dir, "health.json");
        File.WriteAllText(path, """{"consecutiveFailures":1,"lastSuccessTime":"2026-08-01T10:00:00Z"}""");

        var state = new HealthStore().Load(path);

        Assert.Equal(1, state.ConsecutiveFailures);
        Assert.Equal("2026-08-01T10:00:00.0000000+00:00", state.LastSuccessTime?.ToString("O"));
        Assert.False(state.SkipExport);
        Assert.False(state.SkipServe);
        Assert.Equal(0, state.PageReadLogOffset);
    }

    [Fact]
    public void Load_CorruptFile_FallsBackToDefaults()
    {
        var path = Path.Combine(_dir, "health.json");
        File.WriteAllText(path, "{not json!!");

        var state = new HealthStore().Load(path);

        Assert.Equal(0, state.ConsecutiveFailures);
        Assert.Null(state.LastSuccessTime);
    }
}
