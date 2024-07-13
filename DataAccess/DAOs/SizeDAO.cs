using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Azure;
using Azure.Core;
using System.Numerics;

namespace DataAccess.DAOs
{
    public  class SizeDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public SizeDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckSizeAsync(int sizeId, int ownerId,string name)
        {
            if (sizeId == 0)
            {
                Size? size = new Size();
                size = await _context.Sizes.Include(i => i.Owner).Where(i => i.Isdelete == false).Where(i => i.OwnerId == ownerId).SingleOrDefaultAsync(i => i.SizeId == sizeId);
                if (size == null)
                {
                    return true;
                }
            }
            else
            {
                List<Size> getList = await _context.Sizes
                   .Where(i => i.Isdelete == false)
                   .Include(i => i.Owner)
                   .Where(i => i.OwnerId == ownerId)
                   .Where(i => i.SizeId != sizeId)
                   .Where(i => i.Name == name)
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

        public async Task<bool> CheckSizeExistAsync(int sizeId)
        {
            Size? sid = new Size();

            sid = await _context.Sizes.Include(i => i.Owner).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeId == sizeId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }


        //owner,staff
        public async Task<List<SizeDTO>> GetAllSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            List<SizeDTO> listSizeDTO = new List<SizeDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Size> getList = await _context.Sizes
                    .Where(i => i.Isdelete == false)
                    .Include(i => i.Owner)
                    .Where(i => i.Name.Contains(searchQuery))
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<SizeDTO>>(getList);
            }
            else
            {
                List<Size> getList = await _context.Sizes
                    .Where(i => i.Isdelete == false)
                    .Include(i => i.Owner)
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<SizeDTO>>(getList);
            }
            return listSizeDTO;
        }

        public async Task<SizeDTO> GetSizeByIdAsync(int sizeId)
        {
            SizeDTO sizeDTO = new SizeDTO();
            try
            {
                Size? sid = await _context.Sizes.Include(i => i.Owner).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeId == sizeId);
               
                    sizeDTO = _mapper.Map<SizeDTO>(sid);
                return sizeDTO;

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
          
        }

      



        public async Task<bool> CreateSizeAsync(SizeCreateDTO sizeCreateDTO)
        {
            Size size = _mapper.Map<Size>(sizeCreateDTO);
            size.Isdelete = false;
            await _context.Sizes.AddAsync(size);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateSizeAsync(SizeDTO sizeDTO)
        {
            Size? size = await _context.Sizes.Include(i => i.Owner).SingleOrDefaultAsync(i => i.SizeId == sizeDTO.SizeId);
            //ánh xạ đối tượng SizeDTO đc truyền vào cho staff
            sizeDTO.Isdelete = false;
                _mapper.Map(sizeDTO, size);
                 _context.Sizes.Update(size);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeleteSizeAsync(int sizeId)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
            //ánh xạ đối tượng SizeDTO đc truyền vào cho staff

               

            if (size != null)
            {
                size.Isdelete = true;
                 _context.Sizes.Update(size);
                //    _mapper.Map(SizeDTO, staff);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
        public async Task<bool> RestoreSizeAsync(int sizeId)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
            //ánh xạ đối tượng SizeDTO đc truyền vào cho staff



            if (size != null)
            {
                size.Isdelete = false;
                _context.Sizes.Update(size);
       

                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





