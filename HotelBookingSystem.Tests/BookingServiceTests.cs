
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Tests;

public class BookingServiceTests
{
    private sealed class TestDbFactory : IDbContextFactory<HotelDbContext>
    {
        private readonly DbContextOptions<HotelDbContext> _options;

        public TestDbFactory(DbContextOptions<HotelDbContext> options)
        {
            _options = options;
        }

        public HotelDbContext CreateDbContext()
        {
            return new HotelDbContext(_options);
        }
    }

    private static async Task<(BookingService Service, HotelDbContext Db, SqliteConnection Connection)>
        SetupAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new HotelDbContext(options);
        await db.Database.EnsureCreatedAsync();

        var factory = new TestDbFactory(options);
        var service = new BookingService(factory);

        return (service, db, connection);
    }

    private static Room NewRoom(string number, int capacity = 2, bool active = true)
    {
        return new Room
        {
            HotelId = 1,
            Number = number,
            Type = "Standard",
            Capacity = capacity,
            PricePerNight = 100,
            IsActive = active
        };
    }

    private static Booking NewBooking(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return new Booking
        {
            HotelId = 1,
            RoomId = roomId,
            FullName = "Test Guest",
            Email = "test@example.com",
            CheckIn = checkIn,
            CheckOut = checkOut,
            Guests = 2,
            TotalPrice = 200
        };
    }

    [Fact]
    public async Task AvailableRoom_IsReturned()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        db.Rooms.Add(NewRoom("101"));
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);

        var rooms = await service.GetAvailableRoomsAsync(
            1, start, start.AddDays(2), 2);

        Assert.Single(rooms);
        Assert.Equal("101", rooms[0].Number);
    }

    [Fact]
    public async Task BookedRoom_IsNotAvailable()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        var room = NewRoom("101");
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);
        db.Bookings.Add(NewBooking(room.Id, start, start.AddDays(3)));
        await db.SaveChangesAsync();

        var rooms = await service.GetAvailableRoomsAsync(
            1, start.AddDays(1), start.AddDays(2), 2);

        Assert.Empty(rooms);
    }

    [Fact]
    public async Task AdjacentDates_RoomIsAvailable()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        var room = NewRoom("101");
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);
        db.Bookings.Add(NewBooking(room.Id, start, start.AddDays(2)));
        await db.SaveChangesAsync();

        var rooms = await service.GetAvailableRoomsAsync(
            1, start.AddDays(2), start.AddDays(4), 2);

        Assert.Single(rooms);
    }

    [Fact]
    public async Task InactiveRoom_IsNotAvailable()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        db.Rooms.Add(NewRoom("101", active: false));
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);

        var rooms = await service.GetAvailableRoomsAsync(
            1, start, start.AddDays(2), 2);

        Assert.Empty(rooms);
    }

    [Fact]
    public async Task RoomWithInsufficientCapacity_IsNotAvailable()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        db.Rooms.Add(NewRoom("101", capacity: 2));
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);

        var rooms = await service.GetAvailableRoomsAsync(
            1, start, start.AddDays(2), 3);

        Assert.Empty(rooms);
    }

    [Fact]
    public async Task CancelledBooking_MakesRoomAvailableAgain()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        var room = NewRoom("101");
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var start = DateTime.Today.AddDays(5);
        var booking = NewBooking(room.Id, start, start.AddDays(2));

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        var before = await service.GetAvailableRoomsAsync(
            1, start, start.AddDays(2), 2);

        Assert.Empty(before);

        db.Bookings.Remove(booking);
        await db.SaveChangesAsync();

        var after = await service.GetAvailableRoomsAsync(
            1, start, start.AddDays(2), 2);

        Assert.Single(after);
    }

    [Fact]
    public async Task InvalidDates_ReturnEmpty()
    {
        var (service, db, connection) = await SetupAsync();
        await using var context = db;
        await using var conn = connection;

        var start = DateTime.Today.AddDays(5);

        var rooms = await service.GetAvailableRoomsAsync(
            1, start, start, 2);

        Assert.Empty(rooms);
    }
}
