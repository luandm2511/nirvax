using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface ISizeChartRepository
    {

        Task<IEnumerable<SizeChart>> GetAllSizeChartsAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<IEnumerable<SizeChart>> GetSizeChartForUserAsync(string? searchQuery);
        Task<SizeChart> GetSizeChartByIdAsync(int sizeChartId);
        Task<bool> CheckSizeChartAsync(int sizeChartId, string title, string content, int ownerId);

        Task<SizeChart> CreateSizeChartAsync(SizeChartCreateDTO sizeChartCreateDTO);

        Task<SizeChart> UpdateSizeChartAsync(SizeChartDTO sizeChartDTO);
        Task<bool> DeleteSizeChartAsync(int sizeChartId);

    }
}
