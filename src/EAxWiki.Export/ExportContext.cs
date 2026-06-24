using EAxWiki.Core.Models;

namespace EAxWiki.Export;

internal record ExportContext(
    string OutputPath,
    List<(EaElement Element, string PackageDir)> Elements,
    Dictionary<int, (EaElement Element, string PackageDir)> ElementLookup,
    List<(EaDiagram Diagram, string PackageDir)> AllDiagrams,
    Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> DiagramIndex,
    Dictionary<int, List<(EaConnector Connector, int SourceId)>> IncomingIndex,
    Dictionary<int, (string Name, int? ParentId)> PackageLookup
);
