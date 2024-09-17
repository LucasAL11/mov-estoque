using Core.Inferfaces;
using Shared.DTO;
using Data.Interfaces;
using Core.Mappers;

namespace Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepositorie;

        public ProductsService(IProductsRepository productsRepositorie)
        {
            _productsRepositorie = productsRepositorie;
        }

        public Task<bool> ExcluirMovimentacao(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {

            var products = await _productsRepositorie.ObterTodosAsync();

            var productsList = new List<ProductDto>();

            foreach (var product in products)
            {
                productsList.Add(product.ToDto());
            }

            return productsList;
        }

        public async Task<bool> MovimentarEstoque(int idProduto, MovimentacaoDto movimentacao)
        {

            var produto = await _productsRepositorie.ObterProdutoPorIdAsync(idProduto);

            if (produto == null)
            {
                return false;
            }

            var novoSaldo = _productsRepositorie.CorrigirSaldo(movimentacao.Tipo, produto.Saldo, movimentacao.Quantidade);

            if (novoSaldo < 0)
                return false;

            var estoqueMovimentado = await _productsRepositorie.RegistrarMovimentacaoAsync(movimentacao.ToModel(idProduto));

            if (!estoqueMovimentado)
                return false;

            await _productsRepositorie.AtualizarSaldoAsync(novoSaldo, idProduto);

            return true;
        }

        public async Task SaveProduct(ProductDto dto)
        {
            await _productsRepositorie.InserirProdutoAsync(dto.ToModel());
        }
    }
}
