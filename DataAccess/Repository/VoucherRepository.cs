using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly VoucherDAO _voucherDAO;
        public VoucherRepository(VoucherDAO voucherDAO)
        {
            _voucherDAO = voucherDAO;
        }

        public Task<Voucher> GetVoucherById(string voucherId)
        {
            return _voucherDAO.GetVoucherById(voucherId);
        }
    }
}
