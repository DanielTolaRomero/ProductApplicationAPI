using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryOutputDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.Where(c => c.active).ToListAsync();
            return categories.Select(c => new CategoryOutputDTO { id = c.id, category = c.category });
        }

        // 
        public async Task<CategoryOutputDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id && c.active)
                ?? throw new CategoryNotFoundException("Categoria no encontrada");
            return new CategoryOutputDTO { id = category.id, category = category.category };
        }
        public async Task<CategoryOutputDTO> CreateAsync(Category category)
        {
            var createdCategory = _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryOutputDTO { id = createdCategory.Entity.id, category = createdCategory.Entity.category };
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id && c.active)
                ?? throw new CategoryNotFoundException("Categoria no encontrada");
            category.active = false; // Marcar como inactiva en lugar de eliminar físicamente
            await _context.SaveChangesAsync();
        }



        public async Task UpdateAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.id == id && c.active)
                ?? throw new CategoryNotFoundException("Categoria no encontrada");
            existingCategory.category = category.category; // Actualizar los campos necesarios

            await _context.SaveChangesAsync();
        }
    }
}
