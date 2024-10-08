﻿using AutoMapper.Execution;
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
        Task<int> ViewGuestConsultationStatisticsAsync(int ownerId);
        Task<GuestConsultation> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO);
        Task<IEnumerable<GuestConsultationViewDTO>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<IEnumerable<GuestConsultationViewDTO>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<IEnumerable<GuestConsultationViewDTO>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<IEnumerable<GuestConsultationViewDTO>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest);
    }
}
