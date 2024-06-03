using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class ProductDAO 
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => !p.Isdelete).ToListAsync();
        }

        public static async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images)
                    .Include(p => p.Description)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public static async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.OwnerId == ownerId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public static async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .Where(p => p.CategoryId == categoryId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public static async Task<IEnumerable<Product>> GetByBrandAsync(int brandId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.BrandId == brandId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public static async Task<IEnumerable<Product>> SearchAsync(string productName, float? minPrice, float? maxPrice, int? categoryId, int? brandId, int? ownerId)
        {
            var query = _context.Products.Include(p => p.Images).Include(p => p.Description)
                .Include(p => p.Category).Include(p => p.Brand)
                .Include(p => p.Owner).AsQueryable();

            if (!string.IsNullOrEmpty(productName))
                query = query.Where(p => p.Name.Contains(productName));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (brandId.HasValue)
                query = query.Where(p => p.BrandId == brandId.Value);

            if (ownerId.HasValue)
                query = query.Where(p => p.OwnerId == ownerId.Value);

            return await query.ToListAsync();
        }

        public static async Task<bool> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId)
        {
            return await _context.Products
                .OrderByDescending(p => p.QuantitySold)
                .Take(5)
                .Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan && p.OwnerId == ownerId)
                .ToListAsync();
        }
        public static async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan)
                .ToListAsync();
        }

        public static async Task<bool> CheckProductAsync(Product product)
        {
            if (await _context.Products
                    .AnyAsync(p => p.Name == product.Name && p.BrandId != product.BrandId
                    && p.OwnerId == product.OwnerId && !p.Isdelete)) return false;
            return true;
        }

        public static async Task<ProductSize> GetByIdAsync(string id)
        {
            try
            {
                return await _context.ProductSizes.Include(p => p.Product)
                    .Include(p => p.Size)
                    .FirstOrDefaultAsync(p => p.ProductSizeId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with ID {id}.", ex);
            }
        }
    }

}
