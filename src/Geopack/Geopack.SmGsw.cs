using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public T SmToGsw<T>(ComputationContext context, T components) where T : ICartesian<T>
        => SmGswInternal(context, components, OperationType.Direct);

    public T GswToSm<T>(ComputationContext context, T components) where T : ICartesian<T>
        => SmGswInternal(context, components, OperationType.Reversed);

    private static T SmGswInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.SM
                ? T.New(
                    Math.FusedMultiplyAdd(components.X, context.CPS, components.Z * context.SPS),
                    components.Y,
                    Math.FusedMultiplyAdd(components.Z, context.CPS, -components.X * context.SPS),
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in SM system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    Math.FusedMultiplyAdd(components.X, context.CPS, -components.Z * context.SPS),
                    components.Y,
                    Math.FusedMultiplyAdd(components.X, context.SPS, components.Z * context.CPS),
                    CoordinateSystem.SM)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
