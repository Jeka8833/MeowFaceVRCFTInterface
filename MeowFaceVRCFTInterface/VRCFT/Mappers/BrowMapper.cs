using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class BrowMapper : MapperBase
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        if (meowFaceParam.GetShape(MeowFaceParam.BrowInnerUpRight) != null ||
            meowFaceParam.GetShape(MeowFaceParam.BrowInnerUpLeft) != null)  // MediaPipe
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpRight,
                MeowFaceParam.BrowInnerUpRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpLeft,
                MeowFaceParam.BrowInnerUpLeft);
        }
        else
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpRight,
                MeowFaceParam.BrowInnerUp);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.BrowInnerUpLeft,
                MeowFaceParam.BrowInnerUp); 
        }

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