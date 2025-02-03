using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public class JawMapper : IMapperCft
    {
        public void UpdateExpression(MeowFaceParam meowFaceParam)
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawRight, MeowFaceParam.JawRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawLeft, MeowFaceParam.JawLeft);

            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.JawOpen, MeowFaceParam.JawOpen);
        }
    }
}
