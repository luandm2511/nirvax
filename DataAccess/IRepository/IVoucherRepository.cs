﻿using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IVoucherRepository
    {
        Task<bool> CheckVoucherAsync(DateTime startDate, DateTime endDate, string voucherId);
        Task<bool> CheckVoucherExistAsync(VoucherDTO voucherDTO);
        Task<bool> CheckVoucherByIdAsync(string voucherId);
        Task<int> QuantityVoucherUsedStatisticsAsync(int ownerId);
        Task<double> PriceVoucherUsedStatisticsAsync(int ownerId);
        Task<List<Voucher>> GetAllVoucherForUserAsync();
        Task<Voucher> PriceAndQuantityByOrderAsync( string voucherId);
        Task<List<Voucher>> GetAllVouchersAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<Voucher>> GetAllVoucherByOwnerAsync(int ownerId);
        Task<Voucher> GetVoucherDTOByIdAsync(string voucherId);
        Task<Voucher> GetVoucherById(string voucherId);

        Task<bool> CreateVoucherAsync(VoucherCreateDTO voucherCreateDTO);

        Task<bool> UpdateVoucherAsync(VoucherDTO voucherDTO);
        Task<bool> DeleteVoucherAsync(string voucherId);

    }
}
