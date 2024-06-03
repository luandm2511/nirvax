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
        IEnumerable<Category> GetAllCategory();
        Category GetCategoryById(int id);
        bool CreateCategory(Category category);
        bool Update(Category category);
        bool DeleteCategory(Category category);
        bool CheckCategory(Category category);
    }
}
