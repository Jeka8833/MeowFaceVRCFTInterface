using System.Net;
using System.Net.Sockets;
using MeowFaceVRCFTInterface.MeowFace;
using MeowFaceVRCFTInterface.VRCFTMappers;
using MeowFaceVRCFTInterface.VRCFTMappers.Eye;
using Microsoft.Extensions.Logging;
using VRCFaceTracking;
using VRCFaceTracking.Core.Library;

namespace MeowFaceVRCFTInterface
{
    public class MeowFaceVRCFTInterface : ExtTrackingModule
    {
        private const ushort _port = 12345;
        private readonly IMapperCft[] _mappers = { new EyeMapper(), new BrowMapper(), new CheekMapper(),
            new JawMapper(), new LipAndMouthMapper(), new NoseMapper(), new TongueMapper()};

        private MeowUdpClient _udpClient = null!;
        public ILogger SkipSpamLogger { get; private set; } = null!;

        public override (bool SupportsEye, bool SupportsExpression) Supported => (true, true);

        public override (bool eyeSuccess, bool expressionSuccess) Initialize(bool eyeAvailable, bool expressionAvailable)
        {
            SkipSpamLogger = new SkipSpamLogger(Logger);
            _udpClient = new(_port, SkipSpamLogger)
            {
                ReceiveTimeoutMillis = 60_000
            };

            Logger.LogInformation("MeowFace interface is waiting for connection.\n" +
                "Please try entering one of the following addresses ({}) in the \"Enter PC IP Address\" field " +
                "and then set \"Enter PC Port number\" to {}.\n" +
                "If you fail to do so in 60 seconds, the module will be disabled " +
                "and you will have to restart the VRCFT application to try to connect again.", string.Join(", ", GetLocalIPAddresses()), _port);

            if (!_udpClient.TryConnect(60_000))
            {
                Teardown();

                ModuleInformation.Active = false;

                Logger.LogInformation("The Android MeowFace app failed to connect to this computer in 60 seconds. Disabling the module...");

                return (false, false);
            }

            foreach (IMapperCft mapper in _mappers)
            {
                mapper.Initialize(this);
            }

            _udpClient.ReceiveTimeoutMillis = 10_000;

            ModuleInformation.Name = "Meow Face";

            try
            {
                var stream = GetType()
                    .Assembly
                    .GetManifestResourceStream("MeowFaceVRCFTInterface.Assets.meowface-logo.png");

                ModuleInformation.StaticImages =
                    stream != null ? new List<Stream> { stream } : ModuleInformation.StaticImages;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, "Failed to load MeowFace Icon");
            }

            Logger.LogInformation("Android MeowFace app is connected successfully!");

            return (eyeAvailable, expressionAvailable);
        }

        public override void Update()
        {
            try
            {
                if (Status != ModuleState.Active || !(ModuleInformation.UsingEye || ModuleInformation.UsingExpression))
                {
                    Thread.Sleep(100);

                    return;
                }

                MeowFaceParam? meowFaceParamNullable = _udpClient.TryRequest();
                if (meowFaceParamNullable is MeowFaceParam meowFaceParam)
                {
                    foreach (IMapperCft mapper in _mappers)
                    {
                        if (ModuleInformation.UsingEye)
                        {
                            mapper.UpdateEye(meowFaceParam);
                        }

                        if (ModuleInformation.UsingExpression)
                        {
                            mapper.UpdateExpression(meowFaceParam);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SkipSpamLogger.LogWarning(e, "Exception in Module Update Loop");
            }
        }

        public override void Teardown()
        {
            if (_udpClient != null)
            {
                _udpClient.Dispose();

                Logger.LogInformation("UPD Socket Closed");
            }
        }

        private static List<string> GetLocalIPAddresses()
        {
            List<string> ips = new();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ip.ToString());
                }
            }


            return ips;
        }
    }
}
