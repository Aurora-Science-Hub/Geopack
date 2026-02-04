using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// External contract for external magnetic field models
/// </summary>
public interface IExternalFieldModel
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
    /// <param name="psi">Geodipole tilt angle in radians</param>
    /// <param name="location">Location with cartesian GSM coordinates</param>
    CartesianVector<MagneticField> Calculate(int iopt, double[] parmod, double psi, CartesianLocation location);
}
