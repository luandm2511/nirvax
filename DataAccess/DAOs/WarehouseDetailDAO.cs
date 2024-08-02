using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class WarehouseDetailDAO
    {
        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;




        public WarehouseDetailDAO(NirvaxContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        //tổng số loại sản phẩm => số sản phẩm theo productSizeId
        public async Task<int> SumOfKindProdSizeStatisticsAsync(int warehouseId)
        {

            var sumKindProduct = await _context.WarehouseDetails.Where(i => i.WarehouseId == warehouseId).GroupBy(w => w.ProductSizeId).CountAsync();
            return sumKindProduct;
        }


        public async Task<bool> CreateWarehouseDetailAsync(int warehouseId,List<ImportProductDetailCreateDTO> importProductDetailDTO)
        {
            foreach (var item in importProductDetailDTO)
            {
                var productSizeId = $"{item.ProductId}_{item.SizeId}";
                var checkWarehouseDetail = await _context.WarehouseDetails
               .SingleOrDefaultAsync(i => i.WarehouseId == warehouseId && i.ProductSizeId == productSizeId);
                if (checkWarehouseDetail == null)
                {
                    WarehouseDetail warehouseDetail = new WarehouseDetail
                    {
                        WarehouseId = warehouseId,
                        ProductSizeId = productSizeId,
                        Location = ""
                    };
                    await _context.WarehouseDetails.AddAsync(warehouseDetail);
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        //update product size, create warehouse detail
        public async Task<bool> PatchWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
           // ProductSize productSize;
         //   productSize = await _context.ProductSizes.FirstOrDefaultAsync(i => i.ProductSizeId == warehouseDetailDTO.ProductSizeId);
         //   productSize.Quantity += warehouseDetailDTO.QuantityInStock;
            // _context.ProductSizes.Update(productSize);
            WarehouseDetail warehouseDetail = _mapper.Map<WarehouseDetail>(warehouseDetailDTO);
            await _context.WarehouseDetails.AddAsync(warehouseDetail);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateWarehouseDetailAsync(WarehouseDetailDTO warehouseDetailDTO)
        {
            WarehouseDetail? warehouseDetail = await _context.WarehouseDetails
                .Where(i => i.ProductSizeId.Trim() == warehouseDetailDTO.ProductSizeId.Trim())
                .SingleOrDefaultAsync(i => i.WarehouseId == warehouseDetailDTO.WarehouseId);

            _mapper.Map(warehouseDetailDTO, warehouseDetail);
            _context.WarehouseDetails.Update(warehouseDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
