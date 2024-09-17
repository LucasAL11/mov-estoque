using Shared.Enums;
using Shared.Models;

namespace Data.Interfaces
{
    public interface IProductsRepository
    {
        Task InserirProdutoAsync(ProductModel model);
        Task<IEnumerable<ProductModel>> ObterTodosAsync();
        Task<bool> RegistrarMovimentacaoAsync(MovimentacoesModel movimentacao);
        Task<bool> ExcluirUltimaMovimentacaoAsync(int idMovimentacao);
        Task<ProductModel> ObterProdutoPorIdAsync(int idProduto);
        int CorrigirSaldo(ETipo tipoMovimentacao, int saldo, int quantidade);
        Task AtualizarSaldoAsync(int saldo, int idProduto);

    }
}
