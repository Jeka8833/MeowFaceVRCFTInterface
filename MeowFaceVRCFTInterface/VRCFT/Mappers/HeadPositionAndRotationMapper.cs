using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.MeowFace;
using Microsoft.Extensions.Logging;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class HeadPositionAndRotationMapper : MapperBase
{
    private const float UnitConst = 1f / 180f;

    private MeowFaceVRCFTInterface _module = null!;

    public override void Initialize(MeowFaceVRCFTInterface module)
    {
        _module = module;

        UnifiedTracking.Data.Head.HeadPosX = UnifiedTracking.Data.Head.HeadPosX; // Disable module if VRCFT is old
    }

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        if (meowFaceParam.HeadPosition.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosX = meowFaceParam.HeadPosition.Value.X;
            UnifiedTracking.Data.Head.HeadPosY = meowFaceParam.HeadPosition.Value.Y;
            UnifiedTracking.Data.Head.HeadPosZ = meowFaceParam.HeadPosition.Value.Z;
        }

        if (meowFaceParam.HeadRotation.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPitch = meowFaceParam.HeadRotation.Value.Y * UnitConst * 2;
            UnifiedTracking.Data.Head.HeadRoll = meowFaceParam.HeadRotation.Value.Z * UnitConst;
            UnifiedTracking.Data.Head.HeadYaw = meowFaceParam.HeadRotation.Value.X * UnitConst * 2;
        }

        _module.MeowSpamLogger.LogInformation(
            "HeadPosX: {}, HeadPosY: {}, HeadPosZ: {}, HeadPitch: {}, HeadRoll: {}, HeadYaw: {}",
            UnifiedTracking.Data.Head.HeadPosX, UnifiedTracking.Data.Head.HeadPosY, UnifiedTracking.Data.Head.HeadPosZ,
            UnifiedTracking.Data.Head.HeadPitch, UnifiedTracking.Data.Head.HeadRoll, UnifiedTracking.Data.Head.HeadYaw);
    }
}