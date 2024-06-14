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

namespace DataAccess.DAOs
{
    public class ProductSizeDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public ProductSizeDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
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
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.ProductSizeId.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listProductSizeDTO = _mapper.Map<List<ProductSizeDTO>>(getList);
            }
            else
            {
                List<ProductSize> getList = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product)
                    .Where(i => i.Isdelete == false)
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



        public async Task<bool> CreateProductSizeAsync(ProductSizeCreateDTO productSizeCreateDTO, int prodId, int sizeId)
        {
            Product product = await _context.Products.SingleOrDefaultAsync(i => i.ProductId == prodId);
            Size size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
           
            //productSizeCreateDTO.Isdelete = false;
            ProductSize productSize = _mapper.Map<ProductSize>(productSizeCreateDTO);
            productSize.ProductSizeId = product.Name.Trim()+ "_" + size.Name.Trim();
            var checkId = productSize.ProductSizeId;
            List<ProductSize> checkProdSize = await _context.ProductSizes.Where(i => i.ProductSizeId.Trim() == checkId.Trim()).ToListAsync();
            if(checkProdSize.Count > 0) {
                return false;
            } 
            productSize.Isdelete = false;
            productSize.ProductSizeId = product.Name.Trim() + "_" + size.Name.Trim();
            await _context.ProductSizes.AddAsync(productSize);
            int i = await _context.SaveChangesAsync();
            
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateProductSizeAsync(ProductSizeDTO productSizeDTO)
        {
            ProductSize? productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeDTO.ProductSizeId);
            //ánh xạ đối tượng ProductSizeDTO đc truyền vào cho staff

           productSizeDTO.Isdelete= false;
                _mapper.Map(productSizeDTO, productSize);
                 _context.ProductSizes.Update(productSize);
                await _context.SaveChangesAsync();
                return true;
          
        }

        public async Task<bool> DeleteProductSizeAsync(string productSizeId)
        {
            ProductSize? productSize = await _context.ProductSizes.Include(i => i.Size).Include(i => i.Product).SingleOrDefaultAsync(i => i.ProductSizeId == productSizeId);
            //ánh xạ đối tượng ProductSizeDTO đc truyền vào cho staff

            if (productSize != null)
            {
                productSize.Isdelete = true;
                 _context.ProductSizes.Update(productSize);
                //    _mapper.Map(ProductSizeDTO, staff);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
           
           

        }
    }
}





