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
    public class OwnerRepository : IOwnerRepository
    {

        private readonly OwnerDAO _ownerDAO;
        public OwnerRepository(OwnerDAO ownerDAO)
        {
            _ownerDAO = ownerDAO;
        }

        public Task<int> NumberOfOwnerStatisticsAsync()
        {

            return _ownerDAO.NumberOfOwnerStatisticsAsync();
        }
        public Task<List<Owner>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize)
        {

            return _ownerDAO.GetAllOwnersAsync(searchQuery, page, pageSize);
        }

        public Task<List<Owner>> GetAllOwnersForUserAsync(string? searchQuery)
        {

            return _ownerDAO.GetAllOwnersForUserAsync(searchQuery);
        }


        public Task<Owner> GetOwnerByIdAsync(int staffId)
        {
            return (_ownerDAO.GetOwnerByIdAsync(staffId));
        }

        public Task<OwnerDTO> ViewOwnerProfileAsync(string ownerEmail)
        {
            return (_ownerDAO.ViewOwnerProfileAsync(ownerEmail));
        }
       

        public Task<bool> CheckOwnerAsync(OwnerDTO ownerDTO)
        {
            return _ownerDAO.CheckOwnerAsync(ownerDTO);
        }
        public Task<bool> CheckOwnerExistAsync(int ownerId)
        {
            return _ownerDAO.CheckOwnerExistAsync(ownerId);
        }
        public Task<bool> CheckProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            return _ownerDAO.CheckProfileOwnerAsync(ownerProfileDTO);
        }

        public Task<bool> CheckProfileExistAsync(string ownerEmail)
        {
            return _ownerDAO.CheckProfileExistAsync(ownerEmail);
        }
       
        public Task<bool> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPassword,string confirmPassword)
        {
            return _ownerDAO.ChangePasswordOwnerAsync(ownerId, oldPassword, newPassword, confirmPassword); 
        }

        public Task<bool> CreateOwnerAsync(OwnerDTO ownerDTO)
        {
            return _ownerDAO.CreateOwnerAsync(ownerDTO);
        }

        public Task<string> GetEmailAsync(int ownerId)
        {
            return _ownerDAO.GetEmailAsync(ownerId);
        }
        public Task<bool> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            return _ownerDAO.UpdateOwnerAsync(ownerDTO);
        }

        public Task<bool> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            return _ownerDAO.UpdateProfileOwnerAsync(ownerProfileDTO);
        }

        public Task<bool> UpdateAvatarOwnerAsync(OwnerAvatarDTO ownerAvatarDTO)
        {
            return _ownerDAO.UpdateAvatarOwnerAsync(ownerAvatarDTO);
        }
        public Task<bool> BanOwnerAsync(int ownerId)
        {
            return _ownerDAO.BanOwnerAsync(ownerId);
        }

        public Task<bool> UnBanOwnerAsync(int ownerId)
        {
            return _ownerDAO.UnBanOwnerAsync(ownerId);
        }

    }
}
