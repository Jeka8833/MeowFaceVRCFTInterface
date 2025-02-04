using MeowFaceVRCFTInterface.MeowFace;

namespace MeowFaceVRCFTInterface.VRCFTMappers
{
    public interface IMapperCft
    {
        public void Initialize(MeowFaceVRCFTInterface module) { }
        public void UpdateEye(MeowFaceParam meowFaceParam) { }
        public void UpdateExpression(MeowFaceParam meowFaceParam) { }
    }
}
