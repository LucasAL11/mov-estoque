using Shared.DTO;
using Shared.Models;

namespace Core.Mappers
{
    internal static class ProductMapper
    {
        internal static ProductModel ToModel(this ProductDto dto)
        {
            return new ProductModel
            {
                Sku = dto.Sku,
                Descricao = dto.Description,
                DataCadastro = DateTime.UtcNow,
                DataMovimentacao = DateTime.UtcNow,
                Saldo = dto.Balance
            };
        }

        internal static ProductDto ToDto(this ProductModel model)
        {
            return new ProductDto
            {
                Id = model.Id,
                Sku = model.Sku,
                Created = model.DataCadastro,
                Updated = model.DataMovimentacao,
                Description = model.Descricao,
            };
        }

        internal static MovimentacoesModel ToModel(this MovimentacaoDto dto, int IntProd)
        {
            return new MovimentacoesModel
            {
                IdProduto = IntProd,
                Quantidade = dto.Quantidade,
                Data = dto.Data,
                Tipo = dto.Tipo,
            };
        }
    }
}
