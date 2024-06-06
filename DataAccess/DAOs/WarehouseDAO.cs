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

        public async Task<bool> CheckWarehouse(WarehouseDTO warehouseDTO)
        {

            Warehouse? warehouse = new Warehouse();
            warehouse = await _context.Warehouses.FindAsync(warehouseDTO.WarehouseId);
            
            if (warehouse != null)
            {
                List<Warehouse> getList = await _context.Warehouses.Include(i => i.Owner)
                 
                 //check khác Id`
                 .Where(i => i.WarehouseId != warehouseDTO.WarehouseId)
                 .ToListAsync();
                if (getList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
           
            return false;
        }



        //list ra các import thôi
        public async Task<List<ImportProduct>> GetWarehouseByImportProduct(int ownerId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i=> i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts    
                    .Where(i => i.WarehouseId == warehouse.WarehouseId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

     
            
            return listImportProduct;
        }

       
        
        public async Task<Warehouse> UpdateQuantityAndPriceWarehouse(int ownerId)
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
            return warehouse;
        }

        //số lần nhập kho
        public async Task<int> ViewCountImportStatistics(int warehouseId)
        {
          var numberCount =  await _context.ImportProducts.Where(i => i.WarehouseId == warehouseId).CountAsync();
            return numberCount;
        }

        //tổng số sản phẩm lần nhập đó
        public async Task<int> ViewNumberOfProductByImportStatistics(int importId, int ownerId)
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
        public async Task<double> ViewPriceByImportStatistics(int importId, int ownerId)
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
        public async Task<int> QuantityWarehouseStatistics(int ownerId)
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


        public async Task<Warehouse> GetWarehouseById(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<WarehouseDetail> listWarehouseDetail = await _context.WarehouseDetails
                  .Where(i => i.WarehouseId == warehouse.WarehouseId)
               
                .ToListAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts
                    .Where(i => i.WarehouseId == warehouse.WarehouseId)
                   
                    .ToListAsync();

            var totalQuantity = listWarehouseDetail.Sum(p => p.QuantityInStock);
            var totalPrice = listWarehouseDetail.Sum(p =>  p.UnitPrice);

            warehouse.TotalQuantity = totalQuantity;
            warehouse.TotalPrice = totalPrice;

            return warehouse;
        }

        public async Task<int> GetWarehouseIdByOwnerId(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            return warehouse.WarehouseId;
        }



        //list ra detail nhưng mà group by theo product size giống bên warehousedetail

        public async Task<List<WarehouseDetail>> GetAllWarehouseDetail(int ownerId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            var result = await _context.WarehouseDetails
                .Where(wd => wd.WarehouseId == warehouse.WarehouseId)

            .GroupBy(w => new { w.ProductSizeId, w.Location })

        .Select(g => new WarehouseDetail
        {
            ProductSizeId = g.Key.ProductSizeId,
            Location = g.Key.Location,
            QuantityInStock = g.Sum(wd => wd.QuantityInStock),
            UnitPrice = g.Sum(wd => wd.UnitPrice)
        })
        .ToListAsync();

            var paginatedResult = result
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

            return paginatedResult;
        }



        //chỉ là create warehouse cho owner (1 lần duy nhất)
        public async Task<bool> CreateWarehouse(WarehouseDTO warehouseDTO)
        {
            Warehouse warehouseCheck = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == warehouseDTO.OwnerId).FirstOrDefaultAsync();
            if(warehouseCheck == null) {
                Warehouse warehouse = _mapper.Map<Warehouse>(warehouseDTO);
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

        public async Task<bool> UpdateWarehouse(WarehouseDTO warehouseDTO)
        {
            Warehouse? warehouse = await _context.Warehouses.Include(i => i.Owner).SingleOrDefaultAsync(i => i.WarehouseId == warehouseDTO.WarehouseId);
            _mapper.Map(warehouseDTO, warehouse);
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
