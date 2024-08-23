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
        Task<List<ImportProduct>> GetAllImportProductAsync(int ownerId, DateTime? from, DateTime? to);
        
        Task<ImportProduct> GetImportProductByIdAsync(int importId);

        Task<object> ViewImportProductStatisticsAsync(int ownerId);

        Task<bool> CheckImportProductExistAsync(int importId);

        Task<ImportProduct> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO);

        Task<bool> UpdateImportProductAsync(ImportProductDTO importProductDTO);
        Task<bool> UpdateQuantityAndPriceImportProductAsync(int importId);

        Task<List<object>> ViewWeeklyImportProductAsync(int ownerId);


    }
}
