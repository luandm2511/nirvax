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

        public async Task<bool> AddRatingAsync(Product product, int rating) 
        {
            product.RateCount++;
            product.RatePoint = (product.RatePoint * (product.RateCount - 1) + rating) / product.RateCount;
            return await _pro.UpdateAsync(product);
        }

        public async Task<bool> BanProductAsync(Product product) 
        { 
            product.Isban = true;
            return await _pro.UpdateAsync(product);
        } 

        public async Task<bool> CheckProductAsync(Product product) => await _pro.CheckProductAsync(product);

        public async Task<bool> CreateAsync(Product product)
        {
            product.RatePoint = 0;
            product.RateCount = 0;
            product.QuantitySold = 0;
            product.Isban = false;
            product.Isdelete = false;
            return await _pro.CreateAsync(product);
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            product.Isdelete = true;
            return await _pro.UpdateAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _pro.GetAllAsync();

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId) => await _pro.GetByBrandAsync(brandId);

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => await _pro.GetByCategoryAsync(categoryId);

        public async Task<Product> GetByIdAsync(int id) => await _pro.GetByIdAsync(id);

        public async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId) => await _pro.GetByOwnerAsync(ownerId);

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync() => await _pro.GetTopSellingProductsAsync();
        public async Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId) => await _pro.GetTopSellingProductsByOwnerAsync(ownerId);
        public async Task<bool> UpdateAsync(Product product) => await _pro.UpdateAsync(product);

        public async Task<bool> UnbanProductAsync(Product product)
        {
            product.Isban = false;
            return await _pro.UpdateAsync(product);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string? searchTerm, double? minPrice = null, double? maxPrice = null, int? categoryId = null, int? brandId = null, int? sizeId = null)
        {
            return await _pro.SearchProductsAsync(searchTerm, minPrice, maxPrice, categoryId, brandId, sizeId);
        }
    }
}
