using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MeowFaceVRCFTInterface.MeowFace;

public class MeowUdpAutoConnect : IDisposable
{
    private const ushort BroadcastPort = 21412;

    private readonly UdpClient _udpClient;
    private readonly Timer _timer = new();

    /// <exception cref="Exception" />
    public MeowUdpAutoConnect(ushort port, ILogger logger)
    {
        _udpClient = new UdpClient();

        byte[] packet = Encoding.UTF8.GetBytes("{\"port\":" + port + "}");

        _timer.Elapsed += (_, _) =>
        {
            try
            {
                _udpClient.Send(packet, packet.Length, "255.255.255.255", BroadcastPort);
            }
            catch (Exception e)
            {
                logger.LogDebug(e, "Failed to send UDP Packet");
            }
        };

        _timer.AutoReset = true;
        _timer.Interval = 1000;
    }

    public void TryStartBroadcasting()
    {
        _timer.Enabled = true;
    }

    public void TryStopBroadcasting()
    {
        _timer.Enabled = false;
    }

    public void Dispose()
    {
        _timer.Dispose();

        try
        {
            _udpClient.Close();
        }
        catch (Exception)
        {
            // ignored
        }

        _udpClient.Dispose();
    }
}