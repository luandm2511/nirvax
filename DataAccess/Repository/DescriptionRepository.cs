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
    public class DescriptionRepository : IDescriptionRepository
    {
       
        private readonly DescriptionDAO _descriptionDAO;
        public DescriptionRepository(DescriptionDAO descriptionDAO)
        {
            _descriptionDAO = descriptionDAO;
        }

        public Task<bool> CheckDescription(DescriptionDTO descriptionDTO)
        {
            return _descriptionDAO.CheckDescription(descriptionDTO); 
        }


        public Task<bool> CheckDescriptionExist(int descriptionId)
        {
            return _descriptionDAO.CheckDescriptionExist(descriptionId);
        }

        public Task<List<DescriptionDTO>> GetAllDescriptions(string? searchQuery, int page, int pageSize)
        {
            
            return _descriptionDAO.GetAllDescriptions(searchQuery, page,  pageSize);
        }

        public Task<DescriptionDTO> GetDescriptionById(int descriptionId)
        {
            return (_descriptionDAO.GetDescriptionById(descriptionId));
        }
  
        public Task<bool> CreateDesctiption(DescriptionDTO descriptionDTO)
        {
            return _descriptionDAO.CreateDesctiption(descriptionDTO);
        }

        public Task<bool> UpdateDesctiption(DescriptionDTO descriptionDTO)
        {
            return _descriptionDAO.UpdateDesctiption(descriptionDTO);
        }
        public Task<bool> DeleteDesctiption(int descriptionId)
        {
            return _descriptionDAO.DeleteDesctiption(descriptionId);
        }

    }
}
