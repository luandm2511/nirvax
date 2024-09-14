using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using DataAccess.IRepository;
using Pipelines.Sockets.Unofficial.Buffers;

namespace DataAccess.DAOs
{
    public class OwnerDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
      //  private IFileService _fileService;




        public  OwnerDAO(NirvaxContext context, IMapper mapper)
        {
           // this._fileService = fs;
             _context = context;
            _mapper = mapper;
        }



        // thống kê tổng tài khoản owner
        public async Task<int> NumberOfOwnerStatisticsAsync()
        {
            var number = await _context.Owners.Where(i=> i.IsBan == false).CountAsync();
            return number;
        }


        public async Task<bool> CheckOwnerAsync(OwnerDTO ownerDTO)
        {
            // OwnerDTO checkownerDTO = new OwnerDTO();
            Owner? owner = new Owner();
            owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerDTO.OwnerId);
            if (owner != null)
            {
                bool emailExists = await _context.Owners
                    .Where(i => i.OwnerId != ownerDTO.OwnerId)
                    .AnyAsync(i => i.Email.Trim() == ownerDTO.Email.Trim());

                bool emailExists2 = await _context.Staff
                    .AnyAsync(i => i.Email.Trim() == ownerDTO.Email.Trim());

                bool emailExists3 = await _context.Accounts
                    .AnyAsync(i => i.Email.Trim() == ownerDTO.Email.Trim());

                if (emailExists)
                {
                    throw new Exception("Email is already exist!");
                }
                if (emailExists2)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists3)
                {
                    throw new Exception("Email already exists!");
                }


                bool phoneExists = await _context.Owners
                    .Where(i => i.OwnerId != ownerDTO.OwnerId)
                    .AnyAsync(i => i.Phone == ownerDTO.Phone);
                bool phoneExists2 = await _context.Staff
                    .AnyAsync(i => i.Phone == ownerDTO.Phone);

                bool phoneExists3 = await _context.Accounts
                    .AnyAsync(i => i.Phone == ownerDTO.Phone);

