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
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pipelines.Sockets.Unofficial.Buffers;
using Microsoft.Extensions.Caching.Memory;

namespace DataAccess.DAOs
{
    public class VoucherDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;



        public  VoucherDAO(NirvaxContext context, IMapper mapper, IMemoryCache memoryCache)
        {

             _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<Voucher> GetVoucherById(string voucherId)
        {
            return await _context.Vouchers.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId);
        }

        public async Task<bool> CheckVoucherByIdAsync(string voucherId)
        {

            Voucher? voucher = new Voucher();
            voucher = await _context.Vouchers.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId); 


            if (voucher == null)
            {
                return false;

            }
            return true;
        }

        public async Task<bool> CheckVoucherAsync(DateTime startDate, DateTime endDate, string voucherId)
        {

            Voucher? voucher = new Voucher();

            if((startDate >= DateTime.Now) && (startDate < endDate))
            {
                voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherId);
                if (voucher == null)
                {
                    return true;

                }
                else
                {
                    throw new Exception("StartDate should >= Today and StartDate should bottom EndDate!");
                };
            }
            
            return false;
        }

        public async Task<bool> CheckVoucherExistAsync(VoucherDTO voucherDTO)
        {
            Voucher? sid = new Voucher();
            sid = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
            if ((sid != null) && (voucherDTO.StartDate < voucherDTO.EndDate))
            {
                return true;
            } else
            {
                throw new Exception("StartDate should bottom EndDate!");
            }

            return false;
        }


        //owner,staff
        public async Task<List<VoucherDTO>> GetAllVouchersAsync(string? searchQuery, int page, int pageSize)
        {
            List<VoucherDTO> listSizeDTO = new List<VoucherDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Voucher> getList = await _context.Vouchers
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.VoucherId.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<VoucherDTO>>(getList);
            }
            else
            {
                List<Voucher> getList = await _context.Vouchers
                    .Where(i => i.Isdelete == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<VoucherDTO>>(getList);
            }
            return listSizeDTO;
        }

        //user
        public async Task<List<VoucherDTO>> GetAllVoucherForUserAsync()
        {
            List<VoucherDTO> listSizeDTO = new List<VoucherDTO>();
              List<Voucher> getList = await _context.Vouchers
                    .Where(i => i.Isdelete == false)
  
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<VoucherDTO>>(getList);
            
            return listSizeDTO;
        }

        public async Task<VoucherDTO> GetVoucherDTOByIdAsync(string voucherId)
        {
            VoucherDTO voucherDTO = new VoucherDTO();
            try
            {
                Voucher? sid = await _context.Vouchers.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId);
               
                    voucherDTO = _mapper.Map<VoucherDTO>(sid);
                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return voucherDTO;
        }

      



        public async Task<bool> CreateVoucherAsync(VoucherCreateDTO voucherCreateDTO)
        {             
            Voucher voucher = _mapper.Map<Voucher>(voucherCreateDTO);
            voucher.Isdelete = false;
            voucher.QuantityUsed = 0;
            await _context.Vouchers.AddAsync(voucher);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { 
                return false; 
            }

        }

        public async Task<bool> UpdateVoucherAsync(VoucherDTO voucherDTO)
        {
           
                Voucher? voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
                //ánh xạ đối tượng VoucherDTO đc truyền vào cho staff
                voucherDTO.Isdelete = false;
                _mapper.Map(voucherDTO, voucher);
                 _context.Vouchers.Update(voucher);
                await _context.SaveChangesAsync();
                return true;
       
        }

        public async Task<bool> DeleteVoucherAsync(string voucherId)
        {
            Voucher? voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherId);
            //ánh xạ đối tượng VoucherDTO đc truyền vào cho staff

               

            if (voucher != null)
            {
                voucher.Isdelete = true;
                 _context.Vouchers.Update(voucher);
                //    _mapper.Map(VoucherDTO, staff);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }

        public async Task<bool> PriceAndQuantityByOrderAsync(string voucherId, int quantity)
        {
            Voucher? voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherId);
            
            if (voucher == null) {
              
                throw new Exception("Voucher is not exist!");
            }
           // int ownerId = voucher.OwnerId;
            if (quantity <=0)
            {
                throw new Exception("Quantity voucher used should above 0!");

            }
            voucher.Quantity = voucher.Quantity - quantity;
            double price = voucher.Price * quantity;
            if(voucher.QuantityUsed == null) { voucher.QuantityUsed = 0; }
           // if (voucher.TotalPriceUsed == null) { voucher.TotalPriceUsed = 0.0; }

            voucher.QuantityUsed = voucher.QuantityUsed + quantity;
          //  voucher.TotalPriceUsed = voucher.TotalPriceUsed + price;

            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<int> QuantityVoucherUsedStatisticsAsync(int ownerId)
        {
            List<Voucher> voucher = await _context.Vouchers.Where(i => i.OwnerId == ownerId).ToListAsync();
            var totalQuantity = voucher.Sum(i => i.QuantityUsed);
            return totalQuantity ;
        }

       

        public async Task<double> PriceVoucherUsedStatisticsAsync(int ownerId)
        {
            List<Voucher> voucher = await _context.Vouchers.Where(i => i.OwnerId == ownerId).ToListAsync();
            //  var totalPrice = voucher.Sum(i => i.TotalPriceUsed);
            var totalPrice = voucher.Sum(v => (v.QuantityUsed) * v.Price);
            return totalPrice;
        }


    }
}





