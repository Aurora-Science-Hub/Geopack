namespace AuroraScienceHub.Geopack.UnitTests.Utils;

public static class GeopackTestUtils
{
    public static void RoundArray(this float[] array, int decimalPlaces)
    {
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = MathF.Round(array[i], decimalPlaces);
        }
    }

    public static void RoundArray(this List<float> list, int decimalPlaces)
    {
        for (var i = 0; i < list.Count; i++)
        {
            list[i] = MathF.Round(list[i], decimalPlaces);
        }
    }
}
