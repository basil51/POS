namespace POS.Tests;

public class LineTotalTests
{
    [Fact]
    public void Line_total_rounds_away_from_zero()
    {
        var actual = Math.Round(2m * 4.99m, 2, MidpointRounding.AwayFromZero);
        Assert.Equal(9.98m, actual);
    }

    [Fact]
    public void Line_total_handles_midpoint_decimal()
    {
        var actual = Math.Round(3m * 1.335m, 2, MidpointRounding.AwayFromZero);
        Assert.Equal(4.01m, actual);
    }
}
