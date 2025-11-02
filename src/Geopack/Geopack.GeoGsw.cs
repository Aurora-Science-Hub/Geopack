using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public T GeoToGsw<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeoGswInternal(context, components, OperationType.Direct);

    public T GswToGeo<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeoGswInternal(context, components, OperationType.Reversed);

    private static T GeoGswInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                context.A11 * components.X + context.A12 * components.Y + context.A13 * components.Z,
                context.A21 * components.X + context.A22 * components.Y + context.A23 * components.Z,
                context.A31 * components.X + context.A32 * components.Y + context.A33 * components.Z,
                CoordinateSystem.GSW)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                context.A11 * components.X + context.A21 * components.Y + context.A31 * components.Z,
                context.A12 * components.X + context.A22 * components.Y + context.A32 * components.Z,
                context.A13 * components.X + context.A23 * components.Y + context.A33 * components.Z,
                CoordinateSystem.GEO)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GSW system."),
            _ => throw new NotSupportedException("Specify correct OperationType. Available types are Direct and Reversed.")
        };
}
