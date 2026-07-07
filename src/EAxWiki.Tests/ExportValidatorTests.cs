using EAxWiki.Export;

namespace EAxWiki.Tests;

public class ExportValidatorTests
{
    [Fact]
    public void Validate_EmptyFile_ReturnsError()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "empty.md");
        File.WriteAllText(file, "");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Errors);
        Assert.Equal(0, report.Passed);
        Assert.Contains(report.Issues, i => i.Message == "File is empty");
    }

    [Fact]
    public void Validate_ValidElementPage_ReturnsPassed()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "valid.md");
        File.WriteAllText(file, @"---
ea_id: 123
status: Approved
status_option: Approved
ea_hash: abc
notes_hash: def
---
# Title
Some content.");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(0, report.Errors);
        Assert.Equal(0, report.Warnings);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void Validate_NoFrontmatter_NoError()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "simple.md");
        File.WriteAllText(file, "# Just a heading\nSome content.");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(0, report.Errors);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void Validate_UnclosedDiv_ReturnsError()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "unclosed.md");
        File.WriteAllText(file, "# Title\n<div>unclosed content");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Errors);
        Assert.Contains(report.Issues, i => i.Message.Contains("div"));
    }

    [Fact]
    public void Validate_BrokenLink_ReturnsWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "links.md");
        File.WriteAllText(file, "# Title\nSee [missing](nonexistent.md) for details.");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Warnings);
        Assert.Contains(report.Issues, i => i.Message.Contains("nonexistent.md"));
    }

    [Fact]
    public void Validate_MissingImage_ReturnsWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "img.md");
        File.WriteAllText(file, @"# Title
<img src=""missing.png"" alt=""test"">");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(1, report.Warnings);
        Assert.Contains(report.Issues, i => i.Message.Contains("missing.png"));
    }

    [Fact]
    public void Validate_ExistingImage_NoWarning()
    {
        using var dir = new TempDir();
        var file = Path.Combine(dir.Path, "img.md");
        File.WriteAllText(file, @"# Title
<img src=""present.png"" alt=""test"">");
        File.WriteAllText(Path.Combine(dir.Path, "present.png"), "fake-png");

        var report = ExportValidator.Validate(new[] { file }, dir.Path);

        Assert.Equal(0, report.Warnings);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void Validate_MultipleFiles_AllChecks()
    {
        using var dir = new TempDir();
        var good = Path.Combine(dir.Path, "good.md");
        File.WriteAllText(good, "# Good\n<p>ok</p>");
        var bad = Path.Combine(dir.Path, "bad.md");
        File.WriteAllText(bad, "# Bad\n<div>unclosed");

        var report = ExportValidator.Validate(new[] { good, bad }, dir.Path);

        Assert.Equal(1, report.Errors);
        Assert.Equal(1, report.Passed);
    }

    [Fact]
    public void ToJson_ProducesValidJson()
    {
        var report = new ValidationReport(10, 8, 1, 1,
            new List<ValidationIssue> { new("test.md", "error", "Unclosed <div> tag") });

        var json = ExportValidator.ToJson(report);
        Assert.Contains("\"FilesValidated\": 10", json);
        Assert.Contains("\"Errors\": 1", json);
        Assert.Contains("\"Warnings\": 1", json);
    }
}

internal sealed class TempDir : IDisposable
{
    public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
    public TempDir() => Directory.CreateDirectory(Path);
    public void Dispose()
    {
        try { Directory.Delete(Path, true); } catch { }
    }
}
