using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;

namespace AuroraScienceHub.Geopack.Contracts;

/// <summary>
/// Delegate for internal field models
/// </summary>
public delegate CartesianVector<MagneticField> InternalFieldModel(ComputationContext ctx, CartesianLocation location);
