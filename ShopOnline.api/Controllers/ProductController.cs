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
                if (products == null ) return NotFound();
                return Ok(products.ConvertToDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error reteriving data from database");
                throw;
            }
          
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                if (product == null) return NotFound();

                return Ok(product.ConvertToDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error reteriving data from database");
                throw;
            }
        }
        [HttpGet(nameof(GetProductCategories))]
        public async Task<IActionResult> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetProductCategories();
                if (productCategories == null) return NotFound();
                return Ok(productCategories.ConvertToDto());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
                throw;
            }
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);
                return Ok(products.ConvertToDto());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
                throw;
            }
        }
    }
}
