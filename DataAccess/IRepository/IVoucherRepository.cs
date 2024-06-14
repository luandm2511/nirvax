using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IVoucherRepository
    {
        Task<bool> CheckVoucherAsync(VoucherDTO voucherDTO);
        Task<bool> CheckVoucherExistAsync(VoucherDTO voucherDTO);
        Task<bool> CheckVoucherByIdAsync(string voucherId);
        Task<int> QuantityVoucherUsedStatisticsAsync(int ownerId);
        Task<double> PriceVoucherUsedStatisticsAsync(int ownerId);
        Task<List<VoucherDTO>> GetAllVoucherForUserAsync();
        Task<bool> PriceAndQuantityByOrderAsync( string voucherId, int quantity);
        Task<List<VoucherDTO>> GetAllVouchersAsync(string? searchQuery, int page, int pageSize);

        Task<VoucherDTO> GetVoucherByIdAsync(string voucherId);

        Task<bool> CreateVoucherAsync(VoucherDTO voucherDTO);

        Task<bool> UpdateVoucherAsync(VoucherDTO voucherDTO);
        Task<bool> DeleteVoucherAsync(string voucherId);

    }
}
