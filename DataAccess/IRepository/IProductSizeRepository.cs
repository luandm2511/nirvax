using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IProductSizeRepository
    {
        Task<bool> CheckProductSizeAsync(ProductSizeDTO productSizeDTO);
        Task<bool> CheckProductSizeExistAsync(string productSizeId);
        Task<bool> CheckProductSizeByIdAsync(string productSizeId);

        Task<ProductSize> GetByIdAsync(string id);
        Task<bool> UpdateAsync(ProductSize productSize);
        Task<List<ProductSizeDTO>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize);
        Task<List<ProductSizeDTO>> GetProductSizeByProductIdAsync(int productId);

        Task<ProductSizeDTO> GetProductSizeByIdAsync(string productSizeId);

        Task<ProductSize> CreateProductSizeAsync(ProductSizeCreateDTO productSizeCreateDTO);

        Task<bool> UpdateProductSizeAsync(ProductSizeDTO productSizeDTO);
        Task<bool> DeleteProductSizeAsync(string productSizeId);

    }
}
