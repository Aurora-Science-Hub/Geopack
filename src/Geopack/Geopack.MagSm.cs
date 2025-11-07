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
    {
        double x = components.X;
        double y = components.Y;
        double z = components.Z;

        // Cache context coefficients
        double cfi = context.CFI;
        double sfi = context.SFI;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.MAG
                ? T.New(
                    x * cfi - y * sfi,
                    x * sfi + y * cfi,
                    z,
                    CoordinateSystem.SM)
                : throw new InvalidOperationException("Input coordinates must be in MAG system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.SM
                ? T.New(
                    x * cfi + y * sfi,
                    y * cfi - x * sfi,
                    z,
                    CoordinateSystem.MAG)
                : throw new InvalidOperationException("Input coordinates must be in SM system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
