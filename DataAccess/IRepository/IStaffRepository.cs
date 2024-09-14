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
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<Staff> GetStaffByIdAsync(int staffId);
        Task<StaffDTO> ViewStaffProfileAsync(string ownerEmail);
        Task<bool> CheckStaffAsync(int staffId, string email, string phone);
        Task<bool> CheckProfileStaffAsync(StaffProfileDTO staffProfileDTO);
        Task<bool> ChangePasswordStaffAsync(int staffId, string oldPassword, string newPassword, string confirmPassword);
        Task<bool> CreateStaffAsync(StaffCreateDTO staffCreateDTO);
        Task<bool> UpdateStaffAsync(StaffDTO staffDTO);
        Task<bool> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO);
        Task<bool> UpdateAvatarStaffAsync(StaffAvatarDTO staffAvatarDTO);
        Task<bool> DeleteStaffAsync(int staffId);
    }
}
