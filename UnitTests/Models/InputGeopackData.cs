namespace AuroraScienceHub.Geopack.UnitTests.Models;

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

    /// <summary>
    /// Cartesian X-coordinate
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Cartesian Y-coordinate
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Cartesian Z-coordinate
    /// </summary>
    public double Z { get; set; }

    /// <summary>
    /// Solar wind pressure
    /// </summary>
    public double SolarWindPressure { get; set; }

    /// <summary>
    /// Dst index
    /// </summary>
    public double DstIndex { get; set; }

    /// <summary>
    /// Y-component of the interplanetary magnetic field
    /// </summary>
    public double ByIMF { get; set; }

    /// <summary>
    /// Z-component of the interplanetary magnetic field
    /// </summary>
    public double BzIMF { get; set; }
}
