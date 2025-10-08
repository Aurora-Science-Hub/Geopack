using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Contracts.Interfaces;

/// <summary>
/// External contract for internal magnetic field models
/// </summary>
public interface IInternalFieldModel
{
    CartesianFieldVector Calculate(double x, double y, double z);
}
