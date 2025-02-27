using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MeowFaceVRCFTInterface.Core.Part.Eye.CenterCalibration;

public class EyesCenterCalibration : IDisposable
{
    public bool DoCalibration { get; set; }
    public uint AverageSampleCount { get; init; } = 120;
    public ushort WaitBeforeStartCalibrationSeconds { get; init; } = 4;

    public EyesValueStorage Shift { get; set; } = new();
    public EyesNormalizeRangeStateStorage RangeNormalization { get; init; } = new();

    private List<EyesValueStorage> _samples = new();
    private MeowFaceVRCFTInterface _module = null!;
    private long? _moduleStartTime;
    private Timer? _deadlockTimer;

    public void Initialize(MeowFaceVRCFTInterface module)
    {
        _module = module;

        if (DoCalibration)
        {
            double timeoutTime = (WaitBeforeStartCalibrationSeconds + AverageSampleCount / 5d)
                * 1000d + 15_000d;

            _module.MeowLogger.LogInformation("Eyes auto center calibration timeout set {}ms",
                Math.Ceiling(timeoutTime));

            _deadlockTimer = new Timer(timeoutTime);
            _deadlockTimer.Elapsed += (_, _) =>
            {
                _module.MeowLogger.LogWarning("Failed to complete eyes auto center calibration, operation timeout");
                StopCalibration();
            };

            _deadlockTimer.AutoReset = false;
            _deadlockTimer.Enabled = true;
        }
    }

    public void UseCalibrationOrCalibrate(EyesParams eyesParams)
    {
        _moduleStartTime ??= Stopwatch.GetTimestamp();

        if (!DoCalibration ||
            Stopwatch.GetElapsedTime(_moduleStartTime.Value).TotalSeconds < WaitBeforeStartCalibrationSeconds)
        {
            if (RangeNormalization.GazeX)
            {
                eyesParams.LeftGazeX = MathUtil.FixedRangeCenterShift(eyesParams.LeftGazeX, Shift.LeftGazeX);
                eyesParams.RightGazeX = MathUtil.FixedRangeCenterShift(eyesParams.RightGazeX, Shift.RightGazeX);
            }
            else
            {
                eyesParams.LeftGazeX -= Shift.LeftGazeX;
                eyesParams.RightGazeX -= Shift.RightGazeX;
            }

            if (RangeNormalization.GazeY)
            {
                eyesParams.LeftGazeY = MathUtil.FixedRangeCenterShift(eyesParams.LeftGazeY, Shift.LeftGazeY);
                eyesParams.RightGazeY = MathUtil.FixedRangeCenterShift(eyesParams.RightGazeY, Shift.RightGazeY);
            }
            else
            {
                eyesParams.LeftGazeY -= Shift.LeftGazeY;
                eyesParams.RightGazeY -= Shift.RightGazeY;
            }

            return;
        }

        if (eyesParams.LeftGazeX.HasValue || eyesParams.LeftGazeY.HasValue || eyesParams.RightGazeX.HasValue ||
            eyesParams.RightGazeY.HasValue)
        {
            _samples.Add(new EyesValueStorage
            {
                LeftGazeX = eyesParams.LeftGazeX ?? float.NaN,
                LeftGazeY = eyesParams.LeftGazeY ?? float.NaN,
                RightGazeX = eyesParams.RightGazeX ?? float.NaN,
                RightGazeY = eyesParams.RightGazeY ?? float.NaN
            });
        }

        if (_samples.Count >= AverageSampleCount)
        {
            Shift = EyesValueStorage.Average(_samples);

            _module.MeowLogger.LogInformation("Eyes auto center calibration success. Calibration: [{}]", Shift);

            StopCalibration();
        }
        else if (_samples.Count % (AverageSampleCount / 5) == 0)
        {
            _module.MeowLogger.LogInformation("Eyes auto center calibration process {}%",
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

        // Create new object, allow GC to free list internal array
        _samples = new List<EyesValueStorage>();

        _module.ConfigManager.SaveConfigAsync();
    }
}