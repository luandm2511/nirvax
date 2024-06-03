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
        public IEnumerable<Category> GetAllCategory() => CategoryDAO.GetAllCategory();
        public Category GetCategoryById(int id) => CategoryDAO.GetCategoryById(id);

        public bool CreateCategory(Category category) => CategoryDAO.CreateCategory(category);

        public bool Update(Category category) => CategoryDAO.UpdateCategory(category);

        public bool DeleteCategory(Category category) => CategoryDAO.DeleteCategory(category);

        public bool CheckCategory(Category category) => CategoryDAO.CheckCategory(category);
    }
}
