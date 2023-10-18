using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using Sibers.Entities.Interfaces;
using System.Collections;
using Sibers.Entities;
using Sibers.Domain;

namespace Sibers.BL;
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        private readonly DbContext dbContext;

        public EfRepository(SibersContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Catch<IEnumerable<object>>> ApplyODataAsync(ODataQueryOptions<T>? queryOptions,
            ODataValidationSettings oDataQuerySettings, CancellationToken cancellationToken)
        {
            if (queryOptions is null)
            {
                await dbContext.Set<T>().AsQueryable().ToListAsync(cancellationToken);
            }

            try
            {
                queryOptions?.Validate(oDataQuerySettings);
            }
            catch (ODataException)
            {
                return Catch<IEnumerable<dynamic>>.None;
            }

            var queryable = queryOptions.ApplyTo(dbContext.Set<T>().AsQueryable());
            var elementType = queryable.ElementType;

            if (!typeof(ISelectExpandWrapper).IsAssignableFrom(elementType))
            {
                return await queryable.Cast<T>().ToListAsync(cancellationToken);
            }

            return CastExpandWrapperToList(queryable)?.ToList() ?? Catch<IEnumerable<object>>.None;
        }

        private static IEnumerable<object>? CastExpandWrapperToList(IEnumerable queryable)
        {
            var list = new List<object>();
            foreach (var item in queryable)
            {
                var wrapper = item as ISelectExpandWrapper;
                var dic = wrapper.ToDictionary();

                if (!CastExpandWrapperToDictionary(dic)) return null;

                list.Add(dic);
            }

            return list;
        }

        private static bool CastExpandWrapperToDictionary(IDictionary<string, object> dic, int deep = 0)
        {
            if (deep > 5) return false;
            deep++;

            foreach (var kvp in dic)
            {
                if (kvp.Value is not ISelectExpandWrapper expandWrapper) continue;

                var result = expandWrapper.ToDictionary();
                if (result is null) return false;

                CastExpandWrapperToDictionary(result, deep);

                dic[kvp.Key] = result;
            }

            return true;
        }
    }

