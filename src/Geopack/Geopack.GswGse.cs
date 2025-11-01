using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public T GswToGse<T>(ComputationContext context, T coordinates) where T : ICartesian<T>
        => GswGseInternal(context, coordinates, OperationType.Direct);

    public T GseToGsw<T>(ComputationContext context, T coordinates) where T : ICartesian<T>
        => GswGseInternal(context, coordinates, OperationType.Reversed);

    private static T GswGseInternal<T>(ComputationContext context, T coordinates, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => coordinates.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    context.E11 * coordinates.X + context.E12 * coordinates.Y + context.E13 * coordinates.Z,
                    context.E21 * coordinates.X + context.E22 * coordinates.Y + context.E23 * coordinates.Z,
                    context.E31 * coordinates.X + context.E32 * coordinates.Y + context.E33 * coordinates.Z,
                    CoordinateSystem.GSE)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GSW system for Direct operation."),

            OperationType.Reversed => coordinates.CoordinateSystem is CoordinateSystem.GSE
                ? T.New(
                    context.E11 * coordinates.X + context.E21 * coordinates.Y + context.E31 * coordinates.Z,
                    context.E12 * coordinates.X + context.E22 * coordinates.Y + context.E32 * coordinates.Z,
                    context.E13 * coordinates.X + context.E23 * coordinates.Y + context.E33 * coordinates.Z,
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GSE system for Direct operation."),
            _ => throw new NotSupportedException("Specify correct OperationType. Available types are Direct and Reversed.")
        };
}
