using System.ComponentModel.DataAnnotations;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Application.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "UserName Required")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PassWord Required")]
        [MinLength(6, ErrorMessage = "PassWord Should Be 6 Digits At Least")]
        public string Password { get; set; } = string.Empty;

        public Role UserRole { get; set; } = Role.User;
    }
}