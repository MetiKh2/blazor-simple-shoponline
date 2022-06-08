using HiddenVilla_Client.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        public string TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);

            if(item!=null)
                item.TotalPrice=cartItemDto.Price*cartItemDto.Quantity;
            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var itemDto = await ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
            CartChanged();
        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }
        protected async Task UpdateQuantityItem_Click(int id, int quantity)
        {
            try
            {
                if (quantity > 0)
                {
                    var returnedUpdatedItemDto = await ShoppingCartService.UpdateQuantity(new CartItemQuantityUpdateDto
                    {
                        CartItemId = id,
                        Quantity = quantity
                    });
                   await UpdateItemTotalPrice(returnedUpdatedItemDto);
                    CartChanged();
                    if (returnedUpdatedItemDto != null) await JSRuntime.ToastrSuccess("Update Successfully");
                }
                else
                {
                    var item = GetCartItem(id);
                    if (item != null)
                    {
                        item.Quantity = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }
        private void SetQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(p => p.Quantity);
        }
        private void CalculateCartSummaryTotals()
        {
            SetQuantity();
            SetTotalPrice();
        }
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(ShoppingCartItems.Count);
        }
    }
}
