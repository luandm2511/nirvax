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


        public  Task<int> ViewGuestConsultationStatistics()
        {
            return _guestConsultationDAO.ViewGuestConsultationStatistics();

        }
        public Task<GuestConsultationDTO> GetGuestConsultationsById(int guestId)
        {
            return _guestConsultationDAO.GetGuestConsultationsById(guestId);
        }

        public Task<bool> CreateGuestConsultation(GuestConsultationDTO guestConsultationDTO)
        {
            return _guestConsultationDAO.CreateGuestConsultation(guestConsultationDTO);
        }
        public Task<bool> UpdateGuestConsultation(GuestConsultationDTO guestConsultationDTO)
        {
            return _guestConsultationDAO.UpdateGuestConsultation(guestConsultationDTO);
        }

        public Task<bool> CheckGuestConsultationExist(int guestId)
        {
            return _guestConsultationDAO.CheckGuestConsultationExist(guestId);
        }
        public Task<bool> CheckGuestConsultation(GuestConsultationDTO guestConsultationDTO)
        {
            return _guestConsultationDAO.CheckGuestConsultation(guestConsultationDTO);
        }
        public Task<bool> UpdateStatusGuestConsultationt(int guestId, int statusGuestId)
        {
            return _guestConsultationDAO.UpdateStatusGuestConsultationt(guestId, statusGuestId);
        }
        public Task<List<GuestConsultationDTO>> GetAllGuestConsultations(string? searchQuery, int page, int pageSize)
        {
            return _guestConsultationDAO.GetAllGuestConsultations(searchQuery, page, pageSize);
        }

    }
}
