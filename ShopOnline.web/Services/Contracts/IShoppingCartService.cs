﻿using ShopOnline.models.DTOs;

namespace ShopOnline.web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateQuantity(CartItemQuantityUpdateDto cartItemQuantityUpdateDto);
        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQuantity);
    }
}
