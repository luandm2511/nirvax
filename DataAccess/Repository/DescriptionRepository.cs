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
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class DescriptionRepository : IDescriptionRepository
    {
       
        private readonly DescriptionDAO _descriptionDAO;
        public DescriptionRepository(DescriptionDAO descriptionDAO)
        {
            _descriptionDAO = descriptionDAO;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _descriptionDAO.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _descriptionDAO.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _descriptionDAO.RollbackTransactionAsync();
        }

        public Task<bool> CheckDescriptionAsync(int descriptionId, string title, string content)
        {
            return _descriptionDAO.CheckDescriptionAsync( descriptionId,  title, content);
        }


        public Task<bool> CheckDescriptionExistAsync(int descriptionId)
        {
            return _descriptionDAO.CheckDescriptionExistAsync(descriptionId);
        }

        public Task<List<DescriptionDTO>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize)
        {
            
            return _descriptionDAO.GetAllDescriptionsAsync(searchQuery, page,  pageSize);
        }

        public Task<DescriptionDTO> GetDescriptionByIdAsync(int descriptionId)
        {
            return (_descriptionDAO.GetDescriptionByIdAsync(descriptionId));
        }
  
        public Task<bool> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO)
        {
            return _descriptionDAO.CreateDesctiptionAsync(descriptionCreateDTO);
        }

        public Task<bool> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO)
        {
            return _descriptionDAO.UpdateDesctiptionAsync(descriptionDTO);
        }
        public Task<bool> DeleteDesctiptionAsync(int descriptionId)
        {
            return _descriptionDAO.DeleteDesctiptionAsync(descriptionId);
        }

    }
}
