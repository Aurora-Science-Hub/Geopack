using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Contracts.Interfaces;

/// <summary>
/// Basic interface for all cartesian objects
/// </summary>
public interface ICartesian
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
    /// <typeparam name="T">Cartesian object type</typeparam>
    T Create<T>(double x, double y, double z, CoordinateSystem coordinateSystem)
        where T : ICartesian;
}
