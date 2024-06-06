using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Repository.StaffRepository;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace DataAccess.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
       
        private readonly VoucherDAO _voucherDAO;
        public VoucherRepository(VoucherDAO voucherDAO)
        {
            _voucherDAO = voucherDAO;
        }

        public Task<bool> CheckVoucherById(string voucherId)
        {
            return _voucherDAO.CheckVoucherById(voucherId);
        }
        public Task<bool> CheckVoucher(VoucherDTO voucherDTO)
        {
            return _voucherDAO.CheckVoucher(voucherDTO);
        }
        public Task<bool> CheckVoucherExist(VoucherDTO voucherDTO)
        {
            return _voucherDAO.CheckVoucherExist(voucherDTO);
        }

       
        public Task<List<VoucherDTO>> GetAllVouchers(string? searchQuery, int page, int pageSize)
        {
            return _voucherDAO.GetAllVouchers(searchQuery, page, pageSize);
        }

       public Task<List<VoucherDTO>> GetAllVoucherForUser()
        {
            return _voucherDAO.GetAllVoucherForUser();
        }

     
        public Task<VoucherDTO> GetVoucherById(string voucherId)
        {
            return _voucherDAO.GetVoucherById(voucherId);
        }

        public Task<bool> CreateVoucher(VoucherDTO voucherDTO)
        {
            return _voucherDAO.CreateVoucher(voucherDTO);
        }

        public Task<bool> UpdateVoucher(VoucherDTO voucherDTO)
        {
            return _voucherDAO.UpdateVoucher(voucherDTO);
        }
        public Task<bool> DeleteVoucher(string voucherId)
        {
            return _voucherDAO.DeleteVoucher(voucherId);
        }

    }
}
