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
            owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerProfileDTO.OwnerId);
            if (owner != null)
            {
                List<Owner> getList = await _context.Owners
                 .Where(i => i.IsBan == false)
         
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

       

        public async Task<bool> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPasswod)
        { 
                //check password             
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);  
               bool verified = BCrypt.Net.BCrypt.Verify(oldPassword, sid.Password);
                if (verified == true)
                {
                    string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(newPasswod);
                    sid.Password = newpasswordHash;
                    // Ánh xạ dữ liệu từ OwnerDTO sang Owner
                    //_mapper.Map(, sid);
                     _context.Owners.Update(sid);
                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
        }


        //admin
        public async Task<List<OwnerDTO>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize)
        {
            List<OwnerDTO> listOwnerDTO = new List<OwnerDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Owner> getList = await _context.Owners
                    .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()))
                   
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                      
                    .ToListAsync();
                listOwnerDTO = _mapper.Map<List<OwnerDTO>>(getList);
            }
            else
            {
                List<Owner> getList = await _context.Owners
                 
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listOwnerDTO = _mapper.Map<List<OwnerDTO>>(getList);
            }
            return listOwnerDTO;
        }

        //user
        public async Task<List<OwnerDTO>> GetAllOwnersForUserAsync(string? searchQuery)
        {
            List<OwnerDTO> listOwnerDTO = new List<OwnerDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Owner> getList = await _context.Owners
                   .Where(i => i.Fullname.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.IsBan == false)
                    .ToListAsync();
                listOwnerDTO = _mapper.Map<List<OwnerDTO>>(getList);
            }
            else
            {
                List<Owner> getList = await _context.Owners
                     .Where(i => i.IsBan == false)
                    .ToListAsync();
                listOwnerDTO = _mapper.Map<List<OwnerDTO>>(getList);
            }
            return listOwnerDTO;
        }

        //admin
        public async Task<OwnerDTO> GetOwnerByIdAsync(int ownerId)
        {
                OwnerDTO ownerDTO = new OwnerDTO();
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerId);
                ownerDTO = _mapper.Map<OwnerDTO>(sid);
                return ownerDTO;  
        }

        //profile
        public async Task<OwnerDTO> ViewOwnerProfileAsync(string ownerEmail)
        {
                OwnerDTO ownerDTO = new OwnerDTO();
                Owner? sid = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.Email == ownerEmail);
                ownerDTO = _mapper.Map<OwnerDTO>(sid);
                return ownerDTO;      
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
        public async Task<bool> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerDTO.OwnerId);
                _mapper.Map(ownerDTO, owner);
                 _context.Owners.Update(owner);
                await _context.SaveChangesAsync();
                return true;
     

        }

        public async Task<bool> UpdateAvatarOwnerAsync(OwnerAvatarDTO ownerAvatarDTO)
        {
            
            Owner? owner = await _context.Owners.Where(i => i.IsBan == false).SingleOrDefaultAsync(i => i.OwnerId == ownerAvatarDTO.OwnerId);
            owner.Image = ownerAvatarDTO.Image;
           
          
            _mapper.Map(ownerAvatarDTO, owner);
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




