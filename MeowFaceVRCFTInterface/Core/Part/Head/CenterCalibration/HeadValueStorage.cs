namespace MeowFaceVRCFTInterface.Core.Part.Head.CenterCalibration;

public class HeadValueStorage
{
    public float X { get; init; } = 0.5f;
    public float Y { get; init; } = 0.5f;
    public float Z { get; init; } = 0.5f;

    public float Pitch { get; init; }
    public float Roll { get; init; }
    public float Yaw { get; init; }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}, {nameof(Pitch)}: {Pitch}," +
               $" {nameof(Yaw)}: {Yaw}, {nameof(Roll)}: {Roll}";
    }

    public static HeadValueStorage Average(List<HeadValueStorage> list)
    {
        double x = 0d;
        int xCount = 0;

        double y = 0d;
        int yCount = 0;

        double z = 0d;
        int zCount = 0;

        double pitch = 0d;
        int pitchCount = 0;

        double roll = 0d;
        int rollCount = 0;

        double yaw = 0d;
        int yawCount = 0;

        foreach (HeadValueStorage valueStorage in list)
        {
            if (float.IsFinite(valueStorage.X))
            {
                x += valueStorage.X;
                xCount++;
            }

            if (float.IsFinite(valueStorage.Y))
            {
                y += valueStorage.Y;
                yCount++;
            }

            if (float.IsFinite(valueStorage.Z))
            {
                z += valueStorage.Z;
                zCount++;
            }

            if (float.IsFinite(valueStorage.Pitch))
            {
                pitch += valueStorage.Pitch;
                pitchCount++;
            }

            if (float.IsFinite(valueStorage.Roll))
            {
                roll += valueStorage.Roll;
                rollCount++;
            }

            if (float.IsFinite(valueStorage.Yaw))
            {
                yaw += valueStorage.Yaw;
                yawCount++;
            }
        }

        return new HeadValueStorage
        {
            X = xCount > 0 ? (float)(x / xCount) : 0f,
            Y = yCount > 0 ? (float)(y / yCount) : 0f,
            Z = zCount > 0 ? (float)(z / zCount) : 0f,

            Pitch = pitchCount > 0 ? (float)(pitch / pitchCount) : 0f,
            Roll = rollCount > 0 ? (float)(roll / rollCount) : 0f,
            Yaw = yawCount > 0 ? (float)(yaw / yawCount) : 0f
        };
    }
}