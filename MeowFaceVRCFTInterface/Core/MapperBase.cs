using MeowFaceVRCFTInterface.MeowFace;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Core;

public abstract class MapperBase : IDisposable
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

    public virtual void Dispose()
    {
    }
}