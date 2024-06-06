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
            public async Task<ActionResult<IEnumerable<Service>>> GetAllServices(string? searchQuery, int page, int pageSize)
            {
                var list = await _repo.GetAllServices(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list service " + ok,
                        Data = list
                    });
                }
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any service"
                });
            }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServiceForUser()
        {
            var list = await _repo.GetAllServiceForUser();
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list service " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any service"
            });
        }


        [HttpGet("{serviceId}")]
            //  [Authorize]
            public async Task<ActionResult> GetServiceById(int serviceId)
            {
                var checkServiceExist = await _repo.CheckServiceExist(serviceId);
                if (checkServiceExist == true)
                {
                    var service = await _repo.GetServiceById(serviceId);


                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get service by id" + ok,
                        Data = service
                    });


                }

                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any service"
                });
            }

            [HttpPost]
            public async Task<ActionResult> CreateService(ServiceDTO serviceDTO)
            {
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckService(serviceDTO);
                if (checkService == true)
                {
                    var service1 = await _repo.CreateService(serviceDTO);
                    if (service1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create service " + ok,
                            Data = service1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            Result = true,
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        StatusCode = 400,
                        Result = false,
                        Message = "There already exists a service with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Dont't accept empty information!",
            });

        }

            [HttpPut]
            public async Task<ActionResult> UpdateService(ServiceDTO serviceDTO)
        {
            if (ModelState.IsValid)
            {
                var checkService = await _repo.CheckService(serviceDTO);
                if (checkService == true)
                {
                    var service1 = await _repo.UpdateService(serviceDTO);
                    if (service1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update service" + ok,
                            Data = service1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            Result = true,
                            Message = "Server error",
                            Data = ""
                        });
                    }
                }
            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a service with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Dont't accept empty information!",
            });
            }

            [HttpPatch("{serviceId}")]
            public async Task<ActionResult> DeleteService(int serviceId)
            {
                var service1 = await _repo.DeleteService(serviceId);
                if (service1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Delete service " + ok,

                    });
                }
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = badRequest,
                });

            }

        [HttpPatch("{serviceId}")]
        public async Task<ActionResult> RestoreService(int serviceId)
        {
            var service1 = await _repo.RestoreService(serviceId);
            if (service1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Restore service " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }
    }
    }
