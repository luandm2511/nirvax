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
        Task<int> ViewOwnerAdversisementStatisticsAsync(int ownerId);
        Task<int> ViewAdversisementStatisticsAsync();
        Task<Advertisement> GetAdvertisementByIdForUserAsync(int adId);
        Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO);
        
        Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO);
        Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO);

        Task<bool> CheckAdvertisementExistAsync(int adId);
        Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO);
         Task<bool> UpdateStatusAdvertisementByIdAsync(int adId, int statusPostId);
        Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost);
        Task<List<Advertisement>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize);
        Task<List<Advertisement>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId);
        Task<List<Advertisement>> GetAllAdvertisementsByServiceAsync(int serviceId);
        Task<List<Advertisement>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize);
        Task<List<Advertisement>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize);
        Task<List<Advertisement>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize);
        Task<List<Advertisement>> GetAllAdvertisementsForUserAsync(string? searchQuery);
        Task<List<Advertisement>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId);
    }
}
