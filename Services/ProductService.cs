using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context) {
            _context = context;
        }

        // medodo de para crear productos
        public async Task<Product> CreateAsync(ProductCreateDTO productDto)
        {
            var product = new Product
            {
                name = productDto.name,
                category = productDto.category,
                price = productDto.price
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        

        // metodo para obtener todos los productos activos
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Where(p => p.active).ToListAsync();
        }

        // metodo para obtener un producto activos por id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id && p.active)
                ?? throw new ProductNotFoundException("Producto no encontrado");

            return product;
        }

        // metodo para actualizar un producto, se actualizan todos los campos del producto
        public async Task UpdateAsync(int id, ProductUpdateDto productDto)
        { 
            if(id != productDto.id)
            {
                throw new ProductInvalidIdException("El id del producto no coincide con el id del producto a actualizar");
            }
            var product = await GetProductByIdAsync(id);
            product.name = productDto.name;
            product.category = productDto.category;
            product.price = productDto.price;

            await _context.SaveChangesAsync();
        }

        // metodo para eliminar productos, eliminacion logica, se cambia el estado del producto a inactivo
        public async Task DeleteAsync(int id)
        {
            var product = await GetProductByIdAsync(id);

            product.active = false;

            await _context.SaveChangesAsync();
        }
        // metodo para verificar si un producto existe por id

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
