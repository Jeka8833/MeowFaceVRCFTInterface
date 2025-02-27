namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public static class EyesClamp
{
    public static void Clamp(EyesParams eyesParams)
    {
        eyesParams.LeftOpenness = MathUtil.ClampFloat(eyesParams.LeftOpenness, 0f);
        eyesParams.LeftGazeX = MathUtil.ClampFloat(eyesParams.LeftGazeX);
        eyesParams.LeftGazeY = MathUtil.ClampFloat(eyesParams.LeftGazeY);

        eyesParams.RightOpenness = MathUtil.ClampFloat(eyesParams.RightOpenness, 0f);
        eyesParams.RightGazeX = MathUtil.ClampFloat(eyesParams.RightGazeX);
        eyesParams.RightGazeY = MathUtil.ClampFloat(eyesParams.RightGazeY);
    }
}