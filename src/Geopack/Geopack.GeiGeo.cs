using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;

namespace AuroraScienceHub.Geopack;

public sealed partial class Geopack
{
    public T GeiToGeo<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeiGeoInternal(context, components, OperationType.Direct);

    public T GeoToGei<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeiGeoInternal(context, components, OperationType.Reversed);

    private static T GeiGeoInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
        => operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEI
                ? T.New(
                    components.X * context.CGST + components.Y * context.SGST,
                    components.Y * context.CGST - components.X * context.SGST,
                    components.Z,
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GEI system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                    components.X * context.CGST - components.Y * context.SGST,
                    components.Y * context.CGST + components.X * context.SGST,
                    components.Z,
                    CoordinateSystem.GEI)
                : throw new InvalidOperationException("Invalid transformation: the input coordinates must be in GEO system."),
            _ => throw new NotSupportedException("Specify correct OperationType. Available types are Direct and Reversed.")
        };
}
