using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.Tests.TestDoubles;

public class FakeEaReader : IEaReader
{
    public List<(int Id, string Status)> StatusUpdates { get; } = [];
    public List<(int Id, string Notes)> ElementNotesUpdates { get; } = [];
    public List<(int Id, string Notes)> DiagramNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string Type, string Notes)> AttributeNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string ReturnType, bool IsStatic, string Notes)> MethodNotesUpdates { get; } = [];
    public List<(int ElementId, string Name, string Value, string Notes)> TaggedValueNotesUpdates { get; } = [];

    public EaRepository Open(string connectionString, CancellationToken ct = default) => new();
    public bool TestConnection(string connectionString, out string? error) { error = null; return true; }
    public void Close() { }
    public bool ExportDiagramImage(string diagramGuid, string filePath) => true;
    public EaElementSummary? GetElementSummary(int elementId) => null;
    public string RepositoryPath => string.Empty;
    public IReadOnlyList<string> GetStatusTypes() => ["Approved", "Implemented", "Mandatory", "Proposed", "Validated"];
    public string GetElementStatus(int elementId) => "Proposed";

    public void UpdateElementStatus(int elementId, string newStatus) => StatusUpdates.Add((elementId, newStatus));
    public void UpdateElementNotes(int elementId, string newNotesHtml) => ElementNotesUpdates.Add((elementId, newNotesHtml));
    public void UpdateDiagramNotes(int diagramId, string newNotesHtml) => DiagramNotesUpdates.Add((diagramId, newNotesHtml));
    public void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml) =>
        AttributeNotesUpdates.Add((elementId, attributeName, attributeType, newNotesHtml));
    public void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml) =>
        MethodNotesUpdates.Add((elementId, methodName, returnType, isStatic, newNotesHtml));
    public void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml) =>
        TaggedValueNotesUpdates.Add((elementId, tagName, tagValue, newNotesHtml));
}
