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
        public async Task CreateBrandAsync(Brand brand) 
        {
            brand.Isdelete = false;
            await _brandDAO.CreateBrandAsync(brand);
        }
        public async Task UpdateBrandAsync(Brand brand)
        {
            await _brandDAO.UpdateBrandAsync(brand);
        }
        public async Task DeleteBrandAsync(Brand brand)
        {
            brand.Isdelete = true;
            await _brandDAO.UpdateBrandAsync(brand);
        }
        public async Task<bool> CheckBrandAsync(Brand brand) => await _brandDAO.CheckBrandAsync(brand);

        public async Task<IEnumerable<Brand>> SearchBrandsAsync(string keyword)
        {
            return await _brandDAO.SearchBrandsAsync(keyword);
        }
    }
}
