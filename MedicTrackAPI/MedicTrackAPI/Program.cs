using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Interfaces;
using MedicTrack.Application.Options;
using MedicTrack.Infrastructure.Extensions;
using MedicTrack.Infrastructure.Services;
using MedicTrackAPI.AuthModels;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MedicTrack.Application.Auth.Validators;
using MedicTrackAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidator>();

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
app.MapAuthEndpoints();

app.Run();