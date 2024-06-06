using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IServiceImage;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public  class AdvertisementController : ControllerBase
    {
       
            private readonly IConfiguration _config;

            private readonly IAdvertisementRepository _repo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IImageService _serviceImg;

        private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public  AdvertisementController(IConfiguration config, IAdvertisementRepository repo, IWebHostEnvironment hostEnvironment, IImageService serviceImg)
            {
                _config = config;
            _serviceImg = serviceImg;

            _repo = repo;
            this._hostEnvironment = hostEnvironment;
            }


            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisements(string? searchQuery, int page, int pageSize)
            {
                var list =await _repo.GetAllAdvertisements(searchQuery, page, pageSize);
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
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsForUser(string? searchQuery)
        {
            var list =await _repo.GetAllAdvertisementsForUser(searchQuery);
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
            public async Task<ActionResult> GetAdvertisementById(int adId)
            {
                var checkSizeExist =await _repo.CheckAdvertisementExist(adId);
                if (checkSizeExist == true)
                {
                    var advertisement =await _repo.GetAdvertisementById(adId);


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
        public async Task<ActionResult> GetAdvertisementByIdForUser(int adId)
        {
           
                var advertisement =await _repo.GetAdvertisementByIdForUser(adId);
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
            public async Task<ActionResult> CreateAdvertisement([FromForm] AdvertisementCreateDTO advertisementCreateDTO)
            {
            if (ModelState.IsValid)
            {
                var checkAd = await _repo.CheckAdvertisementCreate(advertisementCreateDTO);
                if (checkAd == true)
                {
                    if (advertisementCreateDTO.ImageFile != null)
                    {
                        var imagePath = _serviceImg.SaveImage(advertisementCreateDTO.ImageFile, "ads");
                        advertisementCreateDTO.Image = imagePath;
                    }
                    var advertisement1 = await _repo.CreateAdvertisement(advertisementCreateDTO);
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
            public async Task<ActionResult> UpdateAdvertisement([FromForm] AdvertisementDTO advertisementDTO)
            {
            if (ModelState.IsValid)
            {
                var checkAd =await _repo.CheckAdvertisement(advertisementDTO);
                if (checkAd == true)
                {
                    if (advertisementDTO.ImageFile != null)
                    {
                        try
                        {
                            var imagePath = _serviceImg.SaveImage(advertisementDTO.ImageFile, "ads");
                            var adUpdate = await _repo.GetAdvertisementById(advertisementDTO.AdId);
                            // Xóa ảnh cũ trước khi cập nhật ảnh mới
                            if (!string.IsNullOrEmpty(adUpdate.Image))
                            {
                                _serviceImg.DeleteImage(adUpdate.Image);
                            }
                            advertisementDTO.Image = imagePath;
                        }
                        catch (InvalidOperationException ex)
                        {
                            return BadRequest(new { message = ex.Message });
                        }

                    }
                    var advertisement1 =await _repo.UpdateAdvertisement(advertisementDTO);
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
        public async Task<ActionResult> UpdateStatusAdvertisement(int adId, int StatusPostId)
        { 
            var checkAd =await _repo.CheckAdvertisementExist(adId);
            if (checkAd == true)
            {
                var advertisement1 =await _repo.UpdateStatusAdvertisement(adId, StatusPostId);
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
        public async Task<ActionResult> ViewOwnerBlogStatistics(int ownerId)
        {
            var number = await _repo.ViewOwnerBlogStatistics(ownerId);
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
        public async Task<ActionResult> ViewBlogStatistics()
        {
            var number = await _repo.ViewBlogStatistics();
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

