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
                    Math.FusedMultiplyAdd(components.X, context.CTCL, Math.FusedMultiplyAdd(components.Y, context.CTSL, -components.Z * context.ST0)),
                    Math.FusedMultiplyAdd(components.Y, context.CL0, -components.X * context.SL0),
                    Math.FusedMultiplyAdd(components.X, context.STCL, Math.FusedMultiplyAdd(components.Y, context.STSL, components.Z * context.CT0)),
                    CoordinateSystem.MAG)
                : throw new InvalidOperationException("Input coordinates must be in GEO system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.MAG
                ? T.New(
                    Math.FusedMultiplyAdd(components.X, context.CTCL, Math.FusedMultiplyAdd(-components.Y, context.SL0, components.Z * context.STCL)),
                    Math.FusedMultiplyAdd(components.X, context.CTSL, Math.FusedMultiplyAdd(components.Y, context.CL0, components.Z * context.STSL)),
                    Math.FusedMultiplyAdd(components.Z, context.CT0, -components.X * context.ST0),
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Input coordinates must be in MAG system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
}
