using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Domain.Models;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Domain.IRepositories;


namespace ProjectManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(AppDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PTask?> GetByIdAsync(int id)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == id);
            _logger.LogInformation("Retrieved task with ID: {TaskId}", id);
            return task;
        }

        public async Task<List<PTask>> GetAllAsync()
        {
            var tasks = await _context.ProjectTasks.AsNoTracking().ToListAsync();
            _logger.LogInformation("Retrieved all tasks from the database.");
            return tasks;
        }

        public async Task<List<PTask>> GetAllByProjectAsync(Guid projectId)
        {
            var tasks = await _context.ProjectTasks.AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
            _logger.LogInformation("Retrieved all tasks for project with ID: {ProjectId}", projectId);
            return tasks;
        }

        public async Task<PTask> AddAsync(PTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added new task with ID: {TaskId}", task.Id);
            return task;
        }

        public async Task<PTask> UpdateAsync(PTask task)
        {
            var existingTask = await _context.ProjectTasks.FindAsync(task.Id);
            if (existingTask == null)
            {
                _logger.LogWarning("Attempted to update non-existent task with ID: {TaskId}", task.Id);
                throw new KeyNotFoundException("Task not found");
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.AssignedToId = task.AssignedToId;

            _context.ProjectTasks.Update(existingTask);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated task with ID: {TaskId}", task.Id);
            return existingTask;
        }

        public async Task<bool> DeleteAsync(PTask task)
        {
            var existingTask = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == task.Id);
            if (existingTask == null)
            {
                _logger.LogWarning("Attempted to delete non-existent task with ID: {TaskId}", task.Id);
                return false;
            }

            _context.ProjectTasks.Remove(existingTask);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted task with ID: {TaskId}", task.Id);
            return true;
        }
    }
}
