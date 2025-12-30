using Microsoft.AspNetCore.Http;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Models;
using System.Security.Claims;

namespace ProjectManagement.Application.Services
{
    public class UserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public Guid GetUserId()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
        }

        public string GetUserName()
        {
            return User?.Identity?.Name ?? string.Empty;
        }

        public string GetUserRole()
        {
            return User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public bool IsAuthenticated()
        {
            return User?.Identity?.IsAuthenticated ?? false;
        }

        public UserDto? GetCurrentUser()
        {
            if (!IsAuthenticated())
            {
                return null;
            }

            var userId = GetUserId();
            var userName = GetUserName();
            var roleString = GetUserRole();

            if (userId == Guid.Empty || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleString))
            {
                return null;
            }

            if (!Enum.TryParse<Role>(roleString, out var userRole))
            {
                return null;
            }

            return new UserDto
            {
                Id = userId,
                UserName = userName,
                UserRole = userRole
            };
        }
    }
}