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
    public class AdvertisementRepository : IAdvertisementRepository
    {

        private readonly AdvertisementDAO _advertisementDAO;
        public AdvertisementRepository(AdvertisementDAO advertisementDAO)
        {
            _advertisementDAO = advertisementDAO;
        }



        public Task<AdvertisementDTO> GetAdvertisementById(int adId)
        {
            return _advertisementDAO.GetAdvertisementById(adId);
        }
        public Task<AdvertisementDTO> GetAdvertisementByIdForUser(int adId)
        {
            return _advertisementDAO.GetAdvertisementByIdForUser(adId);
        }

        public Task<bool> CheckAdvertisementCreate(AdvertisementCreateDTO advertisementCreateDTO)
        {
            return _advertisementDAO.CheckAdvertisementCreate(advertisementCreateDTO);
        }
        public Task<bool> CreateAdvertisement(AdvertisementCreateDTO advertisementCreateDTO)
        {
            return _advertisementDAO.CreateAdvertisement(advertisementCreateDTO);
        }
        public Task<bool> UpdateAdvertisement(AdvertisementDTO advertisementDTO)
        {
            return _advertisementDAO.UpdateAdvertisement(advertisementDTO);
        }
        public Task<bool> UpdateStatusAdvertisement(int adId, int statusPostId)
        {
            return _advertisementDAO.UpdateStatusAdvertisement(adId, statusPostId);
        }

        public Task<bool> CheckAdvertisementExist(int adId)
        {
            return _advertisementDAO.CheckAdvertisementExist(adId);
        }
        public Task<bool> CheckAdvertisement(AdvertisementDTO advertisementDTO)
        {
            return _advertisementDAO.CheckAdvertisement(advertisementDTO);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisements(string? searchQuery, int page, int pageSize)
        {
            return _advertisementDAO.GetAllAdvertisements(searchQuery, page, pageSize);
        }
        public Task<List<AdvertisementDTO>> GetAllAdvertisementsForUser(string? searchQuery)
        {
            return _advertisementDAO.GetAllAdvertisementsForUser(searchQuery);
        }

       public Task<int> ViewOwnerBlogStatistics(int ownerId)
        {
            return _advertisementDAO.ViewOwnerBlogStatistics(ownerId);

        }
        public Task<int> ViewBlogStatistics()
        {
            return _advertisementDAO.ViewBlogStatistics();

        }

    }
}
