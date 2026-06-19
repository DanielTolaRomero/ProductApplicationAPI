using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplicationPractica.Data;
using WebApplicationPractica.Exceptions;
using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public class ProductService : IProductService
    {
        private static readonly Expression<Func<Product, ProductOutputDTO>> ProductProjection = p => new ProductOutputDTO
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category.CategoryName,
            Price = p.Price,
            FechaCreacion = p.FechaCreacion
        };

        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context) {
            _context = context;
        }

        private void ValidatePageParameters(ref int page, ref int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            if (pageSize > 100)
            {
                pageSize = 100;
            }
        }
        // --------------------------------------------------Create--------------------------------------------------
        /*
         * medodo de para crear productos, se recibe un objeto ProductCreateDTO, 
         * se crea un nuevo objeto Product con los datos del DTO, se agrega a la base de datos y se guarda los cambios,
         * finalmente se devuelve el producto creado 
         */
        public async Task<ProductOutputDTO> CreateAsync(ProductCreateDTO productDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId) ?? 
                throw new NotFoundException($"La categoria con {productDto.CategoryId} no existe");
                
            var product = new Product
            {
                Name = productDto.Name,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price
            };
            var productSave = _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var productResult = await GetProductDtoByIdAsync(productSave.Entity.Id);
            return productResult;
        }
        // -----------------------------------------------------READ-----------------------------------------------------
        // metodo para obtener todos los productos activos
        public async Task<IEnumerable<ProductOutputDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => p.Active)
                .Select(ProductProjection)
                .ToListAsync();
        }

        // metodo para obtener un producto DTO activos por id
        public async Task<ProductOutputDTO?> GetProductDtoByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && p.Active)
                .Select(ProductProjection)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Producto con id {id} no encontrado");

            return product;
        }

        // metodo para obtener un producto activo por id, se devuelve el objeto Product completo, se utiliza para actualizar y eliminar productos
        public async Task<Product> GetProductEntityByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && p.Active)
                .Include(p => p.Category)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Producto con id {id} no encontrado");

            return product;
        }

        // metodo para actualizar un producto, se actualizan todos los campos del producto
        public async Task UpdateAsync(int id, ProductUpdateDto productDto)
        { 
            var product = await GetProductEntityByIdAsync(id);
            product.Name = productDto.Name;
            product.CategoryId = productDto.CategoryId;
            product.Price = productDto.Price;

            await _context.SaveChangesAsync();
        }

        // metodo para eliminar productos, eliminacion logica, se cambia el estado del producto a inactivo
        public async Task DeleteAsync(int id)
        {
            var product = await GetProductEntityByIdAsync(id);

            product.Active = false;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductOutputDTO>> GetProductsPage(int page, int pageSize)
        {
            ValidatePageParameters(ref page, ref pageSize);
            return await _context.Products.Where(p => p.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ProductProjection)
                .ToListAsync();
                
        }
        /* 
         * Metodo que devuelve el listado de productos activos por categoria, 
         * se utiliza el metodo Where para filtrar los productos por categoria y estado activo,
         * luego se convierte a una lista asincrona y se devuelve como un enumerable
        */
        public async Task<IEnumerable<ProductOutputDTO>> GetProductsByCategoryAsync(int categoryId, int page, int pageSize)
        {
            ValidatePageParameters(ref page, ref pageSize);
            if (page> 0 && pageSize > 0)
            {
                page = 1;
                pageSize = 10;
            }
            return await _context.Products.Where(p => p.CategoryId == categoryId && p.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ProductProjection)
                .ToListAsync();
        }
        /*
         *  Metodo que devuelve el listado de productos activos por rango de precio,
         *  se utiliza el metodo Where para filtrar los productos por rango de precio y estado activo,
         *  luego se convierte a una lista asincrona y se devuelve como un enumerable
         *  el metodo recibe como parametros el precio minimo, el precio maximo, la pagina
         */

        public async Task<IEnumerable<ProductOutputDTO>> GetProductsByRangePriceAsync(decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            ValidatePageParameters(ref page, ref pageSize);
            return await _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.Active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ProductProjection)
                .ToListAsync();
        }

        /*
         * Metodo que devuelve el listado de productos activos ordenados por precio,
         * de acuerdo al termino de ordenamiento recibido como parametro, 
         * se utiliza el metodo OrderBy o OrderByDescending para ordenar los productos por precio,
         */

        public async Task<IEnumerable<ProductOutputDTO>> SortProductsAsync(string sortTerm, int page, int pageSize)
        {
            ValidatePageParameters(ref page, ref pageSize);
            return sortTerm.ToLower() switch
            {
                "asc" => await _context.Products.Where(p => p.Active)
                    .OrderBy(p => p.Price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ProductProjection)
                    .ToListAsync(),
                "desc" => await _context.Products.Where(p => p.Active)
                    .OrderByDescending(p => p.Price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ProductProjection)
                    .ToListAsync(),
                _ => throw new ArgumentException("Invalid sort term")
            };
        }

        public async Task<ProductOutputDTO?> GetProductByNameAsync(string name)
        {
            var product = await _context.Products
                .Where(p => p.Name == name && p.Active)
                .Select(ProductProjection)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Producto con nombre {name} no encontrado");

            return product;
        
        }
    }
}
