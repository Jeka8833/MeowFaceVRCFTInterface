namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class HeadBoost
{
    public float BoostX { get; init; } = 1f;
    public float BoostY { get; init; } = 1f;
    public float BoostZ { get; init; } = 1f;

    public float BoostPitch { get; init; } = 4f;
    public float BoostRoll { get; init; } = 2f;
    public float BoostYaw { get; init; } = 2f;

    public void Update(HeadParams headParams)
    {
        headParams.PosX *= BoostX;
        headParams.PosY *= BoostY;
        headParams.PosZ *= BoostZ;

        headParams.Pitch *= BoostPitch;
        headParams.Roll *= BoostRoll;
        headParams.Yaw *= BoostYaw;
    }
}