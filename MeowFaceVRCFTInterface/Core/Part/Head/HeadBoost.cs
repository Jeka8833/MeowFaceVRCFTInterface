namespace MeowFaceVRCFTInterface.Core.Part.Head;

public class HeadBoost
{
    public float BoostX { get; set; } = 1f;
    public float BoostY { get; set; } = 1f;
    public float BoostZ { get; set; } = 1f;

    public float BoostPitch { get; set; } = 1f;
    public float BoostRoll { get; set; } = 1f;
    public float BoostYaw { get; set; } = 1f;

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