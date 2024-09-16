using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mov_estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> AddProducts()
        {
            try
            {
                return Ok("teste");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
