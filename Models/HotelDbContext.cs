using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Data;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Room> Rooms => Set<Room>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Обязательно первой строкой: настраивает таблицы Identity
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasIndex(b => new
            {
                b.RoomId,
                b.CheckIn,
                b.CheckOut
            });


        // Номер комнаты уникален внутри одного отеля
        modelBuilder.Entity<Room>()
            .HasIndex(r => new { r.HotelId, r.Number })
            .IsUnique();

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Grand Astana Hotel",
                City = "Astana",
                Price = 120,
                Rating = 4.8,
                Image = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800"
            },
            new Hotel
            {
                Id = 2,
                Name = "Almaty Luxury Resort",
                City = "Almaty",
                Price = 180,
                Rating = 4.9,
                Image = "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800"
            },
            new Hotel
            {
                Id = 3,
                Name = "Mountain View Hotel",
                City = "Shymkent",
                Price = 95,
                Rating = 4.6,
                Image = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=800"
            }
        );
    }
}