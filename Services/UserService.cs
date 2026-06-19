using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private static readonly Expression<Func<User, UserOutputDTO>> UserProjection = u => new UserOutputDTO
        {
            Id = u.Id,
            Name = u.UserName,
            Password = u.Password,
            Role = u.Role.RoleName
        };
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        private void validarPagePatch(ref int page, ref int pageSize)
        {
            if (page < 0)
            {
                page = 1;
            }
            if (pageSize < 0)
            {
                pageSize = 10;
            }
            if (pageSize > 100)
            {
                pageSize = 100;
            }
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id && u.IsActive)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El usuario con id {id} no existe");
            return user;
        }

        public async Task<UserOutputDTO> CreateUser(UserInputDTO userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Password = userDto.Password,
                RoleId = userDto.RoleId
            };
            var userCreated = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await GetUserByIdAsync(userCreated.Entity.Id);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserById(id);
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserOutputDTO>> GetAllUsersAsync(int page, int pageSize)
        {
            validarPagePatch(ref page, ref pageSize);

            var users = await _context.Users
                .Where(u => u.IsActive)
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Select(UserProjection)
                .ToListAsync();
            return users;
        }

        public async Task<UserOutputDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id && u.IsActive)
                .Select(UserProjection)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El usuario con id {id} no se a encontrado");

            return user;
        }

        public async Task UpdateUser(int id, UserInputDTO userDto)
        {
            var user = await GetUserById(id);
            user.UserName = userDto.UserName;
            user.RoleId = userDto.RoleId;
            user.Password = userDto.Password;

            await _context.SaveChangesAsync();
        }

        public async Task<UserOutputDTO> GetUserByNameAsync(string name)
        {
            var user = await _context.Users
                .Where(u => u.UserName == name &&  u.IsActive)
                .Select(UserProjection)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El usuario con nombre {name} no existe");

            return user;
        }
    }
}
