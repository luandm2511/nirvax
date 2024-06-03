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
        public IEnumerable<Brand> GetAllBrand() => BrandDAO.GetAllBrand();
        public Brand GetBrandById(int id) => BrandDAO.GetBrandById(id);
        public IEnumerable<Brand> GetBrandsByCategory(int cate_id) => BrandDAO.GetBrandsByCategory(cate_id);

        public bool CreateBrand(Brand brand) => BrandDAO.CreateBrand(brand);

        public bool Update(Brand brand) => BrandDAO.UpdateBrand(brand);

        public bool DeleteBrand(Brand brand) => BrandDAO.DeleteBrand(brand);

        public bool CheckBrand(Brand brand) => BrandDAO.CheckBrand(brand);
    }
}
