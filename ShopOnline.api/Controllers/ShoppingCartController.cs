using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.api.Extensions;
using ShopOnline.api.Repositories.Contracts;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRespository _shoppingCartRespository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IProductRepository productRepository, IShoppingCartRespository shoppingCartRespository)
        {
            _productRepository = productRepository;
            _shoppingCartRespository = shoppingCartRespository;
        }

        [HttpGet("{userId}/getitems")]
        public async Task<IActionResult> GetItems(int userId)
        {
            try
            {
                var cartItems = await _shoppingCartRespository.GetItems(userId);
                if (cartItems == null) return NoContent();
                var products = await _productRepository.GetProducts();
                if (products == null) throw new Exception("No product exist in the system");
                var cartItemsDto = cartItems.ConvertToDto(products);
                return Ok(cartItemsDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var cartItem = await _shoppingCartRespository.GetItem(id);
                if (cartItem == null) return NoContent();
                var product = await _productRepository.GetProduct(cartItem.ProductId);
                if (product == null) throw new Exception("No product exist in the system");
                var cartItemDto = cartItem.ConvertToDto(product);
                return Ok(cartItemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await _shoppingCartRespository.AddItem(cartItemToAddDto);
                if (newCartItem == null) return NoContent();
                var product=await _productRepository.GetProduct(newCartItem.ProductId);
                if (product == null) throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
                var newCartItemDto=newCartItem.ConvertToDto(product);
                return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var item = await _shoppingCartRespository.DeleteItem(id);
                if (item== null) return NotFound();
                var product = await _productRepository.GetProduct(item.ProductId);
                if (product == null) throw new Exception($"Something went wrong when attempting to retrieve product (productId:({item.ProductId})");
                var itemDto = item.ConvertToDto(product);
                return Ok(itemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateQuantity(int id,CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            try
            {
                var item = await _shoppingCartRespository.UpdateQuantity(id,cartItemQuantityUpdateDto);
                if (item == null) return NotFound();
                var product = await _productRepository.GetProduct(item.ProductId);
                if (product == null) throw new Exception($"Something went wrong when attempting to retrieve product (productId:({item.ProductId})");
                var itemDto = item.ConvertToDto(product);
                return Ok(itemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
