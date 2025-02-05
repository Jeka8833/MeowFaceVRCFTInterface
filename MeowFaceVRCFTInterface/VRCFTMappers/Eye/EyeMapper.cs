using MeowFaceVRCFTInterface.MeowFace;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFTMappers.Eye
{
    public class EyeMapper : MapperCft
    {
        private const float _radianConst = 0.01745329251994329576923690768488612713442871888541725456097191440171009114f;

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
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintRight, MeowFaceParam.EyeSquintRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeSquintLeft, MeowFaceParam.EyeSquintLeft);

            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideRight, MeowFaceParam.EyeWideRight);
            meowFaceParam.TrySetToVrcftShape(UnifiedTracking.Data.Shapes, UnifiedExpressions.EyeWideLeft, MeowFaceParam.EyeWideLeft);
        }

        private void UpdateEyeGaze(MeowFaceParam meowFaceParam)
        {
            if (EyeGazeX)
            {
                // Data has wrong polarization
                if (meowFaceParam.EyeLeft is MeowVector eyeLeft)
                {
                    UnifiedTracking.Data.Eye.Left.Gaze.x = eyeLeft.y * _radianConst;
                }

                if (meowFaceParam.EyeRight is MeowVector eyeRight)
                {
                    UnifiedTracking.Data.Eye.Right.Gaze.x = eyeRight.y * _radianConst;
                }
            }

            switch (EyeGazeY)
            {
                case EyeGaze.Vector:    // Data has wrong polarization
                    if (meowFaceParam.EyeLeft is MeowVector eyeLeft1)
                    {
                        UnifiedTracking.Data.Eye.Left.Gaze.y = -eyeLeft1.x * _radianConst;
                    }

                    if (meowFaceParam.EyeRight is MeowVector eyeRight1)
                    {
                        UnifiedTracking.Data.Eye.Right.Gaze.y = -eyeRight1.x * _radianConst;
                    }
                    break;
                case EyeGaze.Shape:
                    float? eyeLookUpLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpLeft);
                    float? eyeLookDownLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownLeft);

                    if (eyeLookUpLeftN != null || eyeLookDownLeftN != null)
                    {
                        float eyeOpennessLeft = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                            MeowFaceParam.EyeBlinkLeft, MeowFaceParam.EyeSquintLeft).GetValueOrDefault(1f);

                        UnifiedTracking.Data.Eye.Left.Gaze.y = (eyeLookUpLeftN.GetValueOrDefault(0f) -
                            eyeLookDownLeftN.GetValueOrDefault(0f)) * eyeOpennessLeft;
                    }


                    float? eyeLookUpRightN = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpRight);
                    float? eyeLookDownRightN = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownRight);

                    if (eyeLookUpRightN != null || eyeLookDownRightN != null)
                    {
                        float eyeOpennessRight = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                            MeowFaceParam.EyeBlinkRight, MeowFaceParam.EyeSquintRight).GetValueOrDefault(1f);

                        UnifiedTracking.Data.Eye.Right.Gaze.y = (eyeLookUpRightN.GetValueOrDefault(0f) -
                            eyeLookDownRightN.GetValueOrDefault(0f)) * eyeOpennessRight;
                    }
                    break;
            }

        }

        private void UpdateEyeOpenness(MeowFaceParam meowFaceParam)
        {
            float? eyeOpennessLeftN = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                MeowFaceParam.EyeBlinkLeft, MeowFaceParam.EyeSquintLeft);

            if (eyeOpennessLeftN is float eyeOpennessLeft)
            {
                UnifiedTracking.Data.Eye.Left.Openness = eyeOpennessLeft;
            }

            float? eyeOpennessRightN = GetEyeOpenness(meowFaceParam, HelpBlinkWithEyeSquint,
                MeowFaceParam.EyeBlinkRight, MeowFaceParam.EyeSquintRight);

            if (eyeOpennessRightN is float eyeOpennessRight)
            {
                UnifiedTracking.Data.Eye.Right.Openness = eyeOpennessRight;
            }
        }

        private static float? GetEyeOpenness(MeowFaceParam meowFaceParam, bool helpBlinkWithEyeSquint,
            string meowEyeBlinkKey, string meowEyeSquintKey)
        {
            float? eyeBlinkN = meowFaceParam.GetShape(meowEyeBlinkKey, 1f);
            if (eyeBlinkN is float eyeBlink)
            {
                float? eyeSquintN = meowFaceParam.GetShape(meowEyeSquintKey, 1f);

                if (helpBlinkWithEyeSquint && eyeSquintN is float eyeSquint)
                {
                    return (float)(1d - Math.Min(1d, eyeBlink +
                        Math.Pow(eyeBlink, 0.33d) * Math.Pow(eyeSquint, 1.25d)));
                }
                else
                {
                    return 1f - Math.Min(1f, eyeBlink);
                }
            }

            return null;
        }
    }
}
