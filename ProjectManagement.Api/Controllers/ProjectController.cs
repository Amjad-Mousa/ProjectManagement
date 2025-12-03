using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Interfaces;

namespace ProjectManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return Ok(project);
        }

        [HttpGet("owner")]
        public async Task<ActionResult<List<ProjectDto>>> GetAllByOwner()
        {
            var projects = await _projectService.GetAllByOwnerAsync();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectDto projectDto)
        {
            var createdProject = await _projectService.CreateAsync(projectDto);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, [FromBody] ProjectDto projectDto)
        {
            if (id != projectDto.Id)
                return BadRequest("Project ID mismatch.");

            var updatedProject = await _projectService.UpdateAsync(projectDto);
            return Ok(updatedProject);
        }

        [Authorize(Roles = "admin")]
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
