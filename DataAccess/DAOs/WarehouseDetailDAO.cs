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

        

        public async Task<bool> CheckWarehouseDetailExist(int warehouseId)
        {
            WarehouseDetail? sid = new WarehouseDetail();

            sid = await _context.WarehouseDetails.SingleOrDefaultAsync(i => i.WarehouseId == warehouseId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }


      
        //khi thêm vô
        //gọi lại lệnh fetch
        //thực thi theo productSizeId
        //cùng productSizeId thì cộng quantity và price
        public async Task<List<WarehouseDetailFinalDTO>> GetAllWarehouseDetailByProductSize(int warehouseId, int page, int pageSize)
        {
           List<WarehouseDetail> listWarehouseDetail= new List<WarehouseDetail>();
          var result= await _context.WarehouseDetails
              //  .Where(i => i.ProductSize.Isdelete == false)    
                .Where(wd=> wd.WarehouseId == warehouseId)
          
            .GroupBy(w => new { w.ProductSizeId })

        .Select(g => new WarehouseDetailFinalDTO
        {
            WarehouseId = warehouseId,
            ProductSizeId = g.Key.ProductSizeId,
            
           Location = g.Select(i => i.Location).FirstOrDefault(),
            TotalQuantity = g.Sum(wd => wd.QuantityInStock),
            TotalUnitPrice = g.Sum(wd => wd.UnitPrice)
        })
        .ToListAsync();

            var paginatedResult = result
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

            return paginatedResult;
       
        }

        //tổng số loại sản phẩm => số sản phẩm theo productSizeId
        public async Task<int> SumOfKindProdSizeStatistics(int warehouseId)
        {

            var sumKindProduct = await _context.WarehouseDetails.Where(i => i.WarehouseId == warehouseId).GroupBy(w => w.ProductSizeId).CountAsync();
            return sumKindProduct;
        }


        public async Task<bool> CreateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
          
            WarehouseDetail warehouseDetail = _mapper.Map<WarehouseDetail>(warehouseDetailDTO);
            await _context.WarehouseDetails.AddAsync(warehouseDetail);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        //update product size, create warehouse detail
        public async Task<bool> PatchWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            ProductSize productSize;
            productSize = await _context.ProductSizes.FirstOrDefaultAsync(i => i.ProductSizeId == warehouseDetailDTO.ProductSizeId);
            productSize.Quantity += warehouseDetailDTO.QuantityInStock;
             _context.ProductSizes.Update(productSize);
            await _context.SaveChangesAsync();
            WarehouseDetail warehouseDetail = _mapper.Map<WarehouseDetail>(warehouseDetailDTO);
            await _context.WarehouseDetails.AddAsync(warehouseDetail);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateWarehouseDetail(WarehouseDetailDTO warehouseDetailDTO)
        {
            WarehouseDetail? warehouseDetail = await _context.WarehouseDetails.SingleOrDefaultAsync(i => i.WarehouseId == warehouseDetailDTO.WarehouseId);
            //ánh xạ đối tượng WarehouseDetailDTO đc truyền vào cho staff
          
            _mapper.Map(warehouseDetailDTO, warehouseDetail);
            _context.WarehouseDetails.Update(warehouseDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
