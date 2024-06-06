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
    public interface IServiceRepository
    {
        Task<List<ServiceDTO>> GetAllServices(string? searchQuery, int page, int pageSize);
        Task<List<ServiceDTO>> GetAllServiceForUser();
        Task<ServiceDTO> GetServiceById(int serviceId);
        Task<bool> CheckService(ServiceDTO serviceDTO);
        Task<bool> CheckServiceExist(int serviceId);
        Task<bool> CreateService(ServiceDTO serviceDTO);
        Task<bool> RestoreService(int serviceId);

        Task<bool> UpdateService(ServiceDTO serviceDTO);
        Task<bool> DeleteService(int serviceId);

    }
}
