using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack.Contracts.Interfaces;

/// <summary>
/// Delegate for internal field models
/// </summary>
public delegate CartesianFieldVector InternalFieldModel(double x, double y, double z);