                if (phoneExists)
                {
                    throw new Exception("Phone number is already exist!");
                }
                if (phoneExists2)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists3)
                {
                    throw new Exception("Phone number already exists!");
                }

                bool nameExists = await _context.Owners
                   .Where(i => i.OwnerId != ownerDTO.OwnerId)
                   .AnyAsync(i => i.Fullname.Trim() == ownerDTO.Fullname.Trim());

                if (nameExists)
                {
                    throw new Exception("This name is already registered by another shop!");
                }

                return true;
            }
            return false;
        }

        public async Task<bool> CheckProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            Owner? owner = await _context.Owners.SingleOrDefaultAsync(i => i.OwnerId == ownerProfileDTO.OwnerId);
            if (owner != null)
            {               
                bool emailExists = await _context.Owners
                    .Where(i => i.OwnerId != ownerProfileDTO.OwnerId) 
                    .AnyAsync(i => i.Email.Trim() == ownerProfileDTO.Email.Trim());

                bool emailExists2 = await _context.Staff
                    .AnyAsync(i => i.Email.Trim() == ownerProfileDTO.Email.Trim());

                bool emailExists3 = await _context.Accounts
                    .AnyAsync(i => i.Email.Trim() == ownerProfileDTO.Email.Trim());

                if (emailExists)
                {
                    throw new Exception("Email is already exist!");
                }

                if (emailExists2)
                {
                    throw new Exception("Email already exists!");
                }
                if (emailExists3)
                {
                    throw new Exception("Email already exists!");
                }

                bool phoneExists = await _context.Owners
                    .Where(i => i.OwnerId != ownerProfileDTO.OwnerId) 
                    .AnyAsync(i => i.Phone == ownerProfileDTO.Phone);

                bool phoneExists2 = await _context.Staff
                   .AnyAsync(i => i.Phone == ownerProfileDTO.Phone);

                bool phoneExists3 = await _context.Accounts
                    .AnyAsync(i => i.Phone == ownerProfileDTO.Phone);

                if (phoneExists)
                {
                    throw new Exception("Phone number is already exist!");
                }
                if (phoneExists2)
                {
                    throw new Exception("Phone number already exists!");
                }
                if (phoneExists3)
                {
                    throw new Exception("Phone number already exists!");
                }

                bool nameExists = await _context.Owners
                   .Where(i => i.OwnerId != ownerProfileDTO.OwnerId)
                   .AnyAsync(i => i.Fullname.Trim() == ownerProfileDTO.Fullname.Trim());

                if (nameExists)
                {
                    throw new Exception("This Name is already exist!");
                }

                return true;
            }

            return false;
        }





        public async Task<bool> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPassword,string confirmPassword)
        { 
                //check password             
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);  
               if (sid == null) { throw new Exception("Not found this owner!"); }
               if(oldPassword.Trim() == newPassword.Trim())
            {
                throw new Exception("The password you want to change is the same as the old password. Please enter the new password!");
            }
               if(newPassword.Trim().Length == 0 || newPassword.Trim().Length == 0 || confirmPassword.Trim().Length== 0) { 
                    throw new Exception("Don't accept empty information!");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(oldPassword, sid.Password);
                if (verified == true)
                {
                if (newPassword.Trim() == confirmPassword.Trim())
                {
                    string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    sid.Password = newpasswordHash;
                    _context.Owners.Update(sid);
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


        //admin
        public async Task<IEnumerable<Owner>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize)
        {
          
            List<Owner> getList = new List<Owner>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.Owners
                    .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()))
                   
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                      
                    .ToListAsync();
               
            }
            else
            {
                getList = await _context.Owners
                 
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            return getList;
        }

        //user
        public async Task<IEnumerable<Owner>> GetAllOwnersForUserAsync(string? searchQuery)
        {
            List<Owner> getList = new List<Owner>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.Owners
                   .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.IsBan == false)
                    .ToListAsync();
               
            }
            else
            {
                getList = await _context.Owners
                     .Where(i => i.IsBan == false)
                    .ToListAsync();
                
            }
            return getList;
        }

        //admin
        public async Task<Owner> GetOwnerByIdAsync(int ownerId)
        {
              
                Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);
               
                return owner;  
        }

        //profile
        public async Task<OwnerDTO> ViewOwnerProfileAsync(string ownerEmail)
        {
            OwnerDTO owner = new OwnerDTO();
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.Email.Trim() == ownerEmail.Trim());
            owner = _mapper.Map<OwnerDTO>(sid);
                return owner;      
        }



        public async Task<bool> CreateOwnerAsync(OwnerDTO ownerDTO)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(ownerDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }
            string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(ownerDTO.Password);
            Owner owner = _mapper.Map<Owner>(ownerDTO);
            owner.IsBan = false;
            owner.Password = newpasswordHash;
            await _context.Owners.AddAsync(owner);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        //admin
    

        public async Task<bool> UpdateAvatarOwnerAsync(OwnerAvatarDTO ownerAvatarDTO)
        {
            
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerAvatarDTO.OwnerId);
            if (ownerAvatarDTO.Image == null)
            {
                throw new Exception("Don't accept null image!");
            }
            owner.Image = ownerAvatarDTO.Image;
         
             _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task<bool> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(ownerDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerDTO.OwnerId);
            _mapper.Map(ownerDTO, owner);
            string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(owner.Password);
            owner.Password = newpasswordHash;
            owner.IsBan = false;
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return true;


        }

        //owner
        public async Task<bool> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(ownerProfileDTO.Email, emailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }

            Owner? owner = await _context.Owners.Where(i => i.IsBan == false)
                                                .SingleOrDefaultAsync(i => i.OwnerId == ownerProfileDTO.OwnerId);

            if (owner == null)
            {
                throw new InvalidOperationException("Owner not found.");
            }

            owner.Image = owner.Image;
            owner.Password = owner.Password;
            _mapper.Map(ownerProfileDTO, owner);

            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanOwnerAsync(int ownerId)
        {
            Owner? owner = await _context.Owners.SingleOrDefaultAsync(i => i.OwnerId == ownerId);
            if (owner != null)
            {
                owner.IsBan = true;
                 _context.Owners.Update(owner);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> GetEmailAsync(int ownerId)
        {
          var owner = await _context.Owners.SingleOrDefaultAsync(i => i.OwnerId == ownerId);
            var ownerEmail = owner.Email;
            return ownerEmail;
        }


        public async Task<bool> UnBanOwnerAsync(int ownerId)
        {
            Owner? owner = await _context.Owners.SingleOrDefaultAsync(i => i.OwnerId == ownerId);
            if (owner != null)
            {
                owner.IsBan = false;
                 _context.Owners.Update(owner);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}




