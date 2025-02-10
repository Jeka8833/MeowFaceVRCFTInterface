using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class JawMapper : MapperBase
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawRight,
            MeowFaceParam.JawRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawLeft,
            MeowFaceParam.JawLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawOpen,
            MeowFaceParam.JawOpen);
    }
}