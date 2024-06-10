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

namespace DataAccess.DAOs
{
    public class VoucherDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public  VoucherDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

       
        public async Task<bool> CheckVoucherById(string voucherId)
        {

            Voucher? voucher = new Voucher();
            voucher = await _context.Vouchers.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId); 


            if (voucher == null)
            {
                return false;

            }
            return true;
        }

        public async Task<bool> CheckVoucher(VoucherDTO voucherDTO)
        {

            Voucher? voucher = new Voucher();

            if((voucherDTO.StartDate >= DateTime.Now) && (voucherDTO.StartDate < voucherDTO.EndDate))
            {
                voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
                if (voucher == null)
                {
                    return true;

                }
                else return false;
            }
            
            return false;
        }

        public async Task<bool> CheckVoucherExist(VoucherDTO voucherDTO)
        {
            Voucher? sid = new Voucher();
            sid = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
            if ((sid != null) && (voucherDTO.StartDate < voucherDTO.EndDate))
            {
                return true;
            }

            return false;
        }


        //owner,staff
        public async Task<List<VoucherDTO>> GetAllVouchers(string? searchQuery, int page, int pageSize)
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
        public async Task<List<VoucherDTO>> GetAllVoucherForUser()
        {
            List<VoucherDTO> listSizeDTO = new List<VoucherDTO>();
              List<Voucher> getList = await _context.Vouchers
                    .Where(i => i.Isdelete == false)
  
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<VoucherDTO>>(getList);
            
            return listSizeDTO;
        }

        public async Task<VoucherDTO> GetVoucherById(string voucherId)
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

      



        public async Task<bool> CreateVoucher(VoucherDTO voucherDTO)
        {
            voucherDTO.Isdelete = false;
            voucherDTO.Isused = false;
            Voucher voucher = _mapper.Map<Voucher>(voucherDTO);
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

        public async Task<bool> UpdateVoucher(VoucherDTO voucherDTO)
        {
           
                Voucher? voucher = await _context.Vouchers.SingleOrDefaultAsync(i => i.VoucherId == voucherDTO.VoucherId);
                //ánh xạ đối tượng VoucherDTO đc truyền vào cho staff
                voucherDTO.Isdelete = false;
                _mapper.Map(voucherDTO, voucher);
                 _context.Vouchers.Update(voucher);
                await _context.SaveChangesAsync();
                return true;
       
        }

        public async Task<bool> DeleteVoucher(string voucherId)
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

        public async Task<int> QuantityVoucherUsedStatistics(int ownerId)
        {
           
            var quantity = await _context.Vouchers.Where(i => i.OwnerId == ownerId).Where(i => i.Isused == true).CountAsync();
            return quantity;
        }

        public async Task<double> TotalPriceVoucherUsedStatistics(int ownerId)
        {
           List<Voucher> listVoucher= await _context.Vouchers.Where(i => i.OwnerId == ownerId).Where(i => i.Isused == true).ToListAsync();
            var totalPrice = listVoucher.Sum(p => p.Price);
            return totalPrice;
        }


    }
}





