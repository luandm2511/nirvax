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
        Task<List<DescriptionDTO>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize);
        Task<DescriptionDTO> GetDescriptionByIdAsync(int sizeId);
        Task<bool> CheckDescriptionAsync(int descriptionId, string title, string content);

        Task<bool> CheckDescriptionExistAsync(int sizeId);
        Task<bool> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO);

        Task<bool> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO);
        Task<bool> DeleteDesctiptionAsync(int sizeId);

    }
}
