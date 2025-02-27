using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class MeowFaceEyesParams
{
    public bool HelpBlinkWithEyeSquint { get; init; } = false;

    public EyesParams ToEyesParams(MeowFaceParam meowFaceParam)
    {
        return new EyesParams
        {
            LeftOpenness = GetEyeOpenness(meowFaceParam, MeowFaceParam.EyeBlinkLeft, MeowFaceParam.EyeSquintLeft),
            RightOpenness = GetEyeOpenness(meowFaceParam, MeowFaceParam.EyeBlinkRight, MeowFaceParam.EyeSquintRight),

            LeftGazeX = meowFaceParam.SubtractShapes(MeowFaceParam.EyeLookOutLeft, MeowFaceParam.EyeLookInLeft),
            LeftGazeY = meowFaceParam.SubtractShapes(MeowFaceParam.EyeLookUpLeft, MeowFaceParam.EyeLookDownLeft),

            RightGazeX = meowFaceParam.SubtractShapes(MeowFaceParam.EyeLookInRight, MeowFaceParam.EyeLookOutRight),
            RightGazeY = meowFaceParam.SubtractShapes(MeowFaceParam.EyeLookUpRight, MeowFaceParam.EyeLookDownRight)
        };
    }

    private float? GetEyeOpenness(MeowFaceParam meowFaceParam, string meowEyeBlinkKey, string meowEyeSquintKey)
    {
        float? eyeBlink = meowFaceParam.GetShape(meowEyeBlinkKey);
        if (!eyeBlink.HasValue) return null;

        if (HelpBlinkWithEyeSquint)
        {
            float? eyeSquint = meowFaceParam.GetShape(meowEyeSquintKey);
            if (eyeSquint.HasValue)
            {
                return (float)(1d - Math.Min(1d,
                    eyeBlink.Value + Math.Pow(eyeBlink.Value, 0.33d) * Math.Pow(eyeSquint.Value, 1.25d)));
            }
        }

        return 1f - eyeBlink;
    }
}