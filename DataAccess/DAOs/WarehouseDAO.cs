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

        public async Task<bool> CheckWarehouseAsync(WarehouseDTO warehouseDTO)
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
        public async Task<List<ImportProduct>> GetWarehouseByImportProductAsync(int ownerId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i=> i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts    
                    .Where(i => i.WarehouseId == warehouse.WarehouseId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

     
            
            return listImportProduct;
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

            if (listImportProduct == null || !listImportProduct.Any()) 
            {
                throw new Exception("This owner has never imported products!");
            }

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


        public async Task<Warehouse> GetWarehouseByIdAsync(int ownerId)
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

        public async Task<int> GetWarehouseIdByOwnerIdAsync(int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            return warehouse.WarehouseId;
        }



        //list ra detail nhưng mà group by theo product size giống bên warehousedetail

        public async Task<List<WarehouseDetail>> GetAllWarehouseDetailAsync(int ownerId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (warehouse == null) 
            {
                throw new Exception("This owner does not have a warehouse yet");
            }

            var result = await _context.WarehouseDetails
                .Where(wd => wd.WarehouseId == warehouse.WarehouseId)

            .GroupBy(w => new { w.ProductSizeId, w.UnitPrice})

        .Select(g => new WarehouseDetail
        {
            ProductSizeId = g.Key.ProductSizeId,
            Location = g.Select(i => i.Location).FirstOrDefault(),
            QuantityInStock = g.Sum(wd => wd.QuantityInStock),
            UnitPrice = g.Key.UnitPrice
        })
        .ToListAsync();
            if (result == null) 
            {
                throw new Exception("This owner does not have a warehouse yet");
            }

            var paginatedResult = result
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

            return paginatedResult;
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

        public async Task<bool> UpdateWarehouseAsync(WarehouseDTO warehouseDTO)
        {
            Warehouse? warehouse = await _context.Warehouses.Include(i => i.Owner).SingleOrDefaultAsync(i => i.WarehouseId == warehouseDTO.WarehouseId);
            if (warehouse == null)
            {
                throw new Exception("Warehouse is not exist!");
            }
                _mapper.Map(warehouseDTO, warehouse);
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
