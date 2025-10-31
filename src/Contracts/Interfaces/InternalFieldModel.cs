using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Contracts.Interfaces;

/// <summary>
/// Delegate for internal field models
/// </summary>
public delegate CartesianVector<MagneticField> InternalFieldModel(ComputationContext ctx, double x, double y, double z);
