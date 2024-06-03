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
        public Task<bool> AddImagesAsync(Image image) => ImageDAO.AddImagesAsync(image);

        public Task<bool> DeleteImagesAsync(Image image) => ImageDAO.DeleteImagesAsync(image);

        public Task<IEnumerable<Image>> GetByDescriptionAsync(int desId) => ImageDAO.GetByDescriptionAsync(desId);

        public Task<Image> GetByIdAsync(int id) => ImageDAO.GetByIdAsync(id);

        public Task<IEnumerable<Image>> GetByProductAsync(int productId) => ImageDAO.GetByProductAsync(productId);
    }
}
