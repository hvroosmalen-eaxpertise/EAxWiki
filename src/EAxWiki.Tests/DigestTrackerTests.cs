using EAxWiki.Core.Monitoring;
using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class DigestTrackerTests : IDisposable
{
    private readonly string _dir;

    public DigestTrackerTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "eaxwiki_digest_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private DigestTracker CreateTracker(HealthState state, out string logDir, out string wikiDir)
    {
        logDir = Path.Combine(_dir, "logs");
        Directory.CreateDirectory(logDir);
        wikiDir = Path.Combine(_dir, "wiki");
        Directory.CreateDirectory(wikiDir);
        File.WriteAllText(Path.Combine(_dir, "digest-template.md"),
            "Activity for @@DIGEST_DATE@@: ~@@PAGE_READS_TODAY@@ reads, @@WRITEBACKS_TODAY@@ write-backs.");
        return new DigestTracker(state, wikiDir, logDir, Path.Combine(_dir, "digest-template.md"));
    }

    [Fact]
    public void CountNewPageReads_CountsOnlyNewTextAndSkipsReconnects()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out var logDir, out _);
        var logPath = Path.Combine(logDir, "serve-20260801_120000.err.log");
        File.WriteAllText(logPath, """
            [10:00:00] Reloading browsers...
            [10:00:05] Browser connected: http://localhost:8000/some/page  # within 10s of reload → skip
            [10:00:30] Browser connected: http://localhost:8000/other        # real read
            """);

        Assert.Equal(1, tracker.CountNewPageReads());
        Assert.Equal(0, tracker.CountNewPageReads()); // no new bytes → 0 new

        // Append a real read; the offset must have advanced.
        File.AppendAllText(logPath, "[10:01:00] Browser connected: http://localhost:8000/third\n");
        Assert.Equal(1, tracker.CountNewPageReads());
    }

    [Fact]
    public void CountNewPageReads_NewLogFile_ResetsOffset()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out var logDir, out _);
        var first = Path.Combine(logDir, "serve-20260801_120000.err.log");
        File.WriteAllText(first, "[10:00:00] Browser connected: http://localhost:8000/a\n");
        Assert.Equal(1, tracker.CountNewPageReads());

        var second = Path.Combine(logDir, "serve-20260802_090000.err.log");
        File.WriteAllText(second, "[09:00:00] Browser connected: http://localhost:8000/b\n");
        Assert.Equal(1, tracker.CountNewPageReads());
    }

    [Fact]
    public void CountNewWritebacks_CountsPerKind()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out _, out var wikiDir);
        var statusDir = Path.Combine(wikiDir, "status");
        Directory.CreateDirectory(statusDir);
        File.WriteAllText(Path.Combine(statusDir, "writeback.log"),
            "2026-08-01 10:00:00 status\n2026-08-01 10:01:00 notes\n2026-08-01 10:02:00 status\n");

        var delta = tracker.CountNewWritebacks();

        Assert.Equal(3, delta.Total);
        Assert.Equal(2, delta.Kinds["status"]);
        Assert.Equal(1, delta.Kinds["notes"]);
    }

    [Fact]
    public void MaybeComposeDailyDigest_FirstRunNoAlert_SetsDate()
    {
        var state = new HealthState();
        var tracker = CreateTracker(state, out _, out _);
        Assert.Null(tracker.MaybeComposeDailyDigest(new DateTime(2026, 8, 1, 10, 0, 0, DateTimeKind.Local)));
        Assert.Equal("2026-08-01", state.LastDigestDate);
    }

    [Fact]
    public void MaybeComposeDailyDigest_DayBoundary_ComposesAndResets()
    {
        var state = new HealthState { LastDigestDate = "2026-07-31", PageReadsToday = 5, WritebacksToday = 2 };
        var tracker = CreateTracker(state, out _, out _);

        var message = tracker.MaybeComposeDailyDigest(new DateTime(2026, 8, 1, 0, 30, 0, DateTimeKind.Local));

        Assert.NotNull(message);
        Assert.Contains("Activity for 2026-07-31: ~5 reads, 2 write-backs.", message);
        Assert.Equal(0, state.PageReadsToday);
        Assert.Equal(0, state.WritebacksToday);
        Assert.Equal("2026-08-01", state.LastDigestDate);
    }
}
