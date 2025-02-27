using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.Core.Part.Eye;
using MeowFaceVRCFTInterface.Core.Part.Eye.CenterCalibration;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class EyeMapper : MapperBase
{
    public MeowFaceEyesParams Source { get; init; } = new();
    public EyesCenterCalibration CenterCalibration { get; init; } = new();
    public EyesBoost Boost { get; init; } = new();
    public EyesFocusRange FocusRange { get; init; } = new();

    public override void Initialize(MeowFaceVRCFTInterface module)
    {
        CenterCalibration.Initialize(module);
        FocusRange.Initialize(module);
    }

    public override void UpdateEye(MeowFaceParam meowFaceParam)
    {
        EyesParams eyesParams = Source.ToEyesParams(meowFaceParam);
        CenterCalibration.UseCalibrationOrCalibrate(eyesParams);
        Boost.Update(eyesParams);
        FocusRange.Update(eyesParams);
        EyesClamp.Clamp(eyesParams);

        if (eyesParams.LeftGazeX.HasValue)
        {
            UnifiedTracking.Data.Eye.Left.Gaze.x = eyesParams.LeftGazeX.Value;
        }

        if (eyesParams.RightGazeX.HasValue)
        {
            UnifiedTracking.Data.Eye.Right.Gaze.x = eyesParams.RightGazeX.Value;
        }

        if (eyesParams.LeftGazeY.HasValue)
        {
            UnifiedTracking.Data.Eye.Left.Gaze.y = eyesParams.LeftGazeY.Value;
        }

        if (eyesParams.RightGazeY.HasValue)
        {
            UnifiedTracking.Data.Eye.Right.Gaze.y = eyesParams.RightGazeY.Value;
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
        CenterCalibration.Dispose();
    }
}