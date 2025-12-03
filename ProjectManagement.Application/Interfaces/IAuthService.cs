using Microsoft.AspNetCore.Http;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;


namespace ProjectManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginRequest request);
        Task<UserDto> LogoutAsync(HttpContext httpContext);
    }
}
