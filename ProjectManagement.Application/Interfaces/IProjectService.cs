using ProjectManagement.Application.DTOs;


namespace ProjectManagement.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(Guid id);
        Task<List<ProjectDto>> GetAllByOwnerAsync();
        Task<ProjectDto> CreateAsync(ProjectDto projectDto);
        Task<ProjectDto> UpdateAsync(ProjectDto projectDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
