namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesBoost
{
    public float X { get; init; } = 1f;
    public float Y { get; init; } = 1f;

    public void Update(EyesParams eyesParams)
    {
        eyesParams.LeftGazeX *= X;
        eyesParams.RightGazeX *= X;

        eyesParams.LeftGazeY *= Y;
        eyesParams.RightGazeY *= Y;
    }
}