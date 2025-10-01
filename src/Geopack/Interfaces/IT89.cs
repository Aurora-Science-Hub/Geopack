using AuroraScienceHub.Geopack.Contracts;

namespace AuroraScienceHub.Geopack.Interfaces;

public interface IT89
{
    /// <summary>
    /// Computes GSM components of the magnetic field produced by extra terrestrial current systems in the magnetosphere.
    /// </summary>
    /// <remarks>
    /// The model is valid up to geocentric distances of 70 RE and is based on merged spacecraft data sets.
    /// </remarks>
    /// <param name="iopt">
    ///     Specifies the ground disturbance level:
    ///     IOPT= 1       2        3        4        5        6      7
    ///     CORRESPOND TO:
    ///     KP= 0,0+  1-,1,1+  2-,2,2+  3-,3,3+  4-,4,4+  5-,5,5+  >=6-
    /// </param>
    /// <param name="parmod">
    ///     Dummy array, not used in this subroutine, provided for compatibility with the new version of the GEOPACK software.
    /// </param>
    /// <param name="ps">Geodipole tilt angle in radians</param>
    /// <param name="x">GSM x-coordinate in Earth radii</param>
    /// <param name="y">GSM y-coordinate in Earth radii</param>
    /// <param name="z">GSM z-coordinate in Earth radii</param>
    CartesianFieldVector Calculate(
        int iopt,
        double[] parmod,
        double ps,
        double x, double y, double z);
}
