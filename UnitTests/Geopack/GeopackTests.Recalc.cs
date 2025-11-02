using AuroraScienceHub.Geopack.Contracts.Engine;
using AuroraScienceHub.Geopack.Contracts.Models;
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
            context.G[i].ShouldBe(approvedData.G![i], MinimalTestsPrecision);
            context.H[i].ShouldBe(approvedData.H![i], MinimalTestsPrecision);
            context.REC[i].ShouldBe(approvedData.REC![i], MinimalTestsPrecision);
        }

        context.ST0.ShouldBe(approvedData.ST0, MinimalTestsPrecision);
        context.CT0.ShouldBe(approvedData.CT0, MinimalTestsPrecision);
        context.SL0.ShouldBe(approvedData.SL0, MinimalTestsPrecision);
        context.CL0.ShouldBe(approvedData.CL0, MinimalTestsPrecision);
        context.CTCL.ShouldBe(approvedData.CTCL, MinimalTestsPrecision);
        context.STCL.ShouldBe(approvedData.STCL, MinimalTestsPrecision);
        context.CTSL.ShouldBe(approvedData.CTSL, MinimalTestsPrecision);
        context.STSL.ShouldBe(approvedData.STSL, MinimalTestsPrecision);
        context.SFI.ShouldBe(approvedData.SFI, MinimalTestsPrecision);
        context.CFI.ShouldBe(approvedData.CFI, MinimalTestsPrecision);
        context.SPS.ShouldBe(approvedData.SPS, MinimalTestsPrecision);
        context.CPS.ShouldBe(approvedData.CPS, MinimalTestsPrecision);
        context.CGST.ShouldBe(approvedData.CGST, MinimalTestsPrecision);
        context.SGST.ShouldBe(approvedData.SGST, MinimalTestsPrecision);
        context.PSI.ShouldBe(approvedData.PSI, MinimalTestsPrecision);
        context.A11.ShouldBe(approvedData.A11, MinimalTestsPrecision);
        context.A21.ShouldBe(approvedData.A21, MinimalTestsPrecision);
        context.A31.ShouldBe(approvedData.A31, MinimalTestsPrecision);
        context.A12.ShouldBe(approvedData.A12, MinimalTestsPrecision);
        context.A22.ShouldBe(approvedData.A22, MinimalTestsPrecision);
        context.A32.ShouldBe(approvedData.A32, MinimalTestsPrecision);
        context.A13.ShouldBe(approvedData.A13, MinimalTestsPrecision);
        context.A23.ShouldBe(approvedData.A23, MinimalTestsPrecision);
        context.A33.ShouldBe(approvedData.A33, MinimalTestsPrecision);
        context.E11.ShouldBe(approvedData.E11, MinimalTestsPrecision);
        context.E21.ShouldBe(approvedData.E21, MinimalTestsPrecision);
        context.E31.ShouldBe(approvedData.E31, MinimalTestsPrecision);
        context.E12.ShouldBe(approvedData.E12, MinimalTestsPrecision);
        context.E22.ShouldBe(approvedData.E22, MinimalTestsPrecision);
        context.E32.ShouldBe(approvedData.E32, MinimalTestsPrecision);
        context.E13.ShouldBe(approvedData.E13, MinimalTestsPrecision);
        context.E23.ShouldBe(approvedData.E23, MinimalTestsPrecision);
        context.E33.ShouldBe(approvedData.E33, MinimalTestsPrecision);
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
