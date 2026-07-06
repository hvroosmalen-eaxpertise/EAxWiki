using EAxWiki.Core.Models;

namespace EAxWiki.Core.Interfaces;

public interface IWikiExporter
{
    Task<ExportResult> ExportAsync(EaRepository repository, EaPackage? startPackage, string outputPath, IEaReader? reader = null, bool force = false, CancellationToken cancellationToken = default);
}
