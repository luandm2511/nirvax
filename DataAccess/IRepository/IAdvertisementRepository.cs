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

        Task<AdvertisementDTO> GetAdvertisementByIdAsync(int adId);
        Task<int> ViewOwnerAdversisementStatisticsAsync(int ownerId);
        Task<int> ViewAdversisementStatisticsAsync();
        Task<AdvertisementDTO> GetAdvertisementByIdForUserAsync(int adId);
        Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO);
        
        Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO);
        Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO);

        Task<bool> CheckAdvertisementExistAsync(int adId);
        Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO);
         Task<bool> UpdateStatusAdvertisementByIdAsync(int adId, int statusPostId);
        Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsByServiceAsync(int serviceId);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsForUserAsync(string? searchQuery);
        Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId);
    }
}
