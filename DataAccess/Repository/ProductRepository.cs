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
    public class ProductRepository : IProductRepository
    {

        private readonly ProductDAO _productDAO;
        public ProductRepository(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }


        public List<ProductDTO> GetAllProducts()
        {

            return _productDAO.GetAllProducts();
        }

    }
}
