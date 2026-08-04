using EAxWiki.Core.Models;
using EAxWiki.Export;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

/// <summary>
/// Verifies that orphaned files and directories are removed after structural EA model changes
/// (package renames, element moves, element/package deletions).
/// These tests use the real filesystem because cleanup operates directly on disk.
/// </summary>
public class CleanupTests : IDisposable
{
    private readonly string _outPath;

    public CleanupTests()
    {
        _outPath = Path.Combine(Path.GetTempPath(), "eaxwiki_cleanup_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_outPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_outPath))
            Directory.Delete(_outPath, recursive: true);
    }

    private MarkdownExporter CreateExporter() =>
        new(new FileOutputWriter(), NullLogger<MarkdownExporter>.Instance);

    private static EaRepository Repo(params (string pkg, string[] elems)[] packages)
    {
        var repo = new EaRepository();
        var elemId = 1;
        var pkgId = 10;
        foreach (var (pkg, elems) in packages)
        {
            var package = new EaPackage { Id = pkgId++, Name = pkg };
            foreach (var e in elems)
                package.Elements.Add(new EaElement { Id = elemId++, Name = e, Type = "Class", ModifiedDate = DateTime.UtcNow });
            repo.RootPackages.Add(package);
        }
        return repo;
    }

    // ── Package rename ─────────────────────────────────────────────────────────

    [Fact]
    public async Task RenamePackage_OldFolderIsRemoved()
    {
        await CreateExporter().ExportAsync(Repo(("OldName", ["Elem"])), null, _outPath);
        Assert.True(Directory.Exists(Path.Combine(_outPath, "OldName")));

        await CreateExporter().ExportAsync(Repo(("NewName", ["Elem"])), null, _outPath);

        Assert.False(Directory.Exists(Path.Combine(_outPath, "OldName")),
            "Old package folder should be deleted after rename.");
    }

    [Fact]
    public async Task RenamePackage_NewFolderIsCreated()
    {
        await CreateExporter().ExportAsync(Repo(("OldName", ["Elem"])), null, _outPath);

        await CreateExporter().ExportAsync(Repo(("NewName", ["Elem"])), null, _outPath);

        Assert.True(Directory.Exists(Path.Combine(_outPath, "NewName")),
            "New package folder should exist after rename.");
        Assert.True(File.Exists(Path.Combine(_outPath, "NewName", "Elem.md")),
            "Element page should exist in new package folder.");
    }

    // ── Element move ────────────────────────────────────────────────────────────

    [Fact]
    public async Task MoveElement_OldFileIsRemoved()
    {
        await CreateExporter().ExportAsync(Repo(("PkgA", ["MovedElem"]), ("PkgB", [])), null, _outPath);
        Assert.True(File.Exists(Path.Combine(_outPath, "PkgA", "MovedElem.md")));

        // Move element from PkgA to PkgB
        await CreateExporter().ExportAsync(Repo(("PkgA", []), ("PkgB", ["MovedElem"])), null, _outPath);

        Assert.False(File.Exists(Path.Combine(_outPath, "PkgA", "MovedElem.md")),
            "Element file should be removed from old package after move.");
    }

    [Fact]
    public async Task MoveElement_NewFileIsWritten()
    {
        await CreateExporter().ExportAsync(Repo(("PkgA", ["MovedElem"]), ("PkgB", [])), null, _outPath);

        await CreateExporter().ExportAsync(Repo(("PkgA", []), ("PkgB", ["MovedElem"])), null, _outPath);

        Assert.True(File.Exists(Path.Combine(_outPath, "PkgB", "MovedElem.md")),
            "Element file should be created in new package after move.");
    }

    [Fact]
    public async Task MoveElement_OldPackageFolderRemainsIfNotEmpty()
    {
        await CreateExporter().ExportAsync(Repo(("PkgA", ["MovedElem", "StaysElem"]), ("PkgB", [])), null, _outPath);

        await CreateExporter().ExportAsync(Repo(("PkgA", ["StaysElem"]), ("PkgB", ["MovedElem"])), null, _outPath);

        Assert.True(Directory.Exists(Path.Combine(_outPath, "PkgA")),
            "Old package folder should remain because it still has elements.");
        Assert.True(File.Exists(Path.Combine(_outPath, "PkgA", "StaysElem.md")));
    }

    // ── Element deletion ────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteElement_FileIsRemoved()
    {
        await CreateExporter().ExportAsync(Repo(("Pkg", ["Keep", "Delete"])), null, _outPath);
        Assert.True(File.Exists(Path.Combine(_outPath, "Pkg", "Delete.md")));

        await CreateExporter().ExportAsync(Repo(("Pkg", ["Keep"])), null, _outPath);

        Assert.False(File.Exists(Path.Combine(_outPath, "Pkg", "Delete.md")),
            "Deleted element's file should be removed.");
        Assert.True(File.Exists(Path.Combine(_outPath, "Pkg", "Keep.md")),
            "Remaining element's file should still exist.");
    }

    // ── Package deletion ────────────────────────────────────────────────────────

    [Fact]
    public async Task DeletePackage_FolderIsRemoved()
    {
        await CreateExporter().ExportAsync(Repo(("Keep", ["E1"]), ("Delete", ["E2"])), null, _outPath);
        Assert.True(Directory.Exists(Path.Combine(_outPath, "Delete")));

        await CreateExporter().ExportAsync(Repo(("Keep", ["E1"])), null, _outPath);

        Assert.False(Directory.Exists(Path.Combine(_outPath, "Delete")),
            "Deleted package folder should be removed.");
        Assert.True(Directory.Exists(Path.Combine(_outPath, "Keep")),
            "Remaining package folder should still exist.");
    }

    // ── Special dirs are never touched ─────────────────────────────────────────

    [Fact]
    public async Task SpecialDirs_AreNeverDeleted()
    {
        await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath);

        // Run again with a different package — special dirs must survive.
        await CreateExporter().ExportAsync(Repo(("OtherPkg", ["Elem"])), null, _outPath);

        foreach (var special in new[] { "diagrams", "types", "glossary", "recent" })
            Assert.True(Directory.Exists(Path.Combine(_outPath, special)),
                $"Special dir '{special}' should never be deleted.");
    }

