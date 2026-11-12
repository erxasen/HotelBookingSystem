namespace HotelBookingSystem.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
    public decimal Price { get; set; }
    public double Rating { get; set; }
    public string Image { get; set; } = "";

    public List<Booking> Bookings { get; set; } = new();
    public List<Room> Rooms { get; set; } = new();
}