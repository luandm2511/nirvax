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
using System.Net.WebSockets;
using Pipelines.Sockets.Unofficial.Buffers;

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
  
        public async Task<bool> CheckSizeChartAsync(int sizeChartId, string title, string content, int ownerId)
        {
            if (sizeChartId == 0)
            {
                bool contentExists = await _context.SizeCharts.Include(i => i.Owner).Where(i => i.Isdelete == false)
.AnyAsync(i => i.OwnerId == ownerId && i.Content.Trim() == content.Trim());

                if (contentExists)
                {
                    throw new Exception("Content already exists!");
                }


                bool titleExists = await _context.SizeCharts.Include(i => i.Owner).Where(i => i.Isdelete == false)
.AnyAsync(i => i.OwnerId == ownerId && i.Title.Trim() == title.Trim());

                if (titleExists)
                {
                    throw new Exception("Title already exists!");
                }
            }
            else
            {
                bool contentExists = await _context.SizeCharts.Include(i => i.Owner).Where(i => i.Isdelete == false).Where(i => i.SizeChartId != sizeChartId)
 .AnyAsync(i => i.OwnerId == ownerId && i.Content.Trim() == content.Trim());

                if (contentExists)
                {
                    throw new Exception("Content already exists!");

                }

                bool titleExists = await _context.SizeCharts.Include(i => i.Owner).Where(i => i.Isdelete == false).Where(i => i.SizeChartId != sizeChartId)
.AnyAsync(i => i.OwnerId == ownerId && i.Title.Trim() == title.Trim());

                if (titleExists)
                {
                    throw new Exception("Title already exists!");
                }
            }

            return true;
          
        }
        //owner,staff
        public async Task<List<SizeChart>> GetAllSizeChartsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            // Check if owner exists
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<SizeChart>(); }

            List<SizeChart> getList = new List<SizeChart>();

   
            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.SizeCharts
                    .Where(i => i.Isdelete == false && i.OwnerId == ownerId && i.Title.Trim().Contains(searchQuery.Trim()))
                    .Include(i => i.Products) 
                    .Select(sc => new SizeChart
                    {
                        SizeChartId = sc.SizeChartId,
                        Title = sc.Title,
                        Content = sc.Content,
                        Isdelete = sc.Isdelete,
                        OwnerId = sc.OwnerId,
                        Products = sc.Products,
                        Images = sc.Images.Where(img => !img.Isdelete).ToList()
                    })
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                getList = await _context.SizeCharts
                    .Where(i => i.Isdelete == false && i.OwnerId == ownerId)
                    .Include(i => i.Products) 
                    .Select(sc => new SizeChart
                    {
                        SizeChartId = sc.SizeChartId,
                        Title = sc.Title,
                        Content = sc.Content,
                        Isdelete = sc.Isdelete,
                        OwnerId = sc.OwnerId,
                        Products = sc.Products,
                        Images = sc.Images.Where(img => !img.Isdelete).ToList() 
                    })
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
                     .Select(sc => new SizeChart
                     {
                         SizeChartId = sc.SizeChartId,
                         Title = sc.Title,
                         Content = sc.Content,
                         Isdelete = sc.Isdelete,
                         OwnerId = sc.OwnerId,
                         Products = sc.Products,
                         Images = sc.Images.Where(img => !img.Isdelete).ToList()
                     })
                    .ToListAsync();
              
            }
            else
            {
                getList = await _context.SizeCharts
                     .Include(i => i.Images)
                  .Include(i => i.Products)
                    .Where(i => i.Isdelete == false)
                     .Select(sc => new SizeChart
                     {
                         SizeChartId = sc.SizeChartId,
                         Title = sc.Title,
                         Content = sc.Content,
                         Isdelete = sc.Isdelete,
                         OwnerId = sc.OwnerId,
                         Products = sc.Products,
                         Images = sc.Images.Where(img => !img.Isdelete).ToList()
                     })
                    .ToListAsync();
                
            }
            return getList;
        }

        public async Task<SizeChart> GetSizeChartByIdAsync(int sizeChartId)
        {
            SizeChart? des = await _context.SizeCharts
                .Where(i => i.Isdelete == false && i.SizeChartId == sizeChartId)
                .Include(i => i.Products)
                .Select(sc => new SizeChart
                {
                    SizeChartId = sc.SizeChartId,
                    Title = sc.Title,
                    Content = sc.Content,
                    Isdelete = sc.Isdelete,
                    OwnerId = sc.OwnerId,
                    Products = sc.Products,
                    Images = sc.Images.Where(img => !img.Isdelete).ToList() 
                })
                .SingleOrDefaultAsync();

            return des;
        }



        public async Task<SizeChart> CreateSizeChartAsync(SizeChartCreateDTO sizeChartCreateDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == sizeChartCreateDTO.OwnerId).SingleOrDefaultAsync();            
            if(checkOwner == null) 
            {
                throw new Exception("Not exist this owner!");
            }
            SizeChart description = _mapper.Map<SizeChart>(sizeChartCreateDTO);
            description.Isdelete = false;
            await _context.SizeCharts.AddAsync(description);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return description;
            }
            else { return null; }

        }

        public async Task<SizeChart> UpdateSizeChartAsync(SizeChartDTO sizeChartDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == sizeChartDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }
            SizeChart? sizeChart = await _context.SizeCharts.Include(i => i.Images)
                  .Include(i => i.Products).SingleOrDefaultAsync(i => i.SizeChartId == sizeChartDTO.SizeChartId);
           
                _mapper.Map(sizeChartDTO, sizeChart);
            sizeChart.Isdelete = false;

            _context.SizeCharts.Update(sizeChart);
                await _context.SaveChangesAsync();
                return sizeChart;
        }

        public async Task<bool> DeleteSizeChartAsync(int sizeChartId)
        {
            SizeChart? sizeChart = await _context.SizeCharts.SingleOrDefaultAsync(i => i.SizeChartId == sizeChartId);

            if (sizeChart != null)
            {
                sizeChart.Isdelete = true;
                _context.SizeCharts.Update(sizeChart);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





