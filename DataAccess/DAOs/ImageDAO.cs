using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class ImageDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();
        public static async Task<Image> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public static async Task<IEnumerable<Image>> GetByProductAsync(int productId)
        {
            return await _context.Images.Include(i => i.Product)
                .Where(i => i.ProductId == productId && !i.Isdelete).ToListAsync();
        }

        public static async Task<IEnumerable<Image>> GetByDescriptionAsync(int desId)
        {
            return await _context.Images.Include(i => i.Description)
                .Where(i => i.DescriptionId == desId && i.Isdelete).ToListAsync();
        }

        public static async Task<bool> AddImagesAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> UpdateImagesAsync(List<Image> images)
        {
            _context.Images.UpdateRange(images);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> DeleteImagesAsync(Image image)
        {
            image.Isdelete = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
