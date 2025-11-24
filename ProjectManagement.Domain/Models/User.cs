
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Domain.Models
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserName { get; set; } = string.Empty;

        public Role UserRole { get; set; } = Role.User;

        [Required]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

    }
}
