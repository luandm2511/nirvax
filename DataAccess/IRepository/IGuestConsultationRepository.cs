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
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<GuestConsultationDTO> GetGuestConsultationsByIdAsync(int guestId);
         Task<int> ViewGuestConsultationStatisticsAsync();
        Task<bool> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO);

        Task<bool> CheckGuestConsultationExistAsync(int guestId);
        Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> UpdateStatusGuestConsultationtByIdAsync(int guestId, int statusGuestId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize,int ownerId);
        Task<List<GuestConsultationDTO>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId);
       Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest);

    }
}
