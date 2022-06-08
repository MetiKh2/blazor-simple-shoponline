using HiddenVilla_Client.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
    public class ProductDetailsBase:ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager{ get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        public string ErrorMessage { get; set; }
        public ProductDto Product{ get; set; }
        private List<CartItemDto> ShoppingCartItems { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
               var cartItemDto= await ShoppingCartService.AddItem(cartItemToAddDto);
                if (cartItemDto != null)
                {
                    await JSRuntime.ToastrSuccess("Product added to cart");
                    NavigationManager.NavigateTo("/cart");
                    ShoppingCartItems.Add(cartItemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                else await JSRuntime.ToastrError("Something is wrong");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await ManageProductsLocalStorageService.GetCollection();

            if (productDtos != null)
            {
                return productDtos.SingleOrDefault(p => p.Id == id);
            }
            return null;
        }
    }
}
