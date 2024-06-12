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
        private readonly BrandDAO _brandDAO;
        public BrandRepository(BrandDAO brandDAO)
        {
            _brandDAO = brandDAO;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandAsync() => await _brandDAO.GetAllBrandAsync();
        public async Task<Brand> GetBrandByIdAsync(int id) => await _brandDAO.GetBrandByIdAsync(id);
        public async Task<IEnumerable<Brand>> GetBrandsByCategoryAsync(int cate_id) => await _brandDAO.GetBrandsByCategoryAsync(cate_id);
        public async Task<bool> CreateBrandAsync(Brand brand) 
        {
            brand.Isdelete = false;
            return await _brandDAO.CreateBrandAsync(brand);
        }
        public async Task<bool> UpdateBrandAsync(Brand brand) => await _brandDAO.UpdateBrandAsync(brand);
        public async Task<bool> DeleteBrandAsync(Brand brand)
        {
            brand.Isdelete = true;
            return await _brandDAO.UpdateBrandAsync(brand);
        }
        public async Task<bool> CheckBrandAsync(Brand brand) => await _brandDAO.CheckBrandAsync(brand);
    }
}
