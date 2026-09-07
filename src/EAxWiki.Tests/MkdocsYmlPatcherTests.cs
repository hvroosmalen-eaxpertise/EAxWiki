using EAxWiki;

namespace EAxWiki.Tests;

public class MkdocsYmlPatcherTests : IDisposable
{
    private readonly string _path;

    public MkdocsYmlPatcherTests()
    {
        _path = Path.Combine(Path.GetTempPath(), "eaxwiki_mkdocs_" + Guid.NewGuid().ToString("N") + ".yml");
    }

    public void Dispose()
    {
        if (File.Exists(_path)) File.Delete(_path);
    }

    private void Write(params string[] lines) => File.WriteAllLines(_path, lines);

    [Fact]
    public void SetLogo_WhenAbsent_InsertsUnderThemeBlock()
    {
        Write(
            "site_name: Wiki",
            "theme:",
            "  name: material",
            "  features:",
            "    - navigation.instant",
            "",
            "extra_css:",
            "  - extra.css");

        MkdocsYmlPatcher.SetLogo(_path, "assets/logo.png");

        var lines = File.ReadAllLines(_path);
        Assert.Contains(lines, l => l.TrimStart() == "logo: assets/logo.png");
        // Under the theme: block, before the next zero-indent key
        var themeIdx = Array.FindIndex(lines, l => l.StartsWith("theme:"));
        var extraIdx = Array.FindIndex(lines, l => l.StartsWith("extra_css:"));
        var logoIdx = Array.FindIndex(lines, l => l.TrimStart().StartsWith("logo:"));
        Assert.True(logoIdx > themeIdx && logoIdx < extraIdx);
    }

    [Fact]
    public void SetLogo_WhenPresent_ReplacesValue()
    {
        Write(
            "theme:",
            "  name: material",
            "  logo: assets/old.png",
            "  features:",
            "    - navigation.instant");

        MkdocsYmlPatcher.SetLogo(_path, "assets/new.png");

        var lines = File.ReadAllLines(_path);
        Assert.Contains(lines, l => l.TrimStart() == "logo: assets/new.png");
        Assert.DoesNotContain(lines, l => l.Contains("assets/old.png"));
    }

    [Fact]
    public void SetLogo_IgnoresCommentedLogoLine_AndInserts()
    {
        Write(
            "theme:",
            "  name: material",
            "  # logo: assets/old.png",
            "  features:",
            "    - navigation.instant");

        MkdocsYmlPatcher.SetLogo(_path, "assets/new.png");

        var lines = File.ReadAllLines(_path);
        Assert.Contains(lines, l => l.TrimStart() == "logo: assets/new.png");
        Assert.Contains(lines, l => l.Contains("# logo: assets/old.png"));
    }

    [Fact]
    public void RemoveLogo_WhenPresent_RemovesLine()
    {
        Write(
            "theme:",
            "  name: material",
            "  logo: assets/x.png",
            "  features:",
            "    - navigation.instant");

        MkdocsYmlPatcher.RemoveLogo(_path);

        var lines = File.ReadAllLines(_path);
        Assert.DoesNotContain(lines, l => l.TrimStart().StartsWith("logo:"));
    }

    [Fact]
    public void RemoveLogo_WhenAbsent_IsNoop()
    {
        Write(
            "theme:",
            "  name: material",
            "  features:",
            "    - navigation.instant");
        var before = File.ReadAllText(_path);

        MkdocsYmlPatcher.RemoveLogo(_path);

        Assert.Equal(before, File.ReadAllText(_path));
    }

    [Fact]
    public void GetLogo_ReturnsCurrentValue()
    {
        Write(
            "theme:",
            "  name: material",
            "  logo: assets/hello.png",
            "  features: []");

        Assert.Equal("assets/hello.png", MkdocsYmlPatcher.GetLogo(_path));
    }

    [Fact]
    public void GetLogo_WhenAbsent_ReturnsNull()
    {
        Write(
            "theme:",
            "  name: material",
            "  features: []");

        Assert.Null(MkdocsYmlPatcher.GetLogo(_path));
    }

    [Fact]
    public void SetLogo_ThrowsIfNoThemeBlock()
    {
        Write("site_name: Wiki");
        Assert.Throws<InvalidOperationException>(() => MkdocsYmlPatcher.SetLogo(_path, "assets/x.png"));
    }
}
