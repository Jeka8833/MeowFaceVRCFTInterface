using VRCFaceTracking.Core.Params.Data;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.MeowFace;

public readonly struct MeowFaceParam
{
    public const string BrowDownLeft = "browDownLeft";
    public const string BrowDownRight = "browDownRight";
    public const string BrowInnerUp = "browInnerUp";
    public const string BrowInnerUpLeft = "browInnerUpLeft";
    public const string BrowInnerUpRight = "browInnerUpRight";
    public const string BrowOuterUpLeft = "browOuterUpLeft";
    public const string BrowOuterUpRight = "browOuterUpRight";

    public const string CheekPuff = "cheekPuff";

    public const string EyeBlinkLeft = "eyeBlinkLeft";
    public const string EyeBlinkRight = "eyeBlinkRight";
    public const string EyeLookDownLeft = "eyeLookDownLeft";
    public const string EyeLookDownRight = "eyeLookDownRight";
    public const string EyeLookInLeft = "eyeLookInLeft";
    public const string EyeLookInRight = "eyeLookInRight";
    public const string EyeLookOutLeft = "eyeLookOutLeft";
    public const string EyeLookOutRight = "eyeLookOutRight";
    public const string EyeLookUpLeft = "eyeLookUpLeft";
    public const string EyeLookUpRight = "eyeLookUpRight";
    public const string EyeSquintLeft = "eyeSquintLeft";
    public const string EyeSquintRight = "eyeSquintRight";
    public const string EyeWideLeft = "eyeWideLeft";
    public const string EyeWideRight = "eyeWideRight";

    public const string HeadDown = "headDown";
    public const string HeadLeft = "headLeft";
    public const string HeadRight = "headRight";
    public const string HeadRollLeft = "headRollLeft";
    public const string HeadRollRight = "headRollRight";
    public const string HeadUp = "headUp";

    public const string JawLeft = "jawLeft";
    public const string JawOpen = "jawOpen";
    public const string JawRight = "jawRight";

    public const string MouthFrownLeft = "mouthFrownLeft";
    public const string MouthFrownRight = "mouthFrownRight";
    public const string MouthFunnel = "mouthFunnel";
    public const string MouthLeft = "mouthLeft";
    public const string MouthLowerDownLeft = "mouthLowerDownLeft";
    public const string MouthLowerDownRight = "mouthLowerDownRight";
    public const string MouthPucker = "mouthPucker";
    public const string MouthRight = "mouthRight";
    public const string MouthRollLower = "mouthRollLower";
    public const string MouthRollUpper = "mouthRollUpper";
    public const string MouthShrugUpper = "mouthShrugUpper";
    public const string MouthSmileLeft = "mouthSmileLeft";
    public const string MouthSmileRight = "mouthSmileRight";
    public const string MouthUpperUpLeft = "mouthUpperUpLeft";
    public const string MouthUpperUpRight = "mouthUpperUpRight";

    public const string NoseSneerLeft = "noseSneerLeft";
    public const string NoseSneerRight = "noseSneerRight";

    public const string TongueOut = "tongueOut";

    public MeowVector? EyeLeft { get; }
    public MeowVector? EyeRight { get; }
    public MeowVector? HeadPos { get; }
    public MeowVector? HeadRotation { get; }

    private readonly Dictionary<string, float> _shapeMap;

    public MeowFaceParam(MeowFaceDto meowDto)
    {
        if (meowDto.EyeLeft.IsValid())
        {
            EyeLeft = meowDto.EyeLeft;
        }

        if (meowDto.EyeRight.IsValid())
        {
            EyeRight = meowDto.EyeRight;
        }

        if (meowDto.VNyanPos.IsValid())
        {
            HeadPos = meowDto.VNyanPos;
        }

        if (meowDto.Rotation.IsValid())
        {
            HeadRotation = meowDto.Rotation;
        }

        _shapeMap = ToShapeMap(meowDto.BlendShapes);
    }

    public float? GetShape(string key, float maxValue = float.MaxValue)
    {
        return _shapeMap.TryGetValue(key, out float value) ? Math.Min(maxValue, value) : null;
    }

    public void TrySetToVrcftShape(UnifiedExpressionShape[] vrcftShape, UnifiedExpressions expression,
        string meowKey, float maxValue = float.MaxValue)
    {
        float? shape = GetShape(meowKey, maxValue);

        if (shape.HasValue)
        {
            vrcftShape[(int)expression].Weight = shape.Value;
        }
    }

    public override string ToString()
    {
        return "{EyeLeft: " + EyeLeft + ", EyeRight: " +
               EyeRight + ", _shapeMap: " + string.Join(", ", _shapeMap) + "}";
    }

    private static Dictionary<string, float> ToShapeMap(MeowShape[] meowShapes)
    {
        return meowShapes
            .Where(shape => shape.IsValid())
            .ToDictionary(shape => shape.K, shape => shape.V);
    }
}