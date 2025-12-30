using ProjectManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Role UserRole { get; set; }
    }
}
