using MeowFaceVRCFTInterface.MeowFace;
using VRCFaceTracking;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.VRCFTMappers.Eye
{
    public class EyeMapper : IMapperCft
    {
        private const float RadianConst = 0.01745329251994329576923690768488612713442871888541725456097191440171009114f;

        public bool EyeGazeX { get; set; } = true;
        public EyeGaze EyeGazeY { get; set; } = EyeGaze.Shape;

        public bool HelpBlinkWithEyeSquint { get; set; } = false;

        public void UpdateEye(MeowFaceParam meowFaceParam)
        {
            UpdateEyeGaze(meowFaceParam);
            UpdateEyeOpenness(meowFaceParam);
        }

        public void UpdateExpression(MeowFaceParam meowFaceParam)
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
                    UnifiedTracking.Data.Eye.Left.Gaze.x = eyeLeft.y * RadianConst;
                }

                if (meowFaceParam.EyeRight is MeowVector eyeRight)
                {
                    UnifiedTracking.Data.Eye.Right.Gaze.x = eyeRight.y * RadianConst;
                }
            }

            switch (EyeGazeY)
            {
                case EyeGaze.Vector:    // Data has wrong polarization
                    if (meowFaceParam.EyeLeft is MeowVector eyeLeft1)
                    {
                        UnifiedTracking.Data.Eye.Left.Gaze.y = -eyeLeft1.x * RadianConst;
                    }

                    if (meowFaceParam.EyeRight is MeowVector eyeRight1)
                    {
                        UnifiedTracking.Data.Eye.Right.Gaze.y = -eyeRight1.x * RadianConst;
                    }
                    break;
                case EyeGaze.Shape:
                    float? eyeLookUpLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpLeft);
                    float? eyeLookDownLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownLeft);

                    if (eyeLookUpLeftN != null && eyeLookDownLeftN != null)
                    {
                        UnifiedTracking.Data.Eye.Left.Gaze.y = eyeLookUpLeftN.Value - eyeLookDownLeftN.Value;
                    }
                    else if (eyeLookUpLeftN != null)
                    {
                        UnifiedTracking.Data.Eye.Left.Gaze.y = eyeLookUpLeftN.Value;
                    }
                    else if (eyeLookDownLeftN != null)
                    {
                        UnifiedTracking.Data.Eye.Left.Gaze.y = -eyeLookDownLeftN.Value;
                    }

                    float? eyeLookUpRightN = meowFaceParam.GetShape(MeowFaceParam.EyeLookUpRight);
                    float? eyeLookDownRightN = meowFaceParam.GetShape(MeowFaceParam.EyeLookDownRight);

                    if (eyeLookUpRightN != null && eyeLookDownRightN != null)
                    {
                        UnifiedTracking.Data.Eye.Right.Gaze.y = eyeLookUpRightN.Value - eyeLookDownRightN.Value;
                    }
                    else if (eyeLookUpRightN != null)
                    {
                        UnifiedTracking.Data.Eye.Right.Gaze.y = eyeLookUpRightN.Value;
                    }
                    else if (eyeLookDownRightN != null)
                    {
                        UnifiedTracking.Data.Eye.Right.Gaze.y = -eyeLookDownRightN.Value;
                    }
                    break;
            }

        }

        private void UpdateEyeOpenness(MeowFaceParam meowFaceParam)
        {
            float? eyeBlinkLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeBlinkLeft);
            if (eyeBlinkLeftN is float eyeBlinkLeft)
            {
                float? eyeSquintLeftN = meowFaceParam.GetShape(MeowFaceParam.EyeSquintLeft);

                if (HelpBlinkWithEyeSquint && eyeSquintLeftN is float eyeSquintLeft)
                {
                    UnifiedTracking.Data.Eye.Right.Openness = (float)(1d - Math.Min(1d,
                        eyeBlinkLeft + Math.Pow(eyeBlinkLeft, 0.33d) * Math.Pow(eyeSquintLeft, 1.25d)));
                }
                else
                {
                    UnifiedTracking.Data.Eye.Right.Openness = 1f - Math.Min(1f, eyeBlinkLeft);
                }
            }

            float? eyeBlinkRightN = meowFaceParam.GetShape(MeowFaceParam.EyeBlinkRight);
            if (eyeBlinkRightN is float eyeBlinkRight)
            {
                float? eyeSquintRightN = meowFaceParam.GetShape(MeowFaceParam.EyeSquintRight);

                if (HelpBlinkWithEyeSquint && eyeSquintRightN is float eyeSquintRight)
                {
                    UnifiedTracking.Data.Eye.Left.Openness = (float)(1d - Math.Min(1d,
                        eyeBlinkRight + Math.Pow(eyeBlinkRight, 0.33d) * Math.Pow(eyeSquintRight, 1.25d)));
                }
                else
                {
                    UnifiedTracking.Data.Eye.Left.Openness = 1f - Math.Min(1f, eyeBlinkRight);
                }
            }
        }
    }
}
