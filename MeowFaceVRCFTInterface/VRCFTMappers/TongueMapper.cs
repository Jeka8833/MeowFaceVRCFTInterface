using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;
using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.VRCFTMappers;

public class TongueMapper : MapperCft
{
    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.TongueOut,
            MeowFaceParam.TongueOut);
    }
}