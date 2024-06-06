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
        Task<AdvertisementDTO> GetAdvertisementById(int adId);
        Task<int> ViewOwnerBlogStatistics(int ownerId);
        Task<int> ViewBlogStatistics();
        Task<AdvertisementDTO> GetAdvertisementByIdForUser(int adId);
        Task<bool> CheckAdvertisementCreate(AdvertisementCreateDTO advertisementCreateDTO);
        
        Task<bool> CreateAdvertisement(AdvertisementCreateDTO advertisementCreateDTO);
        Task<bool> UpdateAdvertisement(AdvertisementDTO advertisementDTO);

        Task<bool> CheckAdvertisementExist(int adId);
        Task<bool> CheckAdvertisement(AdvertisementDTO advertisementDTO);
         Task<bool> UpdateStatusAdvertisement(int adId, int statusPostId);
        Task<List<AdvertisementDTO>> GetAllAdvertisements(string? searchQuery, int page, int pageSize);
        Task<List<AdvertisementDTO>> GetAllAdvertisementsForUser(string? searchQuery);
    }
}
