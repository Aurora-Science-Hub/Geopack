using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public T GeoToGsw<T>(ComputationContext context, T coordinates) where T : ICartesian
        => TransformCoordinates(context, coordinates, OperationType.Direct);

    public T GswToGeo<T>(ComputationContext context, T coordinates) where T : ICartesian
        => TransformCoordinates(context, coordinates, OperationType.Reversed);

    private T TransformCoordinates<T>(ComputationContext context, T coordinates, OperationType operation)
        where T : ICartesian
        => operation switch
        {
            OperationType.Direct => coordinates.Create<T>(
                context.A11 * coordinates.X + context.A12 * coordinates.Y + context.A13 * coordinates.Z,
                context.A21 * coordinates.X + context.A22 * coordinates.Y + context.A23 * coordinates.Z,
                context.A31 * coordinates.X + context.A32 * coordinates.Y + context.A33 * coordinates.Z,
                coordinates.CoordinateSystem),
            OperationType.Reversed => coordinates.Create<T>(
                context.A11 * coordinates.X + context.A21 * coordinates.Y + context.A31 * coordinates.Z,
                context.A12 * coordinates.X + context.A22 * coordinates.Y + context.A32 * coordinates.Z,
                context.A13 * coordinates.X + context.A23 * coordinates.Y + context.A33 * coordinates.Z,
                coordinates.CoordinateSystem),
            _ => throw new NotSupportedException("Specify correct OperationType. Available types are Direct and Reversed.")
        };
}
