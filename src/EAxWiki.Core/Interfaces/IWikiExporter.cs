using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IWikiExporter
{
    Task ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath);
}
