using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class MeowFaceHeadParams
{
    public bool EnablePositionX { get; init; } = true;
    public bool EnablePositionY { get; init; } = true;
    public bool EnablePositionZ { get; init; } = true;

    public bool EnableRotationPitch { get; init; } = true;
    public bool EnableRotationRoll { get; init; } = true;
    public bool EnableRotationYaw { get; init; } = true;

    public HeadParams ToHeadParams(MeowFaceParam meowFaceParam)
    {
        HeadParams headParams = new();

        if (meowFaceParam.HeadPosition.HasValue)
        {
            if (EnablePositionX)
            {
                headParams.HeadPosX = (meowFaceParam.HeadPosition.Value.X - 0.5f) * 2f;
            }

            if (EnablePositionY)
            {
                headParams.HeadPosY = (meowFaceParam.HeadPosition.Value.Y - 0.5f) * 2f;
            }

            if (EnablePositionZ)
            {
                headParams.HeadPosZ = (meowFaceParam.HeadPosition.Value.Z - 0.5f) * 2f;
            }
        }

        if (meowFaceParam.HeadRotation.HasValue)
        {
            if (EnableRotationPitch)
            {
                headParams.HeadPitch = meowFaceParam.HeadRotation.Value.Y / 180f;
            }

            if (EnableRotationRoll)
            {
                headParams.HeadRoll = meowFaceParam.HeadRotation.Value.Z / 180f;
            }

            if (EnableRotationYaw)
            {
                headParams.HeadYaw = meowFaceParam.HeadRotation.Value.X / 180f;
            }
        }

        return headParams;
    }
}