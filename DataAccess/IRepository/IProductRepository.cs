using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByBrandAsync(int brandId);
        Task<IEnumerable<Product>> SearchAsync(string productName, float? minPrice, float? maxPrice, int? categoryId, int? brandId, int? ownerId);
        Task<bool> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<bool> BanProductAsync(Product product);
        Task<bool> UnbanProductAsync(Product product);
        Task<bool> CheckProductAsync(Product product);
        Task<bool> AddRatingAsync(Product product, int rating);
        Task<IEnumerable<Product>> GetTopSellingProductsAsync();
        Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId);
        Task<ProductSize> GetByIdAsync(string id);

    }
}
