using Ardalis.Specification;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Entities.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
        public Task<Catch<IEnumerable<object>>> ApplyODataAsync(ODataQueryOptions<T>? queryOptions,
            ODataValidationSettings oDataQuerySettings, CancellationToken cancellationToken);
    }
}
