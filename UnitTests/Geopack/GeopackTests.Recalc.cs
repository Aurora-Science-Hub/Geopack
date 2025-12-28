using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using AuroraScienceHub.Geopack.UnitTests.Extensions;
using AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Basic test: Computation context should be correct")]
    public async Task RecalcCommonBlocks_ShouldBeCorrect()
    {
        // Act
        ComputationContext context = s_geopack.Recalc(
            fixture.InputData.DateTime,
            CartesianVector<Velocity>.New(fixture.InputData.VGSEX, fixture.InputData.VGSEY, fixture.InputData.VGSEZ,
                CoordinateSystem.GSE));

        // Assert
        string rawData = await EmbeddedResourceReader.ReadTextAsync(CommonsDataFileName);
        GeopackCommons approvedData = GeopackDataParser.ParseRecalcCommons(rawData);

        for (int i = 0; i < context.G.Length; i++)
        {
            context.G[i].ShouldApproximatelyBe(approvedData.G![i]);
            context.H[i].ShouldApproximatelyBe(approvedData.H![i]);
            context.REC[i].ShouldApproximatelyBe(approvedData.REC![i]);
        }

        context.ST0.ShouldApproximatelyBe(approvedData.ST0);
        context.CT0.ShouldApproximatelyBe(approvedData.CT0);
        context.SL0.ShouldApproximatelyBe(approvedData.SL0);
        context.CL0.ShouldApproximatelyBe(approvedData.CL0);
        context.CTCL.ShouldApproximatelyBe(approvedData.CTCL);
        context.STCL.ShouldApproximatelyBe(approvedData.STCL);
        context.CTSL.ShouldApproximatelyBe(approvedData.CTSL);
        context.STSL.ShouldApproximatelyBe(approvedData.STSL);
        context.SFI.ShouldApproximatelyBe(approvedData.SFI);
        context.CFI.ShouldApproximatelyBe(approvedData.CFI);
        context.SPS.ShouldApproximatelyBe(approvedData.SPS);
        context.CPS.ShouldApproximatelyBe(approvedData.CPS);
        context.CGST.ShouldApproximatelyBe(approvedData.CGST);
        context.SGST.ShouldApproximatelyBe(approvedData.SGST);
        context.PSI.ShouldApproximatelyBe(approvedData.PSI);
        context.A11.ShouldApproximatelyBe(approvedData.A11);
        context.A21.ShouldApproximatelyBe(approvedData.A21);
        context.A31.ShouldApproximatelyBe(approvedData.A31);
        context.A12.ShouldApproximatelyBe(approvedData.A12);
        context.A22.ShouldApproximatelyBe(approvedData.A22);
        context.A32.ShouldApproximatelyBe(approvedData.A32);
        context.A13.ShouldApproximatelyBe(approvedData.A13);
        context.A23.ShouldApproximatelyBe(approvedData.A23);
        context.A33.ShouldApproximatelyBe(approvedData.A33);
        context.E11.ShouldApproximatelyBe(approvedData.E11);
        context.E21.ShouldApproximatelyBe(approvedData.E21);
        context.E31.ShouldApproximatelyBe(approvedData.E31);
        context.E12.ShouldApproximatelyBe(approvedData.E12);
        context.E22.ShouldApproximatelyBe(approvedData.E22);
        context.E32.ShouldApproximatelyBe(approvedData.E32);
        context.E13.ShouldApproximatelyBe(approvedData.E13);
        context.E23.ShouldApproximatelyBe(approvedData.E23);
        context.E33.ShouldApproximatelyBe(approvedData.E33);
    }

    [Fact(DisplayName = "Recalc should throw if incorrect sw velocity coordinate system")]
    public void Recalc_Throw()
    {
        // Act
        Action act = () => s_geopack.Recalc(
            fixture.InputData.DateTime,
            CartesianVector<Velocity>.New(fixture.InputData.VGSEX, fixture.InputData.VGSEY, fixture.InputData.VGSEZ,
                CoordinateSystem.GSW));

        // Assert
        act.ShouldThrow<InvalidOperationException>();
    }
}
