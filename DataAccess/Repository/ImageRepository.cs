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
        public async Task<bool> AddImagesAsync(Image image) => await ImageDAO.AddImagesAsync(image);

        public async Task<bool> DeleteImagesAsync(Image image) => await ImageDAO.DeleteImagesAsync(image);

        public async Task<IEnumerable<Image>> GetByDescriptionAsync(int desId) => await ImageDAO.GetByDescriptionAsync(desId);

        public async Task<Image> GetByIdAsync(int id) => await ImageDAO.GetByIdAsync(id);

        public async Task<IEnumerable<Image>> GetByProductAsync(int productId) => await ImageDAO.GetByProductAsync(productId);
    }
}
