using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System;
using System.Threading.Tasks;

namespace ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iuserservice;

        public UserController(IUserService _iuserservice)
        {
            _iuserservice = _iuserservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUserName(string username)
        {
            return Ok(await _service.GetByUserNameAsync(username));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            return Ok(await _service.UpdateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
    }
}
