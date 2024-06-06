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
    public interface ISizeRepository
    {
        Task<List<SizeDTO>> GetAllSizes(string? searchQuery, int page, int pageSize);
        Task<SizeDTO> GetSizeById(int sizeId);
        Task<bool> CheckSize(SizeDTO sizeDTO);

        Task<bool> CheckSizeExist(int sizeId);
        Task<bool> CreateSize(SizeDTO sizeDTO);

        Task<bool> UpdateSize(SizeDTO sizeDTO);
        Task<bool> DeleteSize(int sizeId);
        Task<bool> RestoreSize(int sizeId);

    }
}
