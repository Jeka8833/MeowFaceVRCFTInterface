using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;
using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public class TongueMapper : IMapperCft
    {
        public void UpdateExpression(MeowFaceParam meowFaceParam)
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.TongueOut, MeowFaceParam.TongueOut);
        }
    }
}
