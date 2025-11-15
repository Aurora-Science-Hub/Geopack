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
    {
        double x = components.X;
        double y = components.Y;
        double z = components.Z;

        // Cache context coefficients
        double cps = context.CPS;
        double sps = context.SPS;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.SM
                ? T.New(
                    x * cps + z * sps,
                    y,
                    z * cps - x * sps,
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in SM system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    x * cps - z * sps,
                    y,
                    x * sps + z * cps,
                    CoordinateSystem.SM)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
