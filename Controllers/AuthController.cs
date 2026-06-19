using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationPractica.Models.DTO;
using WebApplicationPractica.Services;

namespace WebApplicationPractica.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthUser(LoginDto entity)
        {
            var user = await _userService.GetUserByNameAsync(entity.LoginName);
            if(user.Password != entity.Password) 
            {
                return Unauthorized();
            }
            var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            /* 
             var token = new JwtSecurityToken(
                issuer: "MiApi",
                audience: user.Name,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
             */
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });

        }


    }
}
