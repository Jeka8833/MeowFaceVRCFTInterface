namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class HeadBoost
{
    public float X { get; init; } = 2f;
    public float Y { get; init; } = 2f;
    public float Z { get; init; } = 2f;

    public float Pitch { get; init; } = 3f;
    public float Roll { get; init; } = 1f;
    public float Yaw { get; init; } = 2f;

    public void Update(HeadParams headParams)
    {
        headParams.PosX *= X;
        headParams.PosY *= Y;
        headParams.PosZ *= Z;

        headParams.Pitch *= Pitch;
        headParams.Roll *= Roll;
        headParams.Yaw *= Yaw;
    }
}