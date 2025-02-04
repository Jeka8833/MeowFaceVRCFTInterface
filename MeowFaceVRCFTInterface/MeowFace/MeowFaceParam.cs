﻿using VRCFaceTracking.Core.Params.Data;
using VRCFaceTracking.Core.Params.Expressions;

namespace MeowFaceVRCFTInterface.MeowFace
{
    public struct MeowFaceParam
    {
        public const string BrowDownLeft = "browDownLeft";//
        public const string BrowDownRight = "browDownRight";//
        public const string BrowInnerUp = "browInnerUp";
        public const string BrowInnerUpLeft = "browInnerUpLeft";//
        public const string BrowInnerUpRight = "browInnerUpRight";//
        public const string BrowOuterUpLeft = "browOuterUpLeft";//
        public const string BrowOuterUpRight = "browOuterUpRight";//

        public const string CheekPuff = "cheekPuff";//

        public const string EyeBlinkLeft = "eyeBlinkLeft";//
        public const string EyeBlinkRight = "eyeBlinkRight";//
        public const string EyeLookDownLeft = "eyeLookDownLeft";//
        public const string EyeLookDownRight = "eyeLookDownRight";//
        public const string EyeLookInLeft = "eyeLookInLeft";
        public const string EyeLookInRight = "eyeLookInRight";
        public const string EyeLookOutLeft = "eyeLookOutLeft";
        public const string EyeLookOutRight = "eyeLookOutRight";
        public const string EyeLookUpLeft = "eyeLookUpLeft";//
        public const string EyeLookUpRight = "eyeLookUpRight";//
        public const string EyeSquintLeft = "eyeSquintLeft";//
        public const string EyeSquintRight = "eyeSquintRight";//
        public const string EyeWideLeft = "eyeWideLeft";//
        public const string EyeWideRight = "eyeWideRight";//

        public const string HeadDown = "headDown";
        public const string HeadLeft = "headLeft";
        public const string HeadRight = "headRight";
        public const string HeadRollLeft = "headRollLeft";
        public const string HeadRollRight = "headRollRight";
        public const string HeadUp = "headUp";

        public const string JawLeft = "jawLeft";//
        public const string JawOpen = "jawOpen";//
        public const string JawRight = "jawRight";//

        public const string MouthFrownLeft = "mouthFrownLeft";//
        public const string MouthFrownRight = "mouthFrownRight";//
        public const string MouthFunnel = "mouthFunnel";//
        public const string MouthLeft = "mouthLeft";//
        public const string MouthLowerDownLeft = "mouthLowerDownLeft";//
        public const string MouthLowerDownRight = "mouthLowerDownRight";//
        public const string MouthPucker = "mouthPucker";//
        public const string MouthRight = "mouthRight";//
        public const string MouthRollLower = "mouthRollLower";//
        public const string MouthRollUpper = "mouthRollUpper";//
        public const string MouthShrugUpper = "mouthShrugUpper";//
        public const string MouthSmileLeft = "mouthSmileLeft";//
        public const string MouthSmileRight = "mouthSmileRight";//
        public const string MouthUpperUpLeft = "mouthUpperUpLeft";//
        public const string MouthUpperUpRight = "mouthUpperUpRight";//

        public const string NoseSneerLeft = "noseSneerLeft";//
        public const string NoseSneerRight = "noseSneerRight";//

        public const string TongueOut = "tongueOut";//


        public readonly MeowVector? EyeLeft { get; }
        public readonly MeowVector? EyeRight { get; }

        private readonly Dictionary<string, float> _shapeMap;

        public MeowFaceParam(MeowFaceDTO meowDTO)
        {
            if (meowDTO.EyeLeft.IsValid())
            {
                EyeLeft = meowDTO.EyeLeft;
            }

            if (meowDTO.EyeRight.IsValid())
            {
                EyeRight = meowDTO.EyeRight;
            }

            _shapeMap = ToShapeMap(meowDTO.BlendShapes);
        }

        public readonly float? GetShape(string key, float maxValue = float.MaxValue)
        {
            return _shapeMap.TryGetValue(key, out float value) ? Math.Min(maxValue, value) : null;
        }

        public readonly void TrySetToVrcftShape(UnifiedExpressionShape[] vrcftShape, UnifiedExpressions expression,
            string meowKey, float maxValue = float.MaxValue)
        {
            float? shapeNull = GetShape(meowKey, maxValue);

            if (shapeNull is float shape)
            {
                vrcftShape[(int)expression].Weight = shape;
            }
        }

        public readonly override string ToString()
        {
            return "{EyeLeft: " + EyeLeft + ", EyeRight: " + EyeRight + ", _shapeMap: " + string.Join(", ", _shapeMap) + "}";
        }

        private static Dictionary<string, float> ToShapeMap(MeowShape[] meowShapes)
        {
            Dictionary<string, float> shapeMap = new();

            foreach (MeowShape shape in meowShapes)
            {
                if (shape.IsValid())
                {
                    shapeMap[shape.k] = shape.v;
                }
            }

            return shapeMap;
        }
    }
}
