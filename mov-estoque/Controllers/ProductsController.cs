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
    }
}
