using Sibers.Entities.Models;
using Sibers.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Interfaces
{
    public interface IProjectFactory
    {
        Result<Project> Create(CreateProjectRequest createBudgetRequest);
    }
}
