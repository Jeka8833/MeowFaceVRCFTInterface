using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.Core.Part.Head;
using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public class HeadPositionAndRotationMapper : MapperBase
{
    public MeowFaceHeadParams Source { get; init; } = new();
    public HeadCenterCalibration CenterCalibration { get; init; } = new();
    public HeadBoost Boost { get; init; } = new();

    public override void Initialize(MeowFaceVRCFTInterface module)
    {
#pragma warning disable CS1717 // Crash and disable mapper if HeadPosX is absent
        UnifiedTracking.Data.Head.HeadPosX = UnifiedTracking.Data.Head.HeadPosX;
#pragma warning restore CS1717

        CenterCalibration.Initialize(module);
    }

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        HeadParams headParams = Source.ToHeadParams(meowFaceParam);
        CenterCalibration.UseCalibrationOrCalibrate(headParams);
        Boost.Update(headParams);
        HeadClamp.Clamp(headParams);

        if (headParams.HeadPosX.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosX = headParams.HeadPosX.Value;
        }

        if (headParams.HeadPosY.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosY = headParams.HeadPosY.Value;
        }

        if (headParams.HeadPosZ.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosZ = headParams.HeadPosZ.Value;
        }

        if (headParams.HeadPitch.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPitch = headParams.HeadPitch.Value;
        }

        if (headParams.HeadRoll.HasValue)
        {
            UnifiedTracking.Data.Head.HeadRoll = headParams.HeadRoll.Value;
        }

        if (headParams.HeadYaw.HasValue)
        {
            UnifiedTracking.Data.Head.HeadYaw = headParams.HeadYaw.Value;
        }
    }

    public override void Dispose()
    {
        CenterCalibration.Dispose();
    }
}