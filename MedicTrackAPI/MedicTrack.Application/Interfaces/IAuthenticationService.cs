using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Auth.Responses;
using MedicTrack.Application.Common;
using Microsoft.AspNetCore.Identity;

namespace MedicTrack.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<UserLoginResponse>> SignUpAsync(SignUpRequest request);
    Task<UserLoginResponse?> LoginAsync(LoginRequest request);
}