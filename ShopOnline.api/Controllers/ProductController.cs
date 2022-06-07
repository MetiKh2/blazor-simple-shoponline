using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.api.Extensions;
using ShopOnline.api.Repositories.Contracts;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                var categories = await _productRepository.GetProductCategories();
                if (products == null || categories == null) return NotFound();
                return Ok(products.ConvertToDto(categories));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error reteriving data from database");
                throw;
            }
          
        }
    }
}
