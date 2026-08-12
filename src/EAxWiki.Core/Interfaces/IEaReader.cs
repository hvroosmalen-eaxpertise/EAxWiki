using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IEaReader
{
    EaRepository Open(string connectionString, CancellationToken ct = default);
    bool TestConnection(string connectionString, out string? error);
    void Close();
    bool ExportDiagramImage(string diagramGuid, string filePath);
    EaElementSummary? GetElementSummary(int elementId);
    EaDiagramSummary? GetDiagramSummary(int diagramId);
    string RepositoryPath { get; }

    /// <summary>
    /// Reflects the reader's last observed connection state. The STA dispatcher flips this to
    /// false the moment a COM work item fails and back on the next success. Consumed by the
    /// write-back server's /readyz endpoint so it reports current state, not a stale startup
    /// snapshot (issue #85). Default is <c>true</c> for readers that don't track this signal.
    /// </summary>
    bool IsHealthy => true;

    IReadOnlyList<string> GetStatusTypes();
    string GetElementStatus(int elementId);
    void UpdateElementStatus(int elementId, string newStatus);
    void UpdateElementNotes(int elementId, string newNotesHtml);
    void UpdateDiagramNotes(int diagramId, string newNotesHtml);
    void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml);
    void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml);
    void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml);
    void UpdatePackageNotes(int packageId, string newNotesHtml);
}
