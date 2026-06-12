using WebApplicationPractica.Models;

namespace WebApplicationPractica.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
         public Task<Category?> GetCategoryByIdAsync(int id);
         public Task<Category> CreateAsync(Category category);
         public Task UpdateAsync(int id, Category category);
         public Task DeleteAsync(int id);
    }
}
