using Shared.Models;

namespace Data.Interfaces
{
    public interface IProductsRepositorie
    {
        Task InserirProdutoAsync(ProductModel model);
        Task<IEnumerable<ProductModel>> ObterTodosAsync();
        Task<bool> RegistrarMovimentacaoAsync(MovimentacoesModel movimentacao)
        Task<bool> ExcluirUltimaMovimentacaoAsync(int idMovimentacao);
    }
}
