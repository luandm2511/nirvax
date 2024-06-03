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
            try
            {
                return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => !p.Isdelete).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving products.", ex);
            }
        }

        public static async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Products.Include(p => p.Images)
                    .Include(p => p.Description)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with ID {id}.", ex);
            }      
        }

        public static async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId)
        {
            try
            { 
                return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.OwnerId == ownerId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with owner {ownerId}.", ex);
            }
        }

        public static async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .Where(p => p.CategoryId == categoryId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with category {categoryId}.", ex);
            }
        }

        public static async Task<IEnumerable<Product>> GetByBrandAsync(int brandId)
        {
            try
            {
                return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.BrandId == brandId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with brand {brandId}.", ex);
            }
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
            try
            {
                product.RatePoint = 0;
                product.RateCount = 0;
                product.QuantitySold = 0;
                product.Isban = false;
                product.Isdelete = false;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when adding product.", ex);
            }
        }

        public static async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating the product with {product.ProductId}.", ex);
            }
        }

        public static async Task<bool> DeleteAsync(Product product)
        {
            try
            {
                product.Isdelete = true;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when deleting the product with {product.ProductId}.", ex);
            }
        }

        public static async Task<bool> BanProductAsync(Product product)
        {
            try
            {
                product.Isban = true;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when banning the product with {product.ProductId}.", ex);
            }
        }

        public static async Task<bool> AddRatingAsync(Product product, int rating)
        {
            try
            {
                product.RateCount++;
                product.RatePoint = (product.RatePoint * (product.RateCount - 1) + rating) / product.RateCount;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when rating the product with {product.ProductId}.", ex);
            }
        }

        public static async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int top)
        {
            try
            {
                return await _context.Products
                .OrderByDescending(p => p.QuantitySold)
                .Take(top)
                .Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when list top {top} products selling.", ex);
            }
        }

        public static async Task<bool> CheckProductAsync(Product product)
        {
            try
            {
                if (await _context.Products
                    .AnyAsync(p => p.Name == product.Name && p.BrandId != product.BrandId
                    && p.OwnerId == product.OwnerId && !p.Isdelete)) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while checking the brand with ID {product.ProductId}.", ex);
            }
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
