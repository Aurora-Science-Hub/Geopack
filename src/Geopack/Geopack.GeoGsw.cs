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
    {
        double x = components.X;
        double y = components.Y;
        double z = components.Z;

        // Cache matrix coefficients from context
        double a11 = context.A11, a12 = context.A12, a13 = context.A13;
        double a21 = context.A21, a22 = context.A22, a23 = context.A23;
        double a31 = context.A31, a32 = context.A32, a33 = context.A33;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                    a11 * x + a12 * y + a13 * z,
                    a21 * x + a22 * y + a23 * z,
                    a31 * x + a32 * y + a33 * z,
                    CoordinateSystem.GSW)
                : throw new InvalidOperationException("Input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GSW
                ? T.New(
                    a11 * x + a21 * y + a31 * z,
                    a12 * x + a22 * y + a32 * z,
                    a13 * x + a23 * y + a33 * z,
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Input coordinates must be in GSW system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
