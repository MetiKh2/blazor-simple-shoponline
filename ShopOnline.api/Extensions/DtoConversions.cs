using ShopOnline.api.Entities;
using ShopOnline.models.DTOs;

namespace ShopOnline.api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from p in products
                     
                    select new ProductDto
                    {
                        CategoryId = p.CategoryId,
                        Description = p.Description,
                        ImageUrl = p.ImageUrl,
                        Name = p.Name,
                        Id = p.Id,
                        Price = p.Price,
                        CategoryName = p.ProductCategory.Name,
                        Quantity = p.Quantity,
                    }).ToList();
        }
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from p in productCategories
                    select new ProductCategoryDto
                    {
                        Name = p.Name,
                        IconCSS = p.IconCSS,
                        Id = p.Id,
                    }).ToList();
        }
        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                CategoryId = product.CategoryId,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Id = product.Id,
                Price = product.Price,
                CategoryName = product.ProductCategory.Name,
                Quantity=product.Quantity
            };
        }
        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                           IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescripttion = product.Description,
                        ProductImageUrl = product.ImageUrl,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Quantity = cartItem.Quantity,
                        TotalPrice = product.Price * cartItem.Quantity
                    }).ToList();
        }
        public static CartItemDto ConvertToDto(this CartItem cartItem,
                                                  Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescripttion = product.Description,
                ProductImageUrl = product.ImageUrl,
                Price = product.Price,
                CartId = cartItem.CartId,
                Quantity = cartItem.Quantity,
                TotalPrice = product.Price * cartItem.Quantity
            };
        }
    }
}
