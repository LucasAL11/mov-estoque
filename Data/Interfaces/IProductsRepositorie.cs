using Shared.Models;

namespace Data.Interfaces
{
    public interface IProductsRepositorie
    {
        Task Save(ProductModel model);
        Task<IEnumerable<ProductModel>> RetrivieAll();
    }
}
