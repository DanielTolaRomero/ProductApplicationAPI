using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface ICategoryService
    {
         public Task<IEnumerable<CategoryOutputDTO>> GetAllCategoriesAsync();
         public Task<CategoryOutputDTO?> GetCategoryByIdAsync(int id);
         public Task<CategoryOutputDTO> CreateAsync(Category category);
         public Task UpdateAsync(int id, Category category);
         public Task DeleteAsync(int id);
    }
}
