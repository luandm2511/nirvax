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
    public class WarehouseRepository : IWarehouseRepository
    {
       
        private readonly WarehouseDAO _warehouseDAO;
        public WarehouseRepository(WarehouseDAO warehouseDAO)
        {
            _warehouseDAO = warehouseDAO;
        }

        public Task<List<WarehouseDetail>> GetAllWarehouseDetailByWarehouseAsync(int warehouseId, int page, int pageSize)
        {
            return _warehouseDAO.GetAllWarehouseDetailByWarehouseAsync(warehouseId, page, pageSize);
        }

    
        public Task<Warehouse> GetWarehouseByOwnerIdAsync(int ownerId)
        {
            return _warehouseDAO.GetWarehouseByOwnerIdAsync(ownerId);
        }

        public Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId)
        {
            return _warehouseDAO.GetWarehouseIdByOwnerIdAsync(ownerId);
        }

        public Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int warehouseId)
        {
            return _warehouseDAO.UpdateQuantityAndPriceWarehouseAsync(warehouseId);
        }


    }
}
