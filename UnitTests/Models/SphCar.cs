namespace AuroraScienceHub.Geopack.UnitTests.Models;

public sealed record SphCar
{
    /// <summary>
    /// Radial distance (rad)
    /// </summary>
    public double R { get; set; }

    /// <summary>
    /// Theta angle (rad)
    /// </summary>
    public double Theta { get; set; }

    /// <summary>
    /// Phi angle (rad)
    /// </summary>
    public double Phi { get; set; }

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
}
