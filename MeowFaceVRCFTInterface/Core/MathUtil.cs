namespace MeowFaceVRCFTInterface.Core;

public static class MathUtil
{
    public static float? ClampFloat(float? value, float min = -1f, float max = 1f)
    {
        if (!value.HasValue) return null;

        return !float.IsFinite(value.Value) ? null : Math.Clamp(value.Value, min, max);
    }

    public static float? FixedRangeCenterShift(float? value, float shift, float min = -1f, float max = 1f)
    {
        if (!value.HasValue) return null;
        if (value < min || value > max) throw new ArgumentException("value < min || value > max, need to change range");

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
}