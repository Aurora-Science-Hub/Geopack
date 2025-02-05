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
    /// Br spherical component
    /// </summary>
    public double Br { get; set; }

    /// <summary>
    /// Btheta spherical component
    /// </summary>
    public double Btheta { get; set; }

    /// <summary>
    /// Bphi spherical component
    /// </summary>
    public double Bphi { get; set; }

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
    /// Bx magnetic field cartesian component
    /// </summary>
    public double Bx { get; set; }

    /// <summary>
    /// By magnetic field cartesian component
    /// </summary>
    public double By { get; set; }

    /// <summary>
    /// Bz magnetic field cartesian component
    /// </summary>
    public double Bz { get; set; }
}
