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
        public Task<bool> AddRatingAsync(Product product, int rating) => ProductDAO.AddRatingAsync(product, rating);

        public Task<bool> BanProductAsync(Product product) => ProductDAO.BanProductAsync(product);

        public Task<bool> CheckProductAsync(Product product) => ProductDAO.CheckProductAsync(product);

        public Task<bool> CreateAsync(Product product) => ProductDAO.CreateAsync(product);

        public Task<bool> DeleteAsync(Product product) => ProductDAO.DeleteAsync(product);

        public Task<IEnumerable<Product>> GetAllAsync() => ProductDAO.GetAllAsync();

        public Task<IEnumerable<Product>> GetByBrandAsync(int brandId) => ProductDAO.GetByBrandAsync(brandId);

        public Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => ProductDAO.GetByCategoryAsync(categoryId);

        public Task<Product> GetByIdAsync(int id) => ProductDAO.GetByIdAsync(id);

        public Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId) => ProductDAO.GetByOwnerAsync(ownerId);

        public Task<IEnumerable<Product>> GetTopSellingProductsAsync(int top) => ProductDAO.GetTopSellingProductsAsync(top);

        public Task<IEnumerable<Product>> SearchAsync(string productName, float? minPrice, float? maxPrice, int? categoryId, int? brandId, int? ownerId)
        {
            return ProductDAO.SearchAsync(productName, minPrice, maxPrice, categoryId, brandId, ownerId);
        }
        public Task<bool> UpdateAsync(Product product) => ProductDAO.UpdateAsync(product);
        public Task<ProductSize> GetByIdAsync(string id) => ProductDAO.GetByIdAsync(id);
    }
}
