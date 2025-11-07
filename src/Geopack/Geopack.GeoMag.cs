using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public T GeoToMag<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeoMagInternal(context, components, OperationType.Direct);

    public T MagToGeo<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeoMagInternal(context, components, OperationType.Reversed);

    private static T GeoMagInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
    {
        double x = components.X;
        double y = components.Y;
        double z = components.Z;

        // Cache context coefficients
        double ctcl = context.CTCL, ctsl = context.CTSL, st0 = context.ST0;
        double cl0 = context.CL0, sl0 = context.SL0;
        double stcl = context.STCL, stsl = context.STSL, ct0 = context.CT0;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                    x * ctcl + y * ctsl - z * st0,
                    y * cl0 - x * sl0,
                    x * stcl + y * stsl + z * ct0,
                    CoordinateSystem.MAG)
                : throw new InvalidOperationException("Input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.MAG
                ? T.New(
                    x * ctcl - y * sl0 + z * stcl,
                    x * ctsl + y * cl0 + z * stsl,
                    z * ct0 - x * st0,
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Input coordinates must be in MAG system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
