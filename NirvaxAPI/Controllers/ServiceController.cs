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
            private readonly IServiceRepository  _repo;
            private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public ServiceController(IServiceRepository repo)
            {
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

                        Message = "Get list of services " + ok,
                        Data = list
                    });
                }
            else
            {
                return NoContent();

            }
        }


            [HttpGet]
        //  [Authorize]
            public async Task<ActionResult<IEnumerable<BusinessObject.Models.Service>>> GetAllServiceForUserAsync(string? searchQuery)
            {
                 var list = await _repo.GetAllServiceForUserAsync(searchQuery);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list service " + ok,
                        Data = list
                    });
                }
            else
            {
                return NoContent();

            }
        }


        [HttpGet("{serviceId}")]
            //  [Authorize]
            public async Task<ActionResult> GetServiceByIdAsync(int serviceId)
            {
               
                    var service = await _repo.GetServiceByIdAsync(serviceId);
            if(service != null) { 
                    return StatusCode(200, new
                    {
                        Message = "Get service by id" + ok,
                        Data = service
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any service"
                    });
                }
        }

            [HttpPost]
            public async Task<ActionResult> CreateServiceAsync(ServiceCreateDTO serviceCreateDTO)
            {
            try { 
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckServiceAsync(0, serviceCreateDTO.Name);
                if (checkService == true)
                {
                    var service1 = await _repo.CreateServiceAsync(serviceCreateDTO);
                    return StatusCode(200, new
                    {
                        Message = "Create service " + ok,
                        Data = service1
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a service with that information",
                    });
                }
            }
            else
            {
                return StatusCode(400, new
                {
                    Message = "Please enter valid Service!",
                });
            }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

            [HttpPut]
            public async Task<ActionResult> UpdateServiceAsync(ServiceDTO serviceDTO)
        {
            try { 
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckServiceAsync(serviceDTO.ServiceId, serviceDTO.Name);
                if (checkService == true)
                {
                    var service1 = await _repo.UpdateServiceAsync(serviceDTO);
                    return StatusCode(200, new
                    {
                        Message = "Update service" + ok,
                        Data = service1
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a service with that information",
                    });
                }

            }
            else
            {
                return StatusCode(400, new
                {


                    Message = "Please enter valid Service!",
                });
            }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
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
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
                    });
                }
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
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
                    });
                }  
        }
    }
}
