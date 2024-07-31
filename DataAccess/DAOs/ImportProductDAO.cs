using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DataAccess.DAOs
{
    public class ImportProductDAO
    {

        private readonly NirvaxContext _context;
         private readonly  IMapper _mapper;

  
        

        public ImportProductDAO(NirvaxContext context, IMapper mapper) 
        {
           
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckImportProductExistAsync(int importId)
        {
            ImportProduct? sid = new ImportProduct();

            sid = await _context.ImportProducts.Include(i => i.Warehouse).SingleOrDefaultAsync(i => i.ImportId == importId) ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }
        public async Task<List<ImportProduct>> GetAllImportProductAsync(int warehouseId,DateTime? from, DateTime? to)
        {
            List<ImportProduct> listImport = new List<ImportProduct>();
         
            var getList = _context.ImportProducts
                .Include(i => i.Warehouse).Where(i => i.WarehouseId == warehouseId).AsQueryable(); 
            if(from == null && to == null)
            {
                from= DateTime.Parse("2013-05-27");
                to = DateTime.Parse("2030-12-12");
            }
            #region Filtering
            if (from.HasValue)
            {
                getList = getList.Where(p => p.ImportDate >= from);
            }
            if (to.HasValue)
            {
                getList = getList.Where(p => p.ImportDate <= to);
            }
            #endregion
            List<ImportProduct> list = await getList.ToListAsync();
       
            return list;
        }

        //tự xem details số liệu
        public async Task<ImportProduct> GetImportProductByIdAsync(int importId)
        {
           
            ImportProduct? sid = await _context.ImportProducts
            .Include(i => i.Warehouse)
            .SingleOrDefaultAsync(x => x.ImportId == importId);
                
            return sid;
        }


        public async  Task<List<ImportProduct>> GetImportProductByWarehouseAsync(int warehouseId)
        {
          
            try
            {
                List<ImportProduct> getList =await _context.ImportProducts.Include(i => i.Warehouse).Where(i => i.WarehouseId == warehouseId).ToListAsync();
                return getList;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
          
        }


        public async Task<ImportProduct> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO)
        {
            if (importProductCreateDTO.ImportDate != null && importProductCreateDTO.ImportDate.Date >= DateTime.Now.Date)
            {
                ImportProduct importProduct = _mapper.Map<ImportProduct>(importProductCreateDTO);
               await  _context.ImportProducts.AddAsync(importProduct);
                int i =await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return importProduct;
                }
                else { return null; }
            }
            else if (importProductCreateDTO.ImportDate == null)
            {
                importProductCreateDTO.ImportDate = DateTime.Now;
                ImportProduct importProduct = _mapper.Map<ImportProduct>(importProductCreateDTO);
                await _context.ImportProducts.AddAsync(importProduct);
               int i= await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return importProduct;
                }
                else { return null; }
              
            }
            else return null;
        }

        public async Task<bool> UpdateQuantityAndPriceImportProductAsync(int importId)
        {
            List<ImportProductDetail> getList = await _context.ImportProductDetails
                .Where(i => i.ImportId == importId)
                .ToListAsync();

            double totalPrice = getList.Sum(i => i.UnitPrice * i.QuantityReceived);

            var importProduct = await _context.ImportProducts.SingleOrDefaultAsync(i => i.ImportId == importId);
            if (importProduct != null)
            {
                importProduct.TotalPrice = totalPrice;
                importProduct.Quantity = getList.Sum(i => i.QuantityReceived);
                _context.ImportProducts.Update(importProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<bool> UpdateImportProductAsync(ImportProductDTO importProductDTO)
        {
            ImportProduct? importProduct = await _context.ImportProducts.SingleOrDefaultAsync(i => i.ImportId == importProductDTO.ImportId);      

            if (importProduct != null)
            {
                _mapper.Map(importProductDTO, importProduct);
                _context.ImportProducts.Update(importProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }

        public async Task<int> ViewImportProductStatisticsAsync(int warehouseId)
        {

            var sumImport = await _context.ImportProducts.Where(i => i.WarehouseId == warehouseId).GroupBy(w => w.ImportId).CountAsync();
            return sumImport;
        }


        //tổng số sản phẩm lần nhập đó
        public async Task<int> ViewNumberOfProductByImportStatisticsAsync(int importId, int ownerId)
        {
            Warehouse warehouse = await _context.Warehouses.Include(i => i.Owner).Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();

            List<ImportProduct> listImportProduct = await _context.ImportProducts
             .Where(i => i.WarehouseId == warehouse.WarehouseId)
             .Where(i => i.ImportId == importId)
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
    }
}

  


