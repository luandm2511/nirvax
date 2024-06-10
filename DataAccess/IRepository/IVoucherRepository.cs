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
        Task<bool> CheckVoucher(VoucherDTO voucherDTO);
        Task<bool> CheckVoucherExist(VoucherDTO voucherDTO);
        Task<bool> CheckVoucherById(string voucherId);
        Task<int> QuantityVoucherUsedStatistics(int ownerId);
        Task<double> TotalPriceVoucherUsedStatistics(int ownerId);
        Task<List<VoucherDTO>> GetAllVoucherForUser();

        Task<List<VoucherDTO>> GetAllVouchers(string? searchQuery, int page, int pageSize);

        Task<VoucherDTO> GetVoucherById(string voucherId);

        Task<bool> CreateVoucher(VoucherDTO voucherDTO);

        Task<bool> UpdateVoucher(VoucherDTO voucherDTO);
        Task<bool> DeleteVoucher(string voucherId);

    }
}
