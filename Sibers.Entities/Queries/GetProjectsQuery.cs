using Mediator;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Query;
using Sibers.Entities.Models;
using Sibers.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Queries
{
    public record GetProjectsQuery(ODataQueryOptions<Project> ODataQueryOptions, ODataValidationSettings ODataQuerySettings) : IQuery<IEnumerable<ProjectResponse>>;
}
