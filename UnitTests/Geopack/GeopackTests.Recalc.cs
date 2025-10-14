using AuroraScienceHub.Geopack.UnitTests.Models;
using AuroraScienceHub.Geopack.UnitTests.Utils;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.Geopack;

public partial class GeopackTests
{
    [Fact(DisplayName = "Basic test: Recalc Common1 & Common2 should be correct")]
    public async Task RecalcCommonBlocks_ShouldBeCorrect()
    {
        // Act
        (Common1 common1, Common2 common2) = _geopack.Recalc_08(
            fixture.InputData.DateTime,
            fixture.InputData.VGSEX,
            fixture.InputData.VGSEY,
            fixture.InputData.VGSEZ);

        // Assert
        string rawData = await EmbeddedResourceReader.ReadTextAsync(CommonsDataFileName);
        GeopackCommons approvedData = GeopackDataParser.ParseRecalcCommons(rawData);

        for (int i = 0; i < common2.G.Length; i++)
        {
            common2.G[i].ShouldBe(approvedData.G![i], MinimalTestsPrecision);
            common2.H[i].ShouldBe(approvedData.H![i], MinimalTestsPrecision);
            common2.REC[i].ShouldBe(approvedData.REC![i], MinimalTestsPrecision);
        }
        common1.ST0.ShouldBe(approvedData.ST0, MinimalTestsPrecision);
        common1.CT0.ShouldBe(approvedData.CT0, MinimalTestsPrecision);
        common1.SL0.ShouldBe(approvedData.SL0, MinimalTestsPrecision);
        common1.CL0.ShouldBe(approvedData.CL0, MinimalTestsPrecision);
        common1.CTCL.ShouldBe(approvedData.CTCL, MinimalTestsPrecision);
        common1.STCL.ShouldBe(approvedData.STCL, MinimalTestsPrecision);
        common1.CTSL.ShouldBe(approvedData.CTSL, MinimalTestsPrecision);
        common1.STSL.ShouldBe(approvedData.STSL, MinimalTestsPrecision);
        common1.SFI.ShouldBe(approvedData.SFI, MinimalTestsPrecision);
        common1.CFI.ShouldBe(approvedData.CFI, MinimalTestsPrecision);
        common1.SPS.ShouldBe(approvedData.SPS, MinimalTestsPrecision);
        common1.CPS.ShouldBe(approvedData.CPS, MinimalTestsPrecision);
        common1.DS3.ShouldBe(approvedData.DS3, MinimalTestsPrecision);
        common1.CGST.ShouldBe(approvedData.CGST, MinimalTestsPrecision);
        common1.SGST.ShouldBe(approvedData.SGST, MinimalTestsPrecision);
        common1.PSI.ShouldBe(approvedData.PSI, MinimalTestsPrecision);
        common1.A11.ShouldBe(approvedData.A11, MinimalTestsPrecision);
        common1.A21.ShouldBe(approvedData.A21, MinimalTestsPrecision);
        common1.A31.ShouldBe(approvedData.A31, MinimalTestsPrecision);
        common1.A12.ShouldBe(approvedData.A12, MinimalTestsPrecision);
        common1.A22.ShouldBe(approvedData.A22, MinimalTestsPrecision);
        common1.A32.ShouldBe(approvedData.A32, MinimalTestsPrecision);
        common1.A13.ShouldBe(approvedData.A13, MinimalTestsPrecision);
        common1.A23.ShouldBe(approvedData.A23, MinimalTestsPrecision);
        common1.A33.ShouldBe(approvedData.A33, MinimalTestsPrecision);
        common1.E11.ShouldBe(approvedData.E11, MinimalTestsPrecision);
        common1.E21.ShouldBe(approvedData.E21, MinimalTestsPrecision);
        common1.E31.ShouldBe(approvedData.E31, MinimalTestsPrecision);
        common1.E12.ShouldBe(approvedData.E12, MinimalTestsPrecision);
        common1.E22.ShouldBe(approvedData.E22, MinimalTestsPrecision);
        common1.E32.ShouldBe(approvedData.E32, MinimalTestsPrecision);
        common1.E13.ShouldBe(approvedData.E13, MinimalTestsPrecision);
        common1.E23.ShouldBe(approvedData.E23, MinimalTestsPrecision);
        common1.E33.ShouldBe(approvedData.E33, MinimalTestsPrecision);
    }
}
