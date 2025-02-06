using MeowFaceVRCFTInterface.MeowFace;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFTMappers.Eye;

public class EyeMapper : MapperCft
{
    private const float RadianConst = 0.01745329251994329576923690768488612713442871888541725456097191440171009114f;

    public bool EyeGazeX { get; set; } = true;

    [JsonConverter(typeof(StringEnumConverter))]
    public EyeGaze EyeGazeY { get; set; } = EyeGaze.Vector;

    public bool HelpBlinkWithEyeSquint { get; set; } = true;

    public override void UpdateEye(MeowFaceParam meowFaceParam)
    {
        UpdateEyeGaze(meowFaceParam);
        UpdateEyeOpenness(meowFaceParam);
    }

    public override void UpdateExpression(MeowFaceParam meowFaceParam)
    {
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintRight,
            MeowFaceParam.EyeSquintRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintLeft,
            MeowFaceParam.EyeSquintLeft);

        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideRight,
            MeowFaceParam.EyeWideRight);
        meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideLeft,
            MeowFaceParam.EyeWideLeft);
    }

    private void UpdateEyeGaze(MeowFaceParam meowFaceParam)
    {
        if (EyeGazeX)
        {
            // Data has wrong polarization
            if (meowFaceParam.EyeLeft.HasValue)
            {
                UnifiedTracking.Data.Eye.Left.Gaze.x = meowFaceParam.EyeLeft.Value.Y * RadianConst;
            }

            if (meowFaceParam.EyeRight.HasValue)
            {
                UnifiedTracking.Data.Eye.Right.Gaze.x = meowFaceParam.EyeRight.Value.Y * RadianConst;
            }
        }

        switch (EyeGazeY)
        {
            case EyeGaze.Vector: // Data has wrong polarization
                if (meowFaceParam.EyeLeft.HasValue)
                {
                    UnifiedTracking.Data.Eye.Left.Gaze.y = -meowFaceParam.EyeLeft.Value.X * RadianConst;
                }

                if (meowFaceParam.EyeRight.HasValue)
                {
                    UnifiedTracking.Data.Eye.Right.Gaze.y = -meowFaceParam.EyeRight.Value.X * RadianConst;
                }

                break;
            case EyeGaze.Shape:
                float? eyeLookUpLeft = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpLeft);
                float? eyeLookDownLeft = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownLeft);

                if (eyeLookUpLeft.HasValue || eyeLookDownLeft.HasValue)
                {
                    float eyeOpennessLeft = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                        MeowFaceParam.EyeBlinkLeft, MeowFaceParam.EyeSquintLeft).GetValueOrDefault(1f);

                    UnifiedTracking.Data.Eye.Left.Gaze.y =
                        (eyeLookUpLeft.GetValueOrDefault(0f) - eyeLookDownLeft.GetValueOrDefault(0f)) *
                        eyeOpennessLeft;
                }


                float? eyeLookUpRight = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpRight);
                float? eyeLookDownRight = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownRight);

                if (eyeLookUpRight.HasValue || eyeLookDownRight.HasValue)
                {
                    float eyeOpennessRight = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                        MeowFaceParam.EyeBlinkRight, MeowFaceParam.EyeSquintRight).GetValueOrDefault(1f);

                    UnifiedTracking.Data.Eye.Right.Gaze.y =
                        (eyeLookUpRight.GetValueOrDefault(0f) - eyeLookDownRight.GetValueOrDefault(0f)) *
                        eyeOpennessRight;
                }

                break;
        }
    }

    private void UpdateEyeOpenness(MeowFaceParam meowFaceParam)
    {
        float? eyeOpennessLeft = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
            MeowFaceParam.EyeBlinkLeft, MeowFaceParam.EyeSquintLeft);

        if (eyeOpennessLeft.HasValue)
        {
            UnifiedTracking.Data.Eye.Left.Openness = eyeOpennessLeft.Value;
        }

        float? eyeOpennessRight = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
            MeowFaceParam.EyeBlinkRight, MeowFaceParam.EyeSquintRight);

        if (eyeOpennessRight.HasValue)
        {
            UnifiedTracking.Data.Eye.Right.Openness = eyeOpennessRight.Value;
        }
    }

    private static float? GetEyeOpenness(MeowFaceParam meowFaceParam, bool helpBlinkWithEyeSquint,
        string meowEyeBlinkKey, string meowEyeSquintKey)
    {
        float? eyeBlink = meowFaceParam.GetShape(meowEyeBlinkKey, 1f);
        if (!eyeBlink.HasValue) return null;

        float? eyeSquint = meowFaceParam.GetShape(meowEyeSquintKey, 1f);

        if (helpBlinkWithEyeSquint && eyeSquint.HasValue)
        {
            return (float)(1d - Math.Min(1d,
                eyeBlink.Value + Math.Pow(eyeBlink.Value, 0.33d) * Math.Pow(eyeSquint.Value, 1.25d)));
        }

        return 1f - Math.Min(1f, eyeBlink.Value);
    }
}