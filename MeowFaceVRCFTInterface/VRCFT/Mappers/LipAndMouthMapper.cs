using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class LipAndMouthMapper : MapperBase
{
    public bool MouthUpperDeepenFromMouthUpperUp { get; init; } = false;
    public float MouthDimpleFromMouthSmile { get; init; } = 0f;
    public float MouthStretchFromMouthFrown { get; init; } = 0f;

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

        // Simulated shapes, from https://github.com/regzo2/VRCFaceTracking-MeowFace/blob/master/MeowFaceExtTrackingInterface/MeowFaceExtTrackingInterface.cs

        //meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.MouthRaiserLower, MeowFaceParam.MouthShrugUpper);

        if (MouthUpperDeepenFromMouthUpperUp)
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes,
                UnifiedExpressions.MouthUpperDeepenRight, MeowFaceParam.MouthUpperUpRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes,
                UnifiedExpressions.MouthUpperDeepenLeft, MeowFaceParam.MouthUpperUpLeft);
        }

        if (MouthDimpleFromMouthSmile is > 0f and <= 1f)
        {
            float? mouthSmileRight =
                meowFaceParam.GetShape(MeowFaceParam.MouthSmileRight) * MouthDimpleFromMouthSmile;
            float? mouthSmileLeft =
                meowFaceParam.GetShape(MeowFaceParam.MouthSmileLeft) * MouthDimpleFromMouthSmile;

            if (mouthSmileRight != null)
            {
                UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthDimpleRight].Weight = mouthSmileRight.Value;
            }

            if (mouthSmileLeft != null)
            {
                UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthDimpleLeft].Weight = mouthSmileLeft.Value;
            }
        }

        if (MouthStretchFromMouthFrown is > 0f and <= 1f)
        {
            float? mouthFrownRight =
                meowFaceParam.GetShape(MeowFaceParam.MouthFrownRight) * MouthStretchFromMouthFrown;
            float? mouthFrownLeft =
                meowFaceParam.GetShape(MeowFaceParam.MouthFrownLeft) * MouthStretchFromMouthFrown;

            if (mouthFrownRight != null)
            {
                UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthStretchRight].Weight = mouthFrownRight.Value;
            }

            if (mouthFrownLeft != null)
            {
                UnifiedTracking.Data.Shapes[(int)UnifiedExpressions.MouthStretchLeft].Weight = mouthFrownLeft.Value;
            }
        }
    }
}