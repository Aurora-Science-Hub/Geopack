using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public T MagToSm<T>(ComputationContext context, T components) where T : ICartesian<T>
        => MagSmInternal(context, components, OperationType.Direct);

    public T SmToMag<T>(ComputationContext context, T components) where T : ICartesian<T>
        => MagSmInternal(context, components, OperationType.Reversed);

    private static T MagSmInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.MAG
                ? T.New(
                    components.X * context.CFI - components.Y * context.SFI,
                    components.X * context.SFI + components.Y * context.CFI,
                    components.Z,
                    CoordinateSystem.SM)
                : throw new InvalidOperationException("Input coordinates must be in MAG system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.SM
                ? T.New(
                    components.X * context.CFI + components.Y * context.SFI,
                    components.Y * context.CFI - components.X * context.SFI,
                    components.Z,
                    CoordinateSystem.MAG)
                : throw new InvalidOperationException("Input coordinates must be in SM system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
