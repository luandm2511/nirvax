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


        public Task<List<ImportProduct>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize)
        {
            return _warehouseDAO.GetWarehouseByImportProductAsync(ownerId, page, pageSize);
        }
        public Task<List<WarehouseDetail>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize)
        {
            return _warehouseDAO.GetAllWarehouseDetailAsync(ownerId, page, pageSize);
        }


        public Task<bool> CheckWarehouseAsync(WarehouseDTO warehouseDTO)
        {
            return _warehouseDAO.CheckWarehouseAsync(warehouseDTO);
        }

        public Task<bool> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO)
        {
            return _warehouseDAO.CreateWarehouseAsync(warehouseCreateDTO);
        }
        public Task<Warehouse> GetWarehouseByIdAsync(int ownerId)
        {
            return _warehouseDAO.GetWarehouseByIdAsync(ownerId);
        }
        public Task<bool> UpdateWarehouseAsync(WarehouseDTO warehouseDTO)
        {
            return _warehouseDAO.UpdateWarehouseAsync(warehouseDTO);
        }
        public Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId)
        {
            return _warehouseDAO.GetWarehouseIdByOwnerIdAsync(ownerId);
        }

        public Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int ownerId)
        {
            return _warehouseDAO.UpdateQuantityAndPriceWarehouseAsync(ownerId);
        }

        public Task<int> ViewCountImportStatisticsAsync(int warehouseId)
        {
            return _warehouseDAO.ViewCountImportStatisticsAsync(warehouseId);
        }
        public Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId)
        {
            return _warehouseDAO.ViewNumberOfProductByImportStatisticsAsync(importId,ownerId);
        }
        public Task<double> ViewPriceByImportStatisticsAsync(int importId, int ownerId)
        {
            return _warehouseDAO.ViewPriceByImportStatisticsAsync(importId,ownerId);
        }
        public Task<int> QuantityWarehouseStatisticsAsync(int ownerId)
        {
            return _warehouseDAO.QuantityWarehouseStatisticsAsync(ownerId);
        }

    }
}
