using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public  class AdvertisementController : ControllerBase
    {
       
            private readonly IConfiguration _config;

            private readonly IAdvertisementRepository _repo;
        private readonly IWebHostEnvironment _hostEnvironment;
   

        private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public  AdvertisementController(IConfiguration config, IAdvertisementRepository repo, IWebHostEnvironment hostEnvironment)
            {
                _config = config;
            
            _repo = repo;
            this._hostEnvironment = hostEnvironment;
            }


            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize)
            {
                var list =await _repo.GetAllAdvertisementsAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list advertisement " + ok,
                        Data = list
                    });
                }
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any advertisement"
                });
            }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            var list =await _repo.GetAllAdvertisementsForUserAsync(searchQuery);
            if (list!=null)
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list advertisement for user" + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any advertisement"
            });
        }


        [HttpGet("{adId}")]
            //  [Authorize]
            public async Task<ActionResult> GetAdvertisementByIdAsync(int adId)
            {
                var checkSizeExist =await _repo.CheckAdvertisementExistAsync(adId);
                if (checkSizeExist == true)
                {
                    var advertisement =await _repo.GetAdvertisementByIdAsync(adId);


                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get advertisement by id" + ok,
                        Data = advertisement
                    });


                }

                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any advertisement"
                });
            }

        [HttpGet("{adId}")]
        //  [Authorize]
        public async Task<ActionResult> GetAdvertisementByIdForUserAsync(int adId)
        {
           
                var advertisement =await _repo.GetAdvertisementByIdForUserAsync(adId);
            if (advertisement != null)
            {

                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get advertisement for user" + ok,
                    Data = advertisement
                });
            }

            return StatusCode(400, new
            {
                Status = "Find fail",
                Message = notFound + "any advertisement"
            });
        }

        [HttpPost]
            public async Task<ActionResult> CreateAdvertisementAsync([FromForm] AdvertisementCreateDTO advertisementCreateDTO)
            {
            if (ModelState.IsValid)
            {
                var checkAd = await _repo.CheckAdvertisementCreateAsync(advertisementCreateDTO);
                if (checkAd == true)
                {
                   
                    var advertisement1 = await _repo.CreateAdvertisementAsync(advertisementCreateDTO);
                    if (advertisement1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create advertisement " + ok,
                            Data = advertisement1
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
                        Message = "There already exists a Advertisement with that information",
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
            public async Task<ActionResult> UpdateAdvertisementAsync([FromForm] AdvertisementDTO advertisementDTO)
            {
            if (ModelState.IsValid)
            {
                var checkAd =await _repo.CheckAdvertisementAsync(advertisementDTO);
                if (checkAd == true)
                {
                    
                    var advertisement1 =await _repo.UpdateAdvertisementAsync(advertisementDTO);
                    if (advertisement1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update advertisement" + ok,
                            Data = advertisement1
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
                        Message = "There already exists a Advertisement with that information",
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
        public async Task<ActionResult> UpdateStatusAdvertisementAsync(int adId, int StatusPostId)
        { 
            var checkAd =await _repo.CheckAdvertisementExistAsync(adId);
            if (checkAd == true)
            {
                var advertisement1 =await _repo.UpdateStatusAdvertisementAsync(adId, StatusPostId);
                if (advertisement1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update advertisement" + ok,
                        Data = advertisement1
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
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "The name advertisement is already exist",
            });

        }
        [HttpGet]
        public async Task<ActionResult> ViewOwnerBlogStatisticsAsync(int ownerId)
        {
            var number = await _repo.ViewOwnerBlogStatisticsAsync(ownerId);
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> ViewBlogStatisticsAsync()
        {
            var number = await _repo.ViewBlogStatisticsAsync();
            if (number != null)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = number,

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

