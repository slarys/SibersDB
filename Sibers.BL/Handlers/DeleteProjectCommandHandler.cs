using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Sibers.Entities.Interfaces;
using Sibers.Entities.Models;
using Sibers.Entities;
using Sibers.Entities.Commands;
using Strings = Sibers.Entities.Strings;

namespace Sibers.BL.Handlers
{
    public class DeleteProjectCommandHandler : ICommandHandler<DeleteBudgetCommand, Result>
    {
        private readonly IServiceProvider serviceProvider;

        public DeleteProjectCommandHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async ValueTask<Result> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var projectRepository = scope.ServiceProvider.GetRequiredService<IRepository<Project>>();

            var project = await projectRepository.GetByIdAsync(command.Id, cancellationToken);
            if (project is null)
            {
                return Result.Failure(Error.CreateFromResourceKey(Strings.Project_NotFound));
            }

            await projectRepository.DeleteAsync(project, cancellationToken);

            return Result.Success();
        }
    }
}
