using WebApplicationPractica.Models;
using WebApplicationPractica.Models.DTO;

namespace WebApplicationPractica.Services
{
    public interface IProductService
    {

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateAsync(ProductCreateDTO product);
        Task UpdateAsync(int id, ProductUpdateDto product);
        Task DeleteAsync(int id);
    }

}
