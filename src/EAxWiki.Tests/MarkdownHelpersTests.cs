using EAxWiki.Export.Helpers;

namespace EAxWiki.Tests;

public class MarkdownHelpersTests
{
    public MarkdownHelpersTests() => MarkdownHelpers.ClearCache();

    // --- SanitizeName ---

    [Fact]
    public void SanitizeName_PlainName_ReturnsUnchanged()
        => Assert.Equal("MyPackage", MarkdownHelpers.SanitizeName("MyPackage"));

    [Fact]
    public void SanitizeName_TrimsWhitespace()
        => Assert.Equal("Name", MarkdownHelpers.SanitizeName("  Name  "));

    [Fact]
    public void SanitizeName_InvalidFileCharsReplaced()
    {
        var result = MarkdownHelpers.SanitizeName("My<Package>Name");
        Assert.DoesNotContain('<', result);
        Assert.DoesNotContain('>', result);
    }

    [Fact]
    public void SanitizeName_HashReplacedWithUnderscore()
        => Assert.Equal("C_", MarkdownHelpers.SanitizeName("C#"));

    [Fact]
    public void SanitizeName_EmptyString_ReturnsUnnamed()
        => Assert.Equal("unnamed", MarkdownHelpers.SanitizeName(""));

    [Fact]
    public void SanitizeName_WhitespaceOnly_ReturnsUnnamed()
        => Assert.Equal("unnamed", MarkdownHelpers.SanitizeName("   "));

    [Fact]
    public void SanitizeName_CachesResult()
    {
        var first = MarkdownHelpers.SanitizeName("Test");
        var second = MarkdownHelpers.SanitizeName("Test");
        Assert.Same(first, second);
    }

    // --- EscapeCell ---

    [Fact]
    public void EscapeCell_NoPipes_ReturnsUnchanged()
        => Assert.Equal("hello", MarkdownHelpers.EscapeCell("hello"));

    [Fact]
    public void EscapeCell_EscapesPipe()
        => Assert.Equal(@"a \| b", MarkdownHelpers.EscapeCell("a | b"));

    [Fact]
    public void EscapeCell_MultiplePipes()
        => Assert.Equal(@"\|\|", MarkdownHelpers.EscapeCell("||"));

    [Fact]
    public void EscapeCell_NewlineReplacedWithSpace()
    {
        var result = MarkdownHelpers.EscapeCell("line1\nline2");
        Assert.DoesNotContain('\n', result);
    }

    [Fact]
    public void EscapeCell_CrlfReplacedWithSpace()
    {
        var result = MarkdownHelpers.EscapeCell("line1\r\nline2");
        Assert.DoesNotContain('\r', result);
        Assert.DoesNotContain('\n', result);
    }

    // --- ParseStereotype ---

    [Fact]
    public void ParseStereotype_Empty_ReturnsUmlUncategorized()
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype("");
        Assert.Equal("UML", lang);
        Assert.Equal("Uncategorized", type);
    }

    [Fact]
    public void ParseStereotype_Whitespace_ReturnsUmlUncategorized()
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype("   ");
        Assert.Equal("UML", lang);
        Assert.Equal("Uncategorized", type);
    }

    [Fact]
    public void ParseStereotype_DoubleColonSeparator()
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype("ArchiMate::ApplicationComponent");
        Assert.Equal("ArchiMate", lang);
        Assert.Equal("ApplicationComponent", type);
    }

    [Fact]
    public void ParseStereotype_UnderscoreSeparatorWithLanguageName()
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype("ESRS_Disclosure");
        Assert.Equal("ESRS", lang);
        Assert.Equal("Disclosure", type);
    }

    [Fact]
    public void ParseStereotype_NoSeparator_LanguageIsUml()
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype("SomeStereotype");
        Assert.Equal("UML", lang);
        Assert.Equal("SomeStereotype", type);
    }

    [Fact]
    public void ParseStereotype_StripsLanguagePrefixFromType()
    {
        // ArchiMate::ArchiMate_ApplicationComponent => type should strip ArchiMate_ prefix
        var (lang, type) = MarkdownHelpers.ParseStereotype("ArchiMate::ArchiMate_ApplicationComponent");
        Assert.Equal("ArchiMate", lang);
        Assert.Equal("ApplicationComponent", type);
    }

    [Fact]
    public void ParseStereotype_LeadingLowercaseUnderscore_FallsBackToUml()
    {
        // underscore at position 0 or lowercase prefix — not a language name
        var (lang, type) = MarkdownHelpers.ParseStereotype("_private");
        Assert.Equal("UML", lang);
        Assert.Equal("_private", type);
    }
}
