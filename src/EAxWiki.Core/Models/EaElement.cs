namespace EAxWiki.Core.Models;

public class EaElement
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Stereotype { get; set; } = string.Empty;
    public string? StereotypeEx { get; set; }
    public string? FQStereotype { get; set; }
    public string? Notes { get; set; }
    public int PackageId { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<EaAttribute> Attributes { get; set; } = new();
    public List<EaMethod> Methods { get; set; } = new();
    public List<EaTaggedValue> TaggedValues { get; set; } = new();
    public List<EaConnector> Connectors { get; set; } = new();
}
