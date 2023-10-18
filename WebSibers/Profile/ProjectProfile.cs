using AutoMapper;
using Sibers.Entities.Commands;
using Sibers.Entities.Models;
using Sibers.Entities.Request;
using Sibers.Entities.Responses;

namespace SibersApi
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectResponse>().ReverseMap();
            CreateMap<Project, CreateProjectRequest>().ReverseMap();
            CreateMap<Project, UpdateProjectCommand>().ReverseMap();

            CreateMap<CreateProjectCommand, CreateProjectRequest>().ReverseMap();
            CreateMap<UpdateProjectCommand, CreateProjectRequest>().ReverseMap();
        }
    }
}
