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

    
      
        public Task<List<ImportProductDTO>> GetAllImportProduct(int warehouseId,DateTime? from, DateTime? to)
        {
            
            return _importProductDAO.GetAllImportProduct(warehouseId, from, to);
        }

        public Task<List<ImportProductDTO>> GetImportProductByWarehouse(int warehouseId)
        {

            return _importProductDAO.GetImportProductByWarehouse(warehouseId);
        }

        public Task<bool> CheckImportProductExist(int importId)
        {
            return _importProductDAO.CheckImportProductExist(importId);
        }
        public Task<ImportProductDTO> GetImportProductById(int importId)
        {
            return (_importProductDAO.GetImportProductById(importId));
        }
  
        public Task<bool> CreateImportProduct(ImportProductDTO importProductDTO)
        {
            return _importProductDAO.CreateImportProduct(importProductDTO);
        }

        public Task<bool> UpdateImportProduct(ImportProductDTO importProductDTO)
        {
            return _importProductDAO.UpdateImportProduct(importProductDTO);
        }

    
      
     

    }
}
