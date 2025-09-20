namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Location (point) coordinates (cartesian, spherical etc.)
/// </summary>
public sealed class Location
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="x">Cartesian X-coordinate</param>
    /// <param name="y">Cartesian Y-coordinate</param>
    /// <param name="z">Cartesian Z-coordinate</param>
    /// <param name="r">Spherical coordinate: radial distance in Earth radius (Re)</param>
    /// <param name="theta">Co-latitude theta in radians</param>
    /// <param name="phi">Longitude phi in radians</param>
    public Location(
        double x, double y, double z,
        double r, double theta, double phi)
    {
        X = x; Y = y; Z = z;
        R = r; Theta = theta; Phi = phi;
    }

    /// <summary>
    /// Cartesian X-coordinate
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Cartesian Y-coordinate
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Cartesian Z-coordinate
    /// </summary>
    public double Z { get; }

    /// <summary>
    /// Spherical coordinate: radial distance in Earth radius (Re)
    /// </summary>
    public double R { get; }

    /// <summary>
    /// Co-latitude theta in radians
    /// </summary>
    public double Theta { get; }

    /// <summary>
    /// Longitude phi in radians
    /// </summary>
    public double Phi { get; }
}
