namespace MeowFaceVRCFTInterface.Core.Part.Eye.CenterCalibration;

public class EyesValueStorage
{
    public float LeftGazeX { get; init; }
    public float LeftGazeY { get; init; }

    public float RightGazeX { get; init; }
    public float RightGazeY { get; init; }

    public override string ToString()
    {
        return $"{nameof(LeftGazeX)}: {LeftGazeX}, {nameof(LeftGazeY)}: {LeftGazeY}, " +
               $"{nameof(RightGazeX)}: {RightGazeX}, {nameof(RightGazeY)}: {RightGazeY}";
    }

    public static EyesValueStorage Average(List<EyesValueStorage> list)
    {
        double leftEyeGazeX = 0d;
        int leftEyeGazeXCount = 0;

        double leftEyeGazeY = 0d;
        int leftEyeGazeYCount = 0;

        double rightEyeGazeX = 0d;
        int rightEyeGazeXCount = 0;

        double rightEyeGazeY = 0d;
        int rightEyeGazeYCount = 0;

        foreach (EyesValueStorage valueStorage in list)
        {
            if (float.IsFinite(valueStorage.LeftGazeX))
            {
                leftEyeGazeX += valueStorage.LeftGazeX;
                leftEyeGazeXCount++;
            }

            if (float.IsFinite(valueStorage.LeftGazeY))
            {
                leftEyeGazeY += valueStorage.LeftGazeY;
                leftEyeGazeYCount++;
            }

            if (float.IsFinite(valueStorage.RightGazeX))
            {
                rightEyeGazeX += valueStorage.RightGazeX;
                rightEyeGazeXCount++;
            }

            if (float.IsFinite(valueStorage.RightGazeY))
            {
                rightEyeGazeY += valueStorage.RightGazeY;
                rightEyeGazeYCount++;
            }
        }

        return new EyesValueStorage
        {
            LeftGazeX = leftEyeGazeXCount > 0 ? (float)(leftEyeGazeX / leftEyeGazeXCount) : 0f,
            LeftGazeY = leftEyeGazeYCount > 0 ? (float)(leftEyeGazeY / leftEyeGazeYCount) : 0f,
            RightGazeX = rightEyeGazeXCount > 0 ? (float)(rightEyeGazeX / rightEyeGazeXCount) : 0f,
            RightGazeY = rightEyeGazeYCount > 0 ? (float)(rightEyeGazeY / rightEyeGazeYCount) : 0f
        };
    }
}