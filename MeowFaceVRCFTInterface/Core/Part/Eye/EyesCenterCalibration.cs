using System.Diagnostics;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesCenterCalibration : IDisposable
{
    public bool DoAutoCalibration { get; set; }

    public Vector2 EyeShiftLeft { get; set; } = Vector2.Zero;
    public Vector2 EyeShiftRight { get; set; } = Vector2.Zero;

    public uint AutoCalibrationAverageSampleCount { get; init; } = 120;
    public ushort WaitBeforeStartAutoCalibrationSeconds { get; init; } = 4;


    private List<float> _eyeShiftLeftXList = new();
    private List<float> _eyeShiftLeftYList = new();
    private List<float> _eyeShiftRightXList = new();
    private List<float> _eyeShiftRightYList = new();

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

        if (!DoAutoCalibration ||
            Stopwatch.GetElapsedTime(_moduleStartTime.Value).TotalSeconds < WaitBeforeStartAutoCalibrationSeconds)
        {
            eyesParams.LeftGazeX -= EyeShiftLeft.X;
            eyesParams.LeftGazeY -= EyeShiftLeft.Y;

            eyesParams.RightGazeX -= EyeShiftRight.X;
            eyesParams.RightGazeY -= EyeShiftRight.Y;

            return;
        }

        if (eyesParams.LeftGazeX.HasValue)
        {
            _eyeShiftLeftXList.Add(eyesParams.LeftGazeX.Value);
        }

        if (eyesParams.LeftGazeY.HasValue)
        {
            _eyeShiftLeftYList.Add(eyesParams.LeftGazeY.Value);
        }

        if (eyesParams.RightGazeX.HasValue)
        {
            _eyeShiftRightXList.Add(eyesParams.RightGazeX.Value);
        }

        if (eyesParams.RightGazeY.HasValue)
        {
            _eyeShiftRightYList.Add(eyesParams.RightGazeY.Value);
        }

        int[] counts =
        {
            _eyeShiftLeftXList.Count, _eyeShiftLeftYList.Count, _eyeShiftRightXList.Count, _eyeShiftRightYList.Count
        };

        int currentSamples = counts.Max();

        if (currentSamples >= AutoCalibrationAverageSampleCount)
        {
            float eyeShiftLeftXAvg = _eyeShiftLeftXList.Count > 0 ? _eyeShiftLeftXList.Average() : 0;
            float eyeShiftLeftYAvg = _eyeShiftLeftYList.Count > 0 ? _eyeShiftLeftYList.Average() : 0;
            float eyeShiftRightXAvg = _eyeShiftRightXList.Count > 0 ? _eyeShiftRightXList.Average() : 0;
            float eyeShiftRightYAvg = _eyeShiftRightYList.Count > 0 ? _eyeShiftRightYList.Average() : 0;

            EyeShiftLeft = new Vector2(eyeShiftLeftXAvg, eyeShiftLeftYAvg);
            EyeShiftRight = new Vector2(eyeShiftRightXAvg, eyeShiftRightYAvg);

            _module.MeowLogger.LogInformation(
                "Eyes auto center calibration success. EyeShiftLeft: {}, EyeShiftRight: {}", EyeShiftLeft,
                EyeShiftRight);

            StopCalibration();
        }
        else if (currentSamples % (AutoCalibrationAverageSampleCount / 5) == 0)
        {
            _module.MeowLogger.LogInformation("Eyes auto center calibration process {}%",
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
        _eyeShiftLeftXList = new List<float>();
        _eyeShiftLeftYList = new List<float>();
        _eyeShiftRightXList = new List<float>();
        _eyeShiftRightYList = new List<float>();

        _module.ConfigManager.SaveConfigAsync();
    }
}