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
    public interface IImportProductDetailRepository
    {

        Task<List<ImportProductDetail>> GetAllImportProductDetailByImportIdAsync(int importId);
        
        Task<List<ImportProductDetail>> GetAllImportProductDetailAsync();

        Task<bool> CreateImportProductDetailAsync(int importId, List<ImportProductDetailCreateDTO> importProductDetailDTO);

        Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailUpdateDTO> importProductDetailDTO);

    }
}
