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
        Task<bool> CheckImportProductDetailExist(int importProductDetailId);

        Task<List<ImportProductDetailDTO>> GetAllImportProductDetailByImportId(int importId);
        

        Task<List<ImportProductDetailDTO>> GetAllImportProductDetail();

       

        Task<bool> CreateImportProductDetail(int importId,List<ImportProductDetailDTO> importProductDetailDTO);

        //Task<bool> UpdateImportDetail(ImportProductDetailDTO importProductDetailDTO);
        Task<bool> UpdateImportProductDetail(int importId, List<ImportProductDetailDTO> importProductDetailDTO);


    }
}
