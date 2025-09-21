namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Field vector components (cartesian, spherical etc.)
/// </summary>
public sealed class MagneticFieldVector
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="bx">Cartesian magnetic field x-component</param>
    /// <param name="by">Cartesian magnetic field y-component</param>
    /// <param name="bz">Cartesian magnetic field z-component</param>
    /// <param name="br">Spherical magnetic field radial component</param>
    /// <param name="btheta">Spherical magnetic field theta-component</param>
    /// <param name="bphi">Spherical magnetic field phi-component</param>
    public MagneticFieldVector(
        double? bx = null, double? by = null, double? bz = null,
        double? br = null, double? btheta = null, double? bphi = null)
    {
        Bx = bx;
        By = by;
        Bz = bz;
        Br = br;
        Btheta = btheta;
        Bphi = bphi;
    }

    /// <summary>
    /// Cartesian magnetic field x-component
    /// </summary>
    public double? Bx { get; }

    /// <summary>
    /// Cartesian magnetic field y-component
    /// </summary>
    public double? By { get; }

    /// <summary>
    /// Cartesian magnetic field z-component
    /// </summary>
    public double? Bz { get; }

    /// <summary>
    /// Spherical magnetic field radial component
    /// </summary>
    public double? Br { get; }

    /// <summary>
    /// Spherical magnetic field theta-component
    /// </summary>
    public double? Btheta { get; }

    /// <summary>
    /// Spherical magnetic field phi-component
    /// </summary>
    public double? Bphi { get; }
}
