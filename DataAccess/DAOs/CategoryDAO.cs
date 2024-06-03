using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class CategoryDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static IEnumerable<Category> GetAllCategory()
        {
            try
            {
                return _context.Categories.Where(c => !c.Isdelete).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public static Category GetCategoryById(int id)
        {
            try
            {
                return _context.Categories.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category with ID {id}.", ex);
            }
        }

        public static bool CreateCategory(Category category)
        {
            try
            {
                category.Isdelete = false;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }
        }

        public static bool UpdateCategory(Category category)
        {
            try
            {
                _context.Entry<Category>(category).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public static bool DeleteCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    category.Isdelete = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category with ID {category.CategoryId}.", ex);
            }
        }

        public static bool CheckCategory(Category category)
        {
            try
            {

                if (_context.Categories.Any(c => c.Name == category.Name && c.CategoryId != category.CategoryId && !c.Isdelete))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while check the category with ID {category.CategoryId}.", ex);
            }
        }
    }

}
