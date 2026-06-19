using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleOutputDTO>> GetAllRolesAsync(int page, int pageSize);
        public Task<RoleOutputDTO> GetRoleByIdAsync(int roleId);
        public Task<RoleOutputDTO> CreateRoleAsync(RoleInputDTO role);
        public Task UpdateRoleAsync(RoleInputDTO role);
        public Task DeleteRoleAsync(int roleId);
    }
}
