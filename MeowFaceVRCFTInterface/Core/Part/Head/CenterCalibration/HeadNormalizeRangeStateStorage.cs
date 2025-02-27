namespace MeowFaceVRCFTInterface.Core.Part.Head.CenterCalibration;

public class HeadNormalizeRangeStateStorage
{
    public bool X { get; init; }
    public bool Y { get; init; }
    public bool Z { get; init; }

    public bool Pitch { get; init; } = true;
    public bool Roll { get; init; } = true;
    public bool Yaw { get; init; } = true;
}