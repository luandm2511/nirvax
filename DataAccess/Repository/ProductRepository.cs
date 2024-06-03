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
        public async Task<bool> AddRatingAsync(Product product, int rating) 
        {
            product.RateCount++;
            product.RatePoint = (product.RatePoint * (product.RateCount - 1) + rating) / product.RateCount;
            return await ProductDAO.UpdateAsync(product);
        }

        public async Task<bool> BanProductAsync(Product product) 
        { 
            product.Isban = true;
            return await ProductDAO.UpdateAsync(product);
        } 

        public async Task<bool> CheckProductAsync(Product product) => await ProductDAO.CheckProductAsync(product);

        public async Task<bool> CreateAsync(Product product)
        {
            product.RatePoint = 0;
            product.RateCount = 0;
            product.QuantitySold = 0;
            product.Isban = false;
            product.Isdelete = false;
            return await ProductDAO.CreateAsync(product);
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            product.Isdelete = true;
            return await ProductDAO.UpdateAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await ProductDAO.GetAllAsync();

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId) => await ProductDAO.GetByBrandAsync(brandId);

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => await ProductDAO.GetByCategoryAsync(categoryId);

        public async Task<Product> GetByIdAsync(int id) => await ProductDAO.GetByIdAsync(id);

        public async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId) => await ProductDAO.GetByOwnerAsync(ownerId);

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync() => await ProductDAO.GetTopSellingProductsAsync();
        public async Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId) => await ProductDAO.GetTopSellingProductsByOwnerAsync(ownerId);

        public async Task<IEnumerable<Product>> SearchAsync(string productName, float? minPrice, float? maxPrice, int? categoryId, int? brandId, int? ownerId)
        {
            return await ProductDAO.SearchAsync(productName, minPrice, maxPrice, categoryId, brandId, ownerId);
        }
        public async Task<bool> UpdateAsync(Product product) => await ProductDAO.UpdateAsync(product);
        public async Task<ProductSize> GetByIdAsync(string id) => await ProductDAO.GetByIdAsync(id);

        public async Task<bool> UnbanProductAsync(Product product)
        {
            product.Isban = false;
            return await ProductDAO.UpdateAsync(product);
        }
    }
}
