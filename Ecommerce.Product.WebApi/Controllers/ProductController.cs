using Ecommerce.Products.WebApi.Dtos;
using Ecommerce.Products.WebApi.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Products.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository productRepository) : ControllerBase
    {
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllProducts(CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllProducts(cancellationToken);
            if (products == null) return NotFound();
            return Ok(products);
        }

        [HttpGet("AddFakeDatas")]
        public async Task<IActionResult> AddFakeDatas()
        {
            var result = await productRepository.AddFakeDatas();
            if (result.Success) return Ok(result);
            return BadRequest();
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(CreateProductDto productDto, CancellationToken cancellationToken)
        {
            var result = await productRepository.CreateProduct(productDto, cancellationToken);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
