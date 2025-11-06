using AuroraScienceHub.Geopack.Contracts.Coordinates;

namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Basic interface for all cartesian objects
/// </summary>
public interface ICartesian<TSelf>
    where TSelf : ICartesian<TSelf>
{
    /// <summary> X-coordinate </summary>
    double X { get; }

    /// <summary> Y-coordinate </summary>
    double Y { get; }

    /// <summary> Z-coordinate </summary>
    double Z { get; }

    /// <summary> Coordinate system </summary>
    CoordinateSystem CoordinateSystem { get; }

    /// <summary>
    /// Factory method to create new instances of ICartesian implementations
    /// </summary>
    /// <param name="x">X-coordinate in Earth's radii (Re)</param>
    /// <param name="y">Y-coordinate in Earth's radii (Re)</param>
    /// <param name="z">Z-coordinate in Earth's radii (Re)</param>
    /// <param name="coordinateSystem">Coordinate system</param>
    static abstract TSelf New(double x, double y, double z, CoordinateSystem coordinateSystem);
}
