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

        public async Task<bool> CreateCategoryParentAsync(CategoryParent category)
        {
            return await _dao.CreateCategoryParentAsync(category);
        }

        public async Task<IEnumerable<CategoryParent>> GetAllCategoryParentAsync()
        {
            return await _dao.GetAllCategoryParentAsync();
        }

        public async Task<CategoryParent> GetCategoryParentByIdAsync(int id)
        {
            return await _dao.GetCategoryParentByIdAsync(id);
        }

        public async Task<bool> UpdateCategoryParentAsync(CategoryParent category)
        {
            return await _dao.UpdateCategoryParentAsync(category);
        }
    }
}
