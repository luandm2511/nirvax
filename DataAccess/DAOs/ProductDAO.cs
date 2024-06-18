using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class ProductDAO 
    {
        private readonly NirvaxContext _context;

        public ProductDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).AsNoTracking().Where(p => !p.Isdelete).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images)
                    .Include(p => p.Description)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .Include(p => p.ProductSizes)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.OwnerId == ownerId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner)
                    .Where(p => p.CategoryId == categoryId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.BrandId == brandId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string? searchTerm, double? minPrice = null, double? maxPrice = null, int? categoryId = null, int? brandId = null, int? sizeId = null)
        {
            var query = _context.Products.Include(p => p.ProductSizes)
                .Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).Where(p => p.BrandId == brandId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.ShortDescription.Contains(searchTerm));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (sizeId.HasValue)
            {
                query = query.Where(p => p.ProductSizes.Any(ps => ps.SizeId == sizeId.Value && ps.Quantity > 0) && !p.Isdelete);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetTopSellingProductsByOwnerAsync(int ownerId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                .Include(p => p.Category).Include(p => p.Brand)
                .Include(p => p.Owner)
                .OrderByDescending(p => p.QuantitySold)
                .Take(5)
                .Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan && p.OwnerId == ownerId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Images).Include(p => p.Description)
                .Include(p => p.Category).Include(p => p.Brand)
                .Include(p => p.Owner)
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan)
                .ToListAsync();
        }

        public async Task<bool> CheckProductAsync(Product product)
        {
            if (await _context.Products
                    .AnyAsync(p => p.Name == product.Name && p.BrandId != product.BrandId
                    && p.OwnerId == product.OwnerId && !p.Isdelete)) return false;
            return true;
        }
    }

}
