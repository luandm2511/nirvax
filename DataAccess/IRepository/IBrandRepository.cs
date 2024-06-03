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
        Task<IEnumerable<Brand>> GetAllBrandAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task<IEnumerable<Brand>> GetBrandsByCategoryAsync(int cate_id);
        Task<bool> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(Brand brand);
        Task<bool> CheckBrandAsync(Brand brand);
    }
}
