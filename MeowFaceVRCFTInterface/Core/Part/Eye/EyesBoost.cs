namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesBoost
{
    public float BoostX { get; init; } = 1f;
    public float BoostY { get; init; } = 1f;

    public void Update(EyesParams eyesParams)
    {
        eyesParams.LeftGazeX *= BoostX;
        eyesParams.RightGazeX *= BoostX;

        eyesParams.LeftGazeY *= BoostY;
        eyesParams.RightGazeY *= BoostY;
    }
}