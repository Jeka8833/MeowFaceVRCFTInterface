using System.Numerics;
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

            Even if it is called Normalized, the values can go beyond -0.6 and 1.6 ((((
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

        // new C0747h("headLeft", Float.valueOf(Math.max(0.0f, m737r.getY()) / 1.5707964f)),
        // new C0747h("headRight", Float.valueOf((-Math.min(0.0f, m737r.getY())) / 1.5707964f)),
        // new C0747h("headUp", Float.valueOf((-Math.min(0.0f, m737r.getX())) / 1.5707964f)),
        // new C0747h("headDown", Float.valueOf(Math.max(0.0f, m737r.getX()) / 1.5707964f)),
        // new C0747h("headRollLeft", Float.valueOf((-Math.min(0.0f, m737r.getZ())) / 1.5707964f)),
        // new C0747h("headRollRight", Float.valueOf(Math.max(0.0f, m737r.getZ()) / 1.5707964f)));

        // euler_rotation_ranges: [45, 88, 40]  -> Max Value: [0.5, 0.98, 0.45] if Weight: 1
        float? rotX = meowFaceParam.SubtractShapes(MeowFaceParam.HeadDown, MeowFaceParam.HeadUp);
        float? rotY = meowFaceParam.SubtractShapes(MeowFaceParam.HeadLeft, MeowFaceParam.HeadRight);
        float? rotZ = meowFaceParam.SubtractShapes(MeowFaceParam.HeadRollRight, MeowFaceParam.HeadRollLeft);

        if (rotX.HasValue || rotY.HasValue || rotZ.HasValue)
        {
            /* MeowFace Code:
                public final Vec3 toEuler() {
                    float x = (float) Math.atan2(2 * ((getR() * getI()) + (getJ() * getK())), 1 - (2 * ((getI() * getI()) + (getJ() * getJ()))));
                    float s = 2 * ((getR() * getJ()) - (getK() * getI()));
                    float y = Math.abs(s) >= 1.0f ? Math.signum(s) * 2 * ((float) 3.141592653589793d) : (float) Math.asin(s);
                    float z = (float) Math.atan2(2 * ((getR() * getK()) + (getI() * getJ())), 1 - (2 * ((getJ() * getJ()) + (getK() * getK()))));
                    return new Vec3(x, y, z);
                }
            */
            Vector3 rotZxy = MathUtil.EulerZYX2ZXYOneScale(new Vector3(rotX ?? 0f, rotY ?? 0f, rotZ ?? 0f));

            headParams.Pitch = rotZxy.X;
            headParams.Roll = rotZxy.Y;
            headParams.Yaw = rotZxy.Z;
        }

        return headParams;
    }
}