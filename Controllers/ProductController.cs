using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;
using WebApplicationPractica.Services;

namespace WebApplicationPractica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        // inyección de dependencias del servicio de productos
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // -------------------------------------------------STATUS-------------------------------------------------
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                status = "active",
                message = "La API de productos está funcionando correctamente.",
                timetamp = DateTime.UtcNow
            });
        }
        // -------------------------------------------------CRUD-------------------------------------------------
        // Nota: En este controlador se delega toda la lógica de negocio al servicio de productos, lo que permite mantener el controlador limpio y enfocado en manejar las solicitudes HTTP y las respuestas.
        // -------------------------------------------------GET-------------------------------------------------
        // GET: api/Product
        // Metodo para obtener todos los productos activos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> getProduct()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/5
        // Metodo para obtener un producto por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) { return NotFound(new { mensaje = $"El producto con id {id} no existe" }); }

            return Ok(product);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            var product = await _productService.GetProductByNameAsync(name);
            if (product == null) { return NotFound(new { mensaje = $"El producto con nombre {name} no existe" }); }

            return Ok(product);
        }

        // GET: api/Product/category/{category}
        // Metodo para obtener productos por categoría
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetProductsByCategoryAsync(category, page, pageSize);
            return Ok(products);
        }

        [HttpGet("page")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetProductsPage(page, pageSize);
            return Ok(products);
        }

        // GET: api/Product/price?minPrice=10&maxPrice=100&page=1&pageSize=10
        // End point para obtener productos dentro de un rango de precios
        [HttpGet("price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByRangePrice([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetProductsByRangePriceAsync(minPrice, maxPrice, page, pageSize);
            return Ok(products);
        }

        // GET: api/Product/sort?sort=asc&page=1&pageSize=10
        // End point para obtener productos ordenados por precio
        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsSortedByPrice([FromQuery] string sort = "asc",
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.SortProductsAsync(sort, page, pageSize);
            return Ok(products);
        }

        // -------------------------------------------------POST-------------------------------------------------
        // POST: api/Product
        // Metodo para crear un nuevo producto
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDTO product)
        {
            var newProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.id }, newProduct);
        }

        // -------------------------------------------------PUT-------------------------------------------------
        // PUT: api/Product/5
        // Metodo para actualizar un producto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto product)
        {
            await _productService.UpdateAsync(id, product);

            // return BadRequest(new { mensaje = "Error al actualizar. Verifique los IDs"});
            return Ok(new { mensaje = "El producto a sido actualizado"});
        }

        // -------------------------------------------------DELETE-------------------------------------------------
        // DELETE: api/Product/5
        // Metodo para eliminar un producto por su ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);

            return Ok(new { mensaje = $"Producto con ID {id}  se elimino con exito." });
        }
    }
}
