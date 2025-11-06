using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicTrackAPI.DAL.Entities;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime BirthDate { get; set; }
}