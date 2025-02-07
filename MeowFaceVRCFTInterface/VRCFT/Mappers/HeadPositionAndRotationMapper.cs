using MeowFaceVRCFTInterface.MeowFace;
using Microsoft.Extensions.Logging;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class HeadPositionAndRotationMapper : MapperCft
{
    private const float RadianConst = 0.01745329251994329576923690768488612713442871888541725456097191440171009114f;

    private MeowFaceVRCFTInterface _module = null!;

    public override void Initialize(MeowFaceVRCFTInterface module)
    {
        _module = module;

        UnifiedTracking.Data.Head.HeadPosX = UnifiedTracking.Data.Head.HeadPosX; // Disable module if VRCFT is old
    }

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        if (meowFaceParam.HeadPos.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosX = meowFaceParam.HeadPos.Value.X;
            UnifiedTracking.Data.Head.HeadPosY = meowFaceParam.HeadPos.Value.Y;
            UnifiedTracking.Data.Head.HeadPosZ = meowFaceParam.HeadPos.Value.Z;
        }

        if (meowFaceParam.HeadRotation.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPitch = meowFaceParam.HeadRotation.Value.Y * RadianConst;
            UnifiedTracking.Data.Head.HeadRoll = meowFaceParam.HeadRotation.Value.Z * RadianConst;
            UnifiedTracking.Data.Head.HeadYaw = meowFaceParam.HeadRotation.Value.X * RadianConst;
        }

        _module.MeowSpamLogger.LogInformation("HeadPosX: {}, HeadPosY: {}, HeadPosZ: {}, HeadPitch: {}, HeadRoll: {}," +
                                              " HeadYaw: {}",
            UnifiedTracking.Data.Head.HeadPosX, UnifiedTracking.Data.Head.HeadPosY, UnifiedTracking.Data.Head.HeadPosZ,
            UnifiedTracking.Data.Head.HeadPitch, UnifiedTracking.Data.Head.HeadRoll, UnifiedTracking.Data.Head.HeadYaw);
    }
}