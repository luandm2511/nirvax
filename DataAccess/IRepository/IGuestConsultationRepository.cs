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
        Task<GuestConsultationDTO> GetGuestConsultationsByIdAsync(int guestId);
         Task<int> ViewGuestConsultationStatisticsAsync();
        Task<bool> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO);

        Task<bool> CheckGuestConsultationExistAsync(int guestId);
        Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, int statusGuestId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize);
       
    }
}
