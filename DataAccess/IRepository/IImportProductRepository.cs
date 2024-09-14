using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IImportProductRepository
    {
        Task<IEnumerable<ImportProduct>> GetAllImportProductAsync(int ownerId, DateTime? from, DateTime? to);
        Task<ImportProduct> GetImportProductByIdAsync(int importId);
        Task<object> ViewImportProductStatisticsAsync(int ownerId);
        Task<ImportProduct> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO);
        Task<bool> UpdateImportProductAsync(ImportProductDTO importProductDTO);
        Task<bool> UpdateQuantityAndPriceImportProductAsync(int importId);
        Task<IEnumerable<object>> ViewWeeklyImportProductAsync(int ownerId);

    }
}
