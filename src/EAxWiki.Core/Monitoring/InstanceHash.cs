using System.Security.Cryptography;
using System.Text;

namespace EAxWiki.Core.Monitoring;

/// <summary>
/// 12-char MD5 of the lowercased wiki output dir, keying the per-instance state folder —
/// identical to the PS monitor's $instanceHash. Lives in Core so the SchedulerUI dashboard
/// can resolve the same folder the monitor writes.
/// </summary>
public static class InstanceHash
{
    public static string Compute(string wikiDir)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(wikiDir.ToLowerInvariant()));
        return Convert.ToHexString(hash)[..12].ToLowerInvariant();
    }
}
