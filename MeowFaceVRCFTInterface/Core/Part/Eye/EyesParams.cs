namespace MeowFaceVRCFTInterface.Core.Part.Eye;

// Openness [0, 1]
// Gaze [-1, 1] -> [-45d, 45d]
public class EyesParams
{
    public float? LeftOpenness;
    public float? LeftGazeX;
    public float? LeftGazeY;

    public float? RightOpenness;
    public float? RightGazeX;
    public float? RightGazeY;
}