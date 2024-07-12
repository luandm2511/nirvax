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
        public async Task<List<ImportProductDTO>> GetAllImportProductAsync(int warehouseId,DateTime? from, DateTime? to)
        {
            List<ImportProductDTO> listImportDTO = new List<ImportProductDTO>();
         
            var getList = _context.ImportProducts
                .Include(i => i.Warehouse).Where(i => i.WarehouseId == warehouseId).AsQueryable(); 
            if(from == null && to == null)
            {
                from= DateTime.Parse("2013-05-27");
                to = DateTime.Parse("2025-12-12");
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

            listImportDTO = _mapper.Map<List<ImportProductDTO>>(list);
            
            return listImportDTO;
        }

        //tự xem details số liệu
        public async Task<ImportProductDTO> GetImportProductByIdAsync(int importId)
        {
            ImportProductDTO importProductDTO = new ImportProductDTO();
            ImportProduct? sid = await _context.ImportProducts
            .Include(i => i.Warehouse)
            .SingleOrDefaultAsync(x => x.ImportId == importId);
            importProductDTO = _mapper.Map<ImportProductDTO>(sid);
            
            return importProductDTO;
        }


        public async  Task<List<ImportProductDTO>> GetImportProductByWarehouseAsync(int warehouseId)
        {
            List<ImportProductDTO> listImportProductDTO;
            try
            {
                List<ImportProduct> getList =await _context.ImportProducts.Include(i => i.Warehouse).Where(i => i.WarehouseId == warehouseId).ToListAsync();
                listImportProductDTO = _mapper.Map<List<ImportProductDTO>>(getList);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return listImportProductDTO;
        }


        public async Task<bool> CreateImportProductAsync(ImportProductCreateDTO importProductCreateDTO)
        {
            if (importProductCreateDTO.ImportDate != null && importProductCreateDTO.ImportDate.Date >= DateTime.Now.Date)
            {
                ImportProduct importProduct = _mapper.Map<ImportProduct>(importProductCreateDTO);
               await  _context.ImportProducts.AddAsync(importProduct);
                int i =await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else { return false; }
            }
            else if (importProductCreateDTO.ImportDate == null)
            {
                importProductCreateDTO.ImportDate = DateTime.Now;
                ImportProduct importProduct = _mapper.Map<ImportProduct>(importProductCreateDTO);
                await _context.ImportProducts.AddAsync(importProduct);
               int i= await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else { return false; }
              
            }
            else return false;
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

      

       
    }
    }

  


