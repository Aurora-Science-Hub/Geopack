using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Contracts.Interfaces;

/// <summary>
/// External contract for external magnetic field models
/// </summary>
public interface IExternalFieldModel
{
    CartesianFieldVector Calculate(int iopt, double[] parmod, double psi, double x, double y, double z);
}
