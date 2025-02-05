using MeowFaceVRCFTInterface.VRCFTMappers;
using MeowFaceVRCFTInterface.VRCFTMappers.Eye;

namespace MeowFaceVRCFTInterface.Config
{
    public class MeowConfig
    {
        public ushort MeowFacePort { get; set; } = 12345;
        public ushort SearchMeowFaceTimeoutSeconds { get; set; } = 60;
        public int MeowFaceReadTimeoutMilliseconds { get; set; } = 10_000;

        public EyeMapper EyeMapper { get; set; } = new();
        public BrowMapper BrowMapper { get; set; } = new();
        public CheekMapper CheekMapper { get; set; } = new();
        public JawMapper JawMapper { get; set; } = new();
        public LipAndMouthMapper LipAndMouthMapper { get; set; } = new();
        public NoseMapper NoseMapper { get; set; } = new();
        public TongueMapper TongueMapper { get; set; } = new();
    }
}
