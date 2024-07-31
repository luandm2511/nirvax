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
        Task<List<ImportProduct>> GetAllImportProductAsync(int warehouseId, DateTime? from, DateTime? to);
        
        Task<List<ImportProduct>> GetImportProductByWarehouseAsync(int warehouseId);
        Task<ImportProduct> GetImportProductByIdAsync(int importId);

        Task<int> ViewImportProductStatisticsAsync(int warehouseId);
        Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId);
        Task<double> ViewPriceByImportStatisticsAsync(int importId, int ownerId);
        Task<int> QuantityWarehouseStatisticsAsync(int ownerId);

        Task<bool> CheckImportProductExistAsync(int importId);

        Task<ImportProduct> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO);

        Task<bool> UpdateImportProductAsync(ImportProductDTO importProductDTO);
        Task<bool> UpdateQuantityAndPriceImportProductAsync(int importId);

    }
}
