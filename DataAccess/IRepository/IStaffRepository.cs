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
    public interface  IStaffRepository
    {
        Task<List<StaffDTO>> GetAllStaffs(string? searchQuery, int page, int pageSize);
        Task<StaffDTO> GetStaffById(int staffId);
        Task<StaffDTO> GetStaffByEmail(string ownerEmail);

        Task<bool> CheckStaff(StaffDTO staffDTO);
       // Task<bool> CheckStaff(string,int staffDTO);

        Task<bool> CheckProfileStaff(StaffProfileDTO staffProfileDTO);
        Task<bool> ChangePasswordStaff(int staffId, string oldPassword, string newPasswod, string confirmPassword);

        Task<bool> CheckStaffExist(int staffId);

        Task<bool> CheckProfileExist(string ownerEmail);


        Task<bool> CreateStaff(StaffDTO staffDTO);

        Task<bool> UpdateStaff(StaffDTO staffDTO);
        Task<bool> UpdateProfileStaff(StaffProfileDTO staffProfileDTO);
        Task<bool> UpdateAvatarStaff(StaffAvatarDTO staffAvatarDTO);

        Task<bool> BanStaff(int staffId);
     

    }
}
