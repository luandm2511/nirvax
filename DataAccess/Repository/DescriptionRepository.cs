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
    {/*
       
        private readonly DescriptionDAO _descriptionDAO;
        public DescriptionRepository(DescriptionDAO descriptionDAO)
        {
            _descriptionDAO = descriptionDAO;
        }

        public Task<bool> CheckDescriptionAsync(int descriptionId, string title, string content)
        {
            return _descriptionDAO.CheckDescriptionAsync( descriptionId,  title, content);
        }
        public Task<List<Description>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            
            return _descriptionDAO.GetAllDescriptionsAsync(searchQuery, page,  pageSize, ownerId);
        }

        public Task<List<Description>> GetDescriptionForUserAsync(string? searchQuery)
        {
            return _descriptionDAO.GetDescriptionForUserAsync(searchQuery);

        }

        public Task<Description> GetDescriptionByIdAsync(int descriptionId)
        {
            return (_descriptionDAO.GetDescriptionByIdAsync(descriptionId));
        }
  
        public Task<Description> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO)
        {
            return _descriptionDAO.CreateDesctiptionAsync(descriptionCreateDTO);
        }

        public Task<Description> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO)
        {
            return _descriptionDAO.UpdateDesctiptionAsync(descriptionDTO);
        }
        public Task<bool> DeleteDesctiptionAsync(int descriptionId)
        {
            return _descriptionDAO.DeleteDesctiptionAsync(descriptionId);
        }
        */

    }
}
