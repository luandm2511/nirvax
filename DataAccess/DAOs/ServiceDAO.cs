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
                service = await _context.Services.SingleOrDefaultAsync(i => i.Name == name);
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

        public async Task<bool> CheckServiceExistAsync(int serviceId)
        {
            Service? sid = new Service();

            sid = await _context.Services.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ServiceId == serviceId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }

        //owner,staff
        public async Task<List<ServiceDTO>> GetAllServicesAsync(string? searchQuery, int page, int pageSize)
        {
            List<ServiceDTO> listSizeDTO = new List<ServiceDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Service> getList = await _context.Services
                    
                    .Where(i => i.Name.Contains(searchQuery))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<ServiceDTO>>(getList);
            }
            else
            {
                List<Service> getList = await _context.Services
                    
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<ServiceDTO>>(getList);
            }
            return listSizeDTO;
        }

        //user
        public async Task<List<ServiceDTO>> GetAllServiceForUserAsync()
        {
            List<ServiceDTO> listSizeDTO = new List<ServiceDTO>();


                List<Service> getList = await _context.Services
                    .Where(i => i.Isdelete == false)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<ServiceDTO>>(getList);
            
            return listSizeDTO;
        }


        public async Task<ServiceDTO> GetServiceByIdAsync(int sizeId)
        {
            ServiceDTO serviceDTO = new ServiceDTO();
            try
            {
                Service? sid = await _context.Services.Where(i => i.Isdelete == false).SingleOrDefaultAsync(i => i.ServiceId == sizeId);

                serviceDTO = _mapper.Map<ServiceDTO>(sid);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return serviceDTO;
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
            //ánh xạ đối tượng ServiceDTO đc truyền vào cho staff


            serviceDTO.Isdelete = false;
            _mapper.Map(serviceDTO, service);
             _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            Service? service = await _context.Services.SingleOrDefaultAsync(i => i.ServiceId == serviceId);
            //ánh xạ đối tượng ServiceDTO đc truyền vào cho staff



            if (service != null)
            {
                service.Isdelete = true;
                 _context.Services.Update(service);
                //    _mapper.Map(ServiceDTO, staff);

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
