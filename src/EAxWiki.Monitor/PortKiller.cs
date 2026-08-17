using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EAxWiki.Monitor;

public interface IPortKiller
{
    /// <summary>Kill the process listening on <paramref name="port"/> (netstat -ano → Stop-Process).</summary>
    void KillPortOwner(int port);
}

public class NetstatPortKiller : IPortKiller
{
    // Matches "TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       49152"
    private static readonly Regex LineRegex =
        new(@"^\s*TCP\s+\S+:(\d+)\s+\S+\s+LISTENING\s+(\d+)\s*$", RegexOptions.Multiline);

    public void KillPortOwner(int port)
    {
        var psi = new ProcessStartInfo("netstat", "-ano")
        {
            RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true,
        };
        using var proc = Process.Start(psi);
        if (proc == null) return;
        var output = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();

        var pid = FindOwnerPid(output, port);
        if (pid == null) return;
        Process.Start("taskkill", $"/PID {pid} /F");
    }

    internal static int? FindOwnerPid(string netstatOutput, int port)
    {
        foreach (Match m in LineRegex.Matches(netstatOutput))
        {
            if (int.Parse(m.Groups[1].Value) == port)
                return int.Parse(m.Groups[2].Value);
        }
        return null;
    }
}