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
            /*
            RectBuilder normalizedRect = new RectBuilder(rect);
            normalizedRect.setX(normalizedRect.getX() / image.getWidth());
            normalizedRect.setY(normalizedRect.getY() / image.getHeight());
            normalizedRect.setWidth(normalizedRect.getWidth() / image.getWidth());
            normalizedRect.setHeight(normalizedRect.getHeight() / image.getHeight());

            Vec2 normalizedImagePosition = headPos.getXy().times(normalizedRect.getExtent()).plus(normalizedRect.getOrigin());

            Even if it is called Normalized, the values can go beyond -0.6 and 1.6??? ((((
            */

            if (EnablePositionX)
            {
                headParams.PosX = meowFaceParam.HeadPosition.Value.X;
            }

            if (EnablePositionY)
            {
                headParams.PosY = meowFaceParam.HeadPosition.Value.Y;
            }

            if (EnablePositionZ)
            {
                headParams.PosZ = meowFaceParam.HeadPosition.Value.Z;
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