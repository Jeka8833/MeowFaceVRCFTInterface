using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Core.Part.Eye;

public class EyesFocusRange
{
    public float FocusStaticShift { get; init; } = 0.05f;

    public void Initialize(MeowFaceVRCFTInterface module)
    {
        if (FocusStaticShift < 0f)
        {
            module.MeowLogger.LogInformation("You've added a squint to your character? FocusStaticShift: {}",
                FocusStaticShift);
        }
    }

    public void Update(EyesParams eyesParams)
    {
        eyesParams.LeftGazeX += FocusStaticShift;
        eyesParams.RightGazeX -= FocusStaticShift;
    }
}