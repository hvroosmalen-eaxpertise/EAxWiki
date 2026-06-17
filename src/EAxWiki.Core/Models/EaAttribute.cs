namespace EAxWiki.Core.Models;

public class EaAttribute
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? DefaultValue { get; set; }
}
