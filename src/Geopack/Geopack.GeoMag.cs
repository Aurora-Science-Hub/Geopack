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
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                    components.X * context.CTCL + components.Y * context.CTSL - components.Z * context.ST0,
                    components.Y * context.CL0 - components.X * context.SL0,
                    components.X * context.STCL + components.Y * context.STSL + components.Z * context.CT0,
                    CoordinateSystem.MAG)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.MAG
                ? T.New(
                    components.X * context.CTCL - components.Y * context.SL0 + components.Z * context.STCL,
                    components.X * context.CTSL + components.Y * context.CL0 + components.Z * context.STSL,
                    components.Z * context.CT0 - components.X * context.ST0,
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in MAG system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
