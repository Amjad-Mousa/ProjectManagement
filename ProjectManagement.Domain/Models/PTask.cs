using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Models
{
    public class PTask : AuditableEntity
    {
        public int Id { get; set; }

        public Guid ProjectId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid? AssignedToId { get; set; }

        public Status Status { get; set; } = Status.Todo;
    }
}

