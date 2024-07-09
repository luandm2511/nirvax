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
    public class CateParentRepository : ICateParentRepository
    {
        private readonly CategoryParentDAO _dao;
        public CateParentRepository(CategoryParentDAO dao)
        {
            _dao = dao;
        }

        public async Task<bool> CheckCategoryParentAsync(CategoryParent category)
        {
            return await _dao.CheckCategoryParentAsync(category);
        }

        public async Task CreateCategoryParentAsync(CategoryParent category)
        {
             await _dao.CreateCategoryParentAsync(category);
        }

        public async Task DeleteCategoryParentAsync(CategoryParent categoryParent)
        {
             await _dao.DeleteCategoryParentAsync(categoryParent);
        }

        public async Task<IEnumerable<CategoryParent>> GetAllCategoryParentAsync()
        {
            return await _dao.GetAllCategoryParentAsync();
        }

        public async Task<CategoryParent> GetCategoryParentByIdAsync(int id)
        {
            return await _dao.GetCategoryParentByIdAsync(id);
        }

        public async Task<IEnumerable<CategoryParent>> SearchCateParentsAsync(string keyword)
        {
            return await _dao.SearchCateParentsAsync(keyword);
        }

        public async Task UpdateCategoryParentAsync(CategoryParent category)
        {
            await _dao.UpdateCategoryParentAsync(category);
        }
    }
}
