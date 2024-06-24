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

        public VoucherDAO(NirvaxContext context)
        {
            _context = context;
        }
        public async Task<Voucher> GetVoucherById(string voucherId)
        {
            return await _context.Vouchers.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.VoucherId == voucherId);
        }
    }
}
