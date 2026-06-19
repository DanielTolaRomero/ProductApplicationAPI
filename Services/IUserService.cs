using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserOutputDTO>> GetAllUsersAsync(int page, int pageSize);
        public Task<UserOutputDTO> GetUserByIdAsync(int id);
        public Task<UserOutputDTO> GetUserByNameAsync(string name);
        public Task<UserOutputDTO> CreateUser(UserInputDTO userDto);
        public Task UpdateUser(int id, UserInputDTO userDto);
        public Task DeleteUserAsync(int id);
    }
}
