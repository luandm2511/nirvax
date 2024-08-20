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

        public Task<bool> CheckServiceAsync(int serviceId, string name)
        {
            return _serviceDAO.CheckServiceAsync(serviceId, name); 
        }
        public Task<List<Service>> GetAllServicesAsync(string? searchQuery, int page, int pageSize)
        {
            
            return _serviceDAO.GetAllServicesAsync(searchQuery, page,  pageSize);
        }
       
             public Task<List<Service>> GetAllServiceForUserAsync(string? searchQuery)
        {

            return _serviceDAO.GetAllServiceForUserAsync(searchQuery);
        }

        public Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return (_serviceDAO.GetServiceByIdAsync(serviceId));
        }
  
        public Task<bool> CreateServiceAsync(ServiceCreateDTO serviceCreateDTO)
        {
            return _serviceDAO.CreateServiceAsync(serviceCreateDTO);
        }

        public Task<bool> UpdateServiceAsync(ServiceDTO serviceDTO)
        {
            return _serviceDAO.UpdateServiceAsync(serviceDTO);
        }
        public Task<bool> RestoreServiceAsync(int serviceId)
        {
            return _serviceDAO.RestoreServiceAsync(serviceId);
        }
        public Task<bool> DeleteServiceAsync(int serviceId)
        {
            return _serviceDAO.DeleteServiceAsync(serviceId);
        }

    }
}
