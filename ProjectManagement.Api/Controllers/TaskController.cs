using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Models;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _itaskService;

        public TaskController(ITaskService itaskService)
        {
            _itaskService = itaskService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Task>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Task>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return BadRequest("Task not found");
            }
            return Ok(task);
        }

        [HttpGet("project/{projectId:guid}")]
        public async Task<ActionResult<List<Task>>> GetAllByProject(Guid projectId)
        {
            var tasks = await _taskService.GetAllByProjectAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> Create(Domain.Models.PTask task)
        {
            var createdTask = await _taskService.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Task>> Update(int id, Domain.Models.PTask task)
        {
            if (id != task.Id)
                return BadRequest("Task ID mismatch.");

            var updatedTask = await _taskService.UpdateAsync(task);
            return Ok(updatedTask);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }
}
