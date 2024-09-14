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

        Task<IEnumerable<ImportProductDetailByImportDTO>> GetAllImportProductDetailByImportIdAsync(int importId);
        
        Task<IEnumerable<ImportProductDetail>> GetAllImportProductDetailAsync();

        Task<bool> CreateImportProductDetailAsync(int importId, List<ImportProductDetailCreateDTO> importProductDetailDTO);

        Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailUpdateDTO> importProductDetailDTO);

    }
}
