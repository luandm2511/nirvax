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
    public interface IWarehouseDetailRepository

    {
        Task<bool> CheckWarehouseDetailExistAsync(int warehouseId);

    
        Task<List<WarehouseDetailFinalDTO>> GetAllWarehouseDetailByProductSizeAsync(int warehouseId, int page, int pageSize);

        Task<int> SumOfKindProdSizeStatisticsAsync(int warehouseId);

        Task<bool> CreateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO);

        Task<bool> PatchWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO);
        Task<bool> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO);


    }
}
