using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _iprojectService;

        public ProjectController(IProjectService iprojectService)
        {
            _iprojectService = iprojectService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            if (projects == null || !projects.Any())
            {
                throw new NotFoundException("No projects found.");
            }
            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                OwnerId = p.OwnerId
            }).ToList();

            return Ok(projectDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                OwnerId = project.OwnerId
            };
            return Ok(projectDto);
        }

        [HttpGet("owner")]
        public async Task<ActionResult<List<ProjectDto>>> GetAllByOwner()
        {
            var projects = await _projectService.GetAllByOwnerAsync();
            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                OwnerId = p.OwnerId
            }).ToList();

            return Ok(projectDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] Project project)
        {
            var createdProject = await _projectService.CreateAsync(project);
            var projectDto = new ProjectDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                Status = createdProject.Status,
                OwnerId = createdProject.OwnerId
            };
            return CreatedAtAction(nameof(GetById), new { id = projectDto.Id }, projectDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, [FromBody] Project project)
        {
            if (id != project.Id)
                return BadRequest("Project ID mismatch.");

            var updatedProject = await _projectService.UpdateAsync(project);
            var projectDto = new ProjectDto
            {
                Id = updatedProject.Id,
                Name = updatedProject.Name,
                Description = updatedProject.Description,
                Status = updatedProject.Status,
                OwnerId = updatedProject.OwnerId
            };
            return Ok(projectDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleted = await _projectService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
