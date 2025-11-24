using AuroraScienceHub.Geopack.Contracts.Coordinates;

namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Basic interface for all spherical objects
/// </summary>
public interface ISpherical<TSelf>
    where TSelf : ISpherical<TSelf>
{
    /// <summary> Radial component in Earth's radii</summary>
    double R { get; }

    /// <summary> Theta angle in radians</summary>
    double Theta { get; }

    /// <summary> Phi angle in radians</summary>
    double Phi { get; }

    /// <summary> Coordinate system</summary>
    CoordinateSystem CoordinateSystem { get; }

    /// <summary>
    /// Factory method to create new instances of ICartesian implementations
    /// </summary>
    /// <param name="r">Radial coordinate in Earth's radii (Re)</param>
    /// <param name="theta">Theta coordinate in radians (Re)</param>
    /// <param name="phi">Phi coordinate in radians (Re)</param>
    /// <param name="coordinateSystem">Coordinate system</param>
    static abstract TSelf New(double r, double theta, double phi, CoordinateSystem coordinateSystem);
}
