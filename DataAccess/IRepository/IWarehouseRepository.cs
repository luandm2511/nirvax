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
    public interface IWarehouseRepository
    {
        Task<int> ViewCountImportStatisticsAsync(int warehouseId);
        Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId);
        Task<double> ViewPriceByImportStatisticsAsync(int importId, int ownerId);
        Task<int> QuantityWarehouseStatisticsAsync(int ownerId);
        Task<List<ImportProduct>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize);
        Task<List<WarehouseDetail>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize);
        Task<Warehouse> GetWarehouseByIdAsync(int ownerId);

        Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId);
        Task<bool> CheckWarehouseAsync(WarehouseDTO warehouseDTO);
        Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int ownerId);
        Task<bool> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO);

        Task<bool> UpdateWarehouseAsync(WarehouseDTO warehouseDTO);


    }
}
