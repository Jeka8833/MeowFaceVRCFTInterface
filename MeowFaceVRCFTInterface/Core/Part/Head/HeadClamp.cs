namespace MeowFaceVRCFTInterface.Core.Part.Head;

public static class HeadClamp
{
    public static void Clamp(HeadParams headData)
    {
        headData.PosX = MathUtil.ClampFloat(headData.PosX);
        headData.PosY = MathUtil.ClampFloat(headData.PosY);
        headData.PosZ = MathUtil.ClampFloat(headData.PosZ);

        headData.Pitch = MathUtil.ClampFloat(headData.Pitch);
        headData.Roll = MathUtil.ClampFloat(headData.Roll);
        headData.Yaw = MathUtil.ClampFloat(headData.Yaw);
    }
}