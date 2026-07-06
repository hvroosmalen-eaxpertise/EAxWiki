using System.Reflection;
using System.Text.RegularExpressions;
using EAxWiki.Core.Models;
using EAxWiki.Export.Exporters;
using EAxWiki.Export.Helpers;
using EAxWiki.Export.Renderers;
using FsCheck;
using FsCheck.Xunit;

namespace EAxWiki.Tests;

public class PropertyBasedTests
{
    public PropertyBasedTests() => MarkdownHelpers.ClearCache();

    // --- SanitizeName ---

    [Property]
    public bool SanitizeName_NeverReturnsNull(string name)
    {
        var result = MarkdownHelpers.SanitizeName(name);
        return result != null;
    }

    [Property]
    public bool SanitizeName_NeverReturnsEmpty(string name)
    {
        var result = MarkdownHelpers.SanitizeName(name);
        return result.Length > 0;
    }

    [Property]
    public bool SanitizeName_ContainsNoInvalidChars(string name)
    {
        var result = MarkdownHelpers.SanitizeName(name);
        var invalid = Path.GetInvalidFileNameChars().Append('#').ToArray();
        return result.All(c => !invalid.Contains(c));
    }

    [Property]
    public bool SanitizeName_IsIdempotent(string name)
    {
        var first = MarkdownHelpers.SanitizeName(name);
        var second = MarkdownHelpers.SanitizeName(first);
        return first == second;
    }

    // --- EscapeCell ---

    [Property]
    public bool EscapeCell_NeverReturnsNull(string raw)
    {
        var result = MarkdownHelpers.EscapeCell(raw);
        return result != null;
    }

    [Property]
    public bool EscapeCell_NoUnescapedPipes(string raw)
    {
        var result = MarkdownHelpers.EscapeCell(raw);
        return !Regex.IsMatch(result, @"(?<!\\)\|");
    }

    [Property]
    public bool EscapeCell_NoNewlines(string raw)
    {
        var result = MarkdownHelpers.EscapeCell(raw);
        return !result.Contains('\n') && !result.Contains('\r');
    }

    [Property]
    public bool EscapeCell_LengthAtLeastInputLength(string raw)
    {
        var result = MarkdownHelpers.EscapeCell(raw);
        return result.Length >= raw.Length;
    }

    // --- ParseStereotype ---

    [Property]
    public bool ParseStereotype_NeverReturnsNull(string stereotype)
    {
        var (lang, type) = MarkdownHelpers.ParseStereotype(stereotype);
        return lang != null && type != null;
    }

    [Property]
    public bool ParseStereotype_EmptyOrWhitespace_ReturnsUmlUncategorized(string stereotype)
    {
        if (!string.IsNullOrWhiteSpace(stereotype))
            return true;
        var (lang, type) = MarkdownHelpers.ParseStereotype(stereotype);
        return lang == "UML" && type == "Uncategorized";
    }

    [Property]
    public bool ParseStereotype_DoubleColon_LanguageIsPrefix(NonEmptyString lang, string type)
    {
        if (lang.Item.Contains(':'))
            return true;
        var input = $"{lang.Item}::{type}";
        var (resultLang, _) = MarkdownHelpers.ParseStereotype(input);
        return resultLang == lang.Item;
    }

    // --- ComputeNotesHash ---

    [Property]
    public bool ComputeNotesHash_NeverReturnsNull(string? notes)
    {
        var hash = HtmlHelpers.ComputeNotesHash(notes);
        return hash != null;
    }

    [Property]
    public bool ComputeNotesHash_Returns8CharHex(string? notes)
    {
        var hash = HtmlHelpers.ComputeNotesHash(notes);
        return hash.Length == 8 && hash.All(Uri.IsHexDigit);
    }

    [Property]
    public bool ComputeNotesHash_Deterministic(string? notes)
    {
        return HtmlHelpers.ComputeNotesHash(notes) == HtmlHelpers.ComputeNotesHash(notes);
    }

    // --- ComputeStatusHash ---

