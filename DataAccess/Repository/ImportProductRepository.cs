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
    public class ImportProductRepository : IImportProductRepository
    {
       
        private readonly ImportProductDAO _importProductDAO;
        public ImportProductRepository(ImportProductDAO importProductDAO)
        {
            _importProductDAO = importProductDAO;
        }

    
      
        public Task<List<ImportProduct>> GetAllImportProductAsync(int warehouseId,DateTime? from, DateTime? to)
        {
            
            return _importProductDAO.GetAllImportProductAsync(warehouseId, from, to);
        }

        public Task<List<ImportProduct>> GetImportProductByWarehouseAsync(int warehouseId)
        {

            return _importProductDAO.GetImportProductByWarehouseAsync(warehouseId);
        }

        public Task<bool> CheckImportProductExistAsync(int importId)
        {
            return _importProductDAO.CheckImportProductExistAsync(importId);
        }
        public Task<ImportProduct> GetImportProductByIdAsync(int importId)
        {
            return (_importProductDAO.GetImportProductByIdAsync(importId));
        }
  
        public Task<ImportProduct> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO)
        {
            return _importProductDAO.CreateImportProductAsync(importProductCreateDTO);
        }

        public Task<bool> UpdateImportProductAsync(ImportProductDTO importProductDTO)
        {
            return _importProductDAO.UpdateImportProductAsync(importProductDTO);
        }
        public Task<bool> UpdateQuantityAndPriceImportProductAsync(int importId)
        {
            return _importProductDAO.UpdateQuantityAndPriceImportProductAsync(importId);
        }
        public Task<int> ViewImportProductStatisticsAsync(int warehouseId)
        {
            return _importProductDAO.ViewImportProductStatisticsAsync(warehouseId);
        }


        public Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId)
        {
            return _importProductDAO.ViewNumberOfProductByImportStatisticsAsync(importId, ownerId);
        }
        public Task<double> ViewPriceByImportStatisticsAsync(int importId, int ownerId)
        {
            return _importProductDAO.ViewPriceByImportStatisticsAsync(importId, ownerId);
        }
        public Task<int> QuantityWarehouseStatisticsAsync(int ownerId)
        {
            return _importProductDAO.QuantityWarehouseStatisticsAsync(ownerId);
        }
    }
}
