﻿using BusinessObject.Models;
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
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pipelines.Sockets.Unofficial.Buffers;
using Microsoft.Extensions.Caching.Memory;

namespace DataAccess.DAOs
{
    public class VoucherDAO
    {
        private readonly NirvaxContext _context;

        private readonly IMapper _mapper;

        public VoucherDAO(NirvaxContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<Voucher> GetVoucherById(string voucherId)
        {
            return await _context.Vouchers.Include(i => i.Owner).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId);
        }

        public async Task<bool> CheckVoucherAsync(DateTime startDate, DateTime endDate, string voucherId)
        {

            Voucher? voucher = new Voucher();

            if ((startDate.Date >= DateTime.Now.Date) && (endDate.Date >= startDate.Date))
            {
                if (startDate.Date == endDate.Date)
                {
                    if (endDate.TimeOfDay <= startDate.TimeOfDay)
                    {
                        throw new Exception("If on the same day then EndDate must be after StartDate in terms of time on the same day!");
                    }
                }
                voucher = await _context.Vouchers.Include(i => i.Owner).SingleOrDefaultAsync(i => i.VoucherId.Trim() == voucherId.Trim());
                if (voucher == null)
                {
                    return true;

                }
                else
                {
                    throw new Exception("Already has this voucher!");
                };
            }
            else
            {
                return false;
                throw new Exception("StartDate should be greater than or equal to Today and StartDate should be less than or equal to EndDate!");
            }

        }

        public async Task<bool> CheckVoucherExistAsync(VoucherDTO voucherDTO)
        {
            Voucher? sid = new Voucher();
            sid = await _context.Vouchers.Include(i => i.Owner).SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);

            if ((sid != null) && (voucherDTO.StartDate < voucherDTO.EndDate))
            {
                return true;

            }
            else

            {
                throw new Exception("StartDate should bottom EndDate!");
            }

            return false;
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<Voucher>(); }
            List<Voucher> getList = new List<Voucher>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.Vouchers
                    .Include(i => i.Owner)
                    .Where(i => i.OwnerId == ownerId)
                    .Where(i => i.VoucherId.Trim().Contains(searchQuery.Trim()) && !i.Isdelete)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
            else
            {
                getList = await _context.Vouchers
                    .Include(i => i.Owner)
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            return getList;
        }

        public async Task<IEnumerable<Voucher>> GetAllVoucherForUserAsync()
        {
            var endDate = DateTime.Now;
            List<Voucher> getList = await _context.Vouchers
              .Include(i => i.Owner)
                  .Where(i => i.Isdelete == false).Where(i => i.EndDate >= endDate)
                  .ToListAsync();
            return getList;
        }

        public async Task<IEnumerable<Voucher>> GetAllVoucherByOwnerAsync(int ownerId)
        {
            List<Voucher> getList = await _context.Vouchers
              .Include(i => i.Owner)
                  .Where(i => i.Isdelete == false)
                    .Where(i => i.OwnerId == ownerId)
                  .ToListAsync();
            return getList;
        }

        public async Task<Voucher> GetVoucherDTOByIdAsync(string voucherId)
        {

            Voucher? sid = await _context.Vouchers.Include(i => i.Owner).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId);

            return sid;
        }

        public async Task<bool> CreateVoucherAsync(VoucherCreateDTO voucherCreateDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == voucherCreateDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }
            Voucher voucher = _mapper.Map<Voucher>(voucherCreateDTO);
            voucher.Isdelete = false;
            voucher.QuantityUsed = 0;
            await _context.Vouchers.AddAsync(voucher);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> UpdateVoucherAsync(VoucherDTO voucherDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == voucherDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }
            Voucher? voucher = await _context.Vouchers.Include(i => i.Owner).SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
            _mapper.Map(voucherDTO, voucher);
            voucher.Isdelete = false;
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVoucherAsync(string voucherId)
        {
            Voucher? voucher = await _context.Vouchers.Include(i => i.Owner).SingleOrDefaultAsync(i => i.VoucherId == voucherId);
            if (voucher != null)
            {
                voucher.Isdelete = true;
                _context.Vouchers.Update(voucher);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<object> ViewVoucherStatisticsAsync(int ownerId)
        {
            List<Voucher> voucher = await _context.Vouchers.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).ToListAsync();
            var totalQuantity = voucher.Sum(i => i.QuantityUsed);
            var totalPrice = voucher.Sum(v => (v.QuantityUsed) * v.Price);
            var result = new Dictionary<string, object>
          {
            { "totalQuantityVoucherUsed", totalQuantity },
            { "totalPriceVoucherUsed", totalPrice },
          };

            return result;
        }
    }
}





