using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace DataAccess.IRepository
{
    public interface IVoucherRepository
    {
        Task<VoucherDTO> GetVoucherById(string voucherId);
    }
}
