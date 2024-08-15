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
using Pipelines.Sockets.Unofficial.Buffers;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.DAOs
{
    public class GuestConsultationDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;



        public GuestConsultationDAO(NirvaxContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
 


        //create
        public async Task<bool> CheckGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
        {

            
            GuestConsultation? guestCreate = new GuestConsultation();
            guestCreate = await _context.GuestConsultations
                   .Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                .Where( i => i.OwnerId == guestConsultationCreateDTO.OwnerId )
                .SingleOrDefaultAsync(i => i.Phone.Trim() == guestConsultationCreateDTO.Phone.Trim() && i.AdId == guestConsultationCreateDTO.AdId);

            if (guestCreate == null)
            {
                return true;
            }
            return false;
        }

      
    //update
        public async Task<bool> CheckGuestConsultationExistAsync(int guestId ) 
        {
            GuestConsultation? sid = new GuestConsultation();

            sid = await _context.GuestConsultations.Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest).SingleOrDefaultAsync(i => i.GuestId  == guestId );

            if (sid == null)
            {
                return false;
            }
            return true;
        }



        public async Task<int> ViewGuestConsultationStatisticsAsync(int ownerId)
        {
            GuestConsultation guest = new GuestConsultation();
            var number = await _context.GuestConsultations.Where(i=> i.Owner.OwnerId == ownerId).CountAsync();
            return number;
        }



        //owner,staff or admin??
        public async Task<List<GuestConsultation>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId) 
        {
          
            IQueryable<GuestConsultation> query = _context.GuestConsultations.Include(i => i.Owner)
                                                       .Include(i => i.Ad)
                                                       .Include(i => i.StatusGuest)
                                                       .Where(i => i.OwnerId == ownerId);
 

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Fullname.Trim().Contains(searchQuery.Trim()) || i.Phone.Trim().Contains(searchQuery.Trim()));
            }

            query = query.OrderBy(i =>
               i.StatusGuest.Name == "WAITING" ? 1 :
               i.StatusGuest.Name == "ACCEPT" ? 2 :
               i.StatusGuest.Name == "DENY" ? 3 :
               4
           );

            List<GuestConsultation> getList = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

               
            return getList;
        }

        public async Task<List<GuestConsultation>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            List<GuestConsultation> getList = new List<GuestConsultation>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.GuestConsultations.Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Fullname.Trim().Contains(searchQuery.Trim()) || i.Phone.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.StatusGuest.Name.Contains("ACCEPT"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            else
            {
                 getList = await _context.GuestConsultations.Include(i => i.Owner).Include(i => i.Ad).Include(i => i.StatusGuest)
                    .Where(i => i.StatusGuest.Name.Contains("ACCEPT"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            return getList;
        }

        public async Task<List<GuestConsultation>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            List<GuestConsultation> getList = new List<GuestConsultation>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
              getList = await _context.GuestConsultations.Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Fullname.Trim().Contains(searchQuery.Trim()) || i.Phone.Trim().Contains(searchQuery.Trim()))
                      .Where(i => i.StatusGuest.Name.Contains("WAITING"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            else
            {
                 getList = await _context.GuestConsultations.Include(i => i.Owner).Include(i => i.Ad).Include(i => i.StatusGuest)
                    .Where(i => i.StatusGuest.Name.Contains("WAITING"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            return getList;
        }

        public async Task<List<GuestConsultation>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            List<GuestConsultation> getList = new List<GuestConsultation>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.GuestConsultations.Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                    .Where(i => i.Content.Trim().Contains(searchQuery.Trim()) || i.Fullname.Trim().Contains(searchQuery.Trim()) || i.Phone.Trim().Contains(searchQuery.Trim()))
                      .Where(i => i.StatusGuest.Name.Contains("DENY"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            else
            {
                getList = await _context.GuestConsultations.Include(i => i.Owner).Include(i => i.Ad).Include(i => i.StatusGuest)
                     .Where(i => i.StatusGuest.Name.Contains("DENY"))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            return getList;
        }


        //owner,staff 
        public async Task<GuestConsultation> GetGuestConsultationsByIdAsync(int guestId )

        {
           
            try
            {
                GuestConsultation? gs = await _context.GuestConsultations.Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest).SingleOrDefaultAsync(i => i.GuestId  == guestId );
               
                return gs;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        
        }

  
        public async Task<GuestConsultation> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO) 
        {
            GuestStatus guestStatus = await _context.GuestStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == "WAITING");
            GuestConsultation guestConsultation = _mapper.Map<GuestConsultation>(guestConsultationCreateDTO);
             guestConsultation.StatusGuestId = guestStatus.StatusGuestId;

            await _context.GuestConsultations.AddAsync(guestConsultation);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return guestConsultation;
            }
            else { return null; }

        }

        //admin

        //oldPass = xyz
        //newPass =1234
        public async Task<bool> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO)
        {
           
            GuestConsultation? staffOrgin = await _context.GuestConsultations
                   .Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                .SingleOrDefaultAsync(i => i.GuestId  == guestConsultationDTO.GuestId );
                _mapper.Map(guestConsultationDTO, staffOrgin);
                 _context.GuestConsultations.Update(staffOrgin);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<bool> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest)
        {
            GuestStatus guestStatus = await _context.GuestStatuses.SingleOrDefaultAsync(i => i.Name.Trim() == statusGuest.Trim());

            GuestConsultation? staffOrgin = await _context.GuestConsultations
                .Include(i => i.Owner)
                .Include(i => i.Ad)
                .Include(i => i.StatusGuest)
                .SingleOrDefaultAsync(i => i.GuestId == guestId);
            staffOrgin.StatusGuestId = guestStatus.StatusGuestId;
            _context.GuestConsultations.Update(staffOrgin);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}





