using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class MeowFaceEyesParams
{
    private const float UnitConst = 1f / 180f;

    public bool UseShapeForY { get; init; } = true;
    public bool HelpBlinkWithEyeSquint { get; init; } = true;

    public EyesParams ToEyesParams(MeowFaceParam meowFaceParam)
    {
        EyesParams eyesParams = new();

        eyesParams.LeftOpenness = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint, MeowFaceParam.EyeBlinkLeft,
            MeowFaceParam.EyeSquintLeft);
        eyesParams.RightOpenness = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint, MeowFaceParam.EyeBlinkRight,
            MeowFaceParam.EyeSquintRight);

        eyesParams.LeftGazeX = meowFaceParam.EyeLeft?.Y * UnitConst;
        eyesParams.RightGazeX = meowFaceParam.EyeRight?.Y * UnitConst;

        if (UseShapeForY)
        {
            eyesParams.LeftGazeY = GetEyeYShape(meowFaceParam, eyesParams.LeftOpenness, MeowFaceParam.EyeLookUpLeft,
                MeowFaceParam.EyeLookDownLeft);
            eyesParams.RightGazeY = GetEyeYShape(meowFaceParam, eyesParams.RightOpenness, MeowFaceParam.EyeLookUpRight,
                MeowFaceParam.EyeLookDownRight);
        }
        else
        {
            eyesParams.LeftGazeY = meowFaceParam.EyeLeft?.X * -UnitConst;
            eyesParams.RightGazeY = meowFaceParam.EyeRight?.X * -UnitConst;
        }

        return eyesParams;
    }

    private static float? GetEyeOpenness(MeowFaceParam meowFaceParam, bool helpBlinkWithEyeSquint,
        string meowEyeBlinkKey, string meowEyeSquintKey)
    {
        float? eyeBlink = meowFaceParam.GetShape(meowEyeBlinkKey);
        if (!eyeBlink.HasValue) return null;

        if (helpBlinkWithEyeSquint)
        {
            float? eyeSquint = meowFaceParam.GetShape(meowEyeSquintKey);
            if (eyeSquint.HasValue)
            {
                return (float)(1d - Math.Min(1d,
                    eyeBlink.Value + Math.Pow(eyeBlink.Value, 0.33d) * Math.Pow(eyeSquint.Value, 1.25d)));
            }
        }

        return 1f - Math.Min(1f, eyeBlink.Value);
    }

    private static float? GetEyeYShape(MeowFaceParam meowFaceParam, float? eyeOpenness,
        string meowEyeLookUpKey, string meowEyeLookDownKey)
    {
        float? eyeLookUp = meowFaceParam.GetShape(meowEyeLookUpKey);
        float? eyeLookDown = meowFaceParam.GetShape(meowEyeLookDownKey);

        if (eyeLookUp.HasValue || eyeLookDown.HasValue)
        {
            return (eyeLookUp.GetValueOrDefault(0f) - eyeLookDown.GetValueOrDefault(0f)) *
                   eyeOpenness.GetValueOrDefault(1f);
        }

        return null;
    }
}