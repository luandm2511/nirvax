﻿using BusinessObject.Models;
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

        public async Task<bool> UpdateAsync(ProductSize productSize)
        {
            _context.ProductSizes.Update(productSize);
            await _context.SaveChangesAsync();
            return true;
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
        public async Task<List<ProductSizeDTO>> GetAllProductSizesAsync(string? searchQuery, int page, int pageSize)
        {
            List<ProductSizeDTO> listProductSizeDTO = new List<ProductSizeDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<ProductSize> getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                  //  .Where(i => i.Isdelete == false)
                    .Where(i => i.ProductSizeId.Trim().Contains(searchQuery.Trim()))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listProductSizeDTO = _mapper.Map<List<ProductSizeDTO>>(getList);
            }
            else
            {
                List<ProductSize> getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                  //  .Where(i => i.Isdelete == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listProductSizeDTO = _mapper.Map<List<ProductSizeDTO>>(getList);
            }
            return listProductSizeDTO;
        }

        //user,guest
       
        public async Task<List<ProductSizeDTO>> GetProductSizeByProductIdAsync(int productId)
        {
            List<ProductSizeDTO> listProductSizeDTO = new List<ProductSizeDTO>();

            List<ProductSize> getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                .Where(i => i.Isdelete == false)
                .Where(i => i.ProductId == productId)
                .ToListAsync();


            listProductSizeDTO = _mapper.Map<List<ProductSizeDTO>>(getList);
            foreach (var productSizeDTO in listProductSizeDTO)
            {
               
                var productSize = getList.Where(i => i.SizeId == productSizeDTO.SizeId).FirstOrDefault(ps => ps.ProductSizeId == productSizeDTO.ProductSizeId);              
            }

            return listProductSizeDTO;
        }

        //detail
        public async Task<ProductSizeDTO> GetProductSizeByIdAsync(string productSizeId)
        {
            ProductSizeDTO productSizeDTO = new ProductSizeDTO();
            try
            {
                ProductSize? sid = await _context.ProductSizes.Include(i=>i.Size).Include(i => i.Product).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);
                
                    productSizeDTO = _mapper.Map<ProductSizeDTO>(sid);
                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return productSizeDTO;
        }



        public async Task<ProductSize> CreateProductSizeAsync(ProductSizeCreateDTO productSizeCreateDTO)
        {
           
                // Fetch the product and size
                Product product = await _context.Products.SingleOrDefaultAsync(i => i.ProductId == productSizeCreateDTO.ProductId);
                Size size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == productSizeCreateDTO.SizeId);

                // Check if product or size is null
                if (product == null)
                {
                    throw new Exception($"Product with ID does not exist.");
                }

                if (size == null)
                {
                    throw new Exception($"Size with ID does not exist.");
                }

                if (size.OwnerId != product.OwnerId)
                {
                   throw new Exception($"Size and Product do not share the same Owner.");
                }
            ProductSize productSize = _mapper.Map<ProductSize>(productSizeCreateDTO);

                
                productSize.ProductId = product.ProductId;
                productSize.SizeId = size.SizeId;


            productSize.ProductSizeId = product.ProductId + "_" + size.SizeId;

              //  var checkId = productSize.ProductSizeId;

            List<ProductSize> checkProdSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).Where(i => i.ProductSizeId.Trim() == productSize.ProductSizeId.Trim()).ToListAsync();

            if (checkProdSize.Count > 0)
                {
                throw new Exception($"ProductSize with ID already exist!.");
                }

               
                productSize.Isdelete = false;

                await _context.ProductSizes.AddAsync(productSize);
               

            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return productSize;
            }
            else { return productSize; }



        }

        public async Task<bool> UpdateProductSizeAsync(ProductSizeDTO productSizeDTO)
        {
            ProductSize? productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeDTO.ProductSizeId);
                productSize.Isdelete= false;
                _context.ProductSizes.Update(productSize);
                await _context.SaveChangesAsync();
                return true;
          
        }
        public async Task<bool> UpdateProductSizeByImportAsync(List<ImportProductDetailDTO> importProductDetailDTO)
        {
            Product? productSize;
            foreach (var item in importProductDetailDTO)
            {
               var result = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == item.ProductSizeId);
                if(result == null)
                {                   
                        throw new Exception("One of the imported product sizes is not in stock");                   
                }
               result.Quantity += item.QuantityReceived;
                _context.ProductSizes.Update(result);
                await _context.SaveChangesAsync();
            }
            return true;

        }

        public async Task<bool> DeleteProductSizeAsync(string productSizeId)
        {
            ProductSize? productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);
            
            if (productSize != null)
            {
                productSize.Isdelete = true;
                 _context.ProductSizes.Update(productSize);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
           
           

        }
    }
}





