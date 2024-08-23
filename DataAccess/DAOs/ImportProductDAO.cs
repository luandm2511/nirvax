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

            sid = await _context.ImportProducts.SingleOrDefaultAsync(i => i.ImportId == importId) ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }
        public async Task<List<ImportProduct>> GetAllImportProductAsync(int ownerId,DateTime? from, DateTime? to)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<ImportProduct>(); }
            List<ImportProduct> listImport = new List<ImportProduct>();
         
            var getList = _context.ImportProducts
                .Where(i => i.OwnerId == ownerId).AsQueryable(); 
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
            .SingleOrDefaultAsync(x => x.ImportId == importId);
                
            return sid;
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



        public async Task<List<object>> ViewWeeklyImportProductAsync(int ownerId)
        {
            var listImportProduct = await _context.ImportProducts
                .Where(i => i.OwnerId == ownerId)
                .GroupBy(i => i.ImportDate.Date) 
                .Select(g => new
                {
                    ImportDate = g.Key,
                    TotalPrice = g.Sum(x => x.TotalPrice) 
                })
                .OrderBy(i => i.ImportDate)
                .ToListAsync();

            if (listImportProduct.Count == 0)
            {
                return new List<object>();
            }
            var weeklyStatistics = new Dictionary<(int year, DateTime weekStart), Dictionary<int, double>>();

            foreach (var product in listImportProduct)
            {
                DateTime currentWeekStart = product.ImportDate.AddDays(-(int)product.ImportDate.DayOfWeek + (int)DayOfWeek.Monday);
                int year = product.ImportDate.Year;
                int dayOfWeek = (int)product.ImportDate.DayOfWeek == 0 ? 8 : (int)product.ImportDate.DayOfWeek + 1;

                var key = (year, currentWeekStart);

                if (!weeklyStatistics.ContainsKey(key))
                {
                    weeklyStatistics[key] = new Dictionary<int, double>();
                }
                if (weeklyStatistics[key].ContainsKey(dayOfWeek))
                {
                    weeklyStatistics[key][dayOfWeek] += product.TotalPrice;
                }
                else
                {
                    weeklyStatistics[key][dayOfWeek] = product.TotalPrice;
                }
            }

            var result = new List<object>();

            foreach (var entry in weeklyStatistics)
            {
                var weekStart = entry.Key.weekStart;
                var weekEnd = weekStart.AddDays(6);

                var weekData = new
                {
                    year = entry.Key.year,
                    startDate = weekStart,
                    endDate = weekEnd,
                    dailyStatistics = entry.Value.Select(d => new
                    {
                        dayOfWeek = d.Key,
                        totalPrice = d.Value
                    }).ToList()
                };

                result.Add(weekData);
            }

            return result;
        }




        //số lần nhập hàng
        public async Task<object> ViewImportProductStatisticsAsync(int ownerId)
        {
            var sumImport = await _context.ImportProducts
                .Where(i => i.OwnerId == ownerId)
                .GroupBy(w => w.ImportId)
                .CountAsync();
            List<ImportProduct> listImportProduct = await _context.ImportProducts
                .Where(i => i.OwnerId == ownerId)
                .ToListAsync();
            var sumOfProduct = listImportProduct.Sum(p => p.Quantity);
            var sumOfPrice = listImportProduct.Sum(p => p.TotalPrice);
            var result = new Dictionary<string, object>
    {
        { "totalImportProduct", sumImport },
        { "totalQuantityByImport", sumOfProduct },
        { "totalPriceByImport", sumOfPrice }
    };

            return result;
        }




    }
}

  


