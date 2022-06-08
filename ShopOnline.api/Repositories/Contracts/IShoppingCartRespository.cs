using ShopOnline.api.Entities;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Repositories.Contracts
{
    public interface IShoppingCartRespository
    {
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateQuantity(int id,CartItemQuantityUpdateDto cartItemToAddDto);
        Task<CartItem> DeleteItem(int id);
        Task<CartItem> GetItem(int id);
        Task<IEnumerable<CartItem>> GetItems(int userId);
    }
}
