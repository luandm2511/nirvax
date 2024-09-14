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
using System.Drawing;

namespace DataAccess.DAOs
{
    public class ImportProductDetailDAO
    {

        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;

        public ImportProductDetailDAO(NirvaxContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImportProductDetailByImportDTO>> GetAllImportProductDetailByImportIdAsync(int importId)
        {
            List<ImportProductDetailByImportDTO> list = new List<ImportProductDetailByImportDTO>();

            List<ImportProductDetail> getList = await _context.ImportProductDetails
            .Include(i => i.Import).Include(i => i.ProductSize.Product).Include(i => i.ProductSize.Size)
            .Where(x => x.ImportId == importId).ToListAsync();
            if (getList == null) { return new List<ImportProductDetailByImportDTO>(); }
            var totalPrice = getList.Sum(i => i.UnitPrice * i.QuantityReceived);
            var totalQuantity = getList.Sum(i => i.QuantityReceived);
            list = _mapper.Map<List<ImportProductDetailByImportDTO>>(getList);
            list.ForEach(dto =>
            {
                dto.TotalPrice = totalPrice;
                dto.TotalQuantity = totalQuantity;
            });
            return list;
        }

        public async Task<IEnumerable<ImportProductDetail>> GetAllImportProductDetailAsync()
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

            var groupedItems = importProductDetailDTO
                .GroupBy(i => new { i.ProductId, i.SizeId })
                .Select(g => new ImportProductDetailCreateDTO
                {
                    ProductId = g.Key.ProductId,
                    SizeId = g.Key.SizeId,
                    QuantityReceived = g.Sum(x => x.QuantityReceived),
                    UnitPrice = g.First().UnitPrice
                })
                .ToList();

            foreach (var item in groupedItems)
            {
                var nameProductSize = $"{item.ProductId}_{item.SizeId}";

                var productSize = await _context.ProductSizes.FirstOrDefaultAsync(i => i.ProductSizeId == nameProductSize);
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

        public async Task<bool> UpdateImportProductDetailAsync(int importId, List<ImportProductDetailUpdateDTO> importProductDetailDTO)
        {
            var importProductDetail = await _context.ImportProductDetails.Where(i => i.ImportId == importId).ToListAsync();

            foreach (var item in importProductDetail)
            {
                foreach (var item1 in importProductDetailDTO)
                {
                    if (item.ProductSizeId == item1.ProductSizeId)
                    {
                        _mapper.Map(item1, item);
                        _context.ImportProductDetails.Update(item);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}




