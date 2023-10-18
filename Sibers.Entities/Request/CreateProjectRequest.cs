using Sibers.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Request
{
    public class CreateProjectRequest
    {
        public required string Name { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public required Company Client { get; set; }
        public required Company Provider { get; set; }

        public required Employee Supervisor { get; set; }
        public required Employee Manager { get; set; }

        [Range(0, 10, ErrorMessage = "Value must be between 0 and 10")]
        public int Priority { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    };
}
