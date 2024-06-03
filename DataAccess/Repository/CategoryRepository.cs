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
    public class CategoryRepository : ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategoryAsync() => CategoryDAO.GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id) => CategoryDAO.GetCategoryByIdAsync(id);
        public Task<bool> CreateCategoryAsync(Category category)
        {
            category.Isdelete = false;
            return CategoryDAO.CreateCategoryAsync(category);
        }

        public Task<bool> UpdateAsync(Category category) => CategoryDAO.UpdateCategoryAsync(category);

        public Task<bool> DeleteCategoryAsync(Category category)
        {
            category.Isdelete = true;
            return CategoryDAO.UpdateCategoryAsync(category);
        }

        public Task<bool> CheckCategoryAsync(Category category) => CategoryDAO.CheckCategoryAsync(category);
    }
}
