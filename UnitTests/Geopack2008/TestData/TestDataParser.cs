using System.Globalization;

namespace UnitTests.Geopack2008.TestData;

public static class TestDataParser
{
    /// <summary>
    /// Read data from file
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    public static string ReadDataFromFile(string path) => File.ReadAllText(path);

    /// <summary>
    /// Parse test data
    /// </summary>
    /// <param name="data"> Test data </param>
    public static TestData Parse(string data)
    {
        var lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new TestData();
        var lineIndex = 0;

        // Parse header
        var headerParts = lines[lineIndex++].Split(new[] { ' ', '=', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var year = int.Parse(headerParts[1]);
        var doy = int.Parse(headerParts[3]);
        var hour = int.Parse(headerParts[5]);
        var minute = int.Parse(headerParts[7]);
        result.DateTime = new DateTime(year, 1, 1, hour, minute, 0).AddDays(doy - 1);

        // Parse solar wind pressure
        var solarWindParts = lines[lineIndex++].Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        result.SolarWindPressure = float.Parse(solarWindParts[5], CultureInfo.InvariantCulture);

        // Parse DST index
        var dstParts = lines[lineIndex++].Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        result.DstIndex = float.Parse(dstParts[1], CultureInfo.InvariantCulture);

        // Parse IMF By and Bz
        var imfParts = lines[lineIndex++].Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        result.ByIMF = float.Parse(imfParts[4], CultureInfo.InvariantCulture);
        result.BzIMF = float.Parse(imfParts[5], CultureInfo.InvariantCulture);

        // Skip the line "THE LINE IN GSW COORDS:"
        lineIndex++;

        // Parse coordinates
        while (lineIndex < lines.Length)
        {
            var coordParts = lines[lineIndex++].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (coordParts.Length != 3)
            {
                continue;
            }

            var x = float.Parse(coordParts[0], CultureInfo.InvariantCulture);
            var y = float.Parse(coordParts[1], CultureInfo.InvariantCulture);
            var z = float.Parse(coordParts[2], CultureInfo.InvariantCulture);
            result.FieldLineCoordinates.Add((x, y, z));
        }

        return result;
    }

    /// <summary>
    /// Fill Solar Wind Parameters for original Tsyganenko's dataset
    /// </summary>
    /// <param name="testData"> Parsed from file dataset </param>
    public static void FillSolarWindVelocity(TestData testData)
    {
        testData.VGSEX = -304.0f;

        testData.VGSEY = -16.0f;
        testData.VGSEY += 29.78f;

        testData.VGSEZ = 4.0f;
    }
}
