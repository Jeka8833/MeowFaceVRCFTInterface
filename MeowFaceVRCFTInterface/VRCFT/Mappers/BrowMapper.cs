﻿using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class BrowMapper : MapperBase
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
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