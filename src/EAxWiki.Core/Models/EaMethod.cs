namespace EAxWiki.Core.Models;

public class EaMethod
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsStatic { get; set; }
}
