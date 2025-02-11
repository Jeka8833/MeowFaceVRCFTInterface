namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesBoost
{
    private const float BoostXDefault = 1.5f;
    private const float BoostYDefault = 2f;

    public float BoostX { get; set; } = BoostXDefault;
    public float BoostY { get; set; } = BoostYDefault;

    public void Update(EyesParams eyesParams)
    {
        eyesParams.LeftGazeX *= BoostX;
        eyesParams.RightGazeX *= BoostX;

        eyesParams.LeftGazeY *= BoostY;
        eyesParams.RightGazeY *= BoostY;
    }
}