using Microsoft.EntityFrameworkCore;
using ShopOnline.api.Data;
using ShopOnline.api.Entities;
using ShopOnline.api.Repositories.Contracts;

namespace ShopOnline.api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ProductRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.Include(p=>p.ProductCategory)
                .SingleOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategories()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetProductCategory(int id)
        {
           return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Include(p=>p.ProductCategory).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
           return await _context.Products.Include(p => p.ProductCategory).Where(p=>p.CategoryId==categoryId).ToListAsync(); 
        }
    }
}
