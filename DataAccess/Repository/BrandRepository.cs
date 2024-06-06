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

namespace DataAccess.Repository
{
    public class BrandRepository : IBrandRepository
    {
       
        private readonly BrandDAO _brandDAO;
        public BrandRepository(BrandDAO brandDAO)
        {
            _brandDAO = brandDAO;
        }


        public List<BrandDTO> GetAllBrands()
        {
            
            return _brandDAO.GetAllBrands();
        }

    }
}
