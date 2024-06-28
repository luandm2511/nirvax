using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface ICateParentRepository
    {
        Task<IEnumerable<CategoryParent>> GetAllCategoryParentAsync();
        Task<CategoryParent> GetCategoryParentByIdAsync(int id);
        Task<bool> CreateCategoryParentAsync(CategoryParent category);
        Task<bool> UpdateCategoryParentAsync(CategoryParent category);
        Task<bool> CheckCategoryParentAsync(CategoryParent category);
    }
}
