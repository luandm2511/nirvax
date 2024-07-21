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

        public async Task<IEnumerable<Product>> GetProductsInHomeAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).AsNoTracking().Where(p => !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images)
                    .Include(p => p.Description)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.Comments)
                    .Include(p => p.Owner)
                    .Include(p => p.ProductSizes)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByOwnerAsync(int ownerId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).AsNoTracking().Where(p => p.OwnerId == ownerId && !p.Isdelete && !p.Isban && !p.Owner.IsBan).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetByOwnerInDashboardAsync(int ownerId)
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Description)
                    .Include(p => p.Category).Include(p => p.Brand)
                    .Include(p => p.Owner).AsNoTracking().Where(p => p.OwnerId == ownerId && !p.Isdelete).ToListAsync();
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

        public async Task<IEnumerable<Product>> SearchProductsInAdminAsync(string? searchTerm)
        {
            var productsQuery = _context.Products
                .Include(p => p.Images).Include(p => p.Description)
                .Include(p => p.Category).Include(p => p.Brand)
                .Include(p => p.Owner)
                .Where(p => !p.Isdelete);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm));
            }

            var products = await productsQuery.ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> SearchProductsInOwnerAsync(string? searchTerm, int ownerId)
        {
            var productsQuery = _context.Products
                .Include(p => p.Images).Include(p => p.Description)
                .Include(p => p.Category).Include(p => p.Brand)
                .Include(p => p.Owner)
                .Where(p => !p.Isdelete && p.OwnerId == ownerId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm));
            }

            var products = await productsQuery.ToListAsync();
            return products;
        }

        public async Task<(IEnumerable<Product> Products, List<Owner> Owners)> SearchProductsAndOwnersAsync(string? searchTerm, double? minPrice, double? maxPrice, List<int> categoryIds, List<int> brandIds, List<int> sizeIds)
        {
            var productsQuery = _context.Products
                .Include(p => p.Owner)
                .Where(p => !p.Isdelete && !p.Isban);

            List<Owner> owners = new List<Owner>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm));
                owners = await _context.Owners
                    .Where(o => !o.IsBan && o.Fullname.Contains(searchTerm))
                    .ToListAsync();
            }

            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }

            if (categoryIds != null && categoryIds.Count > 0)
            {
                productsQuery = productsQuery.Where(p => categoryIds.Contains(p.CategoryId));
            }

            if (brandIds != null && brandIds.Count > 0)
            {
                productsQuery = productsQuery.Where(p => brandIds.Contains(p.BrandId.Value));
            }

            if (sizeIds != null && sizeIds.Count > 0)
            {
                productsQuery = productsQuery.Where(p => p.ProductSizes.Any(ps => sizeIds.Contains(ps.SizeId)));
            }

            var products = await productsQuery.ToListAsync();

            return (products, owners);
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
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
