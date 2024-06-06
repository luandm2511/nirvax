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
    public class ProductSizeRepository : IProductSizeRepository
    {
       
        private readonly ProductSizeDAO _productSizeDAO;
        public ProductSizeRepository(ProductSizeDAO productSizeDAO)
        {
            _productSizeDAO = productSizeDAO;
        }

        public Task<bool> CheckProductSizeById(string productSizeId)
        {
            return _productSizeDAO.CheckProductSizeById(productSizeId);
        }
       
        public Task<bool> CheckProductSize(ProductSizeDTO productSizeDTO)
        {
            return _productSizeDAO.CheckProductSize(productSizeDTO);
        }
        public Task<bool> CheckProductSizeExist(string productSizeId)
        {
            return _productSizeDAO.CheckProductSizeExist(productSizeId);
        }

       
        public Task<List<ProductSizeDTO>> GetAllProductSizes(string? searchQuery, int page, int pageSize)
        {
            return _productSizeDAO.GetAllProductSizes(searchQuery, page, pageSize);
        }
        public Task<List<ProductSizeDTO>> GetProductSizeByProductId(int productId)
        {
            return _productSizeDAO.GetProductSizeByProductId(productId);
        }

        public Task<ProductSizeDTO> GetProductSizeById(string productSizeId)
        {
            return _productSizeDAO.GetProductSizeById(productSizeId);
        }

        public Task<bool> CreateProductSize(ProductSizeDTO productSizeDTO)
        {
            return _productSizeDAO.CreateProductSize(productSizeDTO);
        }

        public Task<bool> UpdateProductSize(ProductSizeDTO productSizeDTO)
        {
            return _productSizeDAO.UpdateProductSize(productSizeDTO);
        }
        public Task<bool> DeleteProductSize(string productSizeId)
        {
            return _productSizeDAO.DeleteProductSize(productSizeId);
        }

    }
}
