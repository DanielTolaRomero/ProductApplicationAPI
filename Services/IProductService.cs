using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface IProductService
    {
        // Metodos para obtener listas de objetos
        // Metodo para obtener todos los productos
        Task<IEnumerable<Product>> GetAllProductsAsync();
        // Metodo para obtener productos paginados
        Task<IEnumerable<Product>> GetProductsPage(int page, int pageSize);
        // Metodo para obtener productos por categoria
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int category, int page, int pageSize);
        // Metodo para buscar productos por rango de precio
        Task<IEnumerable<Product>> GetProductsByRangePriceAsync(decimal minPrice, decimal maxPrice, int page, int pageSize);
        // Metodo para ordenar productos por precio
        Task<IEnumerable<Product>> SortProductsAsync(string sortTerm, int page, int pageSize);
        // -------------------------------------------------------------------------------------------
        // Metodos para obtener Productos unicos
        // Metodo para obtener un producto por su ID
        Task<Product?> GetProductByIdAsync(int id);
        // Metodo para obtener un producto por su nombre
        Task<Product?> GetProductByNameAsync(string name);
        // -------------------------------------------------------------------------------------------
        // Metodos para modificar objetos
        // Metodo para crear un nuevo producto
        Task<Product> CreateAsync(ProductCreateDTO product);
        // Metodo para actualizar un producto existente
        Task UpdateAsync(int id, ProductUpdateDto product);
        // Metodo para eliminar un producto por su ID
        Task DeleteAsync(int id);


    }

}
