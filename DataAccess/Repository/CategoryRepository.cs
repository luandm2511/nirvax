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
        public async Task<IEnumerable<Category>> GetAllCategoryAsync() => await _categoryDAO.GetAllCategoryAsync();
        public async Task<Category> GetCategoryByIdAsync(int id) => await _categoryDAO.GetCategoryByIdAsync(id);
        public async Task CreateCategoryAsync(Category category)
        {
            category.Isdelete = false;
            await _categoryDAO.CreateCategoryAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            await _categoryDAO.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            category.Isdelete = true;
            await _categoryDAO.UpdateCategoryAsync(category);
        }

        public async Task<bool> CheckCategoryAsync(Category category) => await _categoryDAO.CheckCategoryAsync(category);

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string keyword)
        {
            return await _categoryDAO.SearchCategoriesAsync(keyword);
        }
    }
}
