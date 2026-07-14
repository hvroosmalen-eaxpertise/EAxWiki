namespace EAxWiki.Core.Models;

public record EaElementSummary
{
    public int ElementId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Stereotype { get; init; } = string.Empty;
    public string PackagePath { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public List<AttributeInfo> Attributes { get; init; } = [];
    public List<MethodInfo> Methods { get; init; } = [];
    public List<TaggedValueInfo> TaggedValues { get; init; } = [];
    public List<RelationshipInfo> Relationships { get; init; } = [];
}

public record AttributeInfo(string Name, string Type);
public record MethodInfo(string Name, string ReturnType, bool IsStatic);
public record TaggedValueInfo(string Name, string Value);
public record RelationshipInfo(string Type, string Direction, string TargetName, string TargetType, string TargetStereotype, string? TargetNotes, string ConnectorStereotype);

public record EaDiagramSummary
{
    public int DiagramId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public List<DiagramElementInfo> Elements { get; init; } = [];
}

public record DiagramElementInfo(string Name, string Type, string Stereotype, string? Notes);
