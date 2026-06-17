namespace EAxWiki.Core.Models;

public class EaPackage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int? ParentId { get; set; }
    public List<EaPackage> Children { get; set; } = new();
    public List<EaElement> Elements { get; set; } = new();
    public List<EaDiagram> Diagrams { get; set; } = new();
}
