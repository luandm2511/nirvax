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
     public interface IGuestConsultationRepository
    {
        Task<GuestConsultationDTO> GetGuestConsultationsById(int guestId);
         Task<int> ViewGuestConsultationStatistics();
        Task<bool> CreateGuestConsultation(GuestConsultationDTO guestConsultationDTO);
        Task<bool> UpdateGuestConsultation(GuestConsultationDTO guestConsultationDTO);

        Task<bool> CheckGuestConsultationExist(int guestId);
        Task<bool> CheckGuestConsultation(GuestConsultationDTO guestConsultationDTO);
         Task<bool> UpdateStatusGuestConsultationt(int guestId, int statusGuestId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultations(string? searchQuery, int page, int pageSize);
       
    }
}
