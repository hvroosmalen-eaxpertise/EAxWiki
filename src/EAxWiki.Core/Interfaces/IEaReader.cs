using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IEaReader
{
    EaRepository Open(string connectionString);
    void Close();
    bool ExportDiagramImage(string diagramGuid, string filePath);
    string RepositoryPath { get; }
    IReadOnlyList<string> GetStatusTypes();
    void UpdateElementStatus(int elementId, string newStatus);
    void UpdateElementNotes(int elementId, string newNotesHtml);
    void UpdateDiagramNotes(int diagramId, string newNotesHtml);
    void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml);
    void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml);
    void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml);
}
