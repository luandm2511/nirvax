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
                List<Owner> getList = await _context.Owners
                 .Where(i => i.IsBan == false)
         
                 .Where(i => i.OwnerId != ownerDTO.OwnerId)
                 .Where(i => i.Email == ownerDTO.Email || i.Phone == ownerDTO.Phone)
                 .ToListAsync();
                if (getList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            // OwnerDTO checkownerDTO = new OwnerDTO();
            Owner? owner = new Owner();
            owner = await _context.Owners.SingleOrDefaultAsync(i => i.OwnerId == ownerProfileDTO.OwnerId);
            if (owner != null)
            {
                List<Owner> getList = await _context.Owners
                // .Where(i => i.IsBan == false)
         
                 .Where(i => i.OwnerId != ownerProfileDTO.OwnerId)
                 .Where(i => i.Email == ownerProfileDTO.Email || i.Phone == ownerProfileDTO.Phone)
                 .ToListAsync();
                if (getList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        //check xem owner đó có tồn tại hay không
        public async Task<bool> CheckOwnerExistAsync(int ownerId)
        {
            Owner? sid = new Owner();

            sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);

            if (sid == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckProfileExistAsync(string ownerEmail)
        {
            Owner? sid = new Owner();

            sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.Email == ownerEmail);

            if (sid == null)
            {
                return false;
            }
            return true;
        }

       

        public async Task<bool> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPassword,string confirmPassword)
        { 
                //check password             
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);  
               bool verified = BCrypt.Net.BCrypt.Verify(oldPassword, sid.Password);
                if (verified == true)
                {
                if (newPassword == confirmPassword)
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
        public async Task<List<Owner>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize)
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
        public async Task<List<Owner>> GetAllOwnersForUserAsync(string? searchQuery)
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
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.Email == ownerEmail);
            owner = _mapper.Map<OwnerDTO>(sid);
                return owner;      
        }



        public async Task<bool> CreateOwnerAsync(OwnerDTO ownerDTO)
        {
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
            owner.Image = ownerAvatarDTO.Image;
         
             _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task<bool> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerDTO.OwnerId);
            _mapper.Map(ownerDTO, owner);
            string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(owner.Password);
            owner.Password = newpasswordHash;
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return true;


        }

        //owner
        public async Task<bool> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerProfileDTO.OwnerId);

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




