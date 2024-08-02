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
    public class WarehouseDAO
    {
        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;




        public WarehouseDAO(NirvaxContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

       
     
        
        public async Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                throw new Exception("This owner does not have a warehouse yet");
            }

            List<ImportProduct> listImportProduct = await _context.ImportProducts
              .Where(i => i.WarehouseId == warehouse.WarehouseId)
              .ToListAsync();

           

            var totalQuantity = listImportProduct.Sum(p => p.Quantity);
            var totalPrice = listImportProduct.Sum(p => p.TotalPrice);
             warehouse.TotalQuantity = totalQuantity;
            warehouse.TotalPrice = totalPrice;

            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        //số lần nhập kho
        public async Task<int> ViewCountImportStatisticsAsync(int warehouseId)
        {
          var numberCount =  await _context.ImportProducts.Where(i => i.WarehouseId == warehouseId).CountAsync();
            return numberCount;
        }

        //tổng số sản phẩm lần nhập đó
        public async Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts
             .Where(i => i.WarehouseId == warehouse.WarehouseId)
             .Where(i=> i.ImportId ==  importId)
             .ToListAsync();
            var sumOfProduct = listImportProduct.Sum(p => p.Quantity);
            return sumOfProduct;
        }

        //tổng tiền lần nhập đó
        public async Task<double> ViewPriceByImportStatisticsAsync(int importId, int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts
             .Where(i => i.WarehouseId == warehouse.WarehouseId)
             .Where(i => i.ImportId == importId)
             .ToListAsync();
            var sumOfPrice = listImportProduct.Sum(p => p.TotalPrice);
            return sumOfPrice;
        }


        // thống kê tổng số lượng các sản phẩm
        public async Task<int> QuantityWarehouseStatisticsAsync(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            List<ImportProduct> listImportProduct = await _context.ImportProducts
              .Where(i => i.WarehouseId == warehouse.WarehouseId)
              .ToListAsync();

            var totalQuantity = listImportProduct.Sum(p => p.Quantity);
            var totalPrice = listImportProduct.Sum(p => p.TotalPrice);
            warehouse.TotalQuantity = totalQuantity;
            warehouse.TotalPrice = totalPrice;

            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
            return warehouse.TotalQuantity;
        }


        public async Task<Warehouse> GetWarehouseByOwnerIdAsync(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<WarehouseDetail> listWarehouseDetail = await _context.WarehouseDetails
                  .Where(i => i.WarehouseId == warehouse.WarehouseId)
               
                .ToListAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts
                    .Where(i => i.WarehouseId == warehouse.WarehouseId)
                   
                    .ToListAsync();
            return warehouse;
        }

        public async Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            return warehouse.WarehouseId;
        }



        //list ra detail nhưng mà group by theo product size giống bên warehousedetail

        public async Task<List<WarehouseDetail>> GetAllWarehouseDetailByWarehouseAsync(int warehouseId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.WarehouseId == warehouseId).FirstOrDefaultAsync();
            if (warehouse == null) 
            {
                throw new Exception("This owner does not have a warehouse yet");
            }

           // var result = await _context.WarehouseDetails
                //.Where(wd => wd.WarehouseId == warehouse.WarehouseId)
               // .GroupBy(w => new { w.ProductSizeId})

      //  .Select(g => new WarehouseDetail
     //   {
        //    ProductSizeId = g.Key.ProductSizeId,
           // Location = g.Select(i => i.Location).FirstOrDefault(),
     //   })
       // .ToListAsync();
          //  if (result == null) 
         //   {
        //        throw new Exception("This owner does not have a warehouse yet");
          //  }

          //  var paginatedResult = result
             var list = await _context.WarehouseDetails.Include(i => i.ProductSize)
                .Where(i => i.WarehouseId == warehouse.WarehouseId)
                 .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return list;
        }



        //chỉ là create warehouse cho owner (1 lần duy nhất)
        public async Task<bool> CreateWarehouseAsync(WarehouseCreateDTO warehouseCreateDTO)
        {
            Warehouse warehouseCheck = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == warehouseCreateDTO.OwnerId).FirstOrDefaultAsync();
            if(warehouseCheck == null) {
                Warehouse warehouse = _mapper.Map<Warehouse>(warehouseCreateDTO);
                await _context.Warehouses.AddAsync(warehouse);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else 
                { 
                    return false; 
                }
            }
            else {
                throw new Exception("Warehouse is already registed");
            }
        }
    }
}
