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
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.DAOs
{
    public  class DescriptionDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;



        public DescriptionDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }
  
        public async Task<bool> CheckDescriptionAsync(int descriptionId, string title, string content)
        {
            if (descriptionId == 0)
            {
                Description? des = new Description();
                des = await _context.Descriptions.SingleOrDefaultAsync(i => i.Content.Trim() == content.Trim() || i.Title.Trim() == title.Trim());
                if (des == null)
                {
                    return true;
                }
            }
            else
            {
                List<Description> getList = await _context.Descriptions
      
                .Where(i => i.DescriptionId != descriptionId)
               // .Where(i => i.Content.Trim() == content.Trim())
                .Where(i => i.Title.Trim() == title.Trim())
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
        //owner,staff
        public async Task<List<Description>> GetAllDescriptionsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<Description>(); }
            List < Description> getList = new List<Description> ();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.Descriptions
                   .Where(i => i.Isdelete == false)
                  .Include(i => i.Images)
                  .Include(i => i.Products)
                  .Where(i => i.OwnerId == ownerId)
                    .Where(i => i.Title.Trim().Contains(searchQuery.Trim()))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
              
            }
            else
            {
               getList = await _context.Descriptions
                    .Where(i => i.Isdelete == false)
                   .Include(i => i.Images)
                  .Include(i => i.Products)
                  .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            return getList;
        }

        public async Task<List<Description>> GetDescriptionForUserAsync(string? searchQuery)
        {
            List<Description> getList = new List<Description>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.Descriptions
                     .Include(i => i.Images)
                  .Include(i => i.Products)
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.Title.Trim().Contains(searchQuery.Trim()))
                    .ToListAsync();
              
            }
            else
            {
                getList = await _context.Descriptions
                     .Include(i => i.Images)
                  .Include(i => i.Products)
                    .Where(i => i.Isdelete == false)                  
                    .ToListAsync();
                
            }
            return getList;
        }

        public async Task<Description> GetDescriptionByIdAsync(int descriptionId)
        {
               
                Description? des = await _context.Descriptions.Include(i => i.Images).Include(i => i.Products).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.DescriptionId == descriptionId);               
      
                return des;       
        }


        public async Task<Description> CreateDesctiptionAsync(DescriptionCreateDTO descriptionCreateDTO)
        {

            Description description = _mapper.Map<Description>(descriptionCreateDTO);
            description.Isdelete = false;
            await _context.Descriptions.AddAsync(description);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return description;
            }
            else { return null; }

        }

        public async Task<Description> UpdateDesctiptionAsync(DescriptionDTO descriptionDTO)
        {
            Description? description = await _context.Descriptions.Include(i => i.Images)
                  .Include(i => i.Products).SingleOrDefaultAsync(i => i.DescriptionId == descriptionDTO.DescriptionId);
            //ánh xạ đối tượng DescriptionDTO đc truyền vào cho staff
            descriptionDTO.Isdelete = false;
                _mapper.Map(descriptionDTO, description);
                 _context.Descriptions.Update(description);
                await _context.SaveChangesAsync();
                return description;
        }

        public async Task<bool> DeleteDesctiptionAsync(int descriptionId)
        {
            Description? description = await _context.Descriptions.SingleOrDefaultAsync(i => i.DescriptionId == descriptionId);

            if (description != null)
            {
                description.Isdelete = true;
                _context.Descriptions.Update(description);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





