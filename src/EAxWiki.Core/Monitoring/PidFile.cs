using System.Diagnostics;
using System.Text.Json;

namespace EAxWiki.Core.Monitoring;

public record PidFileInfo(int Pid, DateTimeOffset StartTime);

/// <summary>
/// PID + process start time JSON pid files (serve.pid / api.pid / llm.pid). Alive = the PID is
/// running AND its recorded start time matches the actual process start time within 2 s — so a
/// stale file surviving a reboot can't false-positive when the OS reuses a PID.
/// </summary>
public static class PidFile
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static void Write(string path, int pid, DateTimeOffset startTime)
    {
        var info = new { pid, startTime = startTime.ToString("O") };
        File.WriteAllText(path, JsonSerializer.Serialize(info, Options));
    }

    public static PidFileInfo? Read(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("pid", out var pidEl) ||
                !doc.RootElement.TryGetProperty("startTime", out var startEl))
                return null;
            if (!pidEl.TryGetInt32(out var pid)) return null;
            var start = DateTimeOffset.Parse(startEl.GetString() ?? string.Empty);
            return new PidFileInfo(pid, start);
        }
        catch (JsonException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null; // valid JSON with a non-object root (e.g. an array or scalar)
        }
        catch (FormatException)
        {
            return null;
        }
    }

    public static bool IsAlive(string path)
    {
        var info = Read(path);
        if (info == null) return false;
        try
        {
            using var proc = Process.GetProcessById(info.Pid);
            var delta = (info.StartTime - proc.StartTime.ToUniversalTime()).Duration();
            return delta.TotalSeconds <= 2;
        }
        catch (ArgumentException)
        {
            return false; // no process with that id
        }
        catch (System.ComponentModel.Win32Exception)
        {
            return false; // process inaccessible (another session/elevation) - treat as not live
        }
    }
}