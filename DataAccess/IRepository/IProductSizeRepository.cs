using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProductSizeRepository
    {
        Task<bool> CheckProductSize(ProductSizeDTO productSizeDTO);
        Task<bool> CheckProductSizeExist(string productSizeId);
        Task<bool> CheckProductSizeById(string productSizeId);


        


        Task<List<ProductSizeDTO>> GetAllProductSizes(string? searchQuery, int page, int pageSize);
        Task<List<ProductSizeDTO>> GetProductSizeByProductId(int productId);

        Task<ProductSizeDTO> GetProductSizeById(string productSizeId);

        Task<bool> CreateProductSize(ProductSizeDTO productSizeDTO);

        Task<bool> UpdateProductSize(ProductSizeDTO productSizeDTO);
        Task<bool> DeleteProductSize(string productSizeId);

    }
}
