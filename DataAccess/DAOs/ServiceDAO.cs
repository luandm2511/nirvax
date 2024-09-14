using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class ServiceDAO
    {
        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public ServiceDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckServiceAsync(int serviceId, string name)
        {
            if (serviceId == 0)
            {
                Service? service = new Service();
                service = await _context.Services.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.Name.Trim() == name.Trim());
                if (service == null)
                {
                    return true;
                }
            }
            else
            {

                List<Service> getList = await _context.Services

                 //check khác Id`
                 .Where(i => i.ServiceId != serviceId)
                 .Where(i => i.Isdelete == false)
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

        //owner,staff
        public async Task<IEnumerable<Service>> GetAllServicesAsync(string? searchQuery, int page, int pageSize)
        {

            List<Service> getList = new List<Service>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.Services
               //     .Where(i => i.Isdelete == false)
                    .Where(i => i.Name.Trim().Contains(searchQuery.Trim()))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                
            }
            else
            {
                 getList = await _context.Services
                 //   .Where(i => i.Isdelete == false)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
             
            }
            return getList;
        }

        //user
        public async Task<IEnumerable<Service>> GetAllServiceForUserAsync(string? searchQuery)
        {
            List<Service> getList = new List<Service>();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                getList = await _context.Services
                    .Where(i => i.Isdelete == false)
                    .Where(i => i.Name.Trim().Contains(searchQuery.Trim()))
                    .ToListAsync();
               
            }
            else
            {
                 getList = await _context.Services
                               .Where(i => i.Isdelete == false)
                               .ToListAsync();            
            }
            return getList;

            
            
          
        }


        public async Task<Service> GetServiceByIdAsync(int sizeId)
        {
           
                Service? sid = await _context.Services.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ServiceId == sizeId);
                     
            return sid;
        }





        public async Task<bool> CreateServiceAsync(ServiceCreateDTO serviceCreateDTO)
        {
           
            Service service = _mapper.Map<Service>(serviceCreateDTO);
            service.Isdelete = false;
            await _context.Services.AddAsync(service);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> UpdateServiceAsync(ServiceDTO serviceDTO)
        {
            Service? service = await _context.Services.SingleOrDefaultAsync(i => i.ServiceId == serviceDTO.ServiceId);
           
            _mapper.Map(serviceDTO, service);

            service.Isdelete = false;

            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            Service? service = await _context.Services.SingleOrDefaultAsync(i => i.ServiceId == serviceId);
            if (service != null)
            {
                service.Isdelete = true;
                 _context.Services.Update(service);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RestoreServiceAsync(int serviceId)
        {
            Service? service = await _context.Services.SingleOrDefaultAsync(i => i.ServiceId == serviceId);
            if (service != null)
            {
                service.Isdelete = false;
                _context.Services.Update(service);
               

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
