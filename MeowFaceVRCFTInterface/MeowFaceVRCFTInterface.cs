using System.Net;
using System.Net.Sockets;
using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.Core.Config;
using MeowFaceVRCFTInterface.Core.Logger;
using MeowFaceVRCFTInterface.MeowFace;
using Microsoft.Extensions.Logging;
using VRCFaceTracking;
using VRCFaceTracking.Core.Library;

namespace MeowFaceVRCFTInterface;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once InconsistentNaming
public class MeowFaceVRCFTInterface : ExtTrackingModule
{
    // C:\Users\[UserName]\AppData\Local\Packages\[96ba052f-0948-44d8-86c4-a0212e4ae047_4s4k90pjvq32p]\LocalCache\Roaming\VRCFaceTracking\Configs\MeowFace\MeowConfig.json
    // or
    // C:\Users\[UserName]\AppData\Roaming\VRCFaceTracking\Configs\MeowFace\MeowConfig.json
    //
    // Where [UserName] is your Windows username
    // and [96ba052f-0948-44d8-86c4-a0212e4ae047_4s4k90pjvq32p] is the VRCFaceTracking package name, for you, it can be different, try to search similar folder
    //
    // In either case, the path should be printed in the VRCFT logs.
    private static readonly string ConfigPath = Path.Combine(
        VRCFaceTracking.Core.Utils.PersistentDataDirectory, "Configs", "MeowFace", "MeowConfig.json");

    public ILogger MeowLogger { get; private set; } = null!;
    public ILogger MeowSpamLogger { get; private set; } = null!;
    public ConfigManager ConfigManager { get; private set; } = null!;

    private MeowUdpClient _udpClient = null!;

    private ModuleState _previousStatus = ModuleState.Uninitialized;

    public override (bool SupportsEye, bool SupportsExpression) Supported => (true, true);

    public override (bool eyeSuccess, bool expressionSuccess) Initialize(bool eyeAvailable, bool expressionAvailable)
    {
        MeowLogger = new VrcftExceptionFixerLogger(Logger);

        ConfigManager = new ConfigManager(ConfigPath, this, MeowLogger);
        ConfigManager.LoadAndMigrateConfig();

        MeowSpamLogger = ConfigManager.Config.ShowAllLogs ? MeowLogger : new SkipSpamLogger(MeowLogger);

        try
        {
            _udpClient = new MeowUdpClient(ConfigManager.Config.MeowFacePort, MeowSpamLogger);
        }
        catch (Exception e)
        {
            ModuleInformation.Active = false;

            MeowLogger.LogError(e, "Failed to create UDP socket, module disabled.");

            return (false, false);
        }

        MeowLogger.LogInformation("MeowFace interface is waiting for connection.\n" +
                                  "Please try entering one of the following addresses ({}) in the \"Enter PC IP " +
                                  "Address\" field and then set \"Enter PC Port number\" to {}.\n" +
                                  "If you fail to do so in {} seconds, the module will be disabled " +
                                  "and you will have to restart the VRCFT application to try to connect again.",
            string.Join(", ", GetLocalIpAddresses()), _udpClient.SocketPort,
            ConfigManager.Config.SearchMeowFaceTimeoutSeconds);

        if (!_udpClient.TryConnect(ConfigManager.Config.SearchMeowFaceTimeoutSeconds))
        {
            Teardown();

            ModuleInformation.Active = false;

            MeowLogger.LogInformation(
                "The Android MeowFace app failed to connect to this computer in {} seconds. " +
                "Disabling the module...", ConfigManager.Config.SearchMeowFaceTimeoutSeconds);

            return (false, false);
        }

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
            if (_previousStatus != Status)
            {
                Thread.CurrentThread.IsBackground = true;

                if (Status == ModuleState.Active)
                {
                    if (_previousStatus != ModuleState.Uninitialized)
                    {
                        ConfigManager.LoadConfig();
                    }

                    try
                    {
                        _udpClient.ReceiveTimeoutMillis =
                            Convert.ToInt32(ConfigManager.Config.MeowFaceReadTimeoutMilliseconds);
                    }
                    catch (Exception e)
                    {
                        MeowLogger.LogWarning(e, "MeowFaceReadTimeoutMilliseconds is too big");
                    }
                }

                _previousStatus = Status;
            }

            if (Status != ModuleState.Active || !(ModuleInformation.UsingEye || ModuleInformation.UsingExpression))
            {
                Thread.Sleep(100);

                return;
            }

            MeowFaceParam? meowFaceParam = _udpClient.TryRequest();
            if (!meowFaceParam.HasValue) return;

            foreach (MapperBase mapper in ConfigManager.Mappers)
            {
                if (!mapper.IsEnabled || mapper.IsMapperCrashed) continue;

                if (ModuleInformation.UsingEye)
                {
                    mapper.UpdateEye(meowFaceParam.Value);
                }

                if (ModuleInformation.UsingExpression)
                {
                    mapper.UpdateExpression(meowFaceParam.Value);
                }
            }
        }
        catch (ThreadInterruptedException) // VRCFT doesn't send an interrupt when it wants to stop the module ((
        {
        }
        catch (Exception e)
        {
            MeowSpamLogger.LogWarning(e, "Exception in Module Update Loop");
        }
    }

    public override void Teardown()
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        _udpClient?.Dispose();

        MeowLogger.LogInformation("UPD Socket Closed");
    }

    private static IOrderedEnumerable<IPAddress> GetLocalIpAddresses()
    {
        try
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .OrderByDescending(ip => ip.ToString().StartsWith("192.168."));
        }
        catch (Exception)
        {
            return Enumerable.Empty<IPAddress>().OrderBy(_ => 0);
        }
    }
}