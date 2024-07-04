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
        Task CreateCategoryParentAsync(CategoryParent category);
        Task UpdateCategoryParentAsync(CategoryParent category);
        Task DeleteCategoryParentAsync(CategoryParent categoryParent);
        Task<bool> CheckCategoryParentAsync(CategoryParent category);
        Task<IEnumerable<CategoryParent>> SearchCateParentsAsync(string keyword);
    }
}
