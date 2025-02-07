using MeowFaceVRCFTInterface.MeowFace;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.VRCFT.Mappers;

public abstract class MapperCft
{
    public bool IsEnabled { get; set; } = true;

    [JsonIgnore] public bool IsMapperCrashed { get; set; }

    public virtual void Initialize(MeowFaceVRCFTInterface module)
    {
    }

    public virtual void UpdateEye(MeowFaceParam meowFaceParam)
    {
    }

    public virtual void UpdateExpression(MeowFaceParam meowFaceParam)
    {
    }
}