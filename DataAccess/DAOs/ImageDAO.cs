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
            try
            {
                return await _context.Images.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with ID {id}.", ex);
            }

        }

        public static async Task<IEnumerable<Image>> GetByProductAsync(int productId)
        {
            try
            {
                return await _context.Images.Include(i => i.Product)
                    .Where(i => i.ProductId == productId && !i.Isdelete).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the image with product {productId}.", ex);
            }
        }

        public static async Task<IEnumerable<Image>> GetByDescriptionAsync(int desId)
        {
            try
            {
                return await _context.Images.Include(i => i.Description)
                    .Where(i => i.DescriptionId == desId && i.Isdelete).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the image with description {desId}.", ex);
            }
        }

        public static async Task<bool> AddImagesAsync(Image image)
        {
            try
            {
                await _context.Images.AddAsync(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the image.", ex);
            }
        }

        public static async Task<bool> UpdateImagesAsync(List<Image> images)
        {
            try
            {
                _context.Images.UpdateRange(images);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the image.", ex);
            }
        }

        public static async Task<bool> DeleteImagesAsync(Image image)
        {
            try
            {
                image.Isdelete = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the image.", ex);
            }
        }


    }
}
