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

        public async Task<List<ProductDto>> GetAllProducts()
        {

            var products = await _productsRepositorie.RetrivieAll();

            var productsList = new List<ProductDto>();

            foreach (var product in products)
            {
                productsList.Add(product.ToDto());
            }

            return productsList;
        }

        public async Task SaveProduct(ProductDto dto)
        {
            await _productsRepositorie.Save(dto.ToModel());
        }
    }
}
