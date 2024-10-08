﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetProductsInHomeAsync();
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId);
        Task<IEnumerable<Product>> GetByOwnerInDashboardAsync(int ownerId);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByBrandAsync(int brandId);
        Task<(List<Product> Products, List<Owner> Owners)> SearchProductsAndOwnersAsync(string? searchTerm, double? minPrice, double? maxPrice, List<int> categoryIds, List<int> brandIds, List<string> size);
        Task<IEnumerable<Product>> SearchProductsInAdminAsync(string? searchTerm);
        Task<IEnumerable<Product>> SearchProductsInOwnerAsync(string? searchTerm, int ownerId);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task BanProductAsync(Product product);
        Task UnbanProductAsync(Product product);
        Task<bool> CheckProductAsync(Product product);
        Task AddRatingAsync(Product product, double rating);
        Task<IEnumerable<TopProductDTO>> GetTopSellingProductsAsync();
        Task<IEnumerable<TopProductDTO>> GetTopSellingProductsByOwnerAsync(int ownerId);
        Task<OrderDetail> GetOrderDetailAsync(int orderId, string productSizeId);
    }
}
