using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public T GswToGse<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GswGseInternal(context, components, OperationType.Direct);

    public T GseToGsw<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GswGseInternal(context, components, OperationType.Reversed);

    private static T GswGseInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    Math.FusedMultiplyAdd(context.E11, components.X, Math.FusedMultiplyAdd(context.E12, components.Y, context.E13 * components.Z)),
                    Math.FusedMultiplyAdd(context.E21, components.X, Math.FusedMultiplyAdd(context.E22, components.Y, context.E23 * components.Z)),
                    Math.FusedMultiplyAdd(context.E31, components.X, Math.FusedMultiplyAdd(context.E32, components.Y, context.E33 * components.Z)),
                    CoordinateSystem.GSE)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSE
                ? T.New(
                    Math.FusedMultiplyAdd(context.E11, components.X, Math.FusedMultiplyAdd(context.E21, components.Y, context.E31 * components.Z)),
                    Math.FusedMultiplyAdd(context.E12, components.X, Math.FusedMultiplyAdd(context.E22, components.Y, context.E32 * components.Z)),
                    Math.FusedMultiplyAdd(context.E13, components.X, Math.FusedMultiplyAdd(context.E23, components.Y, context.E33 * components.Z)),
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in GSE system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
