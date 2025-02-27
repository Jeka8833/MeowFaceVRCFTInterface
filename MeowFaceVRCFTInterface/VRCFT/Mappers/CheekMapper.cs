using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class CheekMapper : MapperBase
{
    public bool CheekSquintFromMouthSmile { get; init; } = true;

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        // Blend
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekPuffRight,
            MeowFaceParam.CheekPuff);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekPuffLeft,
            MeowFaceParam.CheekPuff);

        // Simulated shapes, from https://github.com/regzo2/VRCFaceTracking-MeowFace/blob/master/MeowFaceExtTrackingInterface/MeowFaceExtTrackingInterface.cs
        if (CheekSquintFromMouthSmile)
        {
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekSquintRight,
                MeowFaceParam.MouthSmileRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.CheekSquintLeft,
                MeowFaceParam.MouthSmileLeft);
        }
    }
}