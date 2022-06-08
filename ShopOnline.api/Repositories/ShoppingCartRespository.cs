using Microsoft.EntityFrameworkCore;
using ShopOnline.api.Data;
using ShopOnline.api.Entities;
using ShopOnline.api.Repositories.Contracts;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Repositories
{
    public class ShoppingCartRespository : IShoppingCartRespository
    {
        private readonly ShopOnlineDbContext _context;

        public ShoppingCartRespository(ShopOnlineDbContext context)
        {
            _context = context;
        }
        private async Task<bool> CartItemExists(int cartId,int productId)
        {
            return await _context.CartItems.
                AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (!await CartItemExists(cartItemToAddDto.CartId,cartItemToAddDto.ProductId))
            {
                var item = await _context.Products.Where(p => p.Id == cartItemToAddDto.ProductId)
                 .Select(p => new CartItem
                 {
                     CartId = cartItemToAddDto.CartId,
                     ProductId = p.Id,
                     Quantity = cartItemToAddDto.Quantity,
                 }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item=await _context.CartItems.FindAsync(id);
            if(item != null)
            {
                _context.Remove(item);
               await _context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await(from cart in this._context.Carts
                         join cartItem in this._context.CartItems
                         on cart.Id equals cartItem.CartId
                         where cartItem.Id == id
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             Quantity = cartItem.Quantity,
                             CartId = cartItem.CartId
                         }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _context.Carts
                          join cartItem in _context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemToAddDto)
        {
            var item =await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Quantity = cartItemToAddDto.Quantity;
                await _context.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
