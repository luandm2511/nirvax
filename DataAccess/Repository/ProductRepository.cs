using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _pro;
        public ProductRepository(ProductDAO pro)
        {
            _pro = pro;
        }

        public async Task AddRatingAsync(Product product, int rating) 
        {
            product.RateCount++;
            product.RatePoint = (product.RatePoint * (product.RateCount - 1) + rating) / product.RateCount;
            await _pro.UpdateAsync(product);
        }

        public async Task BanProductAsync(Product product) 
        { 
            product.Isban = true;
            await _pro.UpdateAsync(product);
        } 

        public async Task<bool> CheckProductAsync(Product product) => await _pro.CheckProductAsync(product);

        public async Task CreateAsync(Product product)
        {
            product.RatePoint = 0;
            product.RateCount = 0;
            product.QuantitySold = 0;
            product.Isban = false;
            product.Isdelete = false;
            await _pro.CreateAsync(product);
        }

        public async Task DeleteAsync(Product product)
        {
            product.Isdelete = true;
            await _pro.UpdateAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _pro.GetAllAsync();

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId) => await _pro.GetByBrandAsync(brandId);

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => await _pro.GetByCategoryAsync(categoryId);

        public async Task<Product> GetByIdAsync(int id) => await _pro.GetByIdAsync(id);

        public async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId) => await _pro.GetByOwnerAsync(ownerId);

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync() => await _pro.GetTopSellingProductsAsync();
        public async Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId) => await _pro.GetTopSellingProductsByOwnerAsync(ownerId);
        public async Task UpdateAsync(Product product)
        {
            await _pro.UpdateAsync(product);
        }

        public async Task UnbanProductAsync(Product product)
        {
            product.Isban = false;
            await _pro.UpdateAsync(product);
        }

        public async Task<(List<Product> Products, List<Owner> Owners)> SearchProductsAndOwnersAsync(string? searchTerm, double? minPrice, double? maxPrice, List<int> categoryIds, List<int> brandIds, List<string> size)
        {
            return ((List<Product> Products, List<Owner> Owners))await _pro.SearchProductsAndOwnersAsync(searchTerm, minPrice, maxPrice, categoryIds, brandIds, size);
        }

        public async Task<IEnumerable<Product>> GetProductsInHomeAsync()
        {
            return await _pro.GetProductsInHomeAsync();
        }

        public async Task<IEnumerable<Product>> GetByOwnerInDashboardAsync(int ownerId)
        {
            return await _pro.GetByOwnerInDashboardAsync(ownerId);
        }

        public async Task<IEnumerable<Product>> SearchProductsInAdminAsync(string? searchTerm)
        {
            return await _pro.SearchProductsInAdminAsync(searchTerm);
        }

        public async Task<IEnumerable<Product>> SearchProductsInOwnerAsync(string? searchTerm, int ownerId)
        {
            return await _pro.SearchProductsInOwnerAsync(searchTerm, ownerId);
        }
    }
}
