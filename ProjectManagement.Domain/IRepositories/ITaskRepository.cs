using ProjectManagement.Domain.Models;


namespace ProjectManagement.Domain.IRepositories
{
    public interface ITaskRepository
    {
        Task<PTask?> GetByIdAsync(int id);
        Task<List<PTask>> GetAllAsync();
        Task<List<PTask>> GetAllByProjectAsync(Guid projectId);
        Task<PTask> AddAsync(PTask task);
        Task<PTask> UpdateAsync(PTask task);
        Task<bool> DeleteAsync(PTask task);
    }
}
