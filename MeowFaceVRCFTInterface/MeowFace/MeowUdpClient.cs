using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.MeowFace;

public class MeowUdpClient : IDisposable
{
    private static IPEndPoint _remoteIpEndPoint = new(IPAddress.Any, 0);

    private readonly UdpClient _udpClient;
    private readonly ILogger _logger;
    private readonly MeowUdpAutoConnect? _meowUdpAutoConnect;
    public ushort SocketPort { get; private set; }
    private long _lastTimestamp;

    public int ReceiveTimeoutMillis
    {
        get => _udpClient.Client.ReceiveTimeout;
        set => _udpClient.Client.ReceiveTimeout = value;
    }

    /// <summary>
    /// The port you passed may be different from the one on which the socket will be opened.
    /// You can get an active port via MeowUdpClient.SocketPort
    /// </summary>
    /// <exception cref="Exception">If UDP Socket can't be opened</exception>
    public MeowUdpClient(ushort port, ILogger logger)
    {
        SocketPort = port;
        _logger = logger;

        _udpClient = TryCreateUdpSocket();

        try
        {
            _meowUdpAutoConnect = new MeowUdpAutoConnect(SocketPort, logger);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to create UDP Socket for auto connect");
        }
    }

    public bool TryConnect(ushort connectTimeoutSeconds)
    {
        _meowUdpAutoConnect?.TryStartBroadcasting();

        ReceiveTimeoutMillis = connectTimeoutSeconds * 1000;

        long startTime = Stopwatch.GetTimestamp();

        do
        {
            MeowFaceParam? data = TryRequest();

            if (data != null) return true;
        } while (Stopwatch.GetElapsedTime(startTime).TotalSeconds < connectTimeoutSeconds);

        return false;
    }

    public MeowFaceParam? TryRequest()
    {
        MeowFaceDto? lastPacket = null;

        do
        {
            try
            {
                byte[] data = _udpClient.Receive(ref _remoteIpEndPoint);
                string dataStr = System.Text.Encoding.UTF8.GetString(data);
                MeowFaceDto? currentPacket = JsonConvert.DeserializeObject<MeowFaceDto>(dataStr);

                // Timestamp == 0 when Timestamp is absent
                if (currentPacket != null && (_lastTimestamp == 0 || currentPacket.Timestamp - _lastTimestamp >= 0))
                {
                    _lastTimestamp = currentPacket.Timestamp;

                    lastPacket = currentPacket;
                }
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (SocketException e)
            {
                _logger.LogWarning("Meow UDP Socket error, is MeowFace app opened and connected?\n" +
                                   "Disable the module via the VRCFT interface to stop spamming the logs.");
                _logger.LogDebug(e, "Additional StackTrace");

                Thread.Sleep(1);

                _meowUdpAutoConnect?.TryStartBroadcasting();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Process UDP packet error");

                Thread.Sleep(1);
            }
        } while (_udpClient.Available > 0); // Has new data, reduce latency

        if (lastPacket == null) return null;

        _meowUdpAutoConnect?.TryStopBroadcasting();

        return new MeowFaceParam(lastPacket);
    }

    public void Dispose()
    {
        try
        {
            _meowUdpAutoConnect?.Dispose();
        }
        catch (Exception)
        {
            // ignored
        }

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

    /// <exception cref="Exception">If it can't create UDP Socket</exception>
    private UdpClient TryCreateUdpSocket()
    {
        for (int i = 0; i <= 0xFFFF; i++)
        {
            try
            {
                return new UdpClient(SocketPort);
            }
            catch (SocketException socketException)
            {
                if (socketException.SocketErrorCode != SocketError.AddressAlreadyInUse) throw;

                _logger.LogInformation("UDP port {} is busy, the next one will be used!", SocketPort);

                SocketPort++;
            }
        }

        throw new Exception("All ports are busy, how?");
    }
}