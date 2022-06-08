using ShopOnline.api.Entities;

namespace ShopOnline.api.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<ProductCategory>> GetProductCategories();
        Task<Product> GetProduct(int id);
        Task<ProductCategory> GetProductCategory(int id);
        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
    }
}
