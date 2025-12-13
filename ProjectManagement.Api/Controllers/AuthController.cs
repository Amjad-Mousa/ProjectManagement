using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Services;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserContextService _userContextService;

        public AuthController(IAuthService authService, UserContextService userContextService)
        {
            _authService = authService;
            _userContextService = userContextService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request);

            if (user == null)
                return Unauthorized(new { Message = "Authentication failed." });

            return Ok(new
            {
                Message = "Login successful",
                User = user
            });
        }

        [HttpGet("current-user")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var user = _userContextService.GetCurrentUser();

            if (user == null)
                return Unauthorized(new { Message = "Not authenticated." });

            return Ok(user);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { Message = "Logout successful" });
        }
    }
}
