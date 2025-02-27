using MeowFaceVRCFTInterface.Core;
using MeowFaceVRCFTInterface.Core.Part.Head;
using MeowFaceVRCFTInterface.Core.Part.Head.CenterCalibration;
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

        if (headParams.PosX.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosX = headParams.PosX.Value;
        }

        if (headParams.PosY.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosY = headParams.PosY.Value;
        }

        if (headParams.PosZ.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPosZ = headParams.PosZ.Value;
        }

        if (headParams.Pitch.HasValue)
        {
            UnifiedTracking.Data.Head.HeadPitch = headParams.Pitch.Value;
        }

        if (headParams.Roll.HasValue)
        {
            UnifiedTracking.Data.Head.HeadRoll = headParams.Roll.Value;
        }

        if (headParams.Yaw.HasValue)
        {
            UnifiedTracking.Data.Head.HeadYaw = headParams.Yaw.Value;
        }
    }

    public override void Dispose()
    {
        CenterCalibration.Dispose();
    }
}