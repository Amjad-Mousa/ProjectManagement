using AutoMapper;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Application.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<PTask, TaskDto>().ReverseMap();
        }
    }
}
