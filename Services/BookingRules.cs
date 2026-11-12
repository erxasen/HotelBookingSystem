namespace HotelBookingSystem.Services;

public static class BookingRules
{
    // Две брони пересекаются, если каждая начинается раньше, чем заканчивается другая.
    // Выезд в день заезда другой брони пересечением не считается.
    public static bool DatesOverlap(
        DateTime checkInA, DateTime checkOutA,
        DateTime checkInB, DateTime checkOutB)
    {
        return checkInA.Date < checkOutB.Date
            && checkInB.Date < checkOutA.Date;
    }

    // Возвращает текст ошибки или null, если даты корректны.
    public static string? ValidateDates(DateTime checkIn, DateTime checkOut, DateTime today)
    {
        if (checkIn.Date < today.Date)
            return "Check-in date cannot be in the past.";

        if (checkOut.Date <= checkIn.Date)
            return "Check-out must be after check-in.";

        return null;
    }
}