using MedicTrack.Application.Interfaces;
using MedicTrack.Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Infrastructure.Services;

public class AuthenticationService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtService jwtService)
    : IAuthenticationService
{
    public async Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        DateTime birthDate)
    {
        var user = new AppUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate
        };

        var result = await userManager.CreateAsync(user, password);
        return result;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return null;
        }

        var token = jwtService.GenerateToken(user);
        return token;
    }
}