﻿using AutoMapper.Execution;
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

        Task<List<ImportProduct>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize);
        Task<List<WarehouseDetail>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize);
        Task<Warehouse> GetWarehouseByOwnerIdAsync(int ownerId);
        Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId);
        Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int ownerId);
        Task<bool> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO);

        Task<bool> UpdateWarehouseAsync(WarehouseDTO warehouseDTO);


    }
}
