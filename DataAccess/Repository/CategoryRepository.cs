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
        private readonly CategoryDAO _categoryDAO;
        public CategoryRepository(CategoryDAO categoryDAO) 
        {
            _categoryDAO = categoryDAO;
        }
        public Task<IEnumerable<Category>> GetAllCategoryAsync() => _categoryDAO.GetAllCategoryAsync();
        public Task<Category> GetCategoryByIdAsync(int id) => _categoryDAO.GetCategoryByIdAsync(id);
        public Task<bool> CreateCategoryAsync(Category category)
        {
            category.Isdelete = false;
            return _categoryDAO.CreateCategoryAsync(category);
        }

        public Task<bool> UpdateAsync(Category category) => _categoryDAO.UpdateCategoryAsync(category);

        public Task<bool> DeleteCategoryAsync(Category category)
        {
            category.Isdelete = true;
            return _categoryDAO.UpdateCategoryAsync(category);
        }

        public Task<bool> CheckCategoryAsync(Category category) => _categoryDAO.CheckCategoryAsync(category);
    }
}
