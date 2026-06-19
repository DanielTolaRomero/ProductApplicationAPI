using Microsoft.AspNetCore.Mvc;
using WebApplicationPractica.Models.DTO;
using WebApplicationPractica.Services;

namespace WebApplicationPractica.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _context;
        public UserController(IUserService context)
        {
            _context = context;
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageZise = 10)
        {
            var users = await _context.GetAllUsersAsync(page, pageZise);
            return Ok(users);
        }
        [HttpGet("get/{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var user = await _context.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpPost("create")]
        public async Task<ActionResult> CreateUser(UserInputDTO dto)
        {
            var newUser = await _context.CreateUser(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserInputDTO dto)
        {
            await _context.UpdateUser(id, dto);
            return NoContent();
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUserById(int id) 
        {
            await _context.DeleteUserAsync(id);
            return NoContent();
        }

    }
}
