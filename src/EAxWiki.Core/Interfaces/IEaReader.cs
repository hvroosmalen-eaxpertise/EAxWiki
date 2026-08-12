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

    // Async siblings consumed by WikiWritebackServer so the ASP.NET request thread returns to the
    // pool while the EA STA dispatcher processes a work item (issue #85). Default implementations
    // just wrap the sync methods — EaReaderStaDispatcher overrides them with real async that
    // returns the underlying TaskCompletionSource.Task instead of blocking on GetResult.
    Task<IReadOnlyList<string>> GetStatusTypesAsync(CancellationToken ct = default) =>
        Task.FromResult(GetStatusTypes());
    Task<EaElementSummary?> GetElementSummaryAsync(int elementId, CancellationToken ct = default) =>
        Task.FromResult(GetElementSummary(elementId));
    Task<EaDiagramSummary?> GetDiagramSummaryAsync(int diagramId, CancellationToken ct = default) =>
        Task.FromResult(GetDiagramSummary(diagramId));
    Task UpdateElementStatusAsync(int elementId, string newStatus, CancellationToken ct = default)
    {
        UpdateElementStatus(elementId, newStatus); return Task.CompletedTask;
    }
    Task UpdateElementNotesAsync(int elementId, string newNotesHtml, CancellationToken ct = default)
    {
        UpdateElementNotes(elementId, newNotesHtml); return Task.CompletedTask;
    }
    Task UpdateDiagramNotesAsync(int diagramId, string newNotesHtml, CancellationToken ct = default)
    {
        UpdateDiagramNotes(diagramId, newNotesHtml); return Task.CompletedTask;
    }
    Task UpdateAttributeNotesAsync(int elementId, string attributeName, string attributeType, string newNotesHtml, CancellationToken ct = default)
    {
        UpdateAttributeNotes(elementId, attributeName, attributeType, newNotesHtml); return Task.CompletedTask;
    }
    Task UpdateMethodNotesAsync(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml, CancellationToken ct = default)
    {
        UpdateMethodNotes(elementId, methodName, returnType, isStatic, newNotesHtml); return Task.CompletedTask;
    }
    Task UpdateTaggedValueNotesAsync(int elementId, string tagName, string tagValue, string newNotesHtml, CancellationToken ct = default)
    {
        UpdateTaggedValueNotes(elementId, tagName, tagValue, newNotesHtml); return Task.CompletedTask;
    }
    Task UpdatePackageNotesAsync(int packageId, string newNotesHtml, CancellationToken ct = default)
    {
        UpdatePackageNotes(packageId, newNotesHtml); return Task.CompletedTask;
    }
}
