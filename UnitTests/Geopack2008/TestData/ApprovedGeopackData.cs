namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData;

public sealed record ApprovedGeopackData
{
    /// <summary>
    /// Date and Time
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// X-component of the solar wind velocity
    /// </summary>
    public float VGSEX { get; set; }

    /// <summary>
    /// Y-component of the solar wind velocity
    /// </summary>
    public float VGSEY { get; set; }

    /// <summary>
    /// Z-component of the solar wind velocity
    /// </summary>
    public float VGSEZ { get; set; }

    /// <summary>
    /// Solar wind pressure
    /// </summary>
    public float SolarWindPressure { get; set; }

    /// <summary>
    /// Dst index
    /// </summary>
    public float DstIndex { get; set; }

    /// <summary>
    /// Y-component of the interplanetary magnetic field
    /// </summary>
    public float ByIMF { get; set; }

    /// <summary>
    /// Z-component of the interplanetary magnetic field
    /// </summary>
    public float BzIMF { get; set; }

    /// <summary>
    /// Field line coordinates
    /// </summary>
    public List<(float X, float Y, float Z)> FieldLineCoordinates { get; set; } = [];
}
