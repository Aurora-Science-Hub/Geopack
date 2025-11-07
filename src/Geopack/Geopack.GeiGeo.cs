using AuroraScienceHub.Geopack.Contracts;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;

namespace AuroraScienceHub.Geopack;

internal sealed partial class Geopack
{
    public T GeiToGeo<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeiGeoInternal(context, components, OperationType.Direct);

    public T GeoToGei<T>(ComputationContext context, T components) where T : ICartesian<T>
        => GeiGeoInternal(context, components, OperationType.Reversed);

    private static T GeiGeoInternal<T>(ComputationContext context, T components, OperationType operation)
        where T : ICartesian<T>
    {
        double xCgst = components.X * context.CGST;
        double xSgst = components.X * context.SGST;
        double yCgst = components.Y * context.CGST;
        double ySgst = components.Y * context.SGST;

        return operation switch
        {
            OperationType.Direct => components.CoordinateSystem is CoordinateSystem.GEI
                ? T.New(
                    xCgst + ySgst,
                    yCgst - xSgst,
                    components.Z,
                    CoordinateSystem.GEO)
                : throw new InvalidOperationException("Input coordinates must be in GEI system."),

            OperationType.Reversed => components.CoordinateSystem is CoordinateSystem.GEO
                ? T.New(
                    xCgst - ySgst,
                    yCgst + xSgst,
                    components.Z,
                    CoordinateSystem.GEI)
                : throw new InvalidOperationException("Input coordinates must be in GEO system."),
            _ => throw new NotSupportedException($"Specify correct OperationType: {operation}. Available types are Direct and Reversed.")
        };
    }
}
