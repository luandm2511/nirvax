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

    
      
        public Task<IEnumerable<ImportProduct>> GetAllImportProductAsync(int ownerId,DateTime? from, DateTime? to)
        {
            
            return _importProductDAO.GetAllImportProductAsync(ownerId, from, to);
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
        public Task<object> ViewImportProductStatisticsAsync(int ownerId)
        {
            return _importProductDAO.ViewImportProductStatisticsAsync(ownerId);
        }

        public Task<IEnumerable<object>> ViewWeeklyImportProductAsync(int ownerId)
        {
            return _importProductDAO.ViewWeeklyImportProductAsync(ownerId);
        }


        
    }
}
