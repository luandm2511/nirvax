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
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<bool> CheckCategoryAsync(Category category);
    }
}
