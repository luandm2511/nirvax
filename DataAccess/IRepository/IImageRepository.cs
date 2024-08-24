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
        Task<IEnumerable<Image>> GetBySizeChartAsync(int sizeChartId);
        Task AddImagesAsync(Image image);
        Task DeleteImagesAsync(Image image);
    }
}
