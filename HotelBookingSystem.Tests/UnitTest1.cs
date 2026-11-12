using HotelBookingSystem.Services;

namespace HotelBookingSystem.Tests;

public class BookingRulesTests
{
    private static readonly DateTime Today = new(2026, 11, 12);

    [Fact]
    public void DatesOverlap_SameDates_ReturnsTrue()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25),
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25));

        Assert.True(result);
    }

    [Fact]
    public void DatesOverlap_PartialOverlap_ReturnsTrue()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25),
            new DateTime(2026, 11, 23), new DateTime(2026, 11, 28));

        Assert.True(result);
    }

    [Fact]
    public void DatesOverlap_NewBookingInsideExisting_ReturnsTrue()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 30),
            new DateTime(2026, 11, 22), new DateTime(2026, 11, 24));

        Assert.True(result);
    }

    [Fact]
    public void DatesOverlap_CheckInOnExistingCheckOut_ReturnsFalse()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25),
            new DateTime(2026, 11, 25), new DateTime(2026, 11, 28));

        Assert.False(result);
    }

    [Fact]
    public void DatesOverlap_CheckOutOnExistingCheckIn_ReturnsFalse()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25),
            new DateTime(2026, 11, 15), new DateTime(2026, 11, 20));

        Assert.False(result);
    }

    [Fact]
    public void DatesOverlap_FarApart_ReturnsFalse()
    {
        var result = BookingRules.DatesOverlap(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25),
            new DateTime(2026, 12, 10), new DateTime(2026, 12, 15));

        Assert.False(result);
    }

    [Fact]
    public void ValidateDates_CheckInInPast_ReturnsError()
    {
        var error = BookingRules.ValidateDates(
            new DateTime(2026, 11, 10), new DateTime(2026, 11, 14), Today);

        Assert.Equal("Check-in date cannot be in the past.", error);
    }

    [Fact]
    public void ValidateDates_CheckOutBeforeCheckIn_ReturnsError()
    {
        var error = BookingRules.ValidateDates(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 18), Today);

        Assert.Equal("Check-out must be after check-in.", error);
    }

    [Fact]
    public void ValidateDates_SameDay_ReturnsError()
    {
        var error = BookingRules.ValidateDates(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 20), Today);

        Assert.NotNull(error);
    }

    [Fact]
    public void ValidateDates_CorrectDates_ReturnsNull()
    {
        var error = BookingRules.ValidateDates(
            new DateTime(2026, 11, 20), new DateTime(2026, 11, 25), Today);

        Assert.Null(error);
    }

    [Fact]
    public void ValidateDates_CheckInToday_ReturnsNull()
    {
        var error = BookingRules.ValidateDates(
            Today, Today.AddDays(1), Today);

        Assert.Null(error);
    }
}