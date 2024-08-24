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
    public  class SizeChartDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;



        public SizeChartDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }
  
        public async Task<bool> CheckSizeChartAsync(int descriptionId, string title, string content)
        {
            if (descriptionId == 0)
            {
                SizeChart? des = new SizeChart();
                des = await _context.SizeCharts.SingleOrDefaultAsync(i => i.Content.Trim() == content.Trim() || i.Title.Trim() == title.Trim());
                if (des == null)
                {
                    return true;
                }
            }
            else
            {
                List<SizeChart> getList = await _context.SizeCharts
      
                .Where(i => i.SizeChartId != descriptionId)
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
        public async Task<List<SizeChart>> GetAllSizeChartsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<SizeChart>(); }
            List < SizeChart> getList = new List<SizeChart> ();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.SizeCharts
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
               getList = await _context.SizeCharts
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

        public async Task<List<SizeChart>> GetSizeChartForUserAsync(string? searchQuery)
        {
            List<SizeChart> getList = new List<SizeChart>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                 getList = await _context.SizeCharts
                     .Include(i => i.Images)
                  .Include(i => i.Products)
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.Title.Trim().Contains(searchQuery.Trim()))
                    .ToListAsync();
              
            }
            else
            {
                getList = await _context.SizeCharts
                     .Include(i => i.Images)
                  .Include(i => i.Products)
                    .Where(i => i.Isdelete == false)                  
                    .ToListAsync();
                
            }
            return getList;
        }

        public async Task<SizeChart> GetSizeChartByIdAsync(int descriptionId)
        {
               
                SizeChart? des = await _context.SizeCharts.Include(i => i.Images).Include(i => i.Products).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeChartId == descriptionId);               
      
                return des;       
        }


        public async Task<SizeChart> CreateSizeChartAsync(SizeChartCreateDTO descriptionCreateDTO)
        {

            SizeChart description = _mapper.Map<SizeChart>(descriptionCreateDTO);
            description.Isdelete = false;
            await _context.SizeCharts.AddAsync(description);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return description;
            }
            else { return null; }

        }

        public async Task<SizeChart> UpdateSizeChartAsync(SizeChartDTO descriptionDTO)
        {
            SizeChart? description = await _context.SizeCharts.Include(i => i.Images)
                  .Include(i => i.Products).SingleOrDefaultAsync(i => i.SizeChartId == descriptionDTO.SizeChartId);
            //ánh xạ đối tượng SizeChartDTO đc truyền vào cho staff
            descriptionDTO.Isdelete = false;
                _mapper.Map(descriptionDTO, description);
                 _context.SizeCharts.Update(description);
                await _context.SaveChangesAsync();
                return description;
        }

        public async Task<bool> DeleteSizeChartAsync(int descriptionId)
        {
            SizeChart? description = await _context.SizeCharts.SingleOrDefaultAsync(i => i.SizeChartId == descriptionId);

            if (description != null)
            {
                description.Isdelete = true;
                _context.SizeCharts.Update(description);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





