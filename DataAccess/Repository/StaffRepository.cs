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

       


        public Task<List<StaffDTO>> GetAllStaffs(string? searchQuery, int page, int pageSize)
        {

            return _staffDAO.GetAllStaffs(searchQuery, page, pageSize);
        }




    public Task<StaffDTO> GetStaffById(int staffId)
        {
            return _staffDAO.GetStaffById(staffId);
        }

public Task<StaffDTO> GetStaffByEmail(string ownerEmail)
{
    return (_staffDAO.GetStaffByEmail(ownerEmail));
}


public Task<bool> CheckStaff(StaffDTO staffDTO)
{
    return _staffDAO.CheckStaff(staffDTO);
}
public Task<bool> CheckStaffExist(int staffId)
{
    return _staffDAO.CheckStaffExist(staffId);
}
public Task<bool> CheckProfileStaff(StaffProfileDTO staffProfileDTO)
{
    return _staffDAO.CheckProfileStaff(staffProfileDTO);
}

public Task<bool> CheckProfileExist(string ownerEmail)
{
    return _staffDAO.CheckProfileExist(ownerEmail);
}

public Task<bool> ChangePasswordStaff(int staffId, string oldPassword, string newPasswod, string confirmPassword)
{
    return _staffDAO.ChangePasswordStaff(staffId, oldPassword, newPasswod, confirmPassword);
}

public Task<bool> CreateStaff(StaffDTO staffDTO)
{
    return _staffDAO.CreateStaff(staffDTO);
}

public Task<bool> UpdateStaff(StaffDTO staffDTO)
{
    return _staffDAO.UpdateStaff(staffDTO);
}

public Task<bool> UpdateProfileStaff(StaffProfileDTO staffProfileDTO)
{
    return _staffDAO.UpdateProfileStaff(staffProfileDTO);
}

        public Task<bool> UpdateAvatarStaff(StaffAvatarDTO staffAvatarDTO)
        {
            return _staffDAO.UpdateAvatarStaff(staffAvatarDTO);
        }
public Task<bool> BanStaff(int staffId)
{
    return _staffDAO.BanStaff(staffId);
}



    }
}
