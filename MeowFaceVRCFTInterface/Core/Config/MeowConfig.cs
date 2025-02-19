using MeowFaceVRCFTInterface.VRCFT.Mappers;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Core.Config;

public class MeowConfig
{
    [JsonProperty(PropertyName = "DontTouchThisField-ConfigVersion")]
    public string ConfigVersion { get; set; } = "";

    public ushort MeowFacePort { get; set; } = 12345;
    public ushort SearchMeowFaceTimeoutSeconds { get; set; } = 60;
    public int MeowFaceReadTimeoutMilliseconds { get; set; } = 5_000;

    public EyeMapper EyeMapper { get; set; } = new();
    public BrowMapper BrowMapper { get; set; } = new();
    public CheekMapper CheekMapper { get; set; } = new();
    public JawMapper JawMapper { get; set; } = new();
    public LipAndMouthMapper LipAndMouthMapper { get; set; } = new();
    public NoseMapper NoseMapper { get; set; } = new();
    public TongueMapper TongueMapper { get; set; } = new();
    public HeadPositionAndRotationMapper HeadPositionAndRotationMapper { get; set; } = new();

    public MapperBase[] GetAllMappers()
    {
        return new MapperBase[]
        {
            EyeMapper, BrowMapper, CheekMapper, JawMapper, LipAndMouthMapper, NoseMapper, TongueMapper,
            HeadPositionAndRotationMapper
        };
    }
}