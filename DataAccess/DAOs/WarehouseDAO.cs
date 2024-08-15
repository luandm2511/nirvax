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

       
     
        
        public async Task<Warehouse> UpdateQuantityAndPriceWarehouseAsync(int warehouseId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Include(i=>i.ImportProducts).Where(i => i.WarehouseId == warehouseId).FirstOrDefaultAsync();
            if (warehouse == null)
            {
                throw new Exception("This owner does not have a warehouse yet");
            }

            List<ImportProduct> listImportProduct = await _context.ImportProducts
              .Where(i => i.WarehouseId == warehouse.WarehouseId)
              .ToListAsync();

           
             warehouse.TotalQuantity = listImportProduct.Sum(p => p.Quantity);
            warehouse.TotalPrice = listImportProduct.Sum(p => p.TotalPrice);

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

        public async Task<List<WarehouseDetailListDTO>> GetAllWarehouseDetailByWarehouseAsync(int warehouseId, int page, int pageSize)
        {
            Warehouse warehouse = await _context.Warehouses.Where(i => i.WarehouseId == warehouseId).FirstOrDefaultAsync();
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
            var list = await _context.WarehouseDetails
         .Where(i => i.WarehouseId == warehouse.WarehouseId)
      //   .Include(i => i.ProductSize.Product)
     //    .Include(i => i.ProductSize.Size)
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .Select(wd => new WarehouseDetailListDTO
         {
             WarehouseId = wd.WarehouseId,
             ProductSizeId = wd.ProductSizeId,
             Location = wd.Location,
             ProductName = wd.ProductSize.Product.Name,
             SizeName = wd.ProductSize.Size.Name,
             Quantity = wd.ProductSize.Quantity,
           })
         .ToListAsync();

            return list;

           
        }
        public async Task<Dictionary<string, double>> ViewWeeklySalesRevenueAsync(int importId, int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses
                .Include(i => i.Owner)
                .Where(i => i.OwnerId == ownerId)
                .FirstOrDefaultAsync();

            var listImportProduct = await _context.ImportProducts
                .Where(i => i.WarehouseId == warehouse.WarehouseId)
                .Where(i => i.ImportId == importId)
                .OrderBy(i => i.ImportDate)
                .ToListAsync();

            if (listImportProduct.Count == 0)
            {
                return new Dictionary<string, double>();
            }

            DateTime firstImportDate = listImportProduct.First().ImportDate;

    
            DateTime startOfFirstWeek = firstImportDate.AddDays(-(int)firstImportDate.DayOfWeek + (int)DayOfWeek.Monday);


            var weeklyRevenue = new Dictionary<string, double>();

            foreach (var product in listImportProduct)
            {
               
                DateTime currentWeekStart = product.ImportDate.AddDays(-(int)product.ImportDate.DayOfWeek + (int)DayOfWeek.Monday);
                DateTime currentWeekEnd = currentWeekStart.AddDays(6);

           
                string weekKey = $"{currentWeekStart:dd/MM/yyyy} - {currentWeekEnd:dd/MM/yyyy}";

             
                if (weeklyRevenue.ContainsKey(weekKey))
                {
                    weeklyRevenue[weekKey] += product.TotalPrice;
                }
                else
                {
                    weeklyRevenue[weekKey] = product.TotalPrice;
                }
            }

            return weeklyRevenue;
        }


    }
}
