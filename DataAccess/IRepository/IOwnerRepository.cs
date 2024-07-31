using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Pipelines.Sockets.Unofficial.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IOwnerRepository
    {
        Task<int> NumberOfOwnerStatisticsAsync();
        Task<List<Owner>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize);

        Task<List<Owner>> GetAllOwnersForUserAsync(string? searchQuery);
        Task<Owner> GetOwnerByIdAsync(int ownerId);
        Task<OwnerDTO> ViewOwnerProfileAsync(string ownerEmail);

        Task<bool> CheckOwnerAsync(OwnerDTO ownerDTO);
       Task<bool> CheckProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO);
        Task<bool> ChangePasswordOwnerAsync(int  ownerId, string oldPassword, string newPassword, string confirmPassword);

        Task<bool> CheckOwnerExistAsync(int ownerId);

        Task<bool> CheckProfileExistAsync(string ownerEmail);

        Task<string> GetEmailAsync(int ownerId);
        Task<bool> CreateOwnerAsync(OwnerDTO ownerDTO);

        Task<bool> UpdateOwnerAsync(OwnerDTO ownerDTO);
        Task<bool> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO);
        Task<bool> UpdateAvatarOwnerAsync(OwnerAvatarDTO ownerAvatarDTO);

        Task<bool> BanOwnerAsync(int ownerId);
        Task<bool> UnBanOwnerAsync(int ownerId);


    }
}
