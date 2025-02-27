using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class MeowFaceHeadParams
{
    public bool EnablePositionX { get; init; } = true;
    public bool EnablePositionY { get; init; } = true;
    public bool EnablePositionZ { get; init; } = true;

    public HeadParams ToHeadParams(MeowFaceParam meowFaceParam)
    {
        HeadParams headParams = new();

        if (meowFaceParam.HeadPosition.HasValue)
        {
            // FusedMultiplyAdd more predictable, if MeowFace make something crazy

            if (EnablePositionX)
            {
                headParams.PosX = float.FusedMultiplyAdd(
                    meowFaceParam.HeadPosition.Value.X, 2f, -1f);
            }

            if (EnablePositionY)
            {
                headParams.PosY = float.FusedMultiplyAdd(
                    meowFaceParam.HeadPosition.Value.Y, 2f, -1f);
            }

            if (EnablePositionZ)
            {
                headParams.PosZ = float.FusedMultiplyAdd(
                    meowFaceParam.HeadPosition.Value.Z, 2f, -1f);
            }
        }

        // euler_rotation_ranges: [45, 88, 40]  -> Max Value: [0.5, 0.98, 0.45]???
        // headParams.HeadPitch -> meowFaceParam.Y
        // headParams.HeadRoll -> meowFaceParam.Z
        // headParams.HeadYaw -> meowFaceParam.X
        headParams.Pitch = meowFaceParam.SubtractShapes(MeowFaceParam.HeadDown, MeowFaceParam.HeadUp);
        headParams.Roll = meowFaceParam.SubtractShapes(MeowFaceParam.HeadRollRight, MeowFaceParam.HeadRollLeft);
        headParams.Yaw = meowFaceParam.SubtractShapes(MeowFaceParam.HeadLeft, MeowFaceParam.HeadRight);

        return headParams;
    }
}