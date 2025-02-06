using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFTMappers;

public class BrowMapper : MapperCft
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        // Also MeowFaceParam.BrowInnerUp, maybe it can be used for getting more stable values
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpRight,
            MeowFaceParam.BrowInnerUpRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpLeft,
            MeowFaceParam.BrowInnerUpLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowOuterUpRight,
            MeowFaceParam.BrowOuterUpRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowOuterUpLeft,
            MeowFaceParam.BrowOuterUpLeft);

        // Blended
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowPinchRight,
            MeowFaceParam.BrowDownRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowLowererRight,
            MeowFaceParam.BrowDownRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowPinchLeft,
            MeowFaceParam.BrowDownLeft);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowLowererLeft,
            MeowFaceParam.BrowDownLeft);
    }
}