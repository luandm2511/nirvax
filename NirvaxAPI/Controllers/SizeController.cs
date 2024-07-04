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
        public async Task<ActionResult<IEnumerable<Size>>> GetAllSizesAsync(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllSizesAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {  
                    Message = "Get list size " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
               
                Message = notFound + "any size"
            });
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetSizeByIdAsync(int sizeId)
        {
            var checkSizeExist = await _repo.CheckSizeExistAsync(sizeId);
            if (checkSizeExist == true) {
                var size = await _repo.GetSizeByIdAsync(sizeId);
                    return StatusCode(200, new
                    { 
                        Message = "Get size by id" + ok,
                        Data = size
                    });
                

            }
         
            return StatusCode(404, new
            {  
                Message = notFound + "any size"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateSizeAsync(SizeCreateDTO sizeCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var checkSize = await _repo.CheckSizeAsync(0, sizeCreateDTO.OwnerId, sizeCreateDTO.Name);
            if (checkSize == true)
            {
                var size1 = await _repo.CreateSizeAsync(sizeCreateDTO);
                if (size1)
                {
                    return StatusCode(200, new
                    { 
                        Message = "Create size " + ok,
                        Data = size1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    { 
                        Message = "Server error",                       
                    });
                }
             }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a size with that information",
                    });
                }
            }
            return StatusCode(400, new
            { 
                Message = "Please enter valid Staff",
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateSizeAsync(SizeDTO sizeDTO)
        {
            if (ModelState.IsValid)
            {
                var checkSize = await _repo.CheckSizeAsync(sizeDTO.SizeId, sizeDTO.OwnerId, sizeDTO.Name);
            if (checkSize == true)
            {
                var size1 = await _repo.UpdateSizeAsync(sizeDTO);
                if (size1)
                {
                    return StatusCode(200, new
                    { 
                        Message = "Update size" + ok,
                        Data = size1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Message = "Server error",
                        
                    });
                }
            }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a staff with that information",
                    });
                }
            }
            return StatusCode(400, new
            {
                Message = "Please enter valid Staff",
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> DeleteSizeAsync(int sizeId)
        {
            var size1 = await _repo.DeleteSizeAsync(sizeId);
            if (size1)
            {
                return StatusCode(200, new
                { 
                    Message = "Delete size " + ok,
                });
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> RestoreSizeAsync(int sizeId)
        {
            var size1 = await _repo.RestoreSizeAsync(sizeId);
            if (size1)
            {
                return StatusCode(200, new
                {
                    Message = "Restore size " + ok,
                });
            }
            return StatusCode(400, new
            { 
                Message = badRequest,
            });

        }
    }
}
