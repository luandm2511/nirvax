using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class CategoryDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.Where(c => !c.Isdelete).ToListAsync();
        }

        public static async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public static async Task<bool> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> CheckCategoryAsync(Category category)
        {
            return !await _context.Categories.AnyAsync(c => c.Name == category.Name && c.CategoryId != category.CategoryId && !c.Isdelete);
        }
    }

}
