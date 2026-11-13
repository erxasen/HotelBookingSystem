
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Services;

public class BookingService
{
    private readonly IDbContextFactory<HotelDbContext> _dbFactory;

    public BookingService(IDbContextFactory<HotelDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(
        int hotelId,
        DateTime checkIn,
        DateTime checkOut,
        int guests)
    {
        if (hotelId <= 0 ||
            checkIn.Date < DateTime.Today ||
            checkOut.Date <= checkIn.Date ||
            guests < 1 ||
            guests > 10)
        {
            return new List<Room>();
        }

        await using var db = await _dbFactory.CreateDbContextAsync();

        var start = checkIn.Date;
        var end = checkOut.Date;

        return await db.Rooms
            .AsNoTracking()
            .Where(r =>
                r.HotelId == hotelId &&
                r.IsActive &&
                r.Capacity >= guests &&
                !db.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    b.CheckIn < end &&
                    start < b.CheckOut))
            .OrderBy(r => r.Number)
            .ToListAsync();
    }
}
