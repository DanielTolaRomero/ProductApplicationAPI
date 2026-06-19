using Microsoft.AspNetCore.Mvc;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;
using WebApplicationPractica.Services;

namespace WebApplicationPractica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // -------------------------------------------------CRUD-------------------------------------------------
        // api/Category
        [HttpGet]
        public async Task<ActionResult> getCategoryAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // api/Category/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult> getCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) { return NotFound(new { mensaje = $"La categoria con id {id} no existe" }); }
            return Ok(category);
        }

        // api/Category/create
        [HttpPost("create")]
        public async Task<ActionResult<CategoryOutputDTO>> createCategoryAsync(CategoryInputDTO category)
        {
            var categoryDto = await _categoryService.CreateAsync(category);
            return Ok(categoryDto);

        }

        // api/Category/update/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult> updateCategoryAsync(int id, CategoryInputDTO category)
        {
            await _categoryService.UpdateAsync(id, category);
            return NoContent();

        }

        // api/Category/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> deleteCategoryAsync(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
