using ShopOnline.api.Entities;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories)
        {
            return (from p in products
                    join pc in productCategories
                    on p.CategoryId equals pc.Id
                    select new ProductDto {
                        CategoryId = p.CategoryId,
                        Description = p.Description,
                        ImageUrl = p.ImageUrl,
                        Name = p.Name,
                        Id = p.Id,
                        Price = p.Price,
                        CategoryName=pc.Name
                    }).ToList();
        }
    }
}
