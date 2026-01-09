using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Auth.Responses;
using MedicTrack.Application.Interfaces;
using MedicTrack.Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using FluentValidation;


namespace MedicTrack.Infrastructure.Services;

public class AuthenticationService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtService jwtService,
    IValidator<RegisterRequest> registerValidator,
    IValidator<LoginRequest> loginValidator)
    : IAuthenticationService
{
    public async Task<UserLoginResponse?> RegisterAsync(RegisterRequest request)
    {
        await registerValidator.ValidateAndThrowAsync(request);

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return null;
        }

        var token = jwtService.GenerateToken(user);

        return new UserLoginResponse
        {
            Token = token
        };
    }

    public async Task<UserLoginResponse?> LoginAsync(LoginRequest request)
    {
        await loginValidator.ValidateAndThrowAsync(request);
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signInResult.Succeeded)
        {
            return null;
        }

        var token = jwtService.GenerateToken(user);

        return new UserLoginResponse
        {
            Token = token
        };
    }
}