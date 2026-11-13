
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int HotelId { get; set; }

    public Hotel Hotel { get; set; } = null!;

    public int? RoomId { get; set; }

    public Room? Room { get; set; }

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    [Required]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    [Range(1, 10)]
    public int Guests { get; set; } = 1;

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}
