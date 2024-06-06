using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DataAccess.DAOs
{
    public class BrandDAO
    {

        private readonly NirvaxContext _context;
         private readonly  IMapper _mapper;

  
        

        public BrandDAO(NirvaxContext context, IMapper mapper) 
        {
           
            _context = context;
            _mapper = mapper;
        }
        public List<BrandDTO> GetAllBrands()
        {
            List<BrandDTO> listBrandDTO = new List<BrandDTO>();
                List<Brand> getList = _context.Brands
                    .Include(i => i.Category)
                    .ToList();
            listBrandDTO = _mapper.Map<List<BrandDTO>>(getList);
            
            return listBrandDTO;
        }

       
    }
    }

  


