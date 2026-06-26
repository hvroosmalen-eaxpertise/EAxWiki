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
}
