namespace AuroraScienceHub.Geopack.UnitTests.Geopack2008.TestData;

public sealed record ApprovedGeopackData
{
    /// <summary>
    /// Date and Time
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// X-component of the solar wind velocity
    /// </summary>
    public float VGSEX { get; set; }

    /// <summary>
    /// Y-component of the solar wind velocity
    /// </summary>
    public float VGSEY { get; set; }

    /// <summary>
    /// Z-component of the solar wind velocity
    /// </summary>
    public float VGSEZ { get; set; }

    /// <summary>
    /// Solar wind pressure
    /// </summary>
    public float SolarWindPressure { get; set; }

    /// <summary>
    /// Dst index
    /// </summary>
    public float DstIndex { get; set; }

    /// <summary>
    /// Y-component of the interplanetary magnetic field
    /// </summary>
    public float ByIMF { get; set; }

    /// <summary>
    /// Z-component of the interplanetary magnetic field
    /// </summary>
    public float BzIMF { get; set; }

    /// <summary>
    /// Common2 block coefficients
    /// </summary>
    public List<(float G, float H, float REC)>? Common2 { get; set; } = [];

    /// <summary>
    /// Field line coordinates
    /// </summary>
    public List<(float X, float Y, float Z)>? FieldLineCoordinates { get; set; } = [];

    /// <summary>
    /// Math conversion and matrix coefficients
    /// </summary>
    public float ST0 { get; set; }
    public float CT0 { get; set; }
    public float SL0 { get; set; }
    public float CL0 { get; set; }
    public float CTCL { get; set; }
    public float STCL { get; set; }
    public float CTSL { get; set; }
    public float STSL { get; set; }
    public float SFI { get; set; }
    public float CFI { get; set; }
    public float SPS { get; set; }
    public float CPS { get; set; }
    public float DS3 { get; set; }
    public float CGST { get; set; }
    public float SGST { get; set; }
    public float PSI { get; set; }
    public float A11 { get; set; }
    public float A21 { get; set; }
    public float A31 { get; set; }
    public float A12  { get; set; }
    public float A22 { get; set; }
    public float A32 { get; set; }
    public float A13 { get; set; }
    public float A23 { get; set; }
    public float A33 { get; set; }
    public float E11 { get; set; }
    public float E21 { get; set; }
    public float E31 { get; set; }
    public float E12 { get; set; }
    public float E22 { get; set; }
    public float E32 { get; set; }
    public float E13 { get; set; }
    public float E23 { get; set; }
    public float E33 { get; set; }
}
