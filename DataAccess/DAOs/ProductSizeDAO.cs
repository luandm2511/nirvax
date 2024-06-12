using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class ProductSizeDAO
    {
        private readonly NirvaxContext _context;

        public ProductSizeDAO(NirvaxContext context)
        {
            _context = context;
        }
        public async Task<ProductSize> GetByIdAsync(string id)
        {
            return await _context.ProductSizes.Include(p => p.Product)
                   .Include(p => p.Size)
                   .FirstOrDefaultAsync(p => p.ProductSizeId == id);
        }

        public async Task<bool> UpdateAsync(ProductSize productSize)
        {
            _context.ProductSizes.Update(productSize);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
