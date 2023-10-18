using Sibers.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Models;

public record Company : Entity<Guid>
{
    public required string Name { get; set; }
}
