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
        Task<int> SumOfKindProdSizeStatisticsAsync(int warehouseId);

        Task<bool> CreateWarehouseDetailAsync(int warehouseId, List<ImportProductDetailCreateDTO> importProductDetailDTO);

        Task<bool> PatchWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO);
        Task<bool> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO);
    }
}
