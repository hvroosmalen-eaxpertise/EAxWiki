using System.Threading;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.Export;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

public class PipelineResilienceTests
{
    /// <summary>
    /// An IOutputWriter that can be configured to fail on specific file base names,
    /// to block until a signal is received, and to record whether cancellation was observed.
    /// </summary>
    private sealed class MockWriter : IOutputWriter
    {
        public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
        public readonly HashSet<string> Directories = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> ThrowForFiles { get; } = new(StringComparer.OrdinalIgnoreCase);
        public TaskCompletionSource? WriteBlocker { get; set; }
        public bool CancellationObserved { get; private set; }

        public Task CreateDirectoryAsync(string path, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Directories.Add(Normalize(path));
            return Task.CompletedTask;
        }

        public async Task WriteFileAsync(string filePath, string content, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            CancellationObserved = true;

            if (WriteBlocker != null)
                await WriteBlocker.Task.WaitAsync(ct);

            if (ThrowForFiles.Count > 0)
            {
                var fileName = Path.GetFileName(filePath);
                if (ThrowForFiles.Contains(fileName))
                    throw new InvalidOperationException($"Simulated write failure: {filePath}");
            }

            Files[Normalize(filePath)] = content;
        }

        public bool WasFileWritten(string path) => Files.ContainsKey(Normalize(path));
        public string? ReadFile(string path) => Files.GetValueOrDefault(Normalize(path));

        private static string Normalize(string path) => path.Replace('\\', '/');
    }

    private static EaRepository Repo(string pkgName, params string[] elementNames)
    {
        var repo = new EaRepository();
        var pkg = new EaPackage { Id = 10, Name = pkgName };
        for (int i = 0; i < elementNames.Length; i++)
            pkg.Elements.Add(new EaElement { Id = i + 1, Name = elementNames[i], Type = "Class", ModifiedDate = DateTime.UtcNow });
        repo.RootPackages.Add(pkg);
        return repo;
    }

    private static string TempPath() =>
        Path.Combine(Path.GetTempPath(), "eaxwiki_pipeline_" + Guid.NewGuid().ToString("N"));

    // ── ExportResult tests ──────────────────────────────────────────────────────

