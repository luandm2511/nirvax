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
            try { 
                var list =await _repo.GetAllAdvertisementsAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    { 
                        Message = "Get list advertisement " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
            {
            try { 
                var list =await _repo.GetAllAdvertisementsWaitingAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    { 
                        Message = "Get list advertisement " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllAdvertisementsAcceptAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllAdvertisementsDenyAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            try { 
            var list =await _repo.GetAllAdvertisementsForUserAsync(searchQuery);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement for user" + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
            try
            {
                var list = await _repo.GetAdvertisementsByOwnerForUserAsync(searchQuery, ownerId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list advertisement by owner" + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            try { 
            var list = await _repo.GetAdvertisementsByOwnerAsync(searchQuery, page, pageSize, ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement by owner" + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {
            try
            {
                var list = await _repo.GetAllAdvertisementsByServiceAsync(serviceId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list advertisement by service" + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }


        [HttpGet("{adId}")]
            //  [Authorize]
            public async Task<ActionResult> GetAdvertisementByIdAsync(int adId)
            {
            try { 
                var checkSizeExist =await _repo.CheckAdvertisementExistAsync(adId);
                if (checkSizeExist == true)
                {
                    var advertisement =await _repo.GetAdvertisementByIdAsync(adId);
                    return StatusCode(200, new
                    {
                        Message = "Get advertisement by id" + ok,
                        Data = advertisement
                    });
                }
                return StatusCode(404, new
                { 
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet("{adId}")]
        //  [Authorize]
        public async Task<ActionResult> GetAdvertisementByIdForUserAsync(int adId)
        {
            try { 
                var advertisement =await _repo.GetAdvertisementByIdForUserAsync(adId);
            if (advertisement != null)
            {

                return StatusCode(200, new
                {
                    Message = "Get advertisement for user" + ok,
                    Data = advertisement
                });
            }

            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpPost]
            public async Task<ActionResult> CreateAdvertisementAsync([FromForm] AdvertisementCreateDTO advertisementCreateDTO)
            {
            try {
                if (ModelState.IsValid)
                {
                    var checkAd = await _repo.CheckAdvertisementCreateAsync(advertisementCreateDTO);
                    if (checkAd == true)
                    {

                        var advertisement1 = await _repo.CreateAdvertisementAsync(advertisementCreateDTO);
                            return StatusCode(200, new
                            {
                                Message = "Create advertisement " + ok,
                                Data = advertisement1
                            });                    
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a Advertisement with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Dont't accept empty information!",
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
            public async Task<ActionResult> UpdateAdvertisementAsync([FromForm] AdvertisementDTO advertisementDTO)
            {
            try { 
            if (ModelState.IsValid)
            {
                var checkAd =await _repo.CheckAdvertisementAsync(advertisementDTO);
                if (checkAd == true)
                {
                    
                    var advertisement1 =await _repo.UpdateAdvertisementAsync(advertisementDTO);          
                        return StatusCode(200, new
                        {
                            Message = "Update advertisement" + ok,
                            Data = advertisement1
                        });               
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a Advertisement with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
                Message = "Dont't accept empty information!",
            });
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
        public async Task<ActionResult> UpdateStatusAdvertisementByIdAsync(int adId, int StatusPostId)
        {
            try { 
            var checkAd =await _repo.CheckAdvertisementExistAsync(adId);
                if (checkAd == true)
                {
                    var advertisement1 = await _repo.UpdateStatusAdvertisementByIdAsync(adId, StatusPostId);
                        return StatusCode(200, new
                        {
                            Message = "Update advertisement" + ok,
                            Data = advertisement1
                        });                 
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "The name advertisement is already exist",
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
        public async Task<ActionResult> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {
            try { 
            var checkAd = await _repo.CheckAdvertisementExistAsync(adId);
            if (checkAd == true)
            {
                var advertisement1 = await _repo.UpdateStatusAdvertisementAsync(adId, statusPost);          
                    return StatusCode(200, new
                    {
                        Message = "Update advertisement" + ok,
                        Data = advertisement1
                    });            
            }
            return StatusCode(400, new
            {
                Message = "The name advertisement is already exist",
            });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> BlogStatisticsAsync(int ownerId)
        {
            var total2 = await _repo.ViewOwnerBlogStatisticsAsync(ownerId);
            var total = await _repo.ViewBlogStatisticsAsync();

            return StatusCode(200, new
                {
                    totalBlog = total,
                    totalOwnerBlog = total2,
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
                    Message = number,
                });
            }
            return StatusCode(200, new
            {
                Message = 0,
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
                    Message = number,

                });
            }
            return StatusCode(200, new
            { 
                Message = 0,
            });

        }


    }
}

