namespace AuroraScienceHub.Geopack.Contracts;

public sealed class Point
{
    /// <summary>
    /// Cartesian X-coordinate
    /// </summary>
    public double? X { get; private set; }

    /// <summary>
    /// Cartesian Y-coordinate
    /// </summary>
    public double? Y { get; private set; }

    /// <summary>
    /// Cartesian Z-coordinate
    /// </summary>
    public double? Z { get ; private set; }

    /// <summary>
    /// Spherical coordinate: radial distance in Earth radius (Re)
    /// </summary>
    public double? R { get; private set; }

    /// <summary>
    /// Spherical coordinate: theta angle
    /// </summary>
    public double? Theta { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public double Phi { get; private set; }
}
