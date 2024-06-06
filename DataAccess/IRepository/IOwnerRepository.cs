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
    public interface IOwnerRepository
    {
        Task<int> NumberOfOwnerStatistics();
        Task<List<OwnerDTO>> GetAllOwners(string? searchQuery, int page, int pageSize);

        Task<List<OwnerDTO>> GetAllOwnersForUser(string? searchQuery);
        Task<OwnerDTO> GetOwnerById(int ownerId);
        Task<OwnerDTO> GetOwnerByEmail(string ownerEmail);

        Task<bool> CheckOwner(OwnerDTO ownerDTO);
       Task<bool> CheckProfileOwner(OwnerProfileDTO ownerProfileDTO);
        Task<bool> ChangePasswordOwner(int  ownerId, string oldPassword, string newPasswod);

        Task<bool> CheckOwnerExist(int ownerId);

        Task<bool> CheckProfileExist(string ownerEmail);


        Task<bool> CreateOwner(OwnerDTO ownerDTO);

        Task<bool> UpdateOwner(OwnerDTO ownerDTO);
        Task<bool> UpdateProfileOwner(OwnerProfileDTO ownerProfileDTO);
        Task<bool> UpdateAvatarOwner(OwnerAvatarDTO ownerAvatarDTO);

        Task<bool> BanOwner(int ownerId);
        Task<bool> UnBanOwner(int ownerId);


    }
}
