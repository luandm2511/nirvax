using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class CategoryParentDAO
    {
        private readonly NirvaxContext _context;

        public CategoryParentDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryParent>> GetAllCategoryParentAsync()
        {
            return await _context.CategoryParents.Where(c => !c.Isdelete).ToListAsync();
        }

        public async Task<CategoryParent> GetCategoryParentByIdAsync(int id)
        {
            return await _context.CategoryParents.Include(c => c.Categories).FirstOrDefaultAsync(c => c.CateParentId == id);
        }

        public async Task<bool> CreateCategoryParentAsync(CategoryParent category)
        {
            await _context.CategoryParents.AddAsync(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryParentAsync(CategoryParent category)
        {
            _context.CategoryParents.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckCategoryParentAsync(CategoryParent category)
        {
            return !await _context.CategoryParents.AnyAsync(c => c.Name == category.Name && c.CateParentId != category.CateParentId && !c.Isdelete);
        }
    }
}
