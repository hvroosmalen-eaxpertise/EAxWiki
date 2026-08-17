using EAxWiki.Monitor;

namespace EAxWiki.Tests;

public class PortKillerTests
{
    [Fact]
    public void FindOwnerPid_ParsesListeningLine()
    {
        const string output = """
            Proto  Local Address          Foreign Address        State           PID
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       49152
            TCP    0.0.0.0:8001           0.0.0.0:0              LISTENING       1234
            """;
        Assert.Equal(49152, NetstatPortKiller.FindOwnerPid(output, 8000));
        Assert.Equal(1234, NetstatPortKiller.FindOwnerPid(output, 8001));
    }

    [Fact]
    public void FindOwnerPid_PortNotListening_ReturnsNull()
    {
        const string output = "TCP    0.0.0.0:9000           0.0.0.0:0              LISTENING       9999\n";
        Assert.Null(NetstatPortKiller.FindOwnerPid(output, 8000));
    }

    [Fact]
    public void FindOwnerPid_PicksFirstMatchingLine()
    {
        const string output = """
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       1111
            TCP    0.0.0.0:8000           0.0.0.0:0              LISTENING       2222
            """;
        Assert.Equal(1111, NetstatPortKiller.FindOwnerPid(output, 8000));
    }

    [Fact]
    public void FindOwnerPid_NoMatch_ReturnsNull()
    {
        Assert.Null(NetstatPortKiller.FindOwnerPid("", 8000));
        Assert.Null(NetstatPortKiller.FindOwnerPid("garbage", 8000));
    }
}