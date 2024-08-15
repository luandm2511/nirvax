using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Azure;
using Azure.Core;
using System.Drawing;
using Size = BusinessObject.Models.Size;
using System.Security.Cryptography;

namespace DataAccess.DAOs
{
    public class ProductSizeDAO
    {
        private readonly NirvaxContext _context;

        private readonly IMapper _mapper;

        public ProductSizeDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<ProductSize> GetByIdAsync(string id)
        {
            return await _context.ProductSizes.Include(p => p.Product)
                   .ThenInclude(p => p.Images)
                   .Include(p => p.Size)
                   .Include(p => p.Product)
                        .ThenInclude(p => p.Owner)
                   .FirstOrDefaultAsync(p => p.ProductSizeId == id);
        }

        public async Task UpdateAsync(ProductSize productSize)
        {
            _context.ProductSizes.Update(productSize);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckProductSizeExistAsync(string productSizeId)
        {

            ProductSize? productSize = new ProductSize();
            productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);


            if (productSize == null)
            {
                return false;

            }
            return true;
        }

        public async Task<bool> CheckProductSizeByIdAsync(string productSizeId)
        {

            ProductSize? productSize = new ProductSize();
            productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);


            if (productSize == null)
            {
                return false;

            }
            return true;
        }

        public async Task<bool> CheckProductSizeAsync(ProductSizeDTO productSizeDTO)
        {

            ProductSize? productSize = new ProductSize();
            productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeDTO.ProductSizeId);


            if (productSize == null)
            {
                return true;

            }
            return false;
        }

        //staff,owner
        public async Task<List<ProductSize>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            List<ProductSize> getList = new List<ProductSize>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                  //  .Where(i => i.Isdelete == false)
                    .Where(i => i.Size.OwnerId == ownerId)
                    .Where(i => i.ProductSizeId.Trim().Contains(searchQuery.Trim()))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
            }
            else
            {
                getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                    //  .Where(i => i.Isdelete == false)
                    .Where(i => i.Size.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
          
            }
            return getList;
        }

        //user,guest
       
        public async Task<List<ProductSize>> GetProductSizeByProductIdAsync(int productId)
        {
            List<ProductSize> getList = new List<ProductSize>();

            getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                .Where(i => i.Isdelete == false)
                .Where(i => i.ProductId == productId)
                .ToListAsync();

            return getList;
        }

        //detail
        public async Task<ProductSize> GetProductSizeByIdAsync(string productSizeId)
        {
        
            try
            {
                ProductSize? sid = await _context.ProductSizes.Include(i=>i.Size).Include(i => i.Product).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);

                return sid;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
  
        }



        public async Task<bool> CreateProductSizeAsync(int ownerId,List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {

            foreach (var item in importProductDetailDTO)
            {
                // Build ProductSizeId from ProductId and SizeId
                var nameProductSize = $"{item.ProductId}_{item.SizeId}";

                // Check if the product and size exist
                Product product = await _context.Products.SingleOrDefaultAsync(i => i.ProductId == item.ProductId);
                Size size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == item.SizeId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} does not exist.");
                }

                if (size == null)
                {
                    throw new Exception($"Size with ID {item.SizeId} does not exist.");
                }

                if (size.OwnerId != product.OwnerId)
                {
                    throw new Exception($"Size and Product do not share the same Owner.");
                }

                var existingProdSize = await _context.ProductSizes
                    .Include(i => i.Size)
                    .Include(i => i.Product)
                    .Where(i => i.ProductSizeId.Trim() == nameProductSize.Trim())
                    .Where(i => i.Product.OwnerId == ownerId)
                    .FirstOrDefaultAsync();

                if (existingProdSize == null)
                {        
                    ProductSize productSize = new ProductSize
                    {
                        ProductId = item.ProductId,
                        SizeId = item.SizeId,
                        ProductSizeId = nameProductSize,
                        Quantity = item.QuantityReceived,
                        Isdelete = false
                    };
                    await _context.ProductSizes.AddAsync(productSize);
                }
                else
                {          
                    existingProdSize.Quantity += item.QuantityReceived;
                    _context.ProductSizes.Update(existingProdSize);
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductSizeByImportDetailAsync(int ownerId, List<ImportProductDetailUpdateDTO> importProductDetail)
        {
            foreach (var item in importProductDetail)
            {
                var oldImportDetail = await _context.ImportProductDetails
                    .Where(i => i.ProductSizeId.Trim() == item.ProductSizeId.Trim())    
                    .Where(i => i.ImportId == item.ImportId)
                    .SingleOrDefaultAsync();
                if (oldImportDetail == null)
                {
                    continue;
                }

                var productSize = await _context.ProductSizes
              .Include(i => i.Size)
              .Include(i => i.Product)
                    .Where(i => i.ProductSizeId.Trim() == item.ProductSizeId.Trim())
               .Where(i => i.Product.OwnerId == ownerId)
              .SingleOrDefaultAsync();

                if (productSize != null)
                {
                    var newQuantity = productSize.Quantity - oldImportDetail.QuantityReceived + item.QuantityReceived;
                   
                    if (newQuantity < 0)
                    {
                        continue;
                    }
                    productSize.Quantity = newQuantity;
                    _context.ProductSizes.Update(productSize);
                }
                else
                {                
                    continue;
                }
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Can't update!!!");
            }

        }

        public async Task<int> ViewQuantityStatisticsAsync(int ownerId)
        {
            List<ProductSize> listProductSize = await _context.ProductSizes
             .Where(i => i.Product.OwnerId == ownerId)
             .ToListAsync();
            var sumOfProduct = listProductSize.Sum(p => p.Quantity);
            return sumOfProduct;
        }
    }
}





