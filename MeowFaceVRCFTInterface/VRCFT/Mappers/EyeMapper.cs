using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.Core.Part.Eye;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class EyeMapper : MapperBase
{
    public bool EnableEyeGazeX { get; init; } = true;
    public bool EnableEyeGazeY { get; init; } = true;

    public MeowFaceEyesParams Source { get; init; } = new();
    public EyesCenterCalibration CalibrateCenter { get; init; } = new();

    public override void Initialize(MeowFaceVRCFTInterface module)
    {
        CalibrateCenter.Initialize(module);
    }

    public override void UpdateEye(MeowFaceParam meowFaceParam)
    {
        EyesParams eyesParams = Source.ToEyesParams(meowFaceParam);
        CalibrateCenter.UseCalibrationOrCalibrate(eyesParams);

        if (EnableEyeGazeX)
        {
            if (eyesParams.LeftGazeX.HasValue)
            {
                UnifiedTracking.Data.Eye.Left.Gaze.x = eyesParams.LeftGazeX.Value;
            }

            if (eyesParams.RightGazeX.HasValue)
            {
                UnifiedTracking.Data.Eye.Right.Gaze.x = eyesParams.RightGazeX.Value;
            }
        }

        if (EnableEyeGazeY)
        {
            if (eyesParams.LeftGazeY.HasValue)
            {
                UnifiedTracking.Data.Eye.Left.Gaze.y = eyesParams.LeftGazeY.Value;
            }

            if (eyesParams.RightGazeY.HasValue)
            {
                UnifiedTracking.Data.Eye.Right.Gaze.y = eyesParams.RightGazeY.Value;
            }
        }

        if (eyesParams.LeftOpenness.HasValue)
        {
            UnifiedTracking.Data.Eye.Left.Openness = eyesParams.LeftOpenness.Value;
        }

        if (eyesParams.RightOpenness.HasValue)
        {
            UnifiedTracking.Data.Eye.Right.Openness = eyesParams.RightOpenness.Value;
        }
    }

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintRight,
            MeowFaceParam.EyeSquintRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintLeft,
            MeowFaceParam.EyeSquintLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideRight,
            MeowFaceParam.EyeWideRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideLeft,
            MeowFaceParam.EyeWideLeft);
    }

    public override void Dispose()
    {
        CalibrateCenter.Dispose();
    }
}