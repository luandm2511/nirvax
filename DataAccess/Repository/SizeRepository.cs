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

        public Task<bool> CheckSizeAsync(int sizeId, int ownerId, string name)
        {
            return _sizeDAO.CheckSizeAsync(sizeId, ownerId, name); 
        }


        public Task<bool> CheckSizeExistAsync(int sizeId)
        {
            return _sizeDAO.CheckSizeExistAsync(sizeId);
        }

        public Task<List<Size>> GetAllSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            
            return _sizeDAO.GetAllSizesAsync(searchQuery, page,  pageSize, ownerId);
        }

        public Task<Size> GetSizeByIdAsync(int sizeId)
        {
            return (_sizeDAO.GetSizeByIdAsync(sizeId));
        }
  
        public Task<bool> CreateSizeAsync(SizeCreateDTO sizeCreateDTO)
        {
            return _sizeDAO.CreateSizeAsync(sizeCreateDTO);
        }

        public Task<bool> UpdateSizeAsync(SizeDTO sizeDTO)
        {
            return _sizeDAO.UpdateSizeAsync(sizeDTO);
        }
        public Task<bool> DeleteSizeAsync(int sizeId)
        {
            return _sizeDAO.DeleteSizeAsync(sizeId);
        }
        public Task<bool> RestoreSizeAsync(int sizeId)
        {
            return _sizeDAO.RestoreSizeAsync(sizeId);
        }

    }
}
