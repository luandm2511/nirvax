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
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class GuestConsultationRepository : IGuestConsultationRepository
    {
       
        private readonly GuestConsultationDAO _guestConsultationDAO;
        public GuestConsultationRepository(GuestConsultationDAO guestConsultationDAO)
        {
            _guestConsultationDAO = guestConsultationDAO;
        }

        public  Task<int> ViewGuestConsultationStatisticsAsync()
        {
            return _guestConsultationDAO.ViewGuestConsultationStatisticsAsync();

        }
        public Task<GuestConsultation> GetGuestConsultationsByIdAsync(int guestId)
        {
            return _guestConsultationDAO.GetGuestConsultationsByIdAsync(guestId);
        }

        public Task<GuestConsultation> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
        {
            return _guestConsultationDAO.CreateGuestConsultationAsync(guestConsultationCreateDTO);
        }
        public Task<bool> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO)
        {
            return _guestConsultationDAO.UpdateGuestConsultationAsync(guestConsultationDTO);
        }

        public Task<bool> CheckGuestConsultationExistAsync(int guestId)
        {
            return _guestConsultationDAO.CheckGuestConsultationExistAsync(guestId);
        }
        public Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
        {
            return _guestConsultationDAO.CheckGuestConsultationAsync(guestConsultationCreateDTO);
        }
        public Task<List<GuestConsultation>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsAsync(searchQuery, page, pageSize, ownerId);
        }

        public Task<List<GuestConsultation>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsAcceptAsync(searchQuery, page, pageSize, ownerId);
        }
        public Task<List<GuestConsultation>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsWaitingAsync(searchQuery, page, pageSize, ownerId);
        }
        public Task<List<GuestConsultation>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsDenyAsync(searchQuery, page, pageSize, ownerId);
        }
        public Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest)
        {
            return _guestConsultationDAO.UpdateStatusGuestConsultationtAsync(guestId, statusGuest);
        }
    }
}
