using Microsoft.AspNetCore.Components;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Shared
{
    public class ProductCategoriesNavMenuBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService{ get; set; }
        public IEnumerable<ProductCategoryDto> ProductCategories{ get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProductCategories = await ProductService.GetProductCategories();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                throw;
            }
        }
    }

}
