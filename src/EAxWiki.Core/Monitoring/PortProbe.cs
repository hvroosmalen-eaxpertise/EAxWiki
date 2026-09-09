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
        // Try IPv4 first, then IPv6 if available. Use a short timeout to avoid slow tests.
        var timeout = TimeSpan.FromMilliseconds(500);
        if (TryConnect(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.IPAddress.Loopback, port, timeout))
            return true;
        if (System.Net.Sockets.Socket.OSSupportsIPv6 && TryConnect(System.Net.Sockets.AddressFamily.InterNetworkV6, System.Net.IPAddress.IPv6Loopback, port, timeout))
            return true;
        return false;
    }

    private static bool TryConnect(System.Net.Sockets.AddressFamily family, System.Net.IPAddress address, int port, TimeSpan timeout)
    {
        using var client = new TcpClient(family);
        try
        {
            var task = client.ConnectAsync(address, port);
            if (!task.Wait(timeout)) return false;
            return client.Connected;
        }
        catch (SocketException)
        {
            return false;
        }
        catch (AggregateException ae) when (ae.InnerException is SocketException)
        {
            return false;
        }
    }
}