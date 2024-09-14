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
     public interface IAdvertisementRepository
    {

        Task<Advertisement> GetAdvertisementByIdAsync(int adId);
        Task<object> ViewAdversisementStatisticsAsync(int ownerId);
        Task<Advertisement> GetAdvertisementByIdForUserAsync(int adId);
        Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO);
        
        Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO);
        Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO);
        Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO);
        Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize);
       Task<IEnumerable<Advertisement>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsByServiceAsync(int serviceId);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize);
       Task<IEnumerable<Advertisement>> GetAllAdvertisementsForUserAsync(string? searchQuery);
       Task<IEnumerable<Advertisement>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId);
    }
}
