using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Net.WebSockets;

namespace DataAccess.DAOs
{
    public class ImportProductDetailDAO
    {

        private readonly NirvaxContext _context;
         private readonly  IMapper _mapper;

  
        

        public ImportProductDetailDAO(NirvaxContext context, IMapper mapper) 
        {
           
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckImportProductDetailExistAsync(int importProductDetailId)
        {
            ImportProductDetail? sid = new ImportProductDetail();

            sid = await _context.ImportProductDetails.Include(i => i.Import).Include(i => i.ProductSize).SingleOrDefaultAsync(i => i.ImportId == importProductDetailId) ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }


        //show list theo import Id => detail import đó
        public async Task<List<ImportProductDetail>> GetAllImportProductDetailByImportIdAsync(int importId)
        {
          
            List<ImportProductDetail> getList = await _context.ImportProductDetails
            .Include(i => i.Import).Include(i => i.ProductSize)
            .Where(x => x.ImportId == importId).ToListAsync();
         
            
            return getList;
        }

        // detail list all các detail của import nhưng 
        public async  Task<List<ImportProductDetail>> GetAllImportProductDetailAsync()
        {
           
             List<ImportProductDetail> getList = await _context.ImportProductDetails
            .Include(i => i.Import).Include(i => i.ProductSize)
            .ToListAsync();
               
            return getList;
        }


        public async Task<bool> CreateImportProductDetailAsync(int importId, List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {
            var importProduct = await _context.ImportProducts.FirstOrDefaultAsync(i => i.ImportId == importId);
            if (importProduct == null)
            {
                throw new Exception("ImportProduct not found.");
            }

            foreach (var item in importProductDetailDTO)
            {
                var nameProductSize = $"{item.ProductId}_{item.SizeId}";

                var productSize = await _context.ProductSizes.SingleOrDefaultAsync(i => i.ProductSizeId == nameProductSize);
                if (productSize == null)
                {
                    throw new Exception($"ProductSize with ID {nameProductSize} does not exist.");
                }

                ImportProductDetail importProductDetail = new ImportProductDetail
                {
                    ImportId = importId,
                    ProductSizeId = nameProductSize,
                    QuantityReceived = item.QuantityReceived,
                    UnitPrice = item.UnitPrice
                };

                await _context.ImportProductDetails.AddAsync(importProductDetail);
            }
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailDTO> importProductDetailDTO)
        {
            var importProduct = await _context.ImportProducts.FirstOrDefaultAsync(i => i.ImportId == importId);
            List<ImportProductDetail> importProductDetails = await _context.ImportProductDetails.Where(i => i.ImportId == importId).ToListAsync();
            //ánh xạ đối tượng staffdto đc truyền vào cho staff

            if (importProduct == null)
            {
                throw new Exception("ImportProduct not found.");
            }
            int totalQuantity = importProductDetailDTO.Sum(d => d.QuantityReceived);
            double totalPrice = importProductDetailDTO.Sum(d => d.UnitPrice);
            if (importProduct.TotalPrice > totalPrice || importProduct.TotalPrice < totalPrice)
            {
                throw new Exception("Sum shoud similar Total Price of Import Product");
            }

            if (importProduct.Quantity < totalQuantity || importProduct.Quantity > totalQuantity)
            {
                throw new Exception("Sum shoud similar Quantity of Import Product");
            }
            foreach (var item in importProductDetailDTO)
            {
                item.ImportId = importId;
                var checkProductSizeId = await _context.ProductSizes.Where(i => i.ProductSizeId == item.ProductSizeId).SingleOrDefaultAsync();
                if (checkProductSizeId == null)
                {
                    throw new Exception("Don't exist that Product Size");
                }
            }

            foreach (var detailDTO in importProductDetailDTO)
            {
                var existingDetail = importProductDetails.SingleOrDefault(d => d.ProductSizeId == detailDTO.ProductSizeId);
                if (existingDetail != null)
                {
                    _mapper.Map(detailDTO, existingDetail);
                    _context.ImportProductDetails.Update(existingDetail);
                }
                else
                {
                    var newDetail = _mapper.Map<ImportProductDetail>(detailDTO);
                    await _context.ImportProductDetails.AddAsync(newDetail);
                }
            }
            await _context.SaveChangesAsync();
            return true;
            

        }

      

       
    }
    }

  


