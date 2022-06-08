using Blazored.LocalStorage;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;
        private const string Key = "ProductCollection";
        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync
                <IEnumerable<ProductDto>>(Key)??await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(Key);
        }
        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetProducts();
            if (productCollection != null)await _localStorageService.SetItemAsync(Key,productCollection);
            return productCollection;
        }
    }
}
