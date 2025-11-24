using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Models;
using ProjectManagement.Domain.IRepositories;


namespace ProjectManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserContextService _userContext;

        public TaskService(ITaskRepository taskRepository, UserContextService userContext)
        {
            _taskRepository = taskRepository;
            _userContext = userContext;
        }

        public async Task<List<PTask>> GetAllAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<PTask> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new NotFoundException("Task not found.");
            }

            return task;
        }

        public async Task<List<PTask>> GetAllByProjectAsync(Guid projectId)
        {
            return await _taskRepository.GetAllByProjectAsync(projectId);
        }

        public async Task<PTask> CreateAsync(Domain.Models.PTask task)
        {
            if (task.Id != 0)
            {
                throw new BadRequestException("New task ID must be 0.");
            }

            var projectTasks = await _taskRepository.GetAllByProjectAsync(task.ProjectId);

            if (projectTasks.Exists(t => t.Name == task.Name))
            {
                throw new BadRequestException("A task with this name already exists in this project.");
            }

            return await _taskRepository.AddAsync(task);
        }

        public async Task<PTask> UpdateAsync(PTask task)
        {
            var existingTask = await _taskRepository.GetByIdAsync(task.Id);

            if (existingTask == null)
            {
                throw new NotFoundException("Task not found.");
            }

            var projectTasks = await _taskRepository.GetAllByProjectAsync(task.ProjectId);

            if (projectTasks.Exists(t => t.Id != task.Id && t.Name == task.Name))
            {
                throw new BadRequestException("A task with this name already exists in this project.");
            }

            return await _taskRepository.UpdateAsync(task)
                   ?? throw new BadRequestException("Failed to update task.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new NotFoundException("Task not found.");
            }

            var deleted = await _taskRepository.DeleteAsync(task);

            if (!deleted)
            {
                throw new BadRequestException("Failed to delete task.");
            }

            return true;
        }
    }
}
