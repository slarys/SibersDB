using AutoMapper;
using Sibers.Entities;
using Sibers.Entities.Interfaces;
using Sibers.Entities.Models;
using Sibers.Entities.Request;

namespace Sibers.BL.Factories
{
    public class ProjectFactory : IProjectFactory
    {
        private readonly IMapper mapper;

        public ProjectFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Result<Project> Create(CreateProjectRequest createProjectRequest)
        {
            return mapper.Map<Project>(createProjectRequest);
        }
    }
}
