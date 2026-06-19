using Microsoft.AspNetCore.Mvc;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;
using WebApplicationPractica.Services;

namespace WebApplicationPractica.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RoleController : ControllerBase
    {
        private ISimpleCrudService<Role, RoleInputDTO, RoleOutputDTO> _context;
        public RoleController(ISimpleCrudService<Role, RoleInputDTO, RoleOutputDTO> context)
        {
            _context = context;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.getDtoById(id);
            return Ok(role);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.getAll();
            return Ok(roles);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(RoleInputDTO dto)
        {
            var newRole = await _context.create(dto);
            return CreatedAtAction(nameof(GetRoleById), new {id = newRole.RoleId},newRole);
            // return Ok(newRole);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleInputDTO dto) 
        { 
            await _context.updateById(id, dto);
            return NoContent();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _context.deleteById(id);
            return NoContent();
        }
}
}
