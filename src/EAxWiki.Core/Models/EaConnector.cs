namespace EAxWiki.Core.Models;

public class EaConnector
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Stereotype { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int SourceId { get; set; }
    public int TargetId { get; set; }
}
