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
    public class StaffRepository : IStaffRepository
    {

        private readonly StaffDAO _staffDAO;
        public StaffRepository(StaffDAO staffDAO)
        {
            _staffDAO = staffDAO;
        }

       


        public Task<List<StaffDTO>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {

            return _staffDAO.GetAllStaffsAsync(searchQuery, page, pageSize, ownerId);
        }




    public Task<StaffDTO> GetStaffByIdAsync(int staffId)
        {
            return _staffDAO.GetStaffByIdAsync(staffId);
        }

public Task<StaffDTO> ViewStaffProfileAsync(string ownerEmail)
{
    return (_staffDAO.ViewStaffProfileAsync(ownerEmail));
}


public Task<bool> CheckStaffAsync(int staffId, string email, string phone, int ownerId)
{
    return _staffDAO.CheckStaffAsync(staffId, email, phone, ownerId);
}
public Task<bool> CheckStaffExistAsync(int staffId)
{
    return _staffDAO.CheckStaffExistAsync(staffId);
}
public Task<bool> CheckProfileStaffAsync(StaffProfileDTO staffProfileDTO)
{
    return _staffDAO.CheckProfileStaffAsync(staffProfileDTO);
}

public Task<bool> CheckProfileExistAsync(string ownerEmail)
{
    return _staffDAO.CheckProfileExistAsync(ownerEmail);
}

public Task<bool> ChangePasswordStaffAsync(int staffId, string oldPassword, string newPassword, string confirmPassword)
{
    return _staffDAO.ChangePasswordStaffAsync(staffId, oldPassword, newPassword, confirmPassword);
}

public Task<bool> CreateStaffAsync(StaffCreateDTO staffCreateDTO)
{
    return _staffDAO.CreateStaffAsync(staffCreateDTO);
}

public Task<bool> UpdateStaffAsync(StaffDTO staffDTO)
{
    return _staffDAO.UpdateStaffAsync(staffDTO);
}

public Task<bool> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO)
{
    return _staffDAO.UpdateProfileStaffAsync(staffProfileDTO);
}

        public Task<bool> UpdateAvatarStaffAsync(StaffAvatarDTO staffAvatarDTO)
        {
            return _staffDAO.UpdateAvatarStaffAsync(staffAvatarDTO);
        }
public Task<bool> DeleteStaffAsync(int staffId)
{
    return _staffDAO.DeleteStaffAsync(staffId);
}



    }
}
