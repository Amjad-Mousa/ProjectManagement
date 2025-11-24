using ProjectManagement.Domain.Models;
using System;

namespace ProjectManagement.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Role UserRole { get; set; }
        public string PasswordHash {  get; set; }   =string.Empty;  
    }
}
