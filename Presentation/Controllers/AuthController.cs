//   aspnetcore.Authentication
//   AspNetCore.Authorization

using Application.DTOs.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService) : ApiBaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest_ request)
    {
        var token = await userService.AuthenticateAsync(request.Username, request.Password);

        return Ok(token);
    }

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterUser([FromForm] UserRequestsDto request, CancellationToken cancellationToken)
    {
        var success = await userService.RegisterUserAsync(request, cancellationToken);

        return Ok(success);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserByIdAsync(CancellationToken cancellationToken)
    {
        var result = await userService.GetUserById(GetUserId(), cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromForm] UserRequestsDto userDto, CancellationToken cancellationToken)
    {
        var result = await userService.UpdateUserById(GetUserId(), userDto, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        var result = await userService.DeleteUser(GetUserId(), cancellationToken);

        return Ok(result);
    }

}
