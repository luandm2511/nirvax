using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
            private readonly IConfiguration _config;
            private readonly IServiceRepository  _repo;
            private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public ServiceController(IConfiguration config, IServiceRepository repo)
            {
                _config = config;
                 _repo = repo;
            }

            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<BusinessObject.Models.Service>>> GetAllServicesAsync(string? searchQuery, int page, int pageSize)
            {
                var list = await _repo.GetAllServicesAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        
                        Message = "Get list service " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Status = "Find fail",
                    Message = notFound + "any service"
                });
            }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<BusinessObject.Models.Service>>> GetAllServiceForUserAsync()
        {
            var list = await _repo.GetAllServiceForUserAsync();
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    
                    Message = "Get list service " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any service"
            });
        }


        [HttpGet("{serviceId}")]
            //  [Authorize]
            public async Task<ActionResult> GetServiceByIdAsync(int serviceId)
            {
                var checkServiceExist = await _repo.CheckServiceExistAsync(serviceId);
                if (checkServiceExist == true)
                {
                    var service = await _repo.GetServiceByIdAsync(serviceId);


                    return StatusCode(200, new
                    {
                        
                        Message = "Get service by id" + ok,
                        Data = service
                    });


                }

                return StatusCode(404, new
                {
                    Status = "Find fail",
                    Message = notFound + "any service"
                });
            }

            [HttpPost]
            public async Task<ActionResult> CreateServiceAsync(ServiceCreateDTO serviceCreateDTO)
            {
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckServiceAsync(0, serviceCreateDTO.Name);
                if (checkService == true)
                {
                    var service1 = await _repo.CreateServiceAsync(serviceCreateDTO);
                    if (service1)
                    {
                        return StatusCode(200, new
                        {

                            
                            Message = "Create service " + ok,
                            Data = service1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                       
                        
                        Message = "There already exists a service with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
               
                
                Message = "Dont't accept empty information!",
            });

        }

            [HttpPut]
            public async Task<ActionResult> UpdateServiceAsync(ServiceDTO serviceDTO)
        {
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckServiceAsync(serviceDTO.ServiceId, serviceDTO.Name);
                if (checkService == true)
                {
                    var service1 = await _repo.UpdateServiceAsync(serviceDTO);
                    if (service1)
                    {
                        return StatusCode(200, new
                        {

                            
                            Message = "Update service" + ok,
                            Data = service1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }
            else
            {
                return StatusCode(400, new
                {
                   
                    
                    Message = "There already exists a service with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {
               
                
                Message = "Dont't accept empty information!",
            });
            }

            [HttpPatch("{serviceId}")]
            public async Task<ActionResult> DeleteServiceAsync(int serviceId)
            {
                var service1 = await _repo.DeleteServiceAsync(serviceId);
                if (service1)
                {
                    return StatusCode(200, new
                    {

                        
                        Message = "Delete service " + ok,

                    });
                }
                return StatusCode(400, new
                {
                   
                    
                    Message = badRequest,
                });

            }

        [HttpPatch("{serviceId}")]
        public async Task<ActionResult> RestoreServiceAsync(int serviceId)
        {
            var service1 = await _repo.RestoreServiceAsync(serviceId);
            if (service1)
            {
                return StatusCode(200, new
                {

                    
                    Message = "Restore service " + ok,

                });
            }
            return StatusCode(400, new
            {
               
                
                Message = badRequest,
            });

        }
    }
    }
