﻿using System.Net;
using System.Net.Sockets;
using MeowFaceVRCFTInterface.Config;
using MeowFaceVRCFTInterface.Logger;
using MeowFaceVRCFTInterface.MeowFace;
using MeowFaceVRCFTInterface.VRCFTMappers;
using Microsoft.Extensions.Logging;
using VRCFaceTracking;
using VRCFaceTracking.Core.Library;

namespace MeowFaceVRCFTInterface
{
    public class MeowFaceVRCFTInterface : ExtTrackingModule
    {
        // C:\Users\[UserName]\AppData\Local\Packages\[96ba052f-0948-44d8-86c4-a0212e4ae047_4s4k90pjvq32p]\LocalCache\Roaming\VRCFaceTracking\Configs\MeowFace\MeowConfig.json
        // or
        // C:\Users\[UserName]\AppData\Roaming\VRCFaceTracking\Configs\MeowFace\MeowConfig.json
        //
        // Where [UserName] is your Windows username
        // and [96ba052f-0948-44d8-86c4-a0212e4ae047_4s4k90pjvq32p] is the VRCFaceTracking package name, for you it can be different, try to search similar folder
        //
        // In either case, the path should be printed in the VRCFT logs.
        private static readonly string _configPath = Path.Combine(
            VRCFaceTracking.Core.Utils.PersistentDataDirectory, "Configs", "MeowFace", "MeowConfig.json");

        public ILogger MeowLogger { get; private set; } = null!;
        public ILogger MeowSpamLogger { get; private set; } = null!;

        private ConfigManager _configManager = null!;
        private MeowUdpClient _udpClient = null!;

        public override (bool SupportsEye, bool SupportsExpression) Supported => (true, true);

        public override (bool eyeSuccess, bool expressionSuccess) Initialize(bool eyeAvailable, bool expressionAvailable)
        {
            MeowLogger = new VrcftExceptionFixerLogger(Logger);
            MeowSpamLogger = new SkipSpamLogger(MeowLogger);

            _configManager = new(_configPath, this, MeowLogger);
            _configManager.LoadConfig();

            UwpConfigPathFinder.PrintConfigPath(MeowLogger, _configPath);

            ushort udpPort = _configManager.Config.MeowFacePort;
            int udpConnectionTimeoutMillis = _configManager.Config.SearchMeowFaceTimeoutSeconds * 1_000;

            _udpClient = new(udpPort, MeowSpamLogger)
            {
                ReceiveTimeoutMillis = udpConnectionTimeoutMillis
            };

            MeowLogger.LogInformation("MeowFace interface is waiting for connection.\n" +
                "Please try entering one of the following addresses ({}) in the \"Enter PC IP Address\" field " +
                "and then set \"Enter PC Port number\" to {}.\n" +
                "If you fail to do so in {} seconds, the module will be disabled " +
                "and you will have to restart the VRCFT application to try to connect again.",
                string.Join(", ", GetLocalIPAddresses()), udpPort, _configManager.Config.SearchMeowFaceTimeoutSeconds);

            if (!_udpClient.TryConnect(udpConnectionTimeoutMillis))
            {
                Teardown();

                ModuleInformation.Active = false;

                MeowLogger.LogInformation("The Android MeowFace app failed to connect to this computer in {} seconds. " +
                    "Disabling the module...", _configManager.Config.SearchMeowFaceTimeoutSeconds);

                return (false, false);
            }

            _udpClient.ReceiveTimeoutMillis = _configManager.Config.MeowFaceReadTimeoutMilliseconds;

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
                MeowLogger.LogWarning(e, "Failed to load MeowFace Icon");
            }

            MeowLogger.LogInformation("Android MeowFace app is connected successfully!");

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
                    foreach (MapperCft mapper in _configManager.Mappers)
                    {
                        if (!mapper.IsEnabled || mapper.IsMapperCrashed) continue;

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
                MeowSpamLogger.LogWarning(e, "Exception in Module Update Loop");
            }
        }

        public override void Teardown()
        {
            if (_udpClient != null)
            {
                _udpClient.Dispose();

                MeowLogger.LogInformation("UPD Socket Closed");
            }
        }

        private static IOrderedEnumerable<IPAddress> GetLocalIPAddresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .OrderBy(ip => ip.ToString().StartsWith("192.168.") ? -1 : 0);
        }
    }
}