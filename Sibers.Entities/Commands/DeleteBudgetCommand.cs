using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Commands
{
    public record DeleteBudgetCommand(Guid Id) : ICommand<Result>;
}
