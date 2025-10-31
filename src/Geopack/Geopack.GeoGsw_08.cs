using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public static T GeoToGsw<T>(ComputationContext context, T coordinates) where T : ICartesian<T>
        => TransformCoordinates(context, coordinates, OperationType.Direct);

    public static T GswToGeo<T>(ComputationContext context, T coordinates) where T : ICartesian<T>
        => TransformCoordinates(context, coordinates, OperationType.Reversed);

    private static T TransformCoordinates<T>(ComputationContext context, T coordinates, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => T.New(
                context.A11 * coordinates.X + context.A12 * coordinates.Y + context.A13 * coordinates.Z,
                context.A21 * coordinates.X + context.A22 * coordinates.Y + context.A23 * coordinates.Z,
                context.A31 * coordinates.X + context.A32 * coordinates.Y + context.A33 * coordinates.Z,
                coordinates.CoordinateSystem),
            OperationType.Reversed => T.New(
                context.A11 * coordinates.X + context.A21 * coordinates.Y + context.A31 * coordinates.Z,
                context.A12 * coordinates.X + context.A22 * coordinates.Y + context.A32 * coordinates.Z,
                context.A13 * coordinates.X + context.A23 * coordinates.Y + context.A33 * coordinates.Z,
                coordinates.CoordinateSystem),
            _ => throw new NotSupportedException("Specify correct OperationType. Available types are Direct and Reversed.")
        };
}
