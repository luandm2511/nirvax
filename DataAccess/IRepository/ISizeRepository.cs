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
        Task<IEnumerable<Size>> GetAllSizesAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<Size> GetSizeByIdAsync(int sizeId);
        Task<bool> CheckSizeAsync(int sizeId, int ownerId, string name);
        Task<bool> CreateSizeAsync(SizeCreateDTO sizeCreateDTO);
        Task<bool> UpdateSizeAsync(SizeDTO sizeDTO);
        Task<bool> DeleteSizeAsync(int sizeId);
        Task<bool> RestoreSizeAsync(int sizeId);
    }
}
