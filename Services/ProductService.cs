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

        /*
         * medodo de para crear productos, se recibe un objeto ProductCreateDTO, 
         * se crea un nuevo objeto Product con los datos del DTO, se agrega a la base de datos y se guarda los cambios,
         * finalmente se devuelve el producto creado 
         */
        public async Task<ProductOutputDTO> CreateAsync(ProductCreateDTO productDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == productDto.categoryId) ?? 
                throw new CategoryNotFoundException("La categoria no existe");
                
            var product = new Product
            {
                name = productDto.name,
                categoryId = productDto.categoryId,
                price = productDto.price
            };
            var productSave = _context.Products.Add(product);

            await _context.SaveChangesAsync();
            return new ProductOutputDTO
            {
                id = productSave.Entity.id,
                name = productSave.Entity.name,
                category = productSave.Entity.category.category,
                price = productSave.Entity.price,
                FechaCreacion = productSave.Entity.FechaCreacion
            };
        }

        // metodo para obtener todos los productos activos
        public async Task<IEnumerable<ProductOutputDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => p.active)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
                .ToListAsync();
        }

        // metodo para obtener un producto activos por id
        public async Task<ProductOutputDTO?> GetProductDtoByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.id == id && p.active)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
                .FirstOrDefaultAsync()
                ?? throw new ProductNotFoundException("Producto no encontrado");

            return product;
        }
        public async Task<Product> GetProductByIdAsyncO(int id)
        {
            var product = await _context.Products
                .Where(p => p.id == id && p.active)
                .Include(p => p.category)
                .FirstOrDefaultAsync()
                ?? throw new ProductNotFoundException("Producto no encontrado");

            return product;
        }

        // metodo para actualizar un producto, se actualizan todos los campos del producto
        public async Task UpdateAsync(int id, ProductUpdateDto productDto)
        { 
            var product = await GetProductByIdAsyncO(id);
            product.name = productDto.name;
            product.categoryId = productDto.categoryId;
            product.price = productDto.price;

            await _context.SaveChangesAsync();
        }

        // metodo para eliminar productos, eliminacion logica, se cambia el estado del producto a inactivo
        public async Task DeleteAsync(int id)
        {
            var product = await GetProductByIdAsyncO(id);

            product.active = false;

            await _context.SaveChangesAsync();
        }
        // metodo para verificar si un producto existe por id

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }

        public async Task<IEnumerable<ProductOutputDTO>> GetProductsPage(int page, int pageSize)
        {
            return await _context.Products.Where(p => p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
                .ToListAsync();
                
        }
        /* 
         * Metodo que devuelve el listado de productos activos por categoria, 
         * se utiliza el metodo Where para filtrar los productos por categoria y estado activo,
         * luego se convierte a una lista asincrona y se devuelve como un enumerable
        */
        public async Task<IEnumerable<ProductOutputDTO>> GetProductsByCategoryAsync(int categoryId, int page, int pageSize)
        {
            return await _context.Products.Where(p => p.categoryId == categoryId && p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
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
            return await _context.Products.Where(p => p.price >= minPrice && p.price <= maxPrice && p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
                .ToListAsync();
        }

        /*
         * Metodo que devuelve el listado de productos activos ordenados por precio,
         * de acuerdo al termino de ordenamiento recibido como parametro, 
         * se utiliza el metodo OrderBy o OrderByDescending para ordenar los productos por precio,
         */

        public async Task<IEnumerable<ProductOutputDTO>> SortProductsAsync(string sortTerm, int page, int pageSize)
        {
            return sortTerm.ToLower() switch
            {
                "asc" => await _context.Products.Where(p => p.active).OrderBy(p => p.price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.category)
                    .Select(p => new ProductOutputDTO
                    {
                        id = p.id,
                        name = p.name,
                        category = p.category.category,
                        price = p.price,
                        FechaCreacion = p.FechaCreacion
                    })
                    .ToListAsync(),
                "desc" => await _context.Products.Where(p => p.active).OrderByDescending(p => p.price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.category)
                    .Select(p => new ProductOutputDTO
                    {
                        id = p.id,
                        name = p.name,
                        category = p.category.category,
                        price = p.price,
                        FechaCreacion = p.FechaCreacion
                    })
                    .ToListAsync(),
                _ => throw new ArgumentException("Invalid sort term")
            };
        }

        public async Task<ProductOutputDTO?> GetProductByNameAsync(string name)
        {
            var product = await _context.Products
                .Where(p => p.name == name && p.active)
                .Include(p => p.category)
                .Select(p => new ProductOutputDTO
                {
                    id = p.id,
                    name = p.name,
                    category = p.category.category,
                    price = p.price,
                    FechaCreacion = p.FechaCreacion
                })
                .FirstOrDefaultAsync()
                ?? throw new ProductNotFoundException("Producto no encontrado");

            return product;
        
        }
    }
}
