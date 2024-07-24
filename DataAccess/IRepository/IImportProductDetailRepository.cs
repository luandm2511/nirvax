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
        Task<bool> CheckImportProductDetailExistAsync(int importProductDetailId);

        Task<List<ImportProductDetailDTO>> GetAllImportProductDetailByImportIdAsync(int importId);
        

        Task<List<ImportProductDetailDTO>> GetAllImportProductDetailAsync();

        Task<bool> CreateImportProductDetailAsync(int importId,List<ImportProductDetailDTO> importProductDetailDTO);

       
        Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailDTO> importProductDetailDTO);


    }
}
