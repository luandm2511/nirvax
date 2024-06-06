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
    public class SizeRepository : ISizeRepository
    {
       
        private readonly SizeDAO _sizeDAO;
        public SizeRepository(SizeDAO sizeDAO)
        {
            _sizeDAO = sizeDAO;
        }

        public Task<bool> CheckSize(SizeDTO sizeDTO)
        {
            return _sizeDAO.CheckSize(sizeDTO); 
        }


        public Task<bool> CheckSizeExist(int sizeId)
        {
            return _sizeDAO.CheckSizeExist(sizeId);
        }

        public Task<List<SizeDTO>> GetAllSizes(string? searchQuery, int page, int pageSize)
        {
            
            return _sizeDAO.GetAllSizes(searchQuery, page,  pageSize);
        }

        public Task<SizeDTO> GetSizeById(int sizeId)
        {
            return (_sizeDAO.GetSizeById(sizeId));
        }
  
        public Task<bool> CreateSize(SizeDTO sizeDTO)
        {
            return _sizeDAO.CreateSize(sizeDTO);
        }

        public Task<bool> UpdateSize(SizeDTO sizeDTO)
        {
            return _sizeDAO.UpdateSize(sizeDTO);
        }
        public Task<bool> DeleteSize(int sizeId)
        {
            return _sizeDAO.DeleteSize(sizeId);
        }
        public Task<bool> RestoreSize(int sizeId)
        {
            return _sizeDAO.RestoreSize(sizeId);
        }

    }
}
