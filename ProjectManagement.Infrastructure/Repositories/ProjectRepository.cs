using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Domain.Models;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Domain.IRepositories;


namespace ProjectManagement.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(AppDbContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            _logger.LogInformation("Retrieved project with ID: {ProjectId}", id);
            return project;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var projects = await _context.Projects.AsNoTracking().ToListAsync();
            _logger.LogInformation("Retrieved all projects from the database.");
            return projects;
        }

        public async Task<List<Project>> GetAllByOwnerAsync(Guid ownerId)
        {
            var projects = await _context.Projects.AsNoTracking()
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
            _logger.LogInformation("Retrieved all projects for owner with ID: {OwnerId}", ownerId);
            return projects;
        }

        public async Task<Project?> AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added new project with ID: {ProjectId}", project.Id);
            return project;
        }

        public async Task<Project?> UpdateAsync(Project project)
        {
            var existingProject = await _context.Projects.FindAsync(project.Id);
            if (existingProject == null)
            {
                _logger.LogWarning("Attempted to update non-existent project with ID: {ProjectId}", project.Id);
                return null;
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.Status = project.Status;

            _context.Projects.Update(existingProject);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated project with ID: {ProjectId}", project.Id);
            return existingProject;
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Project project)
        {
            var existingProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            if (existingProject == null)
            {
                _logger.LogWarning("Attempted to delete non-existent project with ID: {ProjectId}", project.Id);
                return (false, "Project not found");
            }

            var hasTasks = await _context.ProjectTasks.AnyAsync(t => t.ProjectId == project.Id);
            if (hasTasks)
            {
                _logger.LogWarning("Attempted to delete project with ID: {ProjectId} which has tasks.", project.Id);
                return (false, "Cannot delete project that still has tasks");
            }

            _context.Projects.Remove(existingProject);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted project with ID: {ProjectId}", project.Id);
            return (true, "Project deleted successfully");
        }
    }
}
