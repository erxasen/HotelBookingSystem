namespace HotelBookingSystem.Models;

public class Room
{
    public int Id { get; set; }

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;

    public string Number { get; set; } = "";
    public string Type { get; set; } = "Standard";   // Standard / Deluxe / Suite
    public int Capacity { get; set; } = 2;
    public decimal PricePerNight { get; set; }
    public bool IsActive { get; set; } = true;
}