﻿using MeowFaceVRCFTInterface.MeowFace;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public abstract class MapperCft
    {
        public bool IsEnabled { get; set; } = true;

        [JsonIgnore]
        public bool IsMapperCrashed { get; set; } = false;

        public virtual void Initialize(MeowFaceVRCFTInterface module) { }
        public virtual void UpdateEye(MeowFaceParam meowFaceParam) { }
        public virtual void UpdateExpression(MeowFaceParam meowFaceParam) { }
    }
}
