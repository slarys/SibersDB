using AutoMapper;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Sibers.Entities;
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
    public sealed class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, Catch<ProjectResponse>>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;

        public GetProjectByIdQueryHandler(IServiceProvider serviceProvider, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        public async ValueTask<Catch<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var projectRepository = scope.ServiceProvider.GetRequiredService<IRepository<Project>>();

            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            return project is null ? Catch<ProjectResponse>.None : mapper.Map<ProjectResponse>(project);
        }
    }
}
