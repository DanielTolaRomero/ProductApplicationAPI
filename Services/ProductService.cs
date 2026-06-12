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
        public async Task<Product> CreateAsync(ProductCreateDTO productDto)
        {
            var product = new Product
            {
                name = productDto.name,
                categoryId = productDto.categoryId,
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
            product.categoryId = productDto.categoryId;
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

        public Task<IEnumerable<Product>> GetProductsPage(int page, int pageSize)
        {
            return _context.Products.Where(p => p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }
        /* 
         * Metodo que devuelve el listado de productos activos por categoria, 
         * se utiliza el metodo Where para filtrar los productos por categoria y estado activo,
         * luego se convierte a una lista asincrona y se devuelve como un enumerable
        */
        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int category, int page, int pageSize)
        {
            return _context.Products.Where(p => p.categoryId == category && p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }
        /*
         *  Metodo que devuelve el listado de productos activos por rango de precio,
         *  se utiliza el metodo Where para filtrar los productos por rango de precio y estado activo,
         *  luego se convierte a una lista asincrona y se devuelve como un enumerable
         *  el metodo recibe como parametros el precio minimo, el precio maximo, la pagina
         */

        public Task<IEnumerable<Product>> GetProductsByRangePriceAsync(decimal minPrice, decimal maxPrice, int page, int pageSize)
        {
            return _context.Products.Where(p => p.price >= minPrice && p.price <= maxPrice && p.active)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }

        /*
         * Metodo que devuelve el listado de productos activos ordenados por precio,
         * de acuerdo al termino de ordenamiento recibido como parametro, 
         * se utiliza el metodo OrderBy o OrderByDescending para ordenar los productos por precio,
         */

        public Task<IEnumerable<Product>> SortProductsAsync(string sortTerm, int page, int pageSize)
        {
            return sortTerm.ToLower() switch
            {
                "asc" => _context.Products.Where(p => p.active).OrderBy(p => p.price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    .ContinueWith(t => t.Result.AsEnumerable()),
                "desc" => _context.Products.Where(p => p.active).OrderByDescending(p => p.price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    .ContinueWith(t => t.Result.AsEnumerable()),
                _ => throw new ArgumentException("Invalid sort term")
            };
        }

        public Task<Product?> GetProductByNameAsync(string name)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.name == name && p.active);
        
        }
    }
}
