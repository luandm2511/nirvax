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
        Task<int> ViewCountImportStatistics(int warehouseId);
        Task<int> ViewNumberOfProductByImportStatistics(int importId, int ownerId);
        Task<double> ViewPriceByImportStatistics(int importId, int ownerId);
        Task<int> QuantityWarehouseStatistics(int ownerId);
        Task<List<ImportProduct>> GetWarehouseByImportProduct(int ownerId, int page, int pageSize);
        Task<List<WarehouseDetail>> GetAllWarehouseDetail(int ownerId, int page, int pageSize);
        Task<Warehouse> GetWarehouseById(int ownerId);

        Task<int> GetWarehouseIdByOwnerId(int ownerId);
        Task<bool> CheckWarehouse(WarehouseDTO warehouseDTO);
        Task<Warehouse> UpdateQuantityAndPriceWarehouse(int ownerId);
        Task<bool> CreateWarehouse(WarehouseDTO warehouseDTO);

        Task<bool> UpdateWarehouse(WarehouseDTO warehouseDTO);


    }
}
