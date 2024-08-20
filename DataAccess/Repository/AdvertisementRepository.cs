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
using Pipelines.Sockets.Unofficial.Buffers;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {

        private readonly AdvertisementDAO _advertisementDAO;
        public AdvertisementRepository(AdvertisementDAO advertisementDAO)
        {
            _advertisementDAO = advertisementDAO;
        }



        public Task<Advertisement> GetAdvertisementByIdAsync(int adId)
        {
            return _advertisementDAO.GetAdvertisementByIdAsync(adId);
        }
        public Task<Advertisement> GetAdvertisementByIdForUserAsync(int adId)
        {
            return _advertisementDAO.GetAdvertisementByIdForUserAsync(adId);
        }

        public Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO)
        {
            return _advertisementDAO.CheckAdvertisementCreateAsync(advertisementCreateDTO);
        }
        public Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO)
        {
            return _advertisementDAO.CreateAdvertisementAsync(advertisementCreateDTO);
        }
        public Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {
            return _advertisementDAO.UpdateAdvertisementAsync(advertisementDTO);
        }

        public Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {
            return _advertisementDAO.UpdateStatusAdvertisementAsync(adId, statusPost);
        }
        public Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {
            return _advertisementDAO.CheckAdvertisementAsync(advertisementDTO);
        }
        public Task<List<Advertisement>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsAsync(searchQuery, page, pageSize);
        }

        public Task<List<Advertisement>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
            return _advertisementDAO.GetAdvertisementsByOwnerForUserAsync(searchQuery, ownerId);
        }

        public Task<List<Advertisement>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _advertisementDAO.GetAdvertisementsByOwnerAsync(searchQuery, page, pageSize, ownerId);
        }

        public Task<List<Advertisement>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {
            return _advertisementDAO.GetAllAdvertisementsByServiceAsync(serviceId);
        }

        public Task<List<Advertisement>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsWaitingAsync(searchQuery, page, pageSize);
        }
        public Task<List<Advertisement>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsAcceptAsync(searchQuery, page, pageSize);
        }
        public Task<List<Advertisement>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsDenyAsync(searchQuery, page, pageSize);
        }
        public Task<List<Advertisement>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            return _advertisementDAO.GetAllAdvertisementsForUserAsync(searchQuery);
        }

       public Task<int> ViewOwnerAdversisementStatisticsAsync(int ownerId)
        {
            return _advertisementDAO.ViewOwnerAdversisementStatisticsAsync(ownerId);

        }
        public Task<int> ViewAdversisementStatisticsAsync()
        {
            return _advertisementDAO.ViewAdversisementStatisticsAsync();

        }

    }
}
