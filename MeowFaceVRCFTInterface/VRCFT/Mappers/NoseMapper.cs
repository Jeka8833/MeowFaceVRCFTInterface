﻿using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class NoseMapper : MapperBase
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.NoseSneerRight,
            MeowFaceParam.NoseSneerRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.NoseSneerLeft,
            MeowFaceParam.NoseSneerLeft);
    }
}