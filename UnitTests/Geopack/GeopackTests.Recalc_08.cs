using AuroraScienceHub.Geopack.Contracts.Engine;
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
        ComputationContext ctx = _geopack.Recalc_08(
            fixture.InputData.DateTime,
            fixture.InputData.VGSEX,
            fixture.InputData.VGSEY,
            fixture.InputData.VGSEZ);

        // Assert
        string rawData = await EmbeddedResourceReader.ReadTextAsync(CommonsDataFileName);
        GeopackCommons approvedData = GeopackDataParser.ParseRecalcCommons(rawData);

        for (int i = 0; i < ctx.G.Length; i++)
        {
            ctx.G[i].ShouldBe(approvedData.G![i], MinimalTestsPrecision);
            ctx.H[i].ShouldBe(approvedData.H![i], MinimalTestsPrecision);
            ctx.REC[i].ShouldBe(approvedData.REC![i], MinimalTestsPrecision);
        }
        ctx.ST0.ShouldBe(approvedData.ST0, MinimalTestsPrecision);
        ctx.CT0.ShouldBe(approvedData.CT0, MinimalTestsPrecision);
        ctx.SL0.ShouldBe(approvedData.SL0, MinimalTestsPrecision);
        ctx.CL0.ShouldBe(approvedData.CL0, MinimalTestsPrecision);
        ctx.CTCL.ShouldBe(approvedData.CTCL, MinimalTestsPrecision);
        ctx.STCL.ShouldBe(approvedData.STCL, MinimalTestsPrecision);
        ctx.CTSL.ShouldBe(approvedData.CTSL, MinimalTestsPrecision);
        ctx.STSL.ShouldBe(approvedData.STSL, MinimalTestsPrecision);
        ctx.SFI.ShouldBe(approvedData.SFI, MinimalTestsPrecision);
        ctx.CFI.ShouldBe(approvedData.CFI, MinimalTestsPrecision);
        ctx.SPS.ShouldBe(approvedData.SPS, MinimalTestsPrecision);
        ctx.CPS.ShouldBe(approvedData.CPS, MinimalTestsPrecision);
        ctx.CGST.ShouldBe(approvedData.CGST, MinimalTestsPrecision);
        ctx.SGST.ShouldBe(approvedData.SGST, MinimalTestsPrecision);
        ctx.PSI.ShouldBe(approvedData.PSI, MinimalTestsPrecision);
        ctx.A11.ShouldBe(approvedData.A11, MinimalTestsPrecision);
        ctx.A21.ShouldBe(approvedData.A21, MinimalTestsPrecision);
        ctx.A31.ShouldBe(approvedData.A31, MinimalTestsPrecision);
        ctx.A12.ShouldBe(approvedData.A12, MinimalTestsPrecision);
        ctx.A22.ShouldBe(approvedData.A22, MinimalTestsPrecision);
        ctx.A32.ShouldBe(approvedData.A32, MinimalTestsPrecision);
        ctx.A13.ShouldBe(approvedData.A13, MinimalTestsPrecision);
        ctx.A23.ShouldBe(approvedData.A23, MinimalTestsPrecision);
        ctx.A33.ShouldBe(approvedData.A33, MinimalTestsPrecision);
        ctx.E11.ShouldBe(approvedData.E11, MinimalTestsPrecision);
        ctx.E21.ShouldBe(approvedData.E21, MinimalTestsPrecision);
        ctx.E31.ShouldBe(approvedData.E31, MinimalTestsPrecision);
        ctx.E12.ShouldBe(approvedData.E12, MinimalTestsPrecision);
        ctx.E22.ShouldBe(approvedData.E22, MinimalTestsPrecision);
        ctx.E32.ShouldBe(approvedData.E32, MinimalTestsPrecision);
        ctx.E13.ShouldBe(approvedData.E13, MinimalTestsPrecision);
        ctx.E23.ShouldBe(approvedData.E23, MinimalTestsPrecision);
        ctx.E33.ShouldBe(approvedData.E33, MinimalTestsPrecision);
    }
}
