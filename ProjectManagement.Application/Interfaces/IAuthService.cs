using ProjectManagement.Application.DTOs;


namespace ProjectManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginRequest request);
    }
}
