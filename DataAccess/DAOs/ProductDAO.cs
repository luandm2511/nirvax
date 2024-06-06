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
    public class ProductDAO
    {

        private readonly NirvaxContext _context;
         private readonly  IMapper _mapper;

  
        

        public ProductDAO(NirvaxContext context, IMapper mapper) 
        {
           
            _context = context;
            _mapper = mapper;
        }
        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> listProductDTO = new List<ProductDTO>();


                List<Product> getList = _context.Products.Include(i => i.Brand).Include(i => i.Category).Include(i => i.Owner).ToList();
            listProductDTO = _mapper.Map<List<ProductDTO>>(getList);
          
            return listProductDTO;
        }

     
    }
}

  


