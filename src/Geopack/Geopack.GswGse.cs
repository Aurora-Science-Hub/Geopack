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
    {
        double x = components.X;
        double y = components.Y;
        double z = components.Z;

        // Cache matrix coefficients from context
        double e11 = context.E11, e12 = context.E12, e13 = context.E13;
        double e21 = context.E21, e22 = context.E22, e23 = context.E23;
        double e31 = context.E31, e32 = context.E32, e33 = context.E33;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    e11 * x + e12 * y + e13 * z,
                    e21 * x + e22 * y + e23 * z,
                    e31 * x + e32 * y + e33 * z,
                    CoordinateSystem.GSE)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSE
                ? T.New(
                    e11 * x + e21 * y + e31 * z,
                    e12 * x + e22 * y + e32 * z,
                    e13 * x + e23 * y + e33 * z,
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in GSE system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
