using Shared.DTO;

namespace Core.Inferfaces
{
    public interface IProductsService
    {
        Task SaveProduct(ProductDto dto);
        Task<List<ProductDto>> GetAllProducts();
    }
}
