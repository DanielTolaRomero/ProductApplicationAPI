using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface IProductService
    {
        // -------------------------------------Metodos para obtener listas de objetos-----------------------------------------
        // Metodo para obtener todos los productos
        Task<IEnumerable<ProductOutputDTO>> GetAllProductsAsync();
        // Metodo para obtener productos paginados
        Task<IEnumerable<ProductOutputDTO>> GetProductsPage(int page, int pageSize);
        // Metodo para obtener productos por categoria
        Task<IEnumerable<ProductOutputDTO>> GetProductsByCategoryAsync(int category, int page, int pageSize);
        // Metodo para buscar productos por rango de precio
        Task<IEnumerable<ProductOutputDTO>> GetProductsByRangePriceAsync(decimal minPrice, decimal maxPrice, int page, int pageSize);
        // Metodo para ordenar productos por precio
        Task<IEnumerable<ProductOutputDTO>> SortProductsAsync(string sortTerm, int page, int pageSize);
        // --------------------------------------Metodos para obtener Productos unicos-----------------------------------------
        // Metodo para obtener un producto por su ID
        Task<ProductOutputDTO?> GetProductDtoByIdAsync(int id);
        // Metodo para obtener un producto por su nombre
        Task<ProductOutputDTO?> GetProductByNameAsync(string name);
        // --------------------------------------Metodos para modificar objetos-------------------------------------------------
        // Metodo para crear un nuevo producto
        Task<ProductOutputDTO> CreateAsync(ProductCreateDTO product);
        // Metodo para actualizar un producto existente
        Task UpdateAsync(int id, ProductUpdateDto product);
        // Metodo para eliminar un producto por su ID
        Task DeleteAsync(int id);


    }

}
