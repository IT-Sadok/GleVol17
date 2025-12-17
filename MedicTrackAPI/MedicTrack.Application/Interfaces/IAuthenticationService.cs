using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Auth.Responses;
using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Application.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterAsync(RegisterRequest  request);
    Task<UserLoginResponse?> LoginAsync(LoginRequest request);
}