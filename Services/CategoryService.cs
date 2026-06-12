using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;

namespace WebApplicationPractica.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id)
                ?? throw new CategoryNotFoundException("Categoria no encontrada");
            return category;
        }

        public async Task UpdateAsync(int id, Category category)
        {
            await GetCategoryByIdAsync(id); // Verificar que la categoría existe
            if (id != category.id)
            {
                throw new CategoryInvalidValueException("El id de la categoría no coincide con el id de la categoría a actualizar");
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