    [Fact]
    public async Task ExportResult_ReturnsCorrectTotalCount()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B", "C");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(3, result.TotalElements);
    }

    [Fact]
    public async Task ExportResult_ShowsZeroFailuresOnCleanExport()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(0, result.FailedElements);
        Assert.Equal(2, result.SucceededElements);
    }

    [Fact]
    public async Task ExportResult_ElapsedTimeGreaterThanZero()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.True(result.Elapsed.TotalMilliseconds > 0, "Elapsed time should be > 0");
    }

    [Fact]
    public async Task ExportAsync_CancelledToken_ThrowsOperationCanceledException()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");
        var outPath = TempPath();
        var cancelledCt = new CancellationToken(canceled: true);

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            exporter.ExportAsync(repo, null, outPath, cancellationToken: cancelledCt));
    }

    [Fact]
    public async Task ExportAsync_PreCancelledToken_DoesNotProcess()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");
        var outPath = TempPath();
        var cancelledCt = new CancellationToken(canceled: true);

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            exporter.ExportAsync(repo, null, outPath, cancellationToken: cancelledCt));

        Assert.Empty(writer.Files);
    }

    // ── Error isolation tests ───────────────────────────────────────────────────

    [Fact]
    public async Task ExportAsync_ElementFailure_ContinuesPackageExport()
    {
        var writer = new MockWriter();
        writer.ThrowForFiles.Add("B.md");
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B", "C");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "A.md")),
            "Element A should have been written");
        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "C.md")),
            "Element C should have been written despite B's failure");
        Assert.False(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "B.md")),
            "Element B should have failed");
    }

    [Fact]
    public async Task ExportAsync_ElementFailure_CountedInExportResult()
    {
        var writer = new MockWriter();
        writer.ThrowForFiles.Add("B.md");
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B", "C");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(3, result.TotalElements);
        Assert.Equal(1, result.FailedElements);
        Assert.Equal(2, result.SucceededElements);
    }

    [Fact]
    public async Task ExportAsync_MultipleElementFailures_AllCounted()
    {
        var writer = new MockWriter();
        writer.ThrowForFiles.Add("A.md");
        writer.ThrowForFiles.Add("B.md");
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(2, result.TotalElements);
        Assert.Equal(2, result.FailedElements);
        Assert.Equal(0, result.SucceededElements);
    }

    [Fact]
    public async Task ExportAsync_ElementFailure_IndexStillWritten()
    {
        var writer = new MockWriter();
        writer.ThrowForFiles.Add("A.md");
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");
        var outPath = TempPath();

        await exporter.ExportAsync(repo, null, outPath);

        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "index.md")),
            "Package index should be written despite element failures");
    }

    // ── Cancellation during processing ──────────────────────────────────────────

    [Fact]
    public async Task ExportAsync_CancellationDuringElementProcessing_Aborts()
    {
        var writer = new MockWriter();
        var blocker = new TaskCompletionSource();
        writer.WriteBlocker = blocker;
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B", "C");
        var outPath = TempPath();
        using var cts = new CancellationTokenSource();

        var task = exporter.ExportAsync(repo, null, outPath, cancellationToken: cts.Token);

        cts.Cancel();
        blocker.TrySetResult();

        await Assert.ThrowsAsync<OperationCanceledException>(() => task);
    }

    // ── Force mode ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task ForceMode_SafeDeleteContents_DoesNotCrash()
    {
        var outPath = TempPath();
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A");

        await exporter.ExportAsync(repo, null, outPath);
        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "A.md")));

        var writer2 = new MockWriter();
        var exporter2 = new MarkdownExporter(writer2, NullLogger<MarkdownExporter>.Instance);
        var result = await exporter2.ExportAsync(repo, null, outPath, force: true);

        Assert.Equal(1, result.TotalElements);
        Assert.Equal(0, result.FailedElements);
    }

    [Fact]
    public async Task NonForceMode_PreservesExistingFiles()
    {
        var outPath = TempPath();
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A");

        await exporter.ExportAsync(repo, null, outPath);

        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "Pkg", "A.md")));
    }

    // ── CancellationToken threading ─────────────────────────────────────────────

    [Fact]
    public async Task ExportAsync_CancellationToken_ReachesWriter()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A");
        var outPath = TempPath();

        await exporter.ExportAsync(repo, null, outPath);

        Assert.True(writer.CancellationObserved,
            "CancellationToken should be passed to IOutputWriter.WriteFileAsync");
    }

    [Fact]
    public async Task ExportAsync_EmptyPackage_ReturnsZeroFailures()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Empty");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(0, result.TotalElements);
        Assert.Equal(0, result.FailedElements);
        Assert.Equal(0, result.SucceededElements);
    }

    [Fact]
    public async Task ExportAsync_SingleElement_ReturnsOneSuccess()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "Only");
        var outPath = TempPath();

        var result = await exporter.ExportAsync(repo, null, outPath);

        Assert.Equal(1, result.TotalElements);
        Assert.Equal(1, result.SucceededElements);
        Assert.Equal(0, result.FailedElements);
    }

    [Fact]
    public async Task ExportAsync_RootIndex_AlwaysWritten()
    {
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A");
        var outPath = TempPath();

        await exporter.ExportAsync(repo, null, outPath);

        Assert.True(writer.WasFileWritten(Path.Combine(outPath, "index.md")),
            "Root index.md should always be written");
    }

    [Fact]
    public async Task ExportResult_ForceMode_ReturnedCorrectly()
    {
        var outPath = TempPath();
        var writer = new MockWriter();
        var exporter = new MarkdownExporter(writer, NullLogger<MarkdownExporter>.Instance);
        var repo = Repo("Pkg", "A", "B");

        await exporter.ExportAsync(repo, null, outPath, force: true);

        var writer2 = new MockWriter();
        var exporter2 = new MarkdownExporter(writer2, NullLogger<MarkdownExporter>.Instance);
        var result = await exporter2.ExportAsync(repo, null, outPath, force: true);

        Assert.Equal(2, result.TotalElements);
        Assert.Equal(0, result.FailedElements);
    }
}
