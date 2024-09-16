using Shared.DTO;

namespace Core.Inferfaces
{
    public interface IProductsService
    {
        Task SaveProduct(ProductDto dto);
        Task<List<ProductDto>> GetAllProducts();
        Task<bool> ExcluirMovimentacao(int id);
        Task<bool> MovimentarEstoque(int id, MovimentacaoDto movimentacao);
    }
}
