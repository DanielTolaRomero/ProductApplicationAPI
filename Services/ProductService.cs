using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Data;
using WebApplicationPractica.Models;

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

        // metodo para eliminar productos, eliminacion logica, se cambia el estado del producto a inactivo
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.Where(p => p.active).FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return false;
            }

            product.active = false;

            await _context.SaveChangesAsync();
            return true;
        }

        // metodo para obtener todos los productos activos
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Where(p => p.active).ToListAsync();
        }

        // metodo para obtener un producto activos por id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(p => p.active).FirstOrDefaultAsync(p => p.id == id);
        }

        // metodo para actualizar un producto, se actualizan todos los campos del producto
        public async Task<bool> UpdateAsync(int id, ProductUpdateDto productDto)
        {
            var product = new Product
            {
                id = productDto.id,
                name = productDto.name,
                category = productDto.category,
                price = productDto.price
            };

            if(id != product.id)
            {
                return false;
            }
            var productExisting = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.id == id && p.active);
            if(productExisting == null)
            {
                return false;
            }
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
