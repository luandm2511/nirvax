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


        public Task<int> SumOfKindProdSizeStatistics(int warehouseId)
        {
            return _warehouseDetailDAO.SumOfKindProdSizeStatistics(warehouseId);

        }


        public Task<List<WarehouseDetailFinalDTO>> GetAllWarehouseDetailByProductSize(int warehouseId, int page, int pageSize)
        {

            return _warehouseDetailDAO.GetAllWarehouseDetailByProductSize(warehouseId, page, pageSize);
        }

        public Task<bool> CheckWarehouseDetailExist(int warehouseId)
        {
            return _warehouseDetailDAO.CheckWarehouseDetailExist(warehouseId);
        }
        public Task<bool> CreateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            return _warehouseDetailDAO.CreateWarehouseDetail(warehouseDetailDTO);
        }

        public Task<bool> PatchWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            return _warehouseDetailDAO.PatchWarehouseDetail(warehouseDetailDTO);
        }

        public Task<bool> UpdateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            return _warehouseDetailDAO.UpdateWarehouseDetail(warehouseDetailDTO);
        }

    
      
     

    }
}
