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
    public class CategoryDAO
    {
        private readonly NirvaxContext _context;

        public CategoryDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.Include(c => c.CateParent).Where(c => !c.Isdelete).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.CateParent).FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckCategoryAsync(Category category)
        {
            return !await _context.Categories.AnyAsync(c => c.Name == category.Name && c.CategoryId != category.CategoryId && !c.Isdelete);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string keyword)
        {
            return await _context.Categories
                                 .Where(b => !b.Isdelete && b.Name.Contains(keyword))
                                 .ToListAsync();
        }
    }

}
