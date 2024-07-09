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
     public interface IAdvertisementRepository
    {
        Task<AdvertisementDTO> GetAdvertisementByIdAsync(int adId);
        Task<int> ViewOwnerBlogStatisticsAsync(int ownerId);
        Task<int> ViewBlogStatisticsAsync();
        Task<AdvertisementDTO> GetAdvertisementByIdForUserAsync(int adId);
        Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO);
        
        Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO);
        Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO);

        Task<bool> CheckAdvertisementExistAsync(int adId);
        Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO);
         Task<bool> UpdateStatusAdvertisementAsync(int adId, int statusPostId);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsForUserAsync(string? searchQuery);
    }
}
