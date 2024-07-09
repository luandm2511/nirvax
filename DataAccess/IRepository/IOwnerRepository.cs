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
        Task<List<OwnerDTO>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize);

        Task<List<OwnerDTO>> GetAllOwnersForUserAsync(string? searchQuery);
        Task<OwnerDTO> GetOwnerByIdAsync(int ownerId);
        Task<OwnerDTO> GetOwnerByEmailAsync(string ownerEmail);

        Task<bool> CheckOwnerAsync(OwnerDTO ownerDTO);
       Task<bool> CheckProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO);
        Task<bool> ChangePasswordOwnerAsync(int  ownerId, string oldPassword, string newPasswod);

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
