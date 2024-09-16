using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace mov_estoque.Controllers
{
    [Route("api/v1/movimentacoes")]
    [ApiController]
    public class MovimentacoesController : ControllerBase
    {
        [HttpPost("{tipo}/produtos/{idProduto}")]
        public async Task<IActionResult> Movimentar(MovimentacaoDto dto) 
        {
            return Ok();
        }

        [HttpDelete("{IdMovimentacao}")]
        public async Task<IActionResult> ExcluirMovimentacao(int IdMovimentacao)
        {
            return Ok();
        }
    }
}
