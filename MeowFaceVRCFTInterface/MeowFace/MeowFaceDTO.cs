namespace MeowFaceVRCFTInterface.MeowFace
{
    public record MeowFaceDTO
    {
        /**
         * Not used in this program
         * 
         * public int Hotkey { get; init; }
         * public bool FaceFound { get; init; }
         * public MeowVector Rotation { get; init; }
         * public MeowVector Position { get; init; }
         * public MeowVector VNyanPos { get; init; }
         */

        public long Timestamp { get; init; } = 0;
        public MeowVector EyeLeft { get; init; } = MeowVector.EmptyValue;
        public MeowVector EyeRight { get; init; } = MeowVector.EmptyValue;
        public MeowShape[] BlendShapes { get; init; } = Array.Empty<MeowShape>();
    }

#pragma warning disable IDE1006 // Naming Styles
    public record struct MeowVector(float x, float y/*, float z*/) // z is not used in this program
#pragma warning restore IDE1006 // Naming Styles
    {
        public static readonly MeowVector EmptyValue = new(float.NaN, float.NaN);

        public readonly bool IsValid() => float.IsFinite(x) && float.IsFinite(y);
    }

#pragma warning disable IDE1006 // Naming Styles
    public record struct MeowShape(string k, float v)
#pragma warning restore IDE1006 // Naming Styles
    {
        public readonly bool IsValid() => k != null && float.IsFinite(v) && v >= 0;
    }
}
