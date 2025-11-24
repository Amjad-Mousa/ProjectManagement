using ProjectManagement.Domain.Models;


namespace ProjectManagement.Domain.IRepositories

{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<List<Project>> GetAllAsync();
        Task<List<Project>> GetAllByOwnerAsync(Guid ownerId);
        Task<Project> AddAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task<(bool Success, string Message)> DeleteAsync(Project project);
    }
}
