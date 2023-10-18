using Sibers.Entities.Base;
using Sibers.Entities.Enums;
using Sibers.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities
{
    public record Goal : Entity<Guid>
    {
        public required string Name { get; set; }

        public virtual required Employee Author { get; set; }
        public virtual required Employee Provider { get; set; }

        public GoalStatus Status { get; set; } = GoalStatus.Untrack;

        public string? Description { get; set; }

        [Range(0, 10, ErrorMessage = "Value must be between 0 and 10")]
        public int Priority { get; set; }

    }
}
