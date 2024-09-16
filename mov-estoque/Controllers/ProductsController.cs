using Core.Inferfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace mov_estoque.Controllers
{
    [Route("api/v1/produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto dto)
        {
            try
            {
                await _productsService.SaveProduct(dto);
                return Ok("Produto cadastrado com sucesso");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _productsService.GetAllProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/movimentar")]
        public async Task<IActionResult> MovimentarEstoque(int id, [FromBody] MovimentacaoDto movimentacao)
        {
            var atualizado = await _productsService.MovimentarEstoque(id, movimentacao);
            if (atualizado)
                return Ok("Movimentação realizada com sucesso");
            return BadRequest("Erro ao realizar movimentação");
        }

        [HttpDelete("movimentacoes/{id}")]
        public async Task<IActionResult> ExcluirMovimentacao(int id)
        {
            var sucesso = await _productsService.ExcluirMovimentacao(id);
            if (sucesso)
            {
                return Ok("Movimentação excluída com sucesso.");
            }
            return BadRequest("Não foi possível excluir a movimentação. Verifique se é a última movimentação.");
        }
    }
}
