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
        Task<bool> CheckWarehouseDetailExist(int warehouseId);

    
        Task<List<WarehouseDetailFinalDTO>> GetAllWarehouseDetailByProductSize(int warehouseId, int page, int pageSize);

        Task<int> SumOfKindProdSizeStatistics(int warehouseId);

        Task<bool> CreateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO);

        Task<bool> PatchWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO);
        Task<bool> UpdateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO);


    }
}
