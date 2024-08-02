using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Repository.StaffRepository;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class WarehouseDetailRepository : IWarehouseDetailRepository
    {
       
        private readonly WarehouseDetailDAO _warehouseDetailDAO;
        public WarehouseDetailRepository(WarehouseDetailDAO warehouseDetailDAO)
        {
            _warehouseDetailDAO = warehouseDetailDAO;
        }


        public Task<int> SumOfKindProdSizeStatisticsAsync(int warehouseId)
        {
            return _warehouseDetailDAO.SumOfKindProdSizeStatisticsAsync(warehouseId);

        }

        public Task<bool> CreateWarehouseDetailAsync(int warehouseId, List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {
            return _warehouseDetailDAO.CreateWarehouseDetailAsync(warehouseId, importProductDetailDTO);
        }

        public Task<bool> PatchWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
            return _warehouseDetailDAO.PatchWarehouseDetailAsync(warehouseDetailDTO);
        }

        public Task<bool> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
            return _warehouseDetailDAO.UpdateWarehouseDetailAsync(warehouseDetailDTO);
        }

    }
}
