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

       public Task<ProductSize> GetByIdAsync(string id)
        {
            return _productSizeDAO.GetByIdAsync(id);
        }

        public Task<bool> UpdateAsync(ProductSize productSize)
        {
            return _productSizeDAO.UpdateAsync(productSize);
        }
        public Task<bool> CheckProductSizeByIdAsync(string productSizeId)
        {
            return _productSizeDAO.CheckProductSizeByIdAsync(productSizeId);
        }
       
        public Task<bool> CheckProductSizeAsync(ProductSizeDTO productSizeDTO)
        {
            return _productSizeDAO.CheckProductSizeAsync(productSizeDTO);
        }
        public Task<bool> CheckProductSizeExistAsync(string productSizeId)
        {
            return _productSizeDAO.CheckProductSizeExistAsync(productSizeId);
        }

       
        public Task<List<ProductSizeDTO>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize)
        {
            return _productSizeDAO.GetAllProductSizesAsync(searchQuery, page, pageSize);
        }
        public Task<List<ProductSizeDTO>> GetProductSizeByProductIdAsync(int productId)
        {
            return _productSizeDAO.GetProductSizeByProductIdAsync(productId);
        }

        public Task<ProductSizeDTO> GetProductSizeByIdAsync(string productSizeId)
        {
            return _productSizeDAO.GetProductSizeByIdAsync(productSizeId);
        }

        public Task<ProductSize> CreateProductSizeAsync(ProductSizeCreateDTO productSizeCreateDTO)
        {
            return _productSizeDAO.CreateProductSizeAsync(productSizeCreateDTO);
        }

        public Task<bool> UpdateProductSizeAsync(string productSizeId, int quantity)
        {
            return _productSizeDAO.UpdateProductSizeAsync(productSizeId, quantity);
        }
        public Task<bool> DeleteProductSizeAsync(string productSizeId)
        {
            return _productSizeDAO.DeleteProductSizeAsync(productSizeId);
        }

    }
}
