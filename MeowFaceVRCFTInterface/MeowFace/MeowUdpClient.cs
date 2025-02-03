using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.MeowFace
{
    public class MeowUdpClient : IDisposable
    {
        private static IPEndPoint _remoteIpEndPoint = new(IPAddress.Any, 0);

        private readonly UdpClient _udpClient;
        private readonly ILogger _logger;

        private long _lastTimestamp;

        public int ReceiveTimeoutMillis
        {
            get => _udpClient.Client.ReceiveTimeout;
            set => _udpClient.Client.ReceiveTimeout = value;
        }

        public MeowUdpClient(ushort port, ILogger logger)
        {
            _udpClient = new(port);

            ReceiveTimeoutMillis = 10_000;

            _logger = logger;
        }

        public bool TryConnect(long connectTimeoutMillis)
        {
            long startTime = Stopwatch.GetTimestamp();

            while (Stopwatch.GetElapsedTime(startTime).TotalMilliseconds < connectTimeoutMillis)
            {
                MeowFaceParam? data = TryRequest();

                if (data != null) return true;
            }

            return false;
        }

        public MeowFaceParam? TryRequest()
        {
            MeowFaceDTO? lastPacket = null;

            do
            {
                try
                {
                    byte[] data = _udpClient.Receive(ref _remoteIpEndPoint);

                    MeowFaceDTO? currentPacket = JsonConvert.DeserializeObject<MeowFaceDTO>(System.Text.Encoding.UTF8.GetString(data));
                    if (currentPacket != null &&
                        (_lastTimestamp == 0 || currentPacket.Timestamp - _lastTimestamp >= 0))    // Timestamp == 0 when Timestamp is absent
                    {
                        _lastTimestamp = currentPacket.Timestamp;

                        lastPacket = currentPacket;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (SocketException e)
                {
                    _logger.LogWarning(e, "Meow UDP Socket error, is MeowFace app opened and connected?\n" +
                        "Disable the module via the VRCFT interface to stop spamming the logs.");

                    Thread.Sleep(1);
                    continue;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Process UDP packet error");

                    Thread.Sleep(1);
                    continue;
                }
            } while (_udpClient.Available > 0); // Has new data, reduce latancy

            return lastPacket == null ? null : new MeowFaceParam(lastPacket);
        }

        public void Dispose()
        {
            try
            {
                _udpClient.Close();
            }
            catch (Exception)
            {
            }

            _udpClient.Dispose();
        }
    }
}
