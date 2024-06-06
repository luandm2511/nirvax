using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISizeRepository  _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public SizeController(IConfiguration config, ISizeRepository repo)
        {
            _config = config;
             _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Size>>> GetAllSizes(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllSizes(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list size " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any size"
            });
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetSizeById(int sizeId)
        {
            var checkSizeExist = await _repo.CheckSizeExist(sizeId);
            if (checkSizeExist == true) {
                var size = await _repo.GetSizeById(sizeId);
            
         
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get size by id" + ok,
                        Data = size
                    });
                

            }
         
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any size"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateSize(SizeDTO sizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkSize = await _repo.CheckSize(sizeDTO);
            if (checkSize == true)
            {
                var size1 = await _repo.CreateSize(sizeDTO);
                if (size1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Create size " + ok,
                        Data = size1
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
                        Message = "There already exists a size with that information",
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Please enter valid Staff",
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateSize(SizeDTO sizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkSize = await _repo.CheckSize(sizeDTO);
            if (checkSize == true)
            {
                var size1 = await _repo.UpdateSize(sizeDTO);
                if (size1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update size" + ok,
                        Data = size1
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
                        Message = "There already exists a staff with that information",
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = "Please enter valid Staff",
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> DeleteSize(int sizeId)
        {
            var size1 = await _repo.DeleteSize(sizeId);
            if (size1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Delete size " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> RestoreSize(int sizeId)
        {
            var size1 = await _repo.RestoreSize(sizeId);
            if (size1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Restore size " + ok,

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
