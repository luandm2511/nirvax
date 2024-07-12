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
        public Task<GuestConsultationDTO> GetGuestConsultationsByIdAsync(int guestId)
        {
            return _guestConsultationDAO.GetGuestConsultationsByIdAsync(guestId);
        }

        public Task<bool> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
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
        public Task<bool> UpdateStatusGuestConsultationtByIdAsync(int guestId, int statusGuestId)
        {
            return _guestConsultationDAO.UpdateStatusGuestConsultationtByIdAsync(guestId, statusGuestId);
        }
        public Task<List<GuestConsultationDTO>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsAsync(searchQuery, page, pageSize);
        }

        public Task<List<GuestConsultationDTO>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsAcceptAsync(searchQuery, page, pageSize);
        }
        public Task<List<GuestConsultationDTO>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsWaitingAsync(searchQuery, page, pageSize);
        }
        public Task<List<GuestConsultationDTO>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            return _guestConsultationDAO.GetAllGuestConsultationsDenyAsync(searchQuery, page, pageSize);
        }
        public Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest)
        {
            return _guestConsultationDAO.UpdateStatusGuestConsultationtAsync(guestId, statusGuest);
        }
    }
}
