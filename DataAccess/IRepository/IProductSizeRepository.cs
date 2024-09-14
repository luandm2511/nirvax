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
        Task<ProductSize> GetByIdAsync(string id);
        Task UpdateAsync(ProductSize productSize);
        Task<IEnumerable<ProductSizeListDTO>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize, int ownerId);
        Task<IEnumerable<ProductSize>> GetProductSizeByProductIdAsync(int productId);
        Task<ProductSize> GetProductSizeByIdAsync(string productSizeId);
        Task<bool> CreateProductSizeAsync(int ownerId, List<ImportProductDetailCreateDTO> importProductDetailDTO);
        Task<int> ViewProductSizeStatisticsAsync(int ownerId);
        Task<bool> UpdateProductSizeByImportDetailAsync(int ownerId, List<ImportProductDetailUpdateDTO> importProductDetail);
    }
}
