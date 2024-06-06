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
    public interface IImportProductRepository
    {
        Task<List<ImportProductDTO>> GetAllImportProduct(int warehouseId, DateTime? from, DateTime? to);
        
        Task<List<ImportProductDTO>> GetImportProductByWarehouse(int warehouseId);
        Task<ImportProductDTO> GetImportProductById(int importId);

        
        Task<bool> CheckImportProductExist(int importId);

        Task<bool> CreateImportProduct(ImportProductDTO importProductDTO);

        Task<bool> UpdateImportProduct(ImportProductDTO importProductDTO);
    
      


    }
}
