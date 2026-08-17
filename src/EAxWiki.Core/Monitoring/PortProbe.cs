using System.Net.Sockets;

namespace EAxWiki.Core.Monitoring;

public interface IPortProbe
{
    /// <summary>True if something is listening on <paramref name="port"/> on 127.0.0.1.</summary>
    bool IsListening(int port);
}

/// <summary>TCP connect probe with a 500 ms timeout — the PS monitor's TcpClient fallback.</summary>
public class TcpPortProbe : IPortProbe
{
    public bool IsListening(int port)
    {
        using var client = new TcpClient();
        try
        {
            var ar = client.BeginConnect("127.0.0.1", port, null, null);
            if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500))) return false;
            client.EndConnect(ar);
            return client.Connected;
        }
        catch (SocketException)
        {
            return false;
        }
    }
}