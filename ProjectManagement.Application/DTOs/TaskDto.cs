using ProjectManagement.Domain.Models;
using System;

namespace ProjectManagement.Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? AssignedToId { get; set; }
        public Status Status { get; set; }
    }
}
