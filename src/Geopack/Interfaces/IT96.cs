namespace AuroraScienceHub.Geopack.Interfaces;

public interface IT96
{
    /// <summary>
    /// Data-based model calibrated by solar wind pressure (PDYN), DST, BYIMF, and BZIMF.
    /// </summary>
    /// <remarks>
    /// These input parameters should be placed in the first 4 elements of the array PARMOD(10).
    /// </remarks>
    /// <param name="iopt">
    /// Dummy input parameter, necessary to make this subroutine compatible with the new release of the tracing software package (GEOPACK).
    /// IOPT value does not affect the output field.
    /// </param>
    /// <param name="parmod">
    /// Array containing input parameters needed for a unique specification of the external field model.
    /// </param>
    /// <param name="ps">Geodipole tilt angle in radians</param>
    /// <param name="x">GSM x-coordinate in Earth radii</param>
    /// <param name="y">GSM y-coordinate in Earth radii</param>
    /// <param name="z">GSM z-coordinate in Earth radii</param>
    /// <param name="bx">GSM x-component of the external magnetic field in nanotesla</param>
    /// <param name="by">GSM y-component of the external magnetic field in nanotesla</param>
    /// <param name="bz">GSM z-component of the external magnetic field in nanotesla</param>
    void T96_01(
        int iopt,
        float[] parmod,
        float ps,
        float x, float y, float z,
        out float bx, out float by, out float bz);
}
