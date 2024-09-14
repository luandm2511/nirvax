using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Azure;
using Azure.Core;
using System.Security.Cryptography;
using DataAccess.IRepository;
using System.Numerics;

namespace DataAccess.DAOs
{
    public class StaffDAO
    {

        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;
        



        public StaffDAO( NirvaxContext context, IMapper mapper)
        {
           
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckStaffAsync(int staffId, string email, string phone)
        {
            if (staffId == 0) 
            {
                bool emailExists = await _context.Staff
                    .AnyAsync(i=> i.Email.Trim() == email.Trim());
                bool emailExists2 = await _context.Owners
                    .AnyAsync(i => i.Email.Trim() == email.Trim());
                bool emailExists3 = await _context.Accounts
                    .AnyAsync(i => i.Email.Trim() == email.Trim());

                if (emailExists)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists2)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists3)
                {
                    throw new Exception("Email already exists!");
                }


                bool phoneExists = await _context.Staff
                    .AnyAsync(i => i.Phone == phone);
                bool phoneExists2 = await _context.Owners
                    .AnyAsync(i => i.Phone == phone);
                bool phoneExists3 = await _context.Accounts
                    .AnyAsync(i => i.Phone == phone);

                if (phoneExists)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists2)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists3)
                {
                    throw new Exception("Phone number already exists!");
                }
            }
            else 
            {
                bool emailExists = await _context.Staff
                    .Where(i => i.StaffId != staffId) 
                    .AnyAsync(i => i.Email.Trim() == email.Trim());

                bool emailExists2 = await _context.Owners
                    .AnyAsync(i => i.Email.Trim() == email.Trim());
                bool emailExists3 = await _context.Accounts
                    .AnyAsync(i => i.Email.Trim() == email.Trim());

                if (emailExists)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists2)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists3)
                {
                    throw new Exception("Email already exists!");
                }

                bool phoneExists = await _context.Staff
                    .Where(i => i.StaffId != staffId) 
                    .AnyAsync(i => i.Phone == phone);

                bool phoneExists2 = await _context.Owners
                    .AnyAsync(i => i.Phone == phone);
                bool phoneExists3 = await _context.Accounts
                    .AnyAsync(i => i.Phone == phone);


                if (phoneExists)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists2)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists3)
                {
                    throw new Exception("Phone number already exists!");
                }
            }

            return true; 
        }


        public async Task<bool> CheckProfileStaffAsync(StaffProfileDTO staffProfileDTO)
        {
            // StaffDTO checkownerDTO = new StaffDTO();
            Staff? Staff = new Staff();
            Staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffProfileDTO.StaffId);
            var ownerId = Staff.OwnerId;
            if (Staff != null)
            {
                bool emailExists = await _context.Staff
                   .Where(i => i.StaffId != staffProfileDTO.StaffId)
                   .AnyAsync(i => i.OwnerId == ownerId && i.Email.Trim() == staffProfileDTO.Email.Trim());

                bool emailExists2 = await _context.Owners
                   .AnyAsync(i => i.Email.Trim() == staffProfileDTO.Email.Trim());
                bool emailExists3 = await _context.Accounts
                    .AnyAsync(i => i.Email.Trim() == staffProfileDTO.Email.Trim());

                if (emailExists)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists2)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists3)
                {
                    throw new Exception("Email already exists!");
                }
                bool phoneExists = await _context.Staff
                    .Where(i => i.StaffId != staffProfileDTO.StaffId)
                    .AnyAsync(i => i.OwnerId == ownerId && i.Phone == staffProfileDTO.Phone);
                bool phoneExists2 = await _context.Owners
                    .AnyAsync(i => i.Phone == staffProfileDTO.Phone);
                bool phoneExists3 = await _context.Accounts
                    .AnyAsync(i => i.Phone == staffProfileDTO.Phone);


                if (phoneExists)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists2)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists3)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists)
                {
                    throw new Exception("Phone number already exists!");
                }

                    return true;
                
            }
            return false;
        }



        public async Task<bool> ChangePasswordStaffAsync(int staffId, string oldPassword, string newPassword, string confirmPassword)
        {
            //check password             
            Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
            if(sid == null) { throw new Exception("Not found this staff!"); }
            if (oldPassword.Trim() == newPassword.Trim())
            {
                throw new Exception("The password you want to change is the same as the old password. Please enter the new password!");
            }
            if (newPassword.Trim().Length == 0 || newPassword.Trim().Length == 0 || confirmPassword.Trim().Length == 0)
            {
                throw new Exception("Don't accept empty information!");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(oldPassword, sid.Password);
            
            if (verified == true)
            {
                if(newPassword.Trim() == confirmPassword.Trim())
                {
                    string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    sid.Password = newpasswordHash;
                    _context.Staff.Update(sid);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    
                       throw new Exception("Your new password and confirm password not match!");
                    
                }
               
            }
            else
            {

                throw new Exception("Enter wrong your old password!");

            }
        }


        //owner
        public async Task<IEnumerable<Staff>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<Staff>(); }
            List<Staff> listStaff = new List<Staff>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                listStaff = await _context.Staff.Include(i => i.Owner)
                  //  .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()) || i.Email.Trim().Contains(searchQuery.Trim()) || i.Phone.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                
            }
            else
            {
                listStaff = await _context.Staff.Include(i => i.Owner)
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
              
            }
            return listStaff;
        }

        //details
        public async Task<Staff> GetStaffByIdAsync(int staffId)
        {
               
                Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
               
                return sid;        
        }

        //profile
        public async Task<StaffDTO> ViewStaffProfileAsync(string staffEmail)
        {
                StaffDTO staff = new StaffDTO();
                Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.Email.Trim() == staffEmail.Trim());
            staff = _mapper.Map<StaffDTO>(sid);
                return staff;       
        }



        public async Task<bool> CreateStaffAsync(StaffCreateDTO staffCreateDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == staffCreateDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(staffCreateDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }

            string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(staffCreateDTO.Password);
            staffCreateDTO.Password = newpasswordHash;
            Staff staff = _mapper.Map<Staff>(staffCreateDTO);
            

            await _context.Staff.AddAsync(staff);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        //admin

        //oldPass = xyz
        //newPass =1234
        public async Task<bool> UpdateStaffAsync(StaffDTO staffDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == staffDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(staffDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }

            StaffDTO newStaff;
            newStaff = staffDTO;
            Staff? staffOrgin = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffDTO.StaffId);
            bool verified = BCrypt.Net.BCrypt.Verify(staffDTO.Password,staffOrgin.Password); 
            if(verified == false)
            {
                string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(staffDTO.Password);
                staffDTO.Password = newpasswordHash;
            } else
            {
                staffDTO.Password = staffOrgin.Password;
            }
              // staffOrgin.Image = staffDTO.
              
                _mapper.Map(staffDTO, staffOrgin);
            
                 _context.Staff.Update(staffOrgin);
                await _context.SaveChangesAsync();
                return true;
           

        }

        //owner
        public async Task<bool> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(staffProfileDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }

            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffProfileDTO.StaffId);
            staff.OwnerId = staff.OwnerId;
            staff.Image = staff.Image;
            staff.Password = staff.Password;
           
            _mapper.Map(staffProfileDTO, staff);
                
                 _context.Staff.Update(staff);
                await _context.SaveChangesAsync();
                return true;
          

        }
        public async Task<bool> UpdateAvatarStaffAsync(StaffAvatarDTO staffAvatarDTO)
        {
          
            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffAvatarDTO.StaffId);
            if(staffAvatarDTO.Image == null)
            {
                throw new Exception("Don't accept null image!");
            }
            staff.Image = staffAvatarDTO.Image;
          

           // _mapper.Map(staffAvatarDTO, staff);
             _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task<bool> DeleteStaffAsync(int staffId)
        {
            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
            if (staff != null)
            {           
                 _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }
        
    }
}





