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
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.DAOs
{
    public class AdvertisementDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;


        public AdvertisementDAO( NirvaxContext context, IMapper mapper)
        {
            
             _context = context;
            _mapper = mapper;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
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
     



    


        //get all for owner,staff 
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize) 
        {
            List<AdvertisementDTO> listAdvertisementDTO = new List<AdvertisementDTO>();

            IQueryable<Advertisement> query = _context.Advertisements
                                                       .Include(i => i.Owner)
                                                       .Include(i => i.Service)
                                                       .Include(i => i.StatusPost);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()));
            }

           
            query = query.OrderBy(i =>
                i.StatusPost.Name == "WAITING" ? 1 :
                i.StatusPost.Name == "ACCEPT" ? 2 :
                i.StatusPost.Name == "DENY" ? 3 :
                4 
            );

            List<Advertisement> getList = await query.Skip((page - 1) * pageSize)
                                                     .Take(pageSize)
                                                     .ToListAsync();

            listAdvertisementDTO = _mapper.Map<List<AdvertisementDTO>>(getList);

            return listAdvertisementDTO;
        }

        // get status waiting
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("WAITING"))               
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.StatusPost.Name.Contains("WAITING"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listAdDTO;
        }
        //get status deny
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("DENY"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.StatusPost.Name.Contains("DENY"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listAdDTO;
        }
        //get status accept
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listAdDTO;
        }

        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listAdDTO;
        }

        //user
        public async Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i=> i.OwnerId == ownerId)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            else
            {
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i => i.OwnerId == ownerId)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            }
            return listAdDTO;
        }

        public async Task<List<AdvertisementDTO>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {

            List<AdvertisementDTO> listAdvertisementDTO = new List<AdvertisementDTO>();

            IQueryable<Advertisement> query = _context.Advertisements
                                                       .Include(i => i.Owner)
                                                       .Include(i => i.Service)
                                                       .Include(i => i.StatusPost)
                                                       .Where(i => i.OwnerId == ownerId);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()));
            }


            query = query.OrderBy(i =>
                i.StatusPost.Name == "WAITING" ? 1 :
                i.StatusPost.Name == "ACCEPT" ? 2 :
                i.StatusPost.Name == "DENY" ? 3 :
                4
            );

            // Áp dụng phân trang
            List<Advertisement> getList = await query.Skip((page - 1) * pageSize)
                                                     .Take(pageSize)
                                                     .ToListAsync();

            // Ánh xạ từ Advertisement sang AdvertisementDTO
            listAdvertisementDTO = _mapper.Map<List<AdvertisementDTO>>(getList);

            return listAdvertisementDTO;
        }

        //service
        public async Task<List<AdvertisementDTO>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {
            List<AdvertisementDTO> listAdDTO = new List<AdvertisementDTO>();
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i => i.ServiceId == serviceId)
                    .ToListAsync();
                listAdDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            
            return listAdDTO;
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
                PostStatus postStatus = await _context.PostStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == "WAITING");                
                Advertisement advertisement = _mapper.Map<Advertisement>(advertisementCreateDTO);
                advertisement.StatusPostId = postStatus.StatusPostId;
                await _context.Advertisements.AddAsync(advertisement);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                } else return false;

        }

        //staff, owner
        public  async Task<bool> UpdateAdvertisementAsync(AdvertisementDTO advertisementDTO)
        {
            Advertisement? adOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == advertisementDTO.AdId);
            if(adOrgin != null && (adOrgin.StatusPost.Name == "ACCEPT" || adOrgin.StatusPost.Name == "DENY"))
            {
                throw new Exception("Can not update after the admin approved");
            }
            _mapper.Map(advertisementDTO, adOrgin);
                 _context.Advertisements.Update(adOrgin);
                await _context.SaveChangesAsync();
         
                return true;
        }

        public async Task<bool> UpdateStatusAdvertisementByIdAsync(int adId, int statusPostId)
        {

            Advertisement? adOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == adId);
            adOrgin.StatusPostId = statusPostId;
             _context.Advertisements.Update(adOrgin);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {
            PostStatus postStatus = await _context.PostStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == statusPost.Trim());
            Advertisement? adOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == adId);
            adOrgin.StatusPostId = postStatus.StatusPostId;
            _context.Advertisements.Update(adOrgin);
            await _context.SaveChangesAsync();
            return adOrgin;
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





