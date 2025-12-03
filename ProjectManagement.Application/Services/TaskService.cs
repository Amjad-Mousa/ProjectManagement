using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.IRepositories;
using ProjectManagement.Domain.Models;
using AutoMapper;

namespace ProjectManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserContextService _userContext;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, UserContextService userContext, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TaskDto>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new NotFoundException("Task not found.");

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<List<TaskDto>> GetAllByProjectAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetAllByProjectAsync(projectId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> CreateAsync(TaskDto taskDto)
        {
            if (taskDto.Id != 0)
                throw new BadRequestException("New task ID must be 0.");

            var projectTasks = await _taskRepository.GetAllByProjectAsync(taskDto.ProjectId);
            if (projectTasks.Exists(t => t.Name == taskDto.Name))
                throw new BadRequestException("A task with this name already exists in this project.");

            var task = _mapper.Map<PTask>(taskDto);
            var created = await _taskRepository.AddAsync(task);

            return _mapper.Map<TaskDto>(created);
        }

        public async Task<TaskDto> UpdateAsync(TaskDto taskDto)
        {
            var existingTask = await _taskRepository.GetByIdAsync(taskDto.Id);
            if (existingTask == null)
                throw new NotFoundException("Task not found.");

            var projectTasks = await _taskRepository.GetAllByProjectAsync(taskDto.ProjectId);
            if (projectTasks.Exists(t => t.Id != taskDto.Id && t.Name == taskDto.Name))
                throw new BadRequestException("A task with this name already exists in this project.");

            _mapper.Map(taskDto, existingTask);
            var updated = await _taskRepository.UpdateAsync(existingTask) 
                          ?? throw new BadRequestException("Failed to update task.");

            return _mapper.Map<TaskDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new NotFoundException("Task not found.");

            var deleted = await _taskRepository.DeleteAsync(task);
            if (!deleted)
                throw new BadRequestException("Failed to delete task.");

            return true;
        }
    }
}
