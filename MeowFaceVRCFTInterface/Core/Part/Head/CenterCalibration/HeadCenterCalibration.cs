using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MeowFaceVRCFTInterface.Core.Part.Head.CenterCalibration;

public class HeadCenterCalibration : IDisposable
{
    public bool DoCalibration { get; set; }
    public ushort AverageSampleCount { get; set; } = 120;
    public ushort WaitBeforeStartCalibrationSeconds { get; init; } = 4;

    public HeadValueStorage Shift { get; set; } = new();
    public HeadNormalizeRangeStateStorage RangeNormalization { get; init; } = new();

    private List<HeadValueStorage> _samples = new();
    private MeowFaceVRCFTInterface _module = null!;
    private long? _moduleStartTime;
    private Timer? _deadlockTimer;

    public void Initialize(MeowFaceVRCFTInterface module)
    {
        _module = module;

        if (DoCalibration)
        {
            if (AverageSampleCount == 0)
            {
                AverageSampleCount = 1;
                
                _module.MeowLogger.LogWarning("EyesCenterCalibration.AverageSampleCount is 0, set to 1");

                _module.ConfigManager.SaveConfigAsync();
            }
            
            double timeoutTime = (WaitBeforeStartCalibrationSeconds + AverageSampleCount / 5d)
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

        if (!DoCalibration ||
            Stopwatch.GetElapsedTime(_moduleStartTime.Value).TotalSeconds < WaitBeforeStartCalibrationSeconds)
        {
            headParams.PosX -= Shift.X;
            headParams.PosY -= Shift.Y;
            headParams.PosZ -= Shift.Z;

            headParams.Pitch = RangeNormalization.Pitch
                ? MathUtil.FixedRangeCenterShift(headParams.Pitch, Shift.Pitch)
                : headParams.Pitch - Shift.Pitch;

            headParams.Roll = RangeNormalization.Roll
                ? MathUtil.FixedRangeCenterShift(headParams.Roll, Shift.Roll)
                : headParams.Roll - Shift.Roll;

            headParams.Yaw = RangeNormalization.Yaw
                ? MathUtil.FixedRangeCenterShift(headParams.Yaw, Shift.Yaw)
                : headParams.Yaw - Shift.Yaw;

            return;
        }

        if (headParams.PosX.HasValue || headParams.PosY.HasValue || headParams.PosZ.HasValue ||
            headParams.Pitch.HasValue || headParams.Roll.HasValue || headParams.Yaw.HasValue)
        {
            _samples.Add(new HeadValueStorage
            {
                X = headParams.PosX ?? float.NaN,
                Y = headParams.PosY ?? float.NaN,
                Z = headParams.PosZ ?? float.NaN,
                Pitch = headParams.Pitch ?? float.NaN,
                Roll = headParams.Roll ?? float.NaN,
                Yaw = headParams.Yaw ?? float.NaN
            });
        }

        if (_samples.Count >= AverageSampleCount)
        {
            Shift = HeadValueStorage.Average(_samples);

            _module.MeowLogger.LogInformation("Head auto center calibration success. Calibration: [{}]", Shift);

            StopCalibration();
        }
        else if (_samples.Count % (AverageSampleCount / 5) == 0)
        {
            _module.MeowLogger.LogInformation("Head auto center calibration process {}%",
                (int)(_samples.Count * 100f / AverageSampleCount));
        }
    }

    public void Dispose()
    {
        _deadlockTimer?.Dispose();
    }

    private void StopCalibration()
    {
        DoCalibration = false;
        if (_deadlockTimer != null)
        {
            _deadlockTimer.Enabled = false;
        }

        // Create new objects, allow GC to free list internal array
        _samples = new List<HeadValueStorage>();

        _module.ConfigManager.SaveConfigAsync();
    }
}