﻿using AutoMapper.Execution;
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

        public Task<IEnumerable<ImportProductDetailByImportDTO>> GetAllImportProductDetailByImportIdAsync(int importId)
        {

            return _importProductDetailDAO.GetAllImportProductDetailByImportIdAsync(importId);
        }

        public Task<IEnumerable<ImportProductDetail>> GetAllImportProductDetailAsync()
        {

            return _importProductDetailDAO.GetAllImportProductDetailAsync();
        }

        public Task<bool> CreateImportProductDetailAsync(int importId, List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {

            return _importProductDetailDAO.CreateImportProductDetailAsync(importId, importProductDetailDTO);
        }

        public Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailUpdateDTO> importProductDetailDTO)
        {

            return _importProductDetailDAO.UpdateImportProductDetailAsync(importId, importProductDetailDTO);
        }





    }
}
