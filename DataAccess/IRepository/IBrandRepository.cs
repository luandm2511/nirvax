using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAllBrand();
        Brand GetBrandById(int id);
        IEnumerable<Brand> GetBrandsByCategory(int cate_id);
        bool CreateBrand(Brand brand);
        bool Update(Brand brand);
        bool DeleteBrand(Brand brand);
        bool CheckBrand(Brand brand);
    }
}
