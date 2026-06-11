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

        // POST: api/Product
        // Metodo para crear un nuevo producto
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductCreateDTO product)
        {
            var newProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.id }, newProduct);
        }

        // PUT: api/Product/5
        // Metodo para actualizar un producto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto product)
        {
            await _productService.UpdateAsync(id, product);

            // return BadRequest(new { mensaje = "Error al actualizar. Verifique los IDs"});
            return Ok(new { mensaje = "El producto a sido actualizado"});
        }

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
