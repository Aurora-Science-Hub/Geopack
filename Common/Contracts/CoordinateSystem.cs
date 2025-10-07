namespace Common.Contracts;

/// <summary>
/// Available in Geopack-2008 coordinate systems
/// </summary>
public enum CoordinateSystem
{
    /// <summary> Geographic coordinate system </summary>
    GEO,

    /// <summary> Geocentric Solar Wind system </summary>
    GSW,

    /// <summary> Geocentric Solar Equatorial system </summary>
    GSE,

    /// <summary> Dipole Magnetic coordinate system </summary>
    MAG,

    /// <summary> Solar Magnetic coordinate system </summary>
    SM,

    /// <summary> Equatorial Inertial (GEI) coordinate system </summary>
    GEI,

    /// <summary> Geocentric Solar Magnetospheric system </summary>
    GSM
}
