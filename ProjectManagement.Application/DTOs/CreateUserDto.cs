using System.ComponentModel.DataAnnotations;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Application.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public Role UserRole { get; set; } = Role.User;
    }
}