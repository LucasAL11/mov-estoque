using Shared.Models;

namespace Data.Interfaces
{
    public interface IProductsRepositorie
    {
        Task Save(ProductModel model);
        Task<IEnumerable<ProductModel>> RetrivieAll();
        Task<bool> RegistrarMovimentacaoAsync(MovimentacoesModel movimentacao);
    }
}
