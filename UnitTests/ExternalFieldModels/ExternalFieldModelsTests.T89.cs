using AuroraScienceHub.Geopack.Contracts.Cartesian;
using AuroraScienceHub.Geopack.Contracts.Coordinates;
using AuroraScienceHub.Geopack.Contracts.PhysicalQuantities;
using Shouldly;

namespace AuroraScienceHub.Geopack.UnitTests.ExternalFieldModels;

public partial class TModelsTests
{
    [Theory(DisplayName = "Iterate T89 setups and verify result")]
    [InlineData(1, 0.5D, -6.6D, 0.0D, 0.0D, -21.7428582008425, 0.000000000000000E+000, -16.1753178949799)]
    [InlineData(1, 0.5D, 6.6D, 0.0D, 0.0D, 2.77460435954486, 0.000000000000000E+000, 10.4563908886324)]
    [InlineData(7, 1.0D, -1.02D, -1.02D, -1.02D, -40.3174033286441, -4.28475694981233, -21.8189293247679)]
    [InlineData(6, 1.0D, -1.02D, -1.02D, -1.02D, -77.7524838720541, -1.48818282095091, -46.0916345034569)]
    [InlineData(5, 1.0D, -1.02D, -1.02D, -1.02D, -35.7991349979912, -1.12022295377554, -19.4019675803310)]
    [InlineData(4, 1.0D, -1.02D, -1.02D, -1.02D, -31.0211249343846, -0.446400971617370, -18.2770347314801)]
    [InlineData(3, 1.0D, -1.02D, -1.02D, -1.02D, -33.8409563532569, -0.947163635685137, -22.4120161221729)]
    [InlineData(2, 1.0D, -1.02D, -1.02D, -1.02D, -30.3828134463662, -1.13707597340196, -21.7515334042755)]
    public void T89_ShouldReturnCorrectValues(
        int iopt,
        double ps,
        double x, double y, double z,
        double expectedBx, double expectedBy, double expectedBz)
    {
        // Arrange
        CartesianLocation location = CartesianLocation.New(x, y, z, CoordinateSystem.GSW);

        // Act
        CartesianVector<MagneticField> resultField = _t89.Calculate(iopt, new double[10], ps, location);

        // Assert
        resultField.X.ShouldBe(expectedBx, MinimalTestsPrecision);
        resultField.Y.ShouldBe(expectedBy, MinimalTestsPrecision);
        resultField.Z.ShouldBe(expectedBz, MinimalTestsPrecision);
    }

    [Fact(DisplayName = "Parallel T89 calls with mixed IOPT match sequential calls bitwise")]
    public void T89_ParallelCallsWithMixedIopt_ShouldMatchSequentialBitwise()
    {
        // Arrange
        const int pointCount = 2_000;
        const int iterations = 20;
        double[] parmod = new double[10];
        int[] iopts = new int[pointCount];
        double[] tilts = new double[pointCount];
        CartesianLocation[] locations = new CartesianLocation[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            iopts[i] = i % 7 + 1;
            tilts[i] = -0.6D + 1.2D * (i % 13) / 12.0D;
            locations[i] = CartesianLocation.New(
                -30.0D + 0.021D * i,
                -10.0D + 1.3D * (i % 17),
                -8.0D + 1.1D * (i % 15),
                CoordinateSystem.GSW);
        }

        long[] expected = new long[3 * pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            CartesianVector<MagneticField> field = _t89.Calculate(iopts[i], parmod, tilts[i], locations[i]);
            WriteBits(expected, i, field);
        }

        // Act
        int mismatches = 0;
        for (int iteration = 0; iteration < iterations; iteration++)
        {
            long[] actual = new long[3 * pointCount];
            Parallel.For(0, pointCount, i =>
            {
                CartesianVector<MagneticField> field = _t89.Calculate(iopts[i], parmod, tilts[i], locations[i]);
                WriteBits(actual, i, field);
            });

            for (int i = 0; i < pointCount; i++)
            {
                if (actual[3 * i] != expected[3 * i]
                    || actual[3 * i + 1] != expected[3 * i + 1]
                    || actual[3 * i + 2] != expected[3 * i + 2])
                {
                    mismatches++;
                }
            }
        }

        // Assert
        mismatches.ShouldBe(0);
    }

    [Theory(DisplayName = "T89 rejects IOPT outside 1..7")]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(10)]
    public void T89_IoptOutOfRange_ShouldThrow(int iopt)
    {
        // Arrange
        CartesianLocation location = CartesianLocation.New(-6.6D, 0.0D, 0.0D, CoordinateSystem.GSW);

        // Act
        Action act = () => _t89.Calculate(iopt, new double[10], 0.5D, location);

        // Assert
        act.ShouldThrow<ArgumentOutOfRangeException>();
    }

    private static void WriteBits(long[] bits, int index, CartesianVector<MagneticField> field)
    {
        bits[3 * index] = BitConverter.DoubleToInt64Bits(field.X);
        bits[3 * index + 1] = BitConverter.DoubleToInt64Bits(field.Y);
        bits[3 * index + 2] = BitConverter.DoubleToInt64Bits(field.Z);
    }
}
