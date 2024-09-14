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
    public class SizeDAO
    {
        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;

        public SizeDAO(NirvaxContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckSizeAsync(int sizeId, int ownerId, string name)
        {
            if (sizeId == 0)
            {

                Size? size = new Size();
                size = await _context.Sizes.Include(i => i.Owner).Where(i => i.Isdelete == false)
                    .Where(i => i.OwnerId == ownerId).Where(i => i.Name.Trim() == name.Trim()).SingleOrDefaultAsync();
                if (size == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                List<Size> getList = await _context.Sizes
                   .Where(i => i.Isdelete == false)
                   .Include(i => i.Owner)
                   .Where(i => i.OwnerId == ownerId)
                   .Where(i => i.SizeId != sizeId)
                   .Where(i => i.Name.Trim() == name.Trim())
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

        public async Task<IEnumerable<Size>> GetAllSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkOwner == null) { return new List<Size>(); }
            List<Size> listSize = new List<Size>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                listSize = await _context.Sizes
               .Where(i => i.Isdelete == false)
                    .Include(i => i.Owner)
                    .Where(i => i.Name.Trim().Contains(searchQuery.Trim()))
                    .Where(i => i.OwnerId == ownerId && !i.Isdelete)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
            else
            {
                listSize = await _context.Sizes
                  .Where(i => i.Isdelete == false)
                    .Include(i => i.Owner)
                    .Where(i => i.OwnerId == ownerId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
            return listSize;
        }

        public async Task<Size> GetSizeByIdAsync(int sizeId)
        {

            Size? sid = await _context.Sizes.Include(i => i.Owner).Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.SizeId == sizeId);

            return sid;
        }


        public async Task<bool> CreateSizeAsync(SizeCreateDTO sizeCreateDTO)
        {
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == sizeCreateDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }
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
            var checkOwner = await _context.Owners.Where(i => i.OwnerId == sizeDTO.OwnerId).SingleOrDefaultAsync();
            if (checkOwner == null)
            {
                throw new Exception("Not exist this owner!");
            }
            Size? size = await _context.Sizes.Include(i => i.Owner).SingleOrDefaultAsync(i => i.SizeId == sizeDTO.SizeId);

            _mapper.Map(sizeDTO, size);
            size.Isdelete = false;

            _context.Sizes.Update(size);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSizeAsync(int sizeId)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
            if (size != null)
            {
                size.Isdelete = true;
                _context.Sizes.Update(size);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RestoreSizeAsync(int sizeId)
        {
            Size? size = await _context.Sizes.SingleOrDefaultAsync(i => i.SizeId == sizeId);
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