    [Property]
    public bool ComputeStatusHash_NeverReturnsNull(string status)
    {
        var hash = HtmlHelpers.ComputeStatusHash(status);
        return hash != null;
    }

    [Property]
    public bool ComputeStatusHash_Returns8CharHex(string status)
    {
        var hash = HtmlHelpers.ComputeStatusHash(status);
        return hash.Length == 8 && hash.All(Uri.IsHexDigit);
    }

    [Property]
    public bool ComputeStatusHash_Deterministic(string status)
    {
        return HtmlHelpers.ComputeStatusHash(status) == HtmlHelpers.ComputeStatusHash(status);
    }

    // --- GetStereotypeLabel (random inputs) ---

    [Property]
    public bool GetStereotypeLabel_NeverReturnsNull(EaElement element)
    {
        var label = MarkdownHelpers.GetStereotypeLabel(element);
        return label != null;
    }

    [Property]
    public bool GetStereotypeLabel_ContainsSpanWithDataLayer(EaElement element)
    {
        var label = MarkdownHelpers.GetStereotypeLabel(element);
        return label.Contains("<span class=\"sl\"") && label.Contains("data-layer=");
    }

    // --- GetStereotypeLabel (exhaustive map keys) ---

    [Fact]
    public void AllArchiMateKeys_ProduceNonEmptyLabels()
    {
        var mapField = typeof(MarkdownHelpers).GetField("ArchiMateMap",
            BindingFlags.NonPublic | BindingFlags.Static);
        var map = (System.Collections.IDictionary)mapField!.GetValue(null)!;

        foreach (var key in map.Keys)
        {
            var stereotype = (string)key;
            var element = new EaElement
            {
                Name = "test",
                Stereotype = "",
                StereotypeEx = "",
                FQStereotype = stereotype
            };
            var label = MarkdownHelpers.GetStereotypeLabel(element);
            Assert.False(string.IsNullOrEmpty(label), $"Label for ArchiMate key '{stereotype}' should not be empty");
            Assert.Contains("data-layer=", label);
        }
    }

    [Fact]
    public void AllEdgyKeys_ProduceNonEmptyLabels()
    {
        var mapField = typeof(MarkdownHelpers).GetField("EdgyMap",
            BindingFlags.NonPublic | BindingFlags.Static);
        var map = (System.Collections.IDictionary)mapField!.GetValue(null)!;

        foreach (var key in map.Keys)
        {
            var stereotype = (string)key;
            var element = new EaElement
            {
                Name = "test",
                Stereotype = "",
                StereotypeEx = stereotype,
                FQStereotype = ""
            };
            var label = MarkdownHelpers.GetStereotypeLabel(element);
            Assert.False(string.IsNullOrEmpty(label), $"Label for EDGY key '{stereotype}' should not be empty");
            Assert.Contains("data-layer=", label);
        }
    }

    // --- SanitizeForAnchor ---

    [Property]
    public bool SanitizeForAnchor_NeverReturnsNull(string raw)
    {
        var result = StatusDashboardExporter.SanitizeForAnchor(raw);
        return result != null;
    }

    [Property]
    public bool SanitizeForAnchor_OnlyLowercaseLettersDigitsUnderscores(string raw)
    {
        var result = StatusDashboardExporter.SanitizeForAnchor(raw);
        return result.All(c => char.IsLetterOrDigit(c) || c == '_');
    }

    [Property]
    public bool SanitizeForAnchor_AllLowercase(string raw)
    {
        var result = StatusDashboardExporter.SanitizeForAnchor(raw);
        return result.All(c => !char.IsLetter(c) || char.IsLower(c));
    }

    [Property]
    public bool SanitizeForAnchor_SameLengthAsInput(string raw)
    {
        var result = StatusDashboardExporter.SanitizeForAnchor(raw);
        return result.Length == raw.Length;
    }

    [Property]
    public bool SanitizeForAnchor_IsIdempotent(string raw)
    {
        var first = StatusDashboardExporter.SanitizeForAnchor(raw);
        var second = StatusDashboardExporter.SanitizeForAnchor(first);
        return first == second;
    }
}
