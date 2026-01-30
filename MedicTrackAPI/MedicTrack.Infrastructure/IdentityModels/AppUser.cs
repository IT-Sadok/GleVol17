using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Infrastructure.IdentityModels;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
}