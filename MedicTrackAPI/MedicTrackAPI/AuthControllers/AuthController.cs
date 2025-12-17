using MedicTrack.Application.Auth.Requests;
using MedicTrack.Application.Interfaces;
using MedicTrackAPI.AuthModels;
using Microsoft.AspNetCore.Mvc;

namespace MedicTrackAPI.AuthControllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new RegisterRequest
        {
            Email = model.Email,
            Password = model.Password,
            FirstName = model.FirstName,
            LastName = model.LastName,
            BirthDate = model.BirthDate
        };

        var result = await authenticationService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        };

        var response = await authenticationService.LoginAsync(request);
        

        if (response is null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(response);
    }
}