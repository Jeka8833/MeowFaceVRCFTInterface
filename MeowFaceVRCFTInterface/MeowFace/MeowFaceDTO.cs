using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.MeowFace;

public record MeowFaceDto
{
    /**
     * Not used in this program
     *
     * public int Hotkey { get; init; } // Always -1
     * public bool FaceFound { get; init; } // Always true
     * public MeowVector Position { get; init; } // Always 0 0 0
     */

    public long Timestamp { get; init; } = 0;

    public MeowVector EyeLeft { get; init; } = MeowVector.EmptyValue; // z is always 0
    public MeowVector EyeRight { get; init; } = MeowVector.EmptyValue; // z is always 0
    public MeowVector Rotation { get; init; } = MeowVector.EmptyValue;
    public MeowVector VNyanPos { get; init; } = MeowVector.EmptyValue;

    public MeowShape[] BlendShapes { get; init; } = Array.Empty<MeowShape>();
}

public readonly record struct MeowVector(
    [JsonProperty(PropertyName = "x")] float X,
    [JsonProperty(PropertyName = "y")] float Y,
    [JsonProperty(PropertyName = "z")] float Z
)
{
    public static readonly MeowVector EmptyValue = new(float.NaN, float.NaN, float.NaN);

    public bool IsValid() => float.IsFinite(X) && float.IsFinite(Y) && float.IsFinite(Z);
}

public readonly record struct MeowShape(
    [JsonProperty(PropertyName = "k")] string K,
    [JsonProperty(PropertyName = "v")] float V
)
{
    public bool IsValid() => K != null && float.IsFinite(V) && V >= 0;
}