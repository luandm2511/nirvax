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
using Pipelines.Sockets.Unofficial.Buffers;


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
                                           .Where(i => i.AdId != advertisementDTO.AdId)
                                           .Where(i => i.Title.Trim() == advertisementDTO.Title.Trim())
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
                .SingleOrDefaultAsync(i => i.Title.Trim() == advertisementCreateDTO.Title.Trim());
           
             if (StaffCreate != null)
            {

                return false;

            }
            return true;
        }

        public async Task<IEnumerable<AdvertisementDTO>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize) 
        {
          
            List<Advertisement> listAdvertisement = new List<Advertisement>();

            IQueryable<Advertisement> query = _context.Advertisements
                                                       .Include(i => i.Owner)
                                                       .Include(i => i.Service)
                                                       .Include(i => i.StatusPost);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()) || i.Owner.Fullname.Contains(searchQuery.Trim()));
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
            var getListDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            return getListDTO;
        }

        public async Task<IEnumerable<AdvertisementDTO>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
        {
            List<Advertisement> getList = new List<Advertisement>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("WAITING"))               
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();             
            }
            else
            {
                 getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.StatusPost.Name.Contains("WAITING"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();            
            }
            var getListDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            return getListDTO;
        }
        
        public async Task<IEnumerable<AdvertisementDTO>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            List<Advertisement> getList = new List<Advertisement>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                  getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("DENY"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();          
            }
            else
            {
                   getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.StatusPost.Name.Contains("DENY"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();             
            }
            var getListDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            return getListDTO;
        }
       
        public async Task<IEnumerable<AdvertisementDTO>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            List<Advertisement> getList = new List<Advertisement>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();            
            }
            else
            {
                 getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();             
            }
            var getListDTO = _mapper.Map<List<AdvertisementDTO>>(getList);
            return getListDTO;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            List<Advertisement> getList = new List<Advertisement>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                   .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();             
            }
            else
            {
             getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .ToListAsync();           
            }
            return getList;
        }

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<Advertisement>(); }
            List<Advertisement> getList = new List<Advertisement>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
               getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Title.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i=> i.OwnerId == ownerId)
                    .ToListAsync();             
            }
            else
            {
               getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i => i.OwnerId == ownerId)
                    .ToListAsync();           
            }
            return getList;
        }

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
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

            List<Advertisement> getList = await query.Skip((page - 1) * pageSize)
                                                     .Take(pageSize)
                                                     .ToListAsync();
            return getList;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {
        
                List<Advertisement> getList = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost)
                    .Where(i => i.StatusPost.Name.Contains("ACCEPT"))
                    .Where(i => i.ServiceId == serviceId)
                    .ToListAsync();    
            return getList;
        }
 
        public async Task<Advertisement> GetAdvertisementByIdAsync(int adId)

        {         
                Advertisement? ads = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost).SingleOrDefaultAsync(i => i.AdId == adId);              
                return ads;                  
        }

        public async Task<Advertisement> GetAdvertisementByIdForUserAsync(int adId) 
        {
                Advertisement? ads = await _context.Advertisements.Include(i => i.Owner).Include(i => i.Service).Include(i => i.StatusPost).Where(i => i.StatusPost.Name.Contains("ACCEPT")).SingleOrDefaultAsync(i => i.AdId == adId);
                return ads;           
        }

  
        public async Task<bool> CreateAdvertisementAsync(AdvertisementCreateDTO advertisementCreateDTO) 
        {
                PostStatus postStatus = await _context.PostStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == "WAITING");                
                Advertisement advertisement = _mapper.Map<Advertisement>(advertisementCreateDTO);
                advertisement.StatusPostId =  postStatus.StatusPostId;
                await _context.Advertisements.AddAsync(advertisement);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                } else return false;
        }

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

        public async Task<Advertisement> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {       
            PostStatus postStatus = await _context.PostStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == statusPost.Trim());
            Advertisement? adOrgin = await _context.Advertisements
                .Include(i => i.Owner)
                .Include(i => i.Service)
                .Include(i => i.StatusPost)
                .SingleOrDefaultAsync(i => i.AdId == adId);
            if(adOrgin == null) { throw new Exception("Not found this ads!"); }
            adOrgin.StatusPostId = postStatus.StatusPostId;
            _context.Advertisements.Update(adOrgin);
            await _context.SaveChangesAsync();
            return adOrgin;
        }

        public async Task<object> ViewAdversisementStatisticsAsync(int ownerId)
        {
            var number = await _context.Advertisements.Where(i => i.OwnerId == ownerId).CountAsync();
            var number2 = await _context.Advertisements.Include(i => i.Owner).CountAsync();
            var result = new Dictionary<string, object>
                             {
                                  { "totalAdversisement", number },
                                  { "totalOwnerAdversisement", number2 }
                             };
           return result;
        }
    }
}





