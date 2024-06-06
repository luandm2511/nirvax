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

        public async Task<bool> CheckSize(SizeDTO sizeDTO)
        {

            Size? size = new Size();
            size = await _context.Sizes.Where(i => i.Isdelete == false).Where(i => i.OwnerId== sizeDTO.OwnerId).SingleOrDefaultAsync(i => i.SizeId == sizeDTO.SizeId);
            Size? sizeCreate = new Size();
            sizeCreate = await _context.Sizes.Where(i=>i.OwnerId== sizeDTO.OwnerId).SingleOrDefaultAsync(i => i.Name.Trim() == sizeDTO.Name.Trim());
            if (size != null)
            {
                List<Size> getList = await _context.Sizes
                 .Where(i => i.Isdelete == false)
                 //check khác Id`
                 .Where(i => i.SizeId != sizeDTO.SizeId)
                 .Where(i => i.Name == sizeDTO.Name)
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
            else if (sizeCreate == null && size == null)
            {
                return true;
            }
                return false;
        }

        public async Task<bool> CheckSizeExist(int sizeId)
        {
            Size? sid = new Size();

            sid = await _context.Sizes.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeId == sizeId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }


        //owner,staff
        public async Task<List<SizeDTO>> GetAllSizes(string? searchQuery, int page, int pageSize)
        {
            List<SizeDTO> listSizeDTO = new List<SizeDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Size> getList = await _context.Sizes
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.Name.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<SizeDTO>>(getList);
            }
            else
            {
                List<Size> getList = await _context.Sizes
                    .Where(i => i.Isdelete == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<SizeDTO>>(getList);
            }
            return listSizeDTO;
        }

        public async Task<SizeDTO> GetSizeById(int sizeId)
        {
            SizeDTO sizeDTO = new SizeDTO();
            try
            {
                Size? sid = await _context.Sizes.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeId == sizeId);
               
                    sizeDTO = _mapper.Map<SizeDTO>(sid);
                return sizeDTO;

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
          
        }

      



        public async Task<bool> CreateSize(SizeDTO sizeDTO)
        {
            sizeDTO.Isdelete = false;
            Size size = _mapper.Map<Size>(sizeDTO);
            await _context.Sizes.AddAsync(size);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateSize(SizeDTO sizeDTO)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeDTO.SizeId);
            //ánh xạ đối tượng SizeDTO đc truyền vào cho staff
            sizeDTO.Isdelete = false;
                _mapper.Map(sizeDTO, size);
                 _context.Sizes.Update(size);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeleteSize(int sizeId)
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
        public async Task<bool> RestoreSize(int sizeId)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
            //ánh xạ đối tượng SizeDTO đc truyền vào cho staff



            if (size != null)
            {
                size.Isdelete = false;
                _context.Sizes.Update(size);
                //    _mapper.Map(SizeDTO, staff);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





