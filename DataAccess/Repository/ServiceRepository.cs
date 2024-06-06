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
    public class ServiceRepository : IServiceRepository
    {
       
        private readonly ServiceDAO _serviceDAO;
        public ServiceRepository(ServiceDAO serviceDAO)
        {
            _serviceDAO = serviceDAO;
        }

        public Task<bool> CheckService(ServiceDTO serviceDTO)
        {
            return _serviceDAO.CheckService(serviceDTO); 
        }

        public Task<bool> CheckServiceExist(int serviceId)
        {
            return _serviceDAO.CheckServiceExist(serviceId);
        }

        public Task<List<ServiceDTO>> GetAllServices(string? searchQuery, int page, int pageSize)
        {
            
            return _serviceDAO.GetAllServices(searchQuery, page,  pageSize);
        }
       
             public Task<List<ServiceDTO>> GetAllServiceForUser()
        {

            return _serviceDAO.GetAllServiceForUser();
        }

        public Task<ServiceDTO> GetServiceById(int serviceId)
        {
            return (_serviceDAO.GetServiceById(serviceId));
        }
  
        public Task<bool> CreateService(ServiceDTO serviceDTO)
        {
            return _serviceDAO.CreateService(serviceDTO);
        }

        public Task<bool> UpdateService(ServiceDTO serviceDTO)
        {
            return _serviceDAO.UpdateService(serviceDTO);
        }
        public Task<bool> RestoreService(int serviceId)
        {
            return _serviceDAO.RestoreService(serviceId);
        }
        public Task<bool> DeleteService(int serviceId)
        {
            return _serviceDAO.DeleteService(serviceId);
        }

    }
}
