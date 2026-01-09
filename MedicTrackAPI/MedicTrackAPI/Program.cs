using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Interfaces;
using MedicTrack.Application.Options;
using MedicTrack.Infrastructure.Extensions;
using MedicTrack.Infrastructure.Services;
using MedicTrackAPI.AuthModels;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MedicTrack.Application.Auth.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var authGroup = app.MapGroup("/api/auth").WithTags("Auth");

authGroup.MapPost("/register",
    async ([FromBody] RegisterModel model, IAuthenticationService authService) =>
    {
        var request = new RegisterRequest
        {
            Email = model.Email,
            Password = model.Password,
            FirstName = model.FirstName,
            LastName = model.LastName,
            BirthDate = model.BirthDate
        };

        var result = await authService.RegisterAsync(request);
        return result is null ? Results.BadRequest("registration failed") : Results.Ok(result);
    });

authGroup.MapPost("/login",
    async ([FromBody] LoginModel model, IAuthenticationService authService) =>
    {
        var request = new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        };

        var response = await authService.LoginAsync(request);
        return response is null ? Results.Unauthorized() : Results.Ok(response);
    });

app.Run();