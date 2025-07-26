namespace AuroraScienceHub.Geopack.UnitTests.Models;

public sealed record GeopackCommons
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

    public List<double>? G { get; set; } = [];
    public List<double>? H { get; set; } = [];
    public List<double>? REC { get; set; } = [];

    /// <summary>
    /// Field line coordinates
    /// </summary>
    public List<(double x, double y, double z)>? FieldLineCoordinates { get; set; } = [];

    /// <summary>
    /// Math conversion and matrix coefficients
    /// </summary>
    public double ST0 { get; set; }
    public double CT0 { get; set; }
    public double SL0 { get; set; }
    public double CL0 { get; set; }
    public double CTCL { get; set; }
    public double STCL { get; set; }
    public double CTSL { get; set; }
    public double STSL { get; set; }
    public double SFI { get; set; }
    public double CFI { get; set; }
    public double SPS { get; set; }
    public double CPS { get; set; }
    public double DS3 { get; set; }
    public double CGST { get; set; }
    public double SGST { get; set; }
    public double PSI { get; set; }
    public double A11 { get; set; }
    public double A21 { get; set; }
    public double A31 { get; set; }
    public double A12 { get; set; }
    public double A22 { get; set; }
    public double A32 { get; set; }
    public double A13 { get; set; }
    public double A23 { get; set; }
    public double A33 { get; set; }
    public double E11 { get; set; }
    public double E21 { get; set; }
    public double E31 { get; set; }
    public double E12 { get; set; }
    public double E22 { get; set; }
    public double E32 { get; set; }
    public double E13 { get; set; }
    public double E23 { get; set; }
    public double E33 { get; set; }
}
