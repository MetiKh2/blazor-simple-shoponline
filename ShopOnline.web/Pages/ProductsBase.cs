using Microsoft.AspNetCore.Components;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService{ get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
           await ClearLocalStorage();
            Products = await ManageProductsLocalStorageService.GetCollection();
            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            var totalQuantity = shoppingCartItems.Count();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQuantity);
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetProductsByCategory()
        {
            return from p in Products
                   group p by p.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }
        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProducts)
        {
            return groupedProducts.FirstOrDefault(p => p.CategoryId == groupedProducts.Key).CategoryName;
        }
        private async Task ClearLocalStorage()
        {
            await ManageCartItemsLocalStorageService.RemoveCollection();
            await ManageProductsLocalStorageService.RemoveCollection();
        }
    }
}
