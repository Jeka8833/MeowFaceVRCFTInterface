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
        headParams.HeadPosX *= BoostX;
        headParams.HeadPosY *= BoostY;
        headParams.HeadPosZ *= BoostZ;

        headParams.HeadPitch *= BoostPitch;
        headParams.HeadRoll *= BoostRoll;
        headParams.HeadYaw *= BoostYaw;
    }
}