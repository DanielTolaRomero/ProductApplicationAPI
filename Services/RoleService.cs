using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public class RoleService : ISimpleCrudService<Role, RoleInputDTO, RoleOutputDTO>
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleOutputDTO?> create(RoleInputDTO entity)
        {
            var role = new Role
            {
                RoleName = entity.RoleName
            };
            var newRole = _context.Add(role);
            await _context.SaveChangesAsync();
            return await getDtoById(newRole.Entity.Id);

        }

        public async Task deleteById(int id)
        {
            var role = await getEntityById(id);
            role.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleOutputDTO>> getAll()
        {
            return await _context.Roles
                .Where(r => r.IsActive)
                .Select(r => new RoleOutputDTO
                {
                    RoleName = r.RoleName,
                    RoleId = r.Id
                })
                .ToListAsync();
        }

        public async Task<RoleOutputDTO?> getDtoById(int id)
        {
            var role = await _context.Roles
                .Where(r => r.Id == id && r.IsActive)
                .Select(r => new RoleOutputDTO
                {
                    RoleId = r.Id,
                    RoleName = r.RoleName
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El rol con id {id} no existe");
            return role;
        }
        public async Task<Role?> getEntityById(int id)
        {
            return await _context.Roles
                .Where(r => r.Id == id && r.IsActive)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El rol con id {id} no existe");
        }

        public async Task<RoleOutputDTO?> getByName(string name)
        {
            return await _context.Roles
                .Where(r => r.RoleName == name && r.IsActive)
                .Select(r => new RoleOutputDTO
                {
                    RoleId = r.Id,
                    RoleName = r.RoleName
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"El rol con id {name} no existe");
        }

        public async Task updateById(int id, RoleInputDTO dto)
        {
            var role = await getEntityById(id);
            role.RoleName = dto.RoleName;
            await _context.SaveChangesAsync();
        }

    }
}
