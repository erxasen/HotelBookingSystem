
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Tests;

public class BookingIntegrationTests
{
    private static async Task<(HotelDbContext Db, SqliteConnection Connection)> CreateDbAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new HotelDbContext(options);
        await db.Database.EnsureCreatedAsync();

        return (db, connection);
    }

    private static async Task<Room> CreateRoomAsync(HotelDbContext db)
    {
        var room = new Room
        {
            HotelId = 1,
            Number = "101",
            Type = "Standard",
            Capacity = 2,
            PricePerNight = 120,
            IsActive = true
        };

        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        return room;
    }

    private static Booking CreateBooking(int roomId)
    {
        return new Booking
        {
            HotelId = 1,
            RoomId = roomId,
            FullName = "Test Guest",
            Email = "guest@example.com",
            CheckIn = new DateTime(2026, 12, 10),
            CheckOut = new DateTime(2026, 12, 12),
            Guests = 2,
            TotalPrice = 240
        };
    }

    [Fact]
    public async Task Database_SeededHotels_Exist()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        Assert.Equal(3, await db.Hotels.CountAsync());
        Assert.True(await db.Hotels.AnyAsync(h => h.Name == "Grand Astana Hotel"));
    }

    [Fact]
    public async Task Room_CanBeSavedAndLoaded()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        await CreateRoomAsync(db);

        var room = await db.Rooms.SingleAsync();

        Assert.Equal("101", room.Number);
        Assert.Equal(2, room.Capacity);
        Assert.True(room.IsActive);
    }

    [Fact]
    public async Task Booking_CanBeSavedAndLoaded()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        var room = await CreateRoomAsync(db);
        db.Bookings.Add(CreateBooking(room.Id));
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        var booking = await db.Bookings
            .Include(b => b.Room)
            .Include(b => b.Hotel)
            .SingleAsync();

        Assert.Equal("Test Guest", booking.FullName);
        Assert.Equal("101", booking.Room!.Number);
        Assert.Equal("Grand Astana Hotel", booking.Hotel.Name);
        Assert.Equal(240m, booking.TotalPrice);
    }

    [Fact]
    public async Task Booking_CanBeCancelledByDeleting()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        var room = await CreateRoomAsync(db);
        var booking = CreateBooking(room.Id);

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        db.Bookings.Remove(booking);
        await db.SaveChangesAsync();

        Assert.Empty(await db.Bookings.ToListAsync());
    }

    [Fact]
    public async Task Rooms_SameNumberInSameHotel_ThrowsException()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        await CreateRoomAsync(db);

        db.Rooms.Add(new Room
        {
            HotelId = 1,
            Number = "101",
            Type = "Deluxe",
            Capacity = 3,
            PricePerNight = 200
        });

        await Assert.ThrowsAsync<DbUpdateException>(
            () => db.SaveChangesAsync());
    }

    [Fact]
    public async Task Booking_InvalidRoomId_ThrowsException()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        db.Bookings.Add(CreateBooking(99999));

        await Assert.ThrowsAsync<DbUpdateException>(
            () => db.SaveChangesAsync());
    }

    [Fact]
    public async Task Rooms_CanBeFilteredByCapacityAndActiveStatus()
    {
        var (db, connection) = await CreateDbAsync();
        await using var _ = connection;
        await using var context = db;

        db.Rooms.AddRange(
            new Room
            {
                HotelId = 1,
                Number = "101",
                Capacity = 2,
                PricePerNight = 100,
                IsActive = true
            },
            new Room
            {
                HotelId = 1,
                Number = "102",
                Capacity = 4,
                PricePerNight = 150,
                IsActive = true
            },
            new Room
            {
                HotelId = 1,
                Number = "103",
                Capacity = 5,
                PricePerNight = 200,
                IsActive = false
            }
        );

        await db.SaveChangesAsync();

        var available = await db.Rooms
            .Where(r => r.IsActive && r.Capacity >= 3)
            .ToListAsync();

        Assert.Single(available);
        Assert.Equal("102", available[0].Number);
    }
}
