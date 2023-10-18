using Mediator;
using Sibers.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Queries
{
    public record GetProjectByIdQuery(Guid ProjectId) : IQuery<Catch<ProjectResponse>>;
}
