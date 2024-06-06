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


        public Task<List<ImportProduct>> GetWarehouseByImportProduct(int ownerId, int page, int pageSize)
        {
            return _warehouseDAO.GetWarehouseByImportProduct(ownerId, page, pageSize);
        }
        public Task<List<WarehouseDetail>> GetAllWarehouseDetail(int ownerId, int page, int pageSize)
        {
            return _warehouseDAO.GetAllWarehouseDetail(ownerId, page, pageSize);
        }


        public Task<bool> CheckWarehouse(WarehouseDTO warehouseDTO)
        {
            return _warehouseDAO.CheckWarehouse(warehouseDTO);
        }

        public Task<bool> CreateWarehouse(WarehouseDTO warehouseDTO)
        {
            return _warehouseDAO.CreateWarehouse(warehouseDTO);
        }
        public Task<Warehouse> GetWarehouseById(int ownerId)
        {
            return _warehouseDAO.GetWarehouseById(ownerId);
        }
        public Task<bool> UpdateWarehouse(WarehouseDTO warehouseDTO)
        {
            return _warehouseDAO.UpdateWarehouse(warehouseDTO);
        }
        public Task<int> GetWarehouseIdByOwnerId(int ownerId)
        {
            return _warehouseDAO.GetWarehouseIdByOwnerId(ownerId);
        }

        public Task<Warehouse> UpdateQuantityAndPriceWarehouse(int ownerId)
        {
            return _warehouseDAO.UpdateQuantityAndPriceWarehouse(ownerId);
        }

        public Task<int> ViewCountImportStatistics(int warehouseId)
        {
            return _warehouseDAO.ViewCountImportStatistics(warehouseId);
        }
        public Task<int> ViewNumberOfProductByImportStatistics(int importId, int ownerId)
        {
            return _warehouseDAO.ViewNumberOfProductByImportStatistics(importId,ownerId);
        }
        public Task<double> ViewPriceByImportStatistics(int importId, int ownerId)
        {
            return _warehouseDAO.ViewPriceByImportStatistics(importId,ownerId);
        }
        public Task<int> QuantityWarehouseStatistics(int ownerId)
        {
            return _warehouseDAO.QuantityWarehouseStatistics(ownerId);
        }

    }
}
