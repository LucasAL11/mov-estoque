using Core.Inferfaces;
using Shared.DTO;
using Data.Interfaces;
using Core.Mappers;

namespace Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepositorie _productsRepositorie;

        public ProductsService(IProductsRepositorie productsRepositorie)
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
            return await _productsRepositorie.RegistrarMovimentacaoAsync(movimentacao.ToModel(idProduto));
        }

        public async Task SaveProduct(ProductDto dto)
        {
            await _productsRepositorie.InserirProdutoAsync(dto.ToModel());
        }
    }
}
