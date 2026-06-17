namespace EAxWiki.Core.Models;

public class EaDiagram
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PackageId { get; set; }
    public List<EaDiagramObject> DiagramObjects { get; set; } = new();
}
