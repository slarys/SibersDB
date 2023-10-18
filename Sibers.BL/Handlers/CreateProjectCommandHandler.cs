using AutoMapper;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Sibers.Entities.Commands;
using Sibers.Entities.Interfaces;
using Sibers.Entities.Models;
using Sibers.Entities.Request;
using Sibers.Entities.Responses;
using Sibers.Entities;

namespace Sibers.BL.Handlers
{
    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Result<EntityCreatedResponse>>
    {
        private readonly IMapper mapper;
        private readonly IProjectFactory projectFactory;
        private readonly IServiceProvider serviceProvider;

        public CreateProjectCommandHandler(IMapper mapper, IProjectFactory projectFactory, IServiceProvider serviceProvider)
        {
            this.mapper = mapper;
            this.projectFactory = projectFactory;
            this.serviceProvider = serviceProvider;
        }

        public async ValueTask<Result<EntityCreatedResponse>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var request = mapper.Map<CreateProjectRequest>(command);

            var projectResult = projectFactory.Create(request);
            if (projectResult.IsFailure)
            {
                return Result.Failure<EntityCreatedResponse>(projectResult.Error);
            }

            await using var scope = serviceProvider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Project>>();
            var project = await repository.AddAsync(projectResult.Value, cancellationToken);

            return Result.Success(new EntityCreatedResponse(project.Id));
        }
    }
}
