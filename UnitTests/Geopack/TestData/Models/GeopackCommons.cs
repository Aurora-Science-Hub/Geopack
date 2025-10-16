namespace AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.Models;

public sealed record GeopackCommons
{
    public List<double>? G { get; set; } = [];

    public List<double>? H { get; set; } = [];

    public List<double>? REC { get; set; } = [];

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
