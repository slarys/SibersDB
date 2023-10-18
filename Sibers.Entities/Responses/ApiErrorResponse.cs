using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Responses
{
    public record ApiErrorResponse(IReadOnlyCollection<Error> Errors);
}