    // ── Byte-stable full exports ───────────────────────────────────────────────

    [Fact]
    public async Task FullForceExport_PreservesApiToken()
    {
        const string token = "abcdef0123456789abcdef0123456789abcdef01";
        var tokenPath = Path.Combine(_outPath, ".eaxwiki-token");
        File.WriteAllText(tokenPath, token);

        var oldPort = Environment.GetEnvironmentVariable("EAXWIKI_API_PORT");
        Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", "18999");
        try
        {
            await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath, force: true);
        }
        finally
        {
            Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", oldPort);
        }

        Assert.True(File.Exists(tokenPath), "write-back token should survive a --force export");
        Assert.Equal(token, File.ReadAllText(tokenPath).Trim());
    }

    [Fact]
    public async Task FullForceExport_TokenStableAcrossRuns()
    {
        var oldPort = Environment.GetEnvironmentVariable("EAXWIKI_API_PORT");
        Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", "18999");
        try
        {
            await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath, force: true);
            var first = File.ReadAllText(Path.Combine(_outPath, ".eaxwiki-token")).Trim();
            await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath, force: true);
            var second = File.ReadAllText(Path.Combine(_outPath, ".eaxwiki-token")).Trim();
            Assert.Equal(first, second);
        }
        finally
        {
            Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", oldPort);
        }
    }

    [Fact]
    public async Task FullForceExport_WritesGeneratedMarker()
    {
        await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath, force: true);

        var marker = Path.Combine(_outPath, "status", ".generated");
        Assert.True(File.Exists(marker), "status/.generated marker should be written");
        Assert.False(string.IsNullOrWhiteSpace(File.ReadAllText(marker)));
    }

    [Fact]
    public async Task FullForceExport_OutputHasNoGeneratedFooter()
    {
        await CreateExporter().ExportAsync(Repo(("Pkg", ["Elem"])), null, _outPath, force: true);

        foreach (var file in Directory.EnumerateFiles(_outPath, "*.md", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(file);
            Assert.DoesNotContain("*Generated:", content);
        }
    }

    [Fact]
    public async Task FullForceExport_ByteStableAcrossRuns()
    {
        var repo = Repo(("Pkg", ["Elem"]));
        await CreateExporter().ExportAsync(repo, null, _outPath, force: true);
        var first = Snapshot();

        await CreateExporter().ExportAsync(repo, null, _outPath, force: true);
        var second = Snapshot();

        Assert.Equal(first, second);
    }

    private List<(string Path, string Bytes)> Snapshot()
    {
        var entries = new List<(string Path, string Bytes)>();
        foreach (var file in Directory.EnumerateFiles(_outPath, "*", SearchOption.AllDirectories))
        {
            // The .generated marker records the generation time on purpose and is gitignored,
            // so it is exempt from the byte-stability guarantee.
            if (Path.GetFileName(file).Equals(".generated", StringComparison.Ordinal)) continue;
            var rel = Path.GetRelativePath(_outPath, file).Replace('\\', '/');
            entries.Add((rel, Convert.ToBase64String(File.ReadAllBytes(file))));
        }
        return entries.OrderBy(e => e.Path, StringComparer.Ordinal).ToList();
    }
}
