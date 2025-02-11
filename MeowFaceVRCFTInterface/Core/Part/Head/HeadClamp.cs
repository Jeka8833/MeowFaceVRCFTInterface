namespace MeowFaceVRCFTInterface.Core.Part.Head;

public static class HeadClamp
{
    public static void Clamp(HeadParams headData)
    {
        if (headData.HeadPosX.HasValue)
        {
            headData.HeadPosX = !float.IsFinite(headData.HeadPosX.Value)
                ? null
                : Math.Clamp(headData.HeadPosX.Value, -1f, 1f);
        }

        if (headData.HeadPosY.HasValue)
        {
            headData.HeadPosY = !float.IsFinite(headData.HeadPosY.Value)
                ? null
                : Math.Clamp(headData.HeadPosY.Value, -1f, 1f);
        }

        if (headData.HeadPosZ.HasValue)
        {
            headData.HeadPosZ = !float.IsFinite(headData.HeadPosZ.Value)
                ? null
                : Math.Clamp(headData.HeadPosZ.Value, -1f, 1f);
        }


        if (headData.HeadPitch.HasValue)
        {
            headData.HeadPitch = !float.IsFinite(headData.HeadPitch.Value)
                ? null
                : Math.Clamp(headData.HeadPitch.Value, -1f, 1f);
        }

        if (headData.HeadRoll.HasValue)
        {
            headData.HeadRoll = !float.IsFinite(headData.HeadRoll.Value)
                ? null
                : Math.Clamp(headData.HeadRoll.Value, -1f, 1f);
        }

        if (headData.HeadYaw.HasValue)
        {
            headData.HeadYaw = !float.IsFinite(headData.HeadYaw.Value)
                ? null
                : Math.Clamp(headData.HeadYaw.Value, -1f, 1f);
        }
    }
}