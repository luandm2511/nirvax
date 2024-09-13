using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Repository.StaffRepository;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class SizeChartRepository : ISizeChartRepository
    {
       
        private readonly SizeChartDAO _sizeChartDAO;
        public SizeChartRepository(SizeChartDAO sizeChartDAO)
        {
            _sizeChartDAO = sizeChartDAO;
        }

        public Task<bool> CheckSizeChartAsync(int sizeChartId, string title, string content, int ownerId)
        {
            return _sizeChartDAO.CheckSizeChartAsync( sizeChartId,  title, content, ownerId);
        }
        public Task<List<SizeChart>> GetAllSizeChartsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            
            return _sizeChartDAO.GetAllSizeChartsAsync(searchQuery, page,  pageSize, ownerId);
        }

        public Task<List<SizeChart>> GetSizeChartForUserAsync(string? searchQuery)
        {
            return _sizeChartDAO.GetSizeChartForUserAsync(searchQuery);

        }

        public Task<SizeChart> GetSizeChartByIdAsync(int sizeChartId)
        {
            return (_sizeChartDAO.GetSizeChartByIdAsync(sizeChartId));
        }
  
        public Task<SizeChart> CreateSizeChartAsync(SizeChartCreateDTO sizeChartCreateDTO)
        {
            return _sizeChartDAO.CreateSizeChartAsync(sizeChartCreateDTO);
        }

        public Task<SizeChart> UpdateSizeChartAsync(SizeChartDTO sizeChartDTO)
        {
            return _sizeChartDAO.UpdateSizeChartAsync(sizeChartDTO);
        }
        public Task<bool> DeleteSizeChartAsync(int sizeChartId)
        {
            return _sizeChartDAO.DeleteSizeChartAsync(sizeChartId);
        }

    }
}
