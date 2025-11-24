using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Application.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        DateTime birthDate);

    Task<string?> LoginAsync(string email, string password);
}