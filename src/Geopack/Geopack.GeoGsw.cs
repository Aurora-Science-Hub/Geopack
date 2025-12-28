using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
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
                Math.FusedMultiplyAdd(context.A11, components.X, Math.FusedMultiplyAdd(context.A12, components.Y, context.A13 * components.Z)),
                Math.FusedMultiplyAdd(context.A21, components.X, Math.FusedMultiplyAdd(context.A22, components.Y, context.A23 * components.Z)),
                Math.FusedMultiplyAdd(context.A31, components.X, Math.FusedMultiplyAdd(context.A32, components.Y, context.A33 * components.Z)),
                CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                Math.FusedMultiplyAdd(context.A11, components.X, Math.FusedMultiplyAdd(context.A21, components.Y, context.A31 * components.Z)),
                Math.FusedMultiplyAdd(context.A12, components.X, Math.FusedMultiplyAdd(context.A22, components.Y, context.A32 * components.Z)),
                Math.FusedMultiplyAdd(context.A13, components.X, Math.FusedMultiplyAdd(context.A23, components.Y, context.A33 * components.Z)),
                CoordinateSystem.GEO)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
