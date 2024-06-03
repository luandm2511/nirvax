using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class BrandRepository : IBrandRepository
    {
        public async Task<IEnumerable<Brand>> GetAllBrandAsync() => await BrandDAO.GetAllBrandAsync();
        public async Task<Brand> GetBrandByIdAsync(int id) => await BrandDAO.GetBrandByIdAsync(id);
        public async Task<IEnumerable<Brand>> GetBrandsByCategoryAsync(int cate_id) => await BrandDAO.GetBrandsByCategoryAsync(cate_id);
        public async Task<bool> CreateBrandAsync(Brand brand) 
        {
            brand.Isdelete = false;
            return await BrandDAO.CreateBrandAsync(brand);
        }
        public async Task<bool> UpdateBrandAsync(Brand brand) => await BrandDAO.UpdateBrandAsync(brand);
        public async Task<bool> DeleteBrandAsync(Brand brand)
        {
            brand.Isdelete = true;
            return await BrandDAO.UpdateBrandAsync(brand);
        }
        public async Task<bool> CheckBrandAsync(Brand brand) => await BrandDAO.CheckBrandAsync(brand);
    }
}
