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

        var result = await authenticationService.RegisterAsync(
            model.Email,
            model.Password,
            model.FirstName,
            model.LastName,
            model.BirthDate);

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

        var token = await authenticationService.LoginAsync(
            model.Email,
            model.Password);

        if (token is null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new { token });
    }
}