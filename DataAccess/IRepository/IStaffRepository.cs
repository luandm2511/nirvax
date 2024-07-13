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
        Task<List<StaffDTO>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<StaffDTO> GetStaffByIdAsync(int staffId);
        Task<StaffDTO> GetStaffByEmailAsync(string ownerEmail);

     //   Task<bool> CheckStaffAsync(StaffDTO staffDTO);
        Task<bool> CheckStaffAsync(int staffId, string email, string phone, int ownerId);


        Task<bool> CheckProfileStaffAsync(StaffProfileDTO staffProfileDTO);
        Task<bool> ChangePasswordStaffAsync(int staffId, string oldPassword, string newPasswod, string confirmPassword);

        Task<bool> CheckStaffExistAsync(int staffId);

        Task<bool> CheckProfileExistAsync(string ownerEmail);


        Task<bool> CreateStaffAsync(StaffCreateDTO staffCreateDTO);

        Task<bool> UpdateStaffAsync(StaffDTO staffDTO);
        Task<bool> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO);
        Task<bool> UpdateAvatarStaffAsync(StaffAvatarDTO staffAvatarDTO);

        Task<bool> DeleteStaffAsync(int staffId);
     

    }
}
