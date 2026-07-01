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
}
