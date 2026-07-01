using System.Collections.Concurrent;
using EAxWiki.Core.Models;

namespace EAxWiki.Export;

internal record ExportContext(
    string OutputPath,
    List<(EaElement Element, string PackageDir)> Elements,
    Dictionary<int, (EaElement Element, string PackageDir)> ElementLookup,
    List<(EaDiagram Diagram, string PackageDir)> AllDiagrams,
    Dictionary<int, List<(EaDiagram Diagram, string PkgDir)>> DiagramIndex,
    Dictionary<int, List<(EaConnector Connector, int SourceId)>> IncomingIndex,
    Dictionary<int, (string Name, int? ParentId)> PackageLookup,
    bool Force = false
)
{
    /// <summary>Allowed Status values from t_statustypes, used to populate frontmatter status_options.</summary>
    public IReadOnlyList<string> StatusTypes { get; init; } = [];

    /// <summary>Port of the wiki write-back server. 0 means no server — widget is omitted.</summary>
    public int ApiPort { get; init; } = 0;

    /// <summary>Shared secret embedded in write-back widgets and required by the --api server. Empty when ApiPort is 0.</summary>
    public string ApiToken { get; init; } = string.Empty;

    /// <summary>
    /// Absolute paths of every element .md file as actually written (including collision-renamed files).
    /// Populated by PackageExporter; consumed by cleanup.
    /// </summary>
    public ConcurrentBag<string> RegisteredElementFiles { get; } = new();

    /// <summary>
    /// Absolute paths of all package directories in the current model (including packages with no elements).
    /// Used to detect orphaned directories left behind by package renames or moves.
    /// </summary>
    public HashSet<string> AllPackageDirs { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}
