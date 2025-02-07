using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class LipAndMouthMapper : MapperCft
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperUpRight,
            MeowFaceParam.MouthUpperUpRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperUpLeft,
            MeowFaceParam.MouthUpperUpLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthLowerDownRight,
            MeowFaceParam.MouthLowerDownRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthLowerDownLeft,
            MeowFaceParam.MouthLowerDownLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthFrownRight,
            MeowFaceParam.MouthFrownRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthFrownLeft,
            MeowFaceParam.MouthFrownLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthRaiserUpper,
            MeowFaceParam.MouthShrugUpper);

        // Blend
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthCornerPullRight,
            MeowFaceParam.MouthSmileRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthCornerSlantRight,
            MeowFaceParam.MouthSmileRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthCornerPullLeft,
            MeowFaceParam.MouthSmileLeft);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthCornerSlantLeft,
            MeowFaceParam.MouthSmileLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperRight,
            MeowFaceParam.MouthRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthLowerRight,
            MeowFaceParam.MouthRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperLeft,
            MeowFaceParam.MouthLeft);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthLowerLeft,
            MeowFaceParam.MouthLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipSuckLowerRight,
            MeowFaceParam.MouthRollLower);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipSuckLowerLeft,
            MeowFaceParam.MouthRollLower);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipSuckUpperRight,
            MeowFaceParam.MouthRollUpper);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipSuckUpperLeft,
            MeowFaceParam.MouthRollUpper);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipFunnelUpperRight,
            MeowFaceParam.MouthFunnel);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipFunnelUpperLeft,
            MeowFaceParam.MouthFunnel);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipFunnelLowerRight,
            MeowFaceParam.MouthFunnel);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipFunnelLowerLeft,
            MeowFaceParam.MouthFunnel);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipPuckerUpperRight,
            MeowFaceParam.MouthPucker);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipPuckerUpperLeft,
            MeowFaceParam.MouthPucker);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipPuckerLowerRight,
            MeowFaceParam.MouthPucker);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.LipPuckerLowerLeft,
            MeowFaceParam.MouthPucker);

        // Not standard, need to test, from https://github.com/regzo2/VRCFaceTracking-MeowFace/blob/master/MeowFaceExtTrackingInterface/MeowFaceExtTrackingInterface.cs
        /*meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthRaiserLower, MeowFaceParam.MouthShrugUpper);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperDeepenRight, MeowFaceParam.MouthUpperUpRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthUpperDeepenLeft, MeowFaceParam.MouthUpperUpLeft);

        float? mouthSmileRight = meowFaceParam.GetShape(MeowFaceParam.MouthSmileRight);
        float? mouthSmileLeft = meowFaceParam.GetShape(MeowFaceParam.MouthSmileLeft);

        float? mouthFrownRight = meowFaceParam.GetShape(MeowFaceParam.MouthFrownRight);
        float? mouthFrownLeft = meowFaceParam.GetShape(MeowFaceParam.MouthFrownLeft);

        if (mouthSmileRight != null) UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthDimpleRight].Weight = mouthSmileRight.Value * 0.5f;
        if (mouthSmileLeft != null) UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthDimpleLeft].Weight = mouthSmileLeft.Value * 0.5f;

        if (mouthFrownRight != null) UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthStretchRight].Weight = mouthFrownRight.Value * 0.5f;
        if (mouthFrownLeft != null) UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthStretchLeft].Weight = mouthFrownLeft.Value * 0.5f;*/
    }
}