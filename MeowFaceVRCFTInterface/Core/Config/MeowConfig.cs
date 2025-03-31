using MeowFaceVRCFTInterface.VRCFT.Mappers;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Core.Config;

public class MeowConfig
{
    [JsonProperty(PropertyName = "DontTouchThisField-ConfigVersion")]
    public string ConfigVersion { get; set; } = "";

    public ushort MeowFacePort { get; init; } = 12345;
    public ushort SearchMeowFaceTimeoutSeconds { get; init; } = 60;
    public uint MeowFaceReadTimeoutMilliseconds { get; init; } = 5_000;
    public bool ShowAllLogs { get; init; } = false;
    public bool BypassOtherModulesBlock { get; init; } = false;
    public bool BypassOtherModulesBlockEyePriority { get; init; } = false;

    public EyeMapper EyeMapper { get; init; } = new();
    public BrowMapper BrowMapper { get; init; } = new();
    public CheekMapper CheekMapper { get; init; } = new();
    public JawMapper JawMapper { get; init; } = new();
    public LipAndMouthMapper LipAndMouthMapper { get; init; } = new();
    public NoseMapper NoseMapper { get; init; } = new();
    public TongueMapper TongueMapper { get; init; } = new();
    public HeadPositionAndRotationMapper HeadPositionAndRotationMapper { get; init; } = new();

    public MapperBase[] GetAllMappers()
    {
        return new MapperBase[]
        {
            EyeMapper, BrowMapper, CheekMapper, JawMapper, LipAndMouthMapper, NoseMapper, TongueMapper,
            HeadPositionAndRotationMapper
        };
    }
}