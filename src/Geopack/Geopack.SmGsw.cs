using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
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
                    components.X * context.CPS + components.Z * context.SPS,
                    components.Y,
                    components.Z * context.CPS - components.X * context.SPS,
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in SM system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    components.X * context.CPS - components.Z * context.SPS,
                    components.Y,
                    components.X * context.SPS + components.Z * context.CPS,
                    CoordinateSystem.SM)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GSW system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
