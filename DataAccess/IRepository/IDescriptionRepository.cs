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
    public interface IDescriptionRepository
    {
        Task<List<DescriptionDTO>> GetAllDescriptions(string? searchQuery, int page, int pageSize);
        Task<DescriptionDTO> GetDescriptionById(int sizeId);
        Task<bool> CheckDescription(DescriptionDTO descriptionDTO);

        Task<bool> CheckDescriptionExist(int sizeId);
        Task<bool> CreateDesctiption(DescriptionDTO descriptionDTO);

        Task<bool> UpdateDesctiption(DescriptionDTO descriptionDTO);
        Task<bool> DeleteDesctiption(int sizeId);

    }
}
