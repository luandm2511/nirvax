using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteCategoryAsync(Category category);
        Task<bool> CheckCategoryAsync(Category category);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string keyword);
    }
}
