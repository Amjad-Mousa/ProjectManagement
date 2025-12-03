using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.IRepositories;
using ProjectManagement.Domain.Models;
using AutoMapper;

namespace ProjectManagement.Application.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _projectTaskRepository;
        private readonly UserContextService _userContext;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            ITaskRepository projectTaskRepository,
            UserContextService userContext,
            IMapper mapper)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _projectTaskRepository = projectTaskRepository ?? throw new ArgumentNullException(nameof(projectTaskRepository));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new NotFoundException("Project not found.");

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<List<ProjectDto>> GetAllByOwnerAsync()
        {
            var ownerId = _userContext.GetUserId();
            if (ownerId == Guid.Empty)
                throw new UnauthorizedException("User is not authenticated.");

            var projects = await _projectRepository.GetAllByOwnerAsync(ownerId);
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> CreateAsync(ProjectDto projectDto)
        {
            if (projectDto.Id != Guid.Empty)
                throw new BadRequestException("New project ID should be empty.");

            projectDto.OwnerId = _userContext.GetUserId();
            if (projectDto.OwnerId == Guid.Empty)
                throw new UnauthorizedException("User is not authenticated.");

            var existingProjects = await _projectRepository.GetAllByOwnerAsync(projectDto.OwnerId);
            if (existingProjects.Exists(p => p.Name == projectDto.Name))
                throw new BadRequestException("A project with this name already exists for the owner.");

            var project = _mapper.Map<Project>(projectDto);
            var created = await _projectRepository.AddAsync(project);

            return _mapper.Map<ProjectDto>(created);
        }

        public async Task<ProjectDto> UpdateAsync(ProjectDto projectDto)
        {
            var existingProject = await _projectRepository.GetByIdAsync(projectDto.Id);
            if (existingProject == null)
                throw new NotFoundException("Project not found.");

            var currentUserId = _userContext.GetUserId();
            if (existingProject.OwnerId != currentUserId)
                throw new UnauthorizedException("You are not allowed to update this project.");

            var allProjects = await _projectRepository.GetAllByOwnerAsync(currentUserId);
            if (allProjects.Exists(p => p.Id != projectDto.Id && p.Name == projectDto.Name))
                throw new BadRequestException("A project with this name already exists for the owner.");

            var project = _mapper.Map(projectDto, existingProject);
            var updated = await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectDto>(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new NotFoundException("Project not found.");

            var currentUserId = _userContext.GetUserId();
            if (project.OwnerId != currentUserId)
                throw new UnauthorizedException("You are not allowed to delete this project.");

            var tasks = await _projectTaskRepository.GetAllByProjectAsync(project.Id);
            if (tasks.Count > 0)
                throw new BadRequestException("Cannot delete project because it has associated tasks.");

            var (success, message) = await _projectRepository.DeleteAsync(project);
            return success;
        }
    }
}
