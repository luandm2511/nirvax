using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IImageRepository
    {
        Task<Image> GetByIdAsync(int id);
        Task<IEnumerable<Image>> GetByProductAsync(int productId);
        Task<IEnumerable<Image>> GetByDescriptionAsync(int desId);
        Task<bool> AddImagesAsync(Image image);
        Task<bool> DeleteImagesAsync(Image image);
    }
}
