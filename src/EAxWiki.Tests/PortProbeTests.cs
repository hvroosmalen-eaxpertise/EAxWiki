using EAxWiki.Core.Monitoring;

namespace EAxWiki.Tests;

public class PortProbeTests
{
    [Fact]
    public void IsListening_FreePort_ReturnsFalse()
    {
        var probe = new TcpPortProbe();
        Assert.False(probe.IsListening(55991)); // unassigned port; nothing listens here in CI
    }

    [Fact]
    public void IsListening_ListeningPort_ReturnsTrue()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            Assert.True(new TcpPortProbe().IsListening(port));
        }
        finally { listener.Stop(); }
    }
}