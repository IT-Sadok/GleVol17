using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Auth.Responses;
using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Application.Interfaces;

public interface IAuthenticationService
{
    Task<UserLoginResponse> SignUpAsync(SignUpRequest request);
    Task<UserLoginResponse?> LoginAsync(LoginRequest request);
}