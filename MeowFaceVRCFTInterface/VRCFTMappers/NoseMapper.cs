﻿using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public class NoseMapper : MapperCft
    {
        public override void UpdateExpression(MeowFaceParam meowFaceParam)
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.NoseSneerRight, MeowFaceParam.NoseSneerRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.NoseSneerLeft, MeowFaceParam.NoseSneerLeft);
        }
    }
}
