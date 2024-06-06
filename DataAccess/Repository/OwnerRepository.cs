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

        public Task<int> NumberOfOwnerStatistics()
        {

            return _ownerDAO.NumberOfOwnerStatistics();
        }
        public Task<List<OwnerDTO>> GetAllOwners(string? searchQuery, int page, int pageSize)
        {

            return _ownerDAO.GetAllOwners(searchQuery, page, pageSize);
        }

        public Task<List<OwnerDTO>> GetAllOwnersForUser(string? searchQuery)
        {

            return _ownerDAO.GetAllOwnersForUser(searchQuery);
        }


        public Task<OwnerDTO> GetOwnerById(int staffId)
        {
            return (_ownerDAO.GetOwnerById(staffId));
        }

        public Task<OwnerDTO> GetOwnerByEmail(string ownerEmail)
        {
            return (_ownerDAO.GetOwnerByEmail(ownerEmail));
        }
       

        public Task<bool> CheckOwner(OwnerDTO ownerDTO)
        {
            return _ownerDAO.CheckOwner(ownerDTO);
        }
        public Task<bool> CheckOwnerExist(int ownerId)
        {
            return _ownerDAO.CheckOwnerExist(ownerId);
        }
        public Task<bool> CheckProfileOwner(OwnerProfileDTO ownerProfileDTO)
        {
            return _ownerDAO.CheckProfileOwner(ownerProfileDTO);
        }

        public Task<bool> CheckProfileExist(string ownerEmail)
        {
            return _ownerDAO.CheckProfileExist(ownerEmail);
        }
       
        public Task<bool> ChangePasswordOwner(int ownerId, string oldPassword, string newPasswod)
        {
            return _ownerDAO.ChangePasswordOwner(ownerId, oldPassword, newPasswod); 
        }

        public Task<bool> CreateOwner(OwnerDTO ownerDTO)
        {
            return _ownerDAO.CreateOwner(ownerDTO);
        }

        public Task<bool> UpdateOwner(OwnerDTO ownerDTO)
        {
            return _ownerDAO.UpdateOwner(ownerDTO);
        }

        public Task<bool> UpdateProfileOwner(OwnerProfileDTO ownerProfileDTO)
        {
            return _ownerDAO.UpdateProfileOwner(ownerProfileDTO);
        }

        public Task<bool> UpdateAvatarOwner(OwnerAvatarDTO ownerAvatarDTO)
        {
            return _ownerDAO.UpdateAvatarOwner(ownerAvatarDTO);
        }
        public Task<bool> BanOwner(int ownerId)
        {
            return _ownerDAO.BanOwner(ownerId);
        }

        public Task<bool> UnBanOwner(int ownerId)
        {
            return _ownerDAO.UnBanOwner(ownerId);
        }

    }
}
