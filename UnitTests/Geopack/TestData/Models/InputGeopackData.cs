namespace AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.Models;

public sealed record InputGeopackData
{
    /// <summary>
    /// Date and Time
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// X-component of the solar wind velocity
    /// </summary>
    public double VGSEX { get; set; }

    /// <summary>
    /// Y-component of the solar wind velocity
    /// </summary>
    public double VGSEY { get; set; }

    /// <summary>
    /// Z-component of the solar wind velocity
    /// </summary>
    public double VGSEZ { get; set; }
}
