using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking.Core.Params.Expressions;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public class CheekMapper : IMapperCft
    {
        public bool CheekSquintFromMouthSmile { get; set; } = false;

        public void UpdateExpression(MeowFaceParam meowFaceParam)
        {
            // Blend
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekPuffRight, MeowFaceParam.CheekPuff);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekPuffLeft, MeowFaceParam.CheekPuff);

            // Simulated shapes
            if (CheekSquintFromMouthSmile)
            {
                meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekSquintRight, MeowFaceParam.MouthSmileRight);
                meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekSquintLeft, MeowFaceParam.MouthSmileLeft);
            }
        }
    }
}
