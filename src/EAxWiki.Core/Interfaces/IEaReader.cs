using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IEaReader
{
    EaRepository Open(string connectionString);
    void Close();
    bool ExportDiagramImage(string diagramGuid, string filePath);
}
