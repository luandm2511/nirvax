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
    public class CategoryDAO
    {

        private readonly NirvaxContext _context;
         private readonly  IMapper _mapper;

  
        

        public CategoryDAO(NirvaxContext context, IMapper mapper) 
        {
           
            _context = context;
            _mapper = mapper;
        }
        public List<CategoryDTO> GetAllCategories()
        {
            List<CategoryDTO> listCategoryDTO = new List<CategoryDTO>();


                List<Category> getList = _context.Categories.ToList();
            listCategoryDTO = _mapper.Map<List<CategoryDTO>>(getList);
          
            return listCategoryDTO;
        }

     
    }
}

  


