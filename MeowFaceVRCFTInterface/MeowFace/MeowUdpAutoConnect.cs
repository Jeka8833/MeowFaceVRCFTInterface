using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.MeowFace;

public class MeowUdpAutoConnect : IDisposable
{
    private const ushort BroadcastPort = 21412;

    private readonly byte[] _packet;
    private readonly ILogger _logger;
    private readonly UdpClient? _udpClient;

    private Thread? _thread;

    public MeowUdpAutoConnect(ushort port, ILogger logger)
    {
        _packet = Encoding.ASCII.GetBytes("{\"port\":" + port + "}");
        _logger = logger;

        try
        {
            _udpClient = new UdpClient();
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to create UDP Socket");
        }
    }

    public void StartBroadcasting()
    {
        if (_udpClient == null || _thread != null) return;

        _thread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    _udpClient.Send(_packet, _packet.Length, "255.255.255.255", BroadcastPort);

                    Thread.Sleep(500);
                }
                catch (ThreadInterruptedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Failed to send UDP Packet");

                    try
                    {
                        Thread.Sleep(10_000);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        });

        _thread.IsBackground = true;
        _thread.Priority = ThreadPriority.Lowest;
        _thread.Start();
    }

    public void Dispose()
    {
        if (_thread != null)
        {
            try
            {
                _thread.Interrupt();
                _thread.Join();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed to Close Thread");
            }
        }

        try
        {
            _udpClient?.Close();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to Close UDP");
        }

        _udpClient?.Dispose();
    }
}