using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Pipelines.Sockets.Unofficial.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
     public interface IGuestConsultationRepository
    {
        Task<GuestConsultation> GetGuestConsultationsByIdAsync(int guestId);
         Task<int> ViewGuestConsultationStatisticsAsync();
        Task<GuestConsultation> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO);

        Task<bool> CheckGuestConsultationExistAsync(int guestId);
        Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateStatusGuestConsultationtByIdAsync(int guestId, int statusGuestId);
        Task<List<GuestConsultation>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<GuestConsultation>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<GuestConsultation>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize,int ownerId);
        Task<List<GuestConsultation>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId);
       Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest);

    }
}
