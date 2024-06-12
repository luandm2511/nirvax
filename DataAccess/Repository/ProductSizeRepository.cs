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
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly ProductSizeDAO _pro;
        public ProductSizeRepository(ProductSizeDAO pro)
        {
            _pro = pro;
        }
        public Task<ProductSize> GetByIdAsync(string id)
        {
            return _pro.GetByIdAsync(id);
        }

        public Task<bool> UpdateAsync(ProductSize productSize)
        {
            return _pro.UpdateAsync(productSize);
        }
    }
}
