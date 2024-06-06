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
    public  class DescriptionDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public DescriptionDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckDescription(DescriptionDTO descriptionDTO)
        {

            Description? description = new Description();
            description = await _context.Descriptions.SingleOrDefaultAsync(i => i.DescriptionId == descriptionDTO.DescriptionId);
            Description? sizeCreate = new Description();
            sizeCreate = await _context.Descriptions.SingleOrDefaultAsync(i => i.Content.Trim() == descriptionDTO.Content.Trim() || i.Title.Trim() == descriptionDTO.Title.Trim());
            if (description != null)
            {
                List<Description> getList = await _context.Descriptions
               //  .Where(i => i.Isdelete == false)
                 //check khác Id`
                 .Where(i => i.DescriptionId != descriptionDTO.DescriptionId)
                 .Where(i => i.Content == descriptionDTO.Content)
                 .Where(i => i.Title == descriptionDTO.Title)

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
            else if (sizeCreate == null && description == null)
            {
                return true;
            }
                return false;
        }

        public async Task<bool> CheckDescriptionExist(int descriptionId)
        {
            Description? sid = new Description();

            sid = await _context.Descriptions.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.DescriptionId == descriptionId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }


        //owner,staff
        public async Task<List<DescriptionDTO>> GetAllDescriptions(string? searchQuery, int page, int pageSize)
        {
            List<DescriptionDTO> listSizeDTO = new List<DescriptionDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Description> getList = await _context.Descriptions
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.Title.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<DescriptionDTO>>(getList);
            }
            else
            {
                List<Description> getList = await _context.Descriptions
                    .Where(i => i.Isdelete == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<DescriptionDTO>>(getList);
            }
            return listSizeDTO;
        }

        public async Task<DescriptionDTO> GetDescriptionById(int descriptionId)
        {
            DescriptionDTO descriptionDTO = new DescriptionDTO();
            try
            {
                Description? sid = await _context.Descriptions.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.DescriptionId == descriptionId);
               
                    descriptionDTO = _mapper.Map<DescriptionDTO>(sid);
                return descriptionDTO;

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
          
        }

      



        public async Task<bool> CreateDesctiption(DescriptionDTO descriptionDTO)
        {
            descriptionDTO.Isdelete = false;
            Description description = _mapper.Map<Description>(descriptionDTO);
            await _context.Descriptions.AddAsync(description);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateDesctiption(DescriptionDTO descriptionDTO)
        {
            Description? description = await _context.Descriptions.SingleOrDefaultAsync(i => i.DescriptionId == descriptionDTO.DescriptionId);
            //ánh xạ đối tượng DescriptionDTO đc truyền vào cho staff
            descriptionDTO.Isdelete = false;
                _mapper.Map(descriptionDTO, description);
                 _context.Descriptions.Update(description);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<bool> DeleteDesctiption(int descriptionId)
        {
            Description? description = await _context.Descriptions.SingleOrDefaultAsync(i => i.DescriptionId == descriptionId);
            //ánh xạ đối tượng DescriptionDTO đc truyền vào cho staff

               

            if (description != null)
            {
                description.Isdelete = true;
                 _context.Descriptions.Update(description);
                //    _mapper.Map(DescriptionDTO, staff);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
    }
}





