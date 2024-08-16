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
        Task UpdateAsync(ProductSize productSize);
        Task<List<ProductSize>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<List<ProductSize>> GetProductSizeByProductIdAsync(int productId);

        Task<ProductSize> GetProductSizeByIdAsync(string productSizeId);
        Task<bool> DeleteProductSizeAsync(string productSizeId);

    }
}
