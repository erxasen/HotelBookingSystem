using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
}
