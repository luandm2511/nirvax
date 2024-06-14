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

namespace DataAccess.DAOs
{
    public class AdvertisementDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        


        public AdvertisementDAO( NirvaxContext context, IMapper mapper)
        {
            
             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {

            Advertisement? Advertisement = new Advertisement();
            Advertisement = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == advertisementDTO.AdId);
          
            

            if (Advertisement != null)
            {
                List<Advertisement> getList = await _context.Advertisements
                 //check khác Id`
                 .Where(i => i.AdId != advertisementDTO.AdId)
                .Where(i => i.Title.Trim() == advertisementDTO.Title.Trim() || i.Content.Trim() == advertisementDTO.Content.Trim())
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

        public async Task<bool> CheckAdvertisementCreateAsync(AdvertisementCreateDTO advertisementCreateDTO)
        { 


            Advertisement? StaffCreate = new Advertisement();
            StaffCreate = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.Title == advertisementCreateDTO.Title || i.Content.Trim() == advertisementCreateDTO.Content.Trim());

           
             if (StaffCreate == null)
            {

                return true;
            }
            return false;
        }



        public async Task<bool> CheckAdvertisementExistAsync(int adId) 
        {
            Advertisement? sid = new Advertisement();

            sid = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost).SingleOrDefaultAsync(i => i.AdId == adId);

            if (sid == null)
            {
                return false;
            }
            return true;
        }
     



    


        //owner,staff or admin??
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize) 
        {
            List<AdvertisementDTO> listStaffDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i=>i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Contains(searchQuery) || i.Title.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listStaffDTO;
        }

        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            List<AdvertisementDTO> listStaffDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Contains(searchQuery) || i.Title.Contains(searchQuery))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listStaffDTO;
        }

        //owner,staff 
        public async Task<AdvertisementDTO> GetAdvertisementByIdAsync(int adId)

        {
            AdvertisementDTO advertisementDTO = new AdvertisementDTO();
            try
            {
                Advertisement? sid = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost).SingleOrDefaultAsync(i => i.AdId == adId);
                advertisementDTO = _mapper.Map<AdvertisementDTO>(sid);
                return advertisementDTO;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        
        }

        public async Task<AdvertisementDTO> GetAdvertisementByIdForUserAsync(int adId) 
        {
            AdvertisementDTO advertisementDTO = new AdvertisementDTO();
            try
            {
                Advertisement? sid = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost).Where(i => i.StatusPost.Name.Contains("POSTING")).SingleOrDefaultAsync(i => i.AdId == adId);
                advertisementDTO = _mapper.Map<AdvertisementDTO>(sid);
                return advertisementDTO;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

  
        public async Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO) 
        {
           
                Advertisement advertisement = _mapper.Map<Advertisement>(advertisementCreateDTO);
                await _context.Advertisements.AddAsync(advertisement);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                } else return false;

        }

        
        public  async Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {
            
       
            Advertisement? staffOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == advertisementDTO.AdId);
            
            _mapper.Map(advertisementDTO, staffOrgin);
                 _context.Advertisements.Update(staffOrgin);
                await _context.SaveChangesAsync();
         
                return true;
        }

        public async Task<bool> UpdateStatusAdvertisementAsync(int adId, int statusPostId)
        {

            Advertisement? staffOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == adId);
            staffOrgin.StatusPostId = statusPostId;
             _context.Advertisements.Update(staffOrgin);
            await _context.SaveChangesAsync();
            return true;
        }

        //riêng
        public async Task<int> ViewOwnerBlogStatisticsAsync(int ownerId)
        {
            Advertisement ad = new Advertisement();
            var number = await _context.Advertisements.Where(i => i.OwnerId == ownerId).CountAsync();
            return number;
        }

        //chung
        public async Task<int> ViewBlogStatisticsAsync()
        {
            Advertisement ad = new Advertisement();
            var number = await _context.Advertisements.Include(i=>i.Owner).CountAsync();
            return number;
        }

    }
}





