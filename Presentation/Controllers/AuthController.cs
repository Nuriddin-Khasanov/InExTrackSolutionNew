//   aspnetcore.Authentication
//   AspNetCore.Authorization

using InExTrack.Application.DTOs.Requests;
using InExTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace InExTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService _userService) : ApiBaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest_ request)
        {
            var token = await _userService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRequestsDto request, CancellationToken cancellationToken)
        {
            var success = await _userService.RegisterUserAsync(request, cancellationToken);

            return Ok(success);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync(CancellationToken cancellationToken)
        {
            if (UserUid is null || UserUid.Value == Guid.Empty)
                return BadRequest("UserUid не определён.");

            var result = await _userService.GetUserById(UserUid.Value, cancellationToken);

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UserRequestsDto userDto, CancellationToken cancellationToken)
        {
            if (UserUid is null || UserUid.Value == Guid.Empty)
                return BadRequest("UserUid не определён.");

            var result = await _userService.UpdateUserById(UserUid.Value, userDto, cancellationToken);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            if (UserUid is null || UserUid.Value == Guid.Empty)
                return BadRequest("UserUid не определён.");

            var result = await _userService.DeleteUser(UserUid.Value, cancellationToken);

            return Ok(result);
        }
    }
}
