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
    public class ImportProductDetailRepository  : IImportProductDetailRepository{ 

       private readonly ImportProductDetailDAO _importProductDetailDAO;
       public ImportProductDetailRepository(ImportProductDetailDAO importProductDetailDAO)
       {
           _importProductDetailDAO = importProductDetailDAO;
       }
        public Task<bool> CheckImportProductDetailExist(int importProductDetailId)
        {

            return _importProductDetailDAO.CheckImportProductDetailExist(importProductDetailId);
        }

        public Task<List<ImportProductDetailDTO>> GetAllImportProductDetailByImportId(int importId)
        {

            return _importProductDetailDAO.GetAllImportProductDetailByImportId(importId);
        }

        public Task<List<ImportProductDetailDTO>> GetAllImportProductDetail()
        {

            return _importProductDetailDAO.GetAllImportProductDetail();
        }



        public Task<bool> CreateImportProductDetail(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {

            return _importProductDetailDAO.CreateImportProductDetail(importId, importProductDetailDTO);
        }

        public Task<bool> UpdateImportProductDetail(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {

            return _importProductDetailDAO.UpdateImportProductDetail(importId, importProductDetailDTO);
        }









    }
}
