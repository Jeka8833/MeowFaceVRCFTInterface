using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public abstract class MapperCft
    {
        public bool IsEnabled { get; set; } = true;

        public virtual void Initialize(MeowFaceVRCFTInterface module) { }
        public virtual void UpdateEye(MeowFaceParam meowFaceParam) { }
        public virtual void UpdateExpression(MeowFaceParam meowFaceParam) { }
    }
}
