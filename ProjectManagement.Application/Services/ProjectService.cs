using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Models;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.IRepositories;

namespace ProjectManagement.Application.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _projectTaskRepository;
        private readonly UserContextService _userContext;

        public ProjectService(
            IProjectRepository projectRepository,
            ITaskRepository projectTaskRepository,
            UserContextService userContext)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _projectTaskRepository = projectTaskRepository ?? throw new ArgumentNullException(nameof(projectTaskRepository));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new NotFoundException("Project not found.");

            return project;
        }

        public async Task<List<Project>> GetAllByOwnerAsync()
        {
            var ownerId = _userContext.GetUserId();
            if (ownerId == Guid.Empty)
                throw new UnauthorizedException("User is not authenticated.");

            return await _projectRepository.GetAllByOwnerAsync(ownerId);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            if (project.Id != Guid.Empty)
                throw new BadRequestException("New project ID should be empty.");

            project.OwnerId = _userContext.GetUserId();
            if (project.OwnerId == Guid.Empty)
                throw new UnauthorizedException("User is not authenticated.");

            var existingProjects = await _projectRepository.GetAllByOwnerAsync(project.OwnerId);
            if (existingProjects.Exists(p => p.Name == project.Name))
                throw new BadRequestException("A project with this name already exists for the owner.");

            return await _projectRepository.AddAsync(project);
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            var existingProject = await _projectRepository.GetByIdAsync(project.Id);
            if (existingProject == null)
                throw new NotFoundException("Project not found.");

            var currentUserId = _userContext.GetUserId();
            if (existingProject.OwnerId != currentUserId)
                throw new UnauthorizedException("You are not allowed to update this project.");

            var allProjects = await _projectRepository.GetAllByOwnerAsync(currentUserId);
            if (allProjects.Exists(p => p.Id != project.Id && p.Name == project.Name))
                throw new BadRequestException("A project with this name already exists for the owner.");

            return await _projectRepository.UpdateAsync(project);
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
