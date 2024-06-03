using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class BrandDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static IEnumerable<Brand> GetAllBrand()
        {
            try
            {
                return _context.Brands.Include(b => b.Category).Where(b => !b.Isdelete).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving brands.", ex);
            }
        }

        public static Brand GetBrandById(int id)
        {
            try
            {
                return _context.Brands.Include(b => b.Category).SingleOrDefault(b => b.BrandId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the brand with ID {id}.", ex);
            }
        }

        public static IEnumerable<Brand> GetBrandsByCategory(int cate_id)
        {
            try
            {
                return _context.Brands.Include(b => b.Category).Where(b => !b.Isdelete && b.CategoryId == cate_id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the brand with category {cate_id}.", ex);
            }
        }

        public static bool CreateBrand(Brand brand)
        {
            try
            {
                brand.Isdelete = false;
                _context.Brands.Add(brand);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the brand.", ex);
            }
        }

        public static bool UpdateBrand(Brand brand)
        {
            try
            {
                _context.Entry<Brand>(brand).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the brand.", ex);
            }
        }

        public static bool DeleteBrand(Brand brand)
        {
            try
            {
                if (brand != null)
                {
                    brand.Isdelete = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category with ID {brand.BrandId}.", ex);
            }
        }

        public static bool CheckBrand(Brand brand)
        {
            try
            {

                if (_context.Brands.Any(b => b.Name == brand.Name && b.BrandId != brand.BrandId && !b.Isdelete))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while checking the brand with ID {brand.BrandId}.", ex);
            }
        }
    }
}
