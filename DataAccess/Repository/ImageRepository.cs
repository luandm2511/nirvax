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
    public class ImageRepository : IImageRepository
    {
        private readonly ImageDAO _img;
        public ImageRepository(ImageDAO img)
        {
            _img = img;
        }
        public async Task AddImagesAsync(Image image)
        {
            await _img.AddImagesAsync(image);
        }

        public async Task DeleteImagesAsync(Image image)
        {
            await _img.DeleteImagesAsync(image);
        }

        public async Task<IEnumerable<Image>> GetBySizeChartAsync(int sizeChartId) => await _img.GetBySizeChartAsync(sizeChartId);

        public async Task<Image> GetByIdAsync(int id) => await _img.GetByIdAsync(id);

        public async Task<IEnumerable<Image>> GetByProductAsync(int productId) => await _img.GetByProductAsync(productId);
    }
}
