using EAxWiki.Core.Models;

namespace EAxWiki.Export;

internal static class ConnectorIndexBuilder
{
    public static Dictionary<int, List<(EaConnector Connector, int SourceId)>> Build(
        List<(EaElement Element, string PackageDir)> elements)
    {
        var incomingIndex = new Dictionary<int, List<(EaConnector Connector, int SourceId)>>();
        var seenConnectors = new HashSet<int>();
        foreach (var (elem, _) in elements)
        {
            foreach (var conn in elem.Connectors)
            {
                if (!seenConnectors.Add(conn.Id)) continue;
                if (!incomingIndex.ContainsKey(conn.TargetId))
                    incomingIndex[conn.TargetId] = new List<(EaConnector, int)>();
                incomingIndex[conn.TargetId].Add((conn, conn.SourceId));
            }
        }

        return incomingIndex;
    }
}
