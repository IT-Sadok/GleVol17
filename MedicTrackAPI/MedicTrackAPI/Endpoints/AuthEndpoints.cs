using MedicTrack.Application.Interfaces;
using MedicTrack.Application.Auth.Requests;
using MedicTrackAPI.AuthModels;

namespace MedicTrackAPI.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/sign-up", async (
            RegisterModel model,
            IAuthenticationService authService) =>
        {
            var request = new SignUpRequest
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate
            };

            var result = await authService.SignUpAsync(request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        });

        group.MapPost("/login", async (
            LoginModel model,
            IAuthenticationService authService) =>
        {
            var request = new LoginRequest
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await authService.LoginAsync(request);
            return result is null
                ? Results.Unauthorized()
                : Results.Ok(result);
        });

        return group;
    }
}