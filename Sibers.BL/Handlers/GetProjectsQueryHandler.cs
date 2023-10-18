using AutoMapper;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Sibers.Entities.Interfaces;
using Sibers.Entities.Models;
using Sibers.Entities.Queries;
using Sibers.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.BL.Handlers
{
    public sealed class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, IEnumerable<ProjectResponse>>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;

        public GetProjectsQueryHandler(IServiceProvider serviceProvider, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        public async ValueTask<IEnumerable<ProjectResponse>> Handle(GetProjectsQuery query, CancellationToken cancellationToken = default)
        {
            await using var scope = serviceProvider.CreateAsyncScope();

            var projectReadRepository = scope.ServiceProvider.GetRequiredService<IReadRepository<Project>>();

            var projects = await projectReadRepository.ApplyODataAsync(query.ODataQueryOptions, query.ODataQuerySettings, cancellationToken);

            return projects.HasNoValue ?
                Enumerable.Empty<ProjectResponse>() :
                mapper.Map<IEnumerable<ProjectResponse>>(projects.Value);
        }
    }
}
