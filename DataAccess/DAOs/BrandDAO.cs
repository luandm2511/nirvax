using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class BrandDAO
    {
        private readonly NirvaxContext _context;

        public BrandDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brands
                .Include(b => b.Category)
                .Where(b => !b.Isdelete)
                .ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brands
                .Include(b => b.Category)
                .SingleOrDefaultAsync(b => b.BrandId == id);
        }

        public async Task<IEnumerable<Brand>> GetBrandsByCategoryAsync(int cate_id)
        {
            return await _context.Brands
                .Include(b => b.Category)
                .Where(b => !b.Isdelete && b.CategoryId == cate_id)
                .ToListAsync();
        }

        public async Task<bool> CreateBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckBrandAsync(Brand brand)
        {
            return !await _context.Brands.AnyAsync(b => b.Name == brand.Name && b.BrandId != brand.BrandId && !b.Isdelete);
        }
    }
}
