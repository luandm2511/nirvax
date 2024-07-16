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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _advertisementDAO.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _advertisementDAO.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _advertisementDAO.RollbackTransactionAsync();
        }

        public Task<AdvertisementDTO> GetAdvertisementByIdAsync(int adId)
        {
            return _advertisementDAO.GetAdvertisementByIdAsync(adId);
        }
        public Task<AdvertisementDTO> GetAdvertisementByIdForUserAsync(int adId)
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
        public Task<bool> UpdateStatusAdvertisementByIdAsync(int adId, int statusPostId)
        {
            return _advertisementDAO.UpdateStatusAdvertisementByIdAsync(adId, statusPostId);
        }
        public Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {
            return _advertisementDAO.UpdateStatusAdvertisementAsync(adId, statusPost);
        }

        public Task<bool> CheckAdvertisementExistAsync(int adId)
        {
            return _advertisementDAO.CheckAdvertisementExistAsync(adId);
        }
        public Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {
            return _advertisementDAO.CheckAdvertisementAsync(advertisementDTO);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsAsync(searchQuery, page, pageSize);
        }

        public Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
            return _advertisementDAO.GetAdvertisementsByOwnerForUserAsync(searchQuery, ownerId);
        }

        public Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _advertisementDAO.GetAdvertisementsByOwnerAsync(searchQuery, page, pageSize, ownerId);
        }

        public Task<List<AdvertisementDTO>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {
            return _advertisementDAO.GetAllAdvertisementsByServiceAsync(serviceId);
        }

        public Task<List<AdvertisementDTO>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsWaitingAsync(searchQuery, page, pageSize);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsAcceptAsync(searchQuery, page, pageSize);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisementsDenyAsync(searchQuery, page, pageSize);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            return _advertisementDAO.GetAllAdvertisementsForUserAsync(searchQuery);
        }

       public Task<int> ViewOwnerBlogStatisticsAsync(int ownerId)
        {
            return _advertisementDAO.ViewOwnerBlogStatisticsAsync(ownerId);

        }
        public Task<int> ViewBlogStatisticsAsync()
        {
            return _advertisementDAO.ViewBlogStatisticsAsync();

        }

    }
}
