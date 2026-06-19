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
            var categories = await _context.Categories.Where(c => c.Active).ToListAsync();
            return categories.Select(c => new CategoryOutputDTO { Id = c.Id, Category = c.CategoryName });
        }

        // 
        public async Task<CategoryOutputDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.Active)
                ?? throw new NotFoundException($"Categoriacon id {id} no encontrada");
            return new CategoryOutputDTO { Id = category.Id, Category = category.CategoryName };
        }
        public async Task<CategoryOutputDTO> CreateAsync(CategoryInputDTO categoryDto)
        {
            var category = new Category
            {
                CategoryName = categoryDto.Category,
            };
            var createdCategory = _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryOutputDTO { Id = createdCategory.Entity.Id, Category = createdCategory.Entity.CategoryName };
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.Active)
                ?? throw new NotFoundException($"Categoria con id {id} no encontrada");
            category.Active = false; // Marcar como inactiva en lugar de eliminar físicamente
            await _context.SaveChangesAsync();
        }



        public async Task UpdateAsync(int id, CategoryInputDTO category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.Active)
                ?? throw new NotFoundException($"Categoria con id {id} no encontrada");
            existingCategory.CategoryName = category.Category; // Actualizar los campos necesarios

            await _context.SaveChangesAsync();
        }
    }
}
