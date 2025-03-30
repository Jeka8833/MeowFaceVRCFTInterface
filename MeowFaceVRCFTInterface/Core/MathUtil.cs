using System.Numerics;

namespace MeowFaceVRCFTInterface.Core;

public static class MathUtil
{
    private const double HalfPi = Math.PI / 2d;

    public static float? ClampFloat(float? value, float min = -1f, float max = 1f)
    {
        if (!value.HasValue) return null;

        return !float.IsFinite(value.Value) ? null : Math.Clamp(value.Value, min, max);
    }

    public static float? FixedRangeCenterShift(float? value, float shift, float min = -1f, float max = 1f)
    {
        if (!value.HasValue) return null;
        if (value < min || value > max)
        {
            throw new ArgumentException("value < min || value > max, need to change range; value: " + value +
                                        ", min: " + min + ", max: " + max + " shift: " + shift);
        }

        float result = (float)CalculateShiftRange(value.Value, shift, min, max);

        return float.IsFinite(result) ? result : null;
    }

    // x: min -> y: min
    // x: (min + max) / 2 - shift -> y: (min + max) / 2
    // x: max -> y: max
    // 
    // double value to prevent loss of accuracy.
    private static double CalculateShiftRange(double x, double shift, double min, double max)
    {
        // FusedMultiplyAdd more predictable if something large happened
        double shiftedCenter = double.FusedMultiplyAdd(min + max, 0.5d, shift);

        double secondPoint = x <= shiftedCenter ? min : max;

        double div = shiftedCenter - secondPoint;
        if (Math.Abs(div) == 0d) return secondPoint;

        return double.FusedMultiplyAdd(secondPoint, shift,
            -(secondPoint + shift - shiftedCenter) * x) / div;
    }

    public static Vector3 EulerZYX2ZXYOneScale(Vector3 eulerZYXOneScale)
    {
        // Wolfram Mathematica code, to validate formula:
        // FullSimplify[Solve[EulerMatrix[{x0, y0, z0}, {3, 2, 1}] == EulerMatrix[{x1, y1, z1}, {3, 1, 2}], {x1, y1, z1}]]

        (double sinX, double cosX) = Math.SinCos(eulerZYXOneScale.X * HalfPi);
        (double sinY, double cosY) = Math.SinCos(eulerZYXOneScale.Y * HalfPi);
        (double sinZ, double cosZ) = Math.SinCos(eulerZYXOneScale.Z * HalfPi);

        double len =
            Math.ReciprocalSqrtEstimate(Math.FusedMultiplyAdd(sinY * sinZ, sinY * sinZ, cosZ * cosZ)); // Can be Inf

        double x = Math.Atan2((cosZ * sinX - cosX * sinY * sinZ) * len,
            Math.FusedMultiplyAdd(sinX * sinZ, sinY, cosX * cosZ) * len);
        double y = Math.Atan2(cosY * sinZ, (1 - cosY * cosY * sinZ * sinZ) * len);
        double z = Math.Atan2(sinY * len, cosY * cosZ * len);

        return new Vector3((float)(x / HalfPi), (float)(y / HalfPi), (float)(z / HalfPi));
    }
}