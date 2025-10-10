using AuroraScienceHub.Geopack.Contracts.Interfaces;
using AuroraScienceHub.Geopack.Contracts.Models;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Trace field line from geostationary orbit")]
    public void TraceFieldLineFromGeostationaryOrbit()
    {
        // Arrange
        InternalFieldModel internalField = _geopack.IgrfGsw;

        double GeoLat = 75.0D;
        double GeoLon = 45.0D;
        double Re = 1.0D;
        double CoLat= (90.0D-GeoLat) * 0.01745329D;
        double XLon = GeoLon * 0.01745329D;

        AuroraScienceHub.Geopack.Geopack.TraceDirection dir = AuroraScienceHub.Geopack.Geopack.TraceDirection.AntiParallel;
        double dsmax = 1.0D;
        double err = 0.0001D;
        double rlim = 60.0D;
        double r0 = 1.0D;
        int iopt = 1;
        double[] parmod = new double[10];
        int lmax = 500;

        // Act
        _geopack.Recalc(fixture.InputData.DateTime, -304.0D, -16.0D, 4.0D+29.78D);
        CartesianLocation geoLoc = _geopack.SphCar(Re, CoLat, XLon);
        CartesianLocation gswLoc = _geopack.GeoGsw(geoLoc.X, geoLoc.Y, geoLoc.Z);

        FieldLine fieldLine = _geopack.Trace_08(
            gswLoc.X, gswLoc.Y, gswLoc.Z,
            dir, dsmax, err, rlim, r0,
            iopt, parmod,
            _t89, internalField,
            lmax);

        // Assert
        fieldLine.ActualPointCount.ShouldBePositive();
    }
}
