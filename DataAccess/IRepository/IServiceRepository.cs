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
        Task<List<ServiceDTO>> GetAllServicesAsync(string? searchQuery, int page, int pageSize);
        Task<List<ServiceDTO>> GetAllServiceForUserAsync(string? searchQuery);
        Task<ServiceDTO> GetServiceByIdAsync(int serviceId);
        Task<bool> CheckServiceAsync(int serviceId, string name);
        Task<bool> CheckServiceExistAsync(int serviceId);
        Task<bool> CreateServiceAsync(ServiceCreateDTO serviceCreateDTO);
        Task<bool> RestoreServiceAsync(int serviceId);

        Task<bool> UpdateServiceAsync(ServiceDTO serviceDTO);
        Task<bool> DeleteServiceAsync(int serviceId);

    }
}
