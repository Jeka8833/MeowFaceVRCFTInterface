using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesFocusRange
{
    public float FocusStaticShift { get; init; } = 2.5f;

    public void Initialize(MeowFaceVRCFTInterface module)
    {
        if (FocusStaticShift < 0f)
        {
            module.MeowLogger.LogInformation("Did you intentionally add the strabismus? FocusStaticShift: {}",
                FocusStaticShift);
        }
    }

    public void Update(EyesParams eyesParams)
    {
        eyesParams.LeftGazeX += FocusStaticShift / 45f;
        eyesParams.RightGazeX -= FocusStaticShift / 45f;
    }
}