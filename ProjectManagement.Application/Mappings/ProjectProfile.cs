using AutoMapper;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
