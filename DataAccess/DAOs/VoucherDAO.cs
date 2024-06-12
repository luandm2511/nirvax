using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
