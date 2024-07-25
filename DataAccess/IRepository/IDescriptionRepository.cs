using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IDescriptionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<List<DescriptionDTO>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize);
        Task<List<DescriptionDTO>> GetAllDescriptionsForUserAsync(string? searchQuery);
        Task<DescriptionDTO> GetDescriptionByIdAsync(int sizeId);
        Task<bool> CheckDescriptionAsync(int descriptionId, string title, string content);

        Task<bool> CheckDescriptionExistAsync(int sizeId);
        Task<Description> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO);

        Task<Description> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO);
        Task<bool> DeleteDesctiptionAsync(int sizeId);

    }
}
