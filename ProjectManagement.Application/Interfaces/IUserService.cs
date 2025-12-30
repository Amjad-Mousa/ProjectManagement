using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByUserNameAsync(string username);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(UpdateUserDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<User> GetByUserNameForAuthAsync(string userName);
    }
}
