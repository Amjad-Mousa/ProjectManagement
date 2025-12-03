using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Models;
using ProjectManagement.Application.DTOs;


namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<TaskDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound("Task not found");

            return Ok(task);
        }

        [HttpGet("project/{projectId:guid}")]
        public async Task<ActionResult<List<TaskDto>>> GetAllByProject(Guid projectId)
        {
            var tasks = await _taskService.GetAllByProjectAsync(projectId);
            return Ok(tasks);
        }


        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] TaskDto taskDto)
        {
            var createdTask = await _taskService.CreateAsync(taskDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskDto>> Update(int id, [FromBody] TaskDto taskDto)
        {
            if (id != taskDto.Id)
                return BadRequest("Task ID mismatch.");

            var updatedTask = await _taskService.UpdateAsync(taskDto);
            return Ok(updatedTask);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }

}
