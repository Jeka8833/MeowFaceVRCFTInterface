namespace MeowFaceVRCFTInterface.Core.Part.Head;

/// <summary>
/// Contains head rotation and position data.
/// Head coordinate system should be *LEFT handed, Y-up* (matching Unity coordinate system)
/// Head rotation values should be normalized to a [-1, 1] range representing -90d to 90d rotation (0 meaning facing directly forwards)
///     Yaw - Y axis rotation (positive values = looking right)
///     Pitch - X axis rotation (positive values = looking down)
///     Roll - Z axis rotation (positive values = side-tilt left)
/// Head position values should be normalized / capped to a [-1, 1] range and represent deviation from a set user origin point in meters
///     The normalized values should be *approximately* represent a 1x1x1 meter movement region about the user's origin point 
///     (i.e. HeadPosX = 1 means the user is 0.5m to their right from their starting point, HeadPosX = -1 the user is 0.5m to their left)
///
/// All rotation in Euler angles is in ZXY order (https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform-eulerAngles.html)
/// </summary>
public class HeadParams
{
    public float? PosX;
    public float? PosY;
    public float? PosZ;

    public float? Pitch;
    public float? Roll;
    public float? Yaw;
}