using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class HeadCenterCalibration : IDisposable
{
    public bool DoAutoCalibration { get; set; }

    public float HeadShiftX { get; set; }
    public float HeadShiftY { get; set; }
    public float HeadShiftZ { get; set; }

    public float HeadShiftPitch { get; set; }
    public float HeadShiftYaw { get; set; }
    public float HeadShiftRoll { get; set; }

    public uint AutoCalibrationAverageSampleCount { get; init; } = 120;
    public ushort WaitBeforeStartAutoCalibrationSeconds { get; init; } = 4;


    private List<float> _headShiftXList = new();
    private List<float> _headShiftYList = new();
    private List<float> _headShiftZList = new();

    private List<float> _headShiftPitchList = new();
    private List<float> _headShiftRollList = new();
    private List<float> _headShiftYawList = new();

    private MeowFaceVRCFTInterface _module = null!;
    private long? _moduleStartTime;
    private Timer? _deadlockTimer;

    public void Initialize(MeowFaceVRCFTInterface module)
    {
        _module = module;

        if (DoAutoCalibration)
        {
            double timeoutTime = (WaitBeforeStartAutoCalibrationSeconds + AutoCalibrationAverageSampleCount / 5d)
                * 1000d + 15_000d;

            _module.MeowLogger.LogInformation("Head auto center calibration timeout set {}ms",
                Math.Ceiling(timeoutTime));

            _deadlockTimer = new Timer(timeoutTime);
            _deadlockTimer.Elapsed += (_, _) =>
            {
                _module.MeowLogger.LogWarning("Failed to complete head auto center calibration, operation timeout");
                StopCalibration();
            };

            _deadlockTimer.AutoReset = false;
            _deadlockTimer.Enabled = true;
        }
    }

    public void UseCalibrationOrCalibrate(HeadParams headParams)
    {
        _moduleStartTime ??= Stopwatch.GetTimestamp();

        if (!DoAutoCalibration ||
            Stopwatch.GetElapsedTime(_moduleStartTime.Value).TotalSeconds < WaitBeforeStartAutoCalibrationSeconds)
        {
            headParams.HeadPosX -= HeadShiftX;
            headParams.HeadPosY -= HeadShiftY;
            headParams.HeadPosZ -= HeadShiftZ;

            headParams.HeadPitch -= HeadShiftPitch;
            headParams.HeadRoll -= HeadShiftRoll;
            headParams.HeadYaw -= HeadShiftYaw;

            return;
        }

        if (headParams.HeadPosX.HasValue)
        {
            _headShiftXList.Add(headParams.HeadPosX.Value);
        }

        if (headParams.HeadPosY.HasValue)
        {
            _headShiftYList.Add(headParams.HeadPosY.Value);
        }

        if (headParams.HeadPosZ.HasValue)
        {
            _headShiftZList.Add(headParams.HeadPosZ.Value);
        }

        if (headParams.HeadPitch.HasValue)
        {
            _headShiftPitchList.Add(headParams.HeadPitch.Value);
        }

        if (headParams.HeadRoll.HasValue)
        {
            _headShiftRollList.Add(headParams.HeadRoll.Value);
        }

        if (headParams.HeadYaw.HasValue)
        {
            _headShiftYawList.Add(headParams.HeadYaw.Value);
        }

        int[] counts =
        {
            _headShiftXList.Count, _headShiftYList.Count, _headShiftZList.Count, _headShiftPitchList.Count,
            _headShiftRollList.Count, _headShiftYawList.Count
        };

        int currentSamples = counts.Max();

        if (currentSamples >= AutoCalibrationAverageSampleCount)
        {
            HeadShiftX = _headShiftXList.Count > 0 ? _headShiftXList.Average() : 0;
            HeadShiftY = _headShiftYList.Count > 0 ? _headShiftYList.Average() : 0;
            HeadShiftZ = _headShiftZList.Count > 0 ? _headShiftZList.Average() : 0;
            HeadShiftPitch = _headShiftPitchList.Count > 0 ? _headShiftPitchList.Average() : 0;
            HeadShiftRoll = _headShiftRollList.Count > 0 ? _headShiftRollList.Average() : 0;
            HeadShiftYaw = _headShiftYawList.Count > 0 ? _headShiftYawList.Average() : 0;

            _module.MeowLogger.LogInformation(
                "Head auto center calibration success. HeadShiftX: {}, HeadShiftY: {}, HeadShiftZ: {}, " +
                "HeadShiftPitch: {}, HeadShiftRoll: {}, HeadShiftYaw: {}",
                HeadShiftX, HeadShiftY, HeadShiftZ, HeadShiftPitch, HeadShiftRoll, HeadShiftYaw);

            StopCalibration();
        }
        else if (currentSamples % (AutoCalibrationAverageSampleCount / 5) == 0)
        {
            _module.MeowLogger.LogInformation("Head auto center calibration process {}%",
                (int)(currentSamples * 100f / AutoCalibrationAverageSampleCount));
        }
    }

    public void Dispose()
    {
        _deadlockTimer?.Dispose();
    }

    private void StopCalibration()
    {
        DoAutoCalibration = false;
        if (_deadlockTimer != null)
        {
            _deadlockTimer.Enabled = false;
        }

        // Create new objects, allow GC to free list internal array
        _headShiftXList = new List<float>();
        _headShiftYList = new List<float>();
        _headShiftZList = new List<float>();
        _headShiftPitchList = new List<float>();
        _headShiftRollList = new List<float>();
        _headShiftYawList = new List<float>();

        _module.ConfigManager.SaveConfigAsync();
    }
}