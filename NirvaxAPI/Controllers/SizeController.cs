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
        private readonly ISizeRepository  _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public SizeController(ISizeRepository repo)
        {
             _repo = repo;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Size>>> GetAllSizesAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var list = await _repo.GetAllSizesAsync(searchQuery, page, pageSize, ownerId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list of sizes " + ok,
                        Data = list
                    });
                }
            else
            {
                return NoContent();

            }
        }


        [HttpGet("{sizeId}")]
        //  [Authorize]
        public async Task<ActionResult> GetSizeByIdAsync(int sizeId)
        {
            try { 
          
                    var size = await _repo.GetSizeByIdAsync(sizeId);
                if(size != null) { 
                    return StatusCode(200, new
                    {
                        Message = "Get size by id" + ok,
                        Data = size
                    });


                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any size"
                    });
                }
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateSizeAsync(SizeCreateDTO sizeCreateDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkSize = await _repo.CheckSizeAsync(0, sizeCreateDTO.OwnerId, sizeCreateDTO.Name);
                    if (checkSize == true)
                    {
                        var size1 = await _repo.CreateSizeAsync(sizeCreateDTO);                    
                            return StatusCode(200, new
                            {
                                Message = "Create size " + ok,
                                Data = size1
                            });                      
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a size with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Size!",
                    });
                }
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateSizeAsync(SizeDTO sizeDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkSize = await _repo.CheckSizeAsync(sizeDTO.SizeId, sizeDTO.OwnerId, sizeDTO.Name);
                    if (checkSize == true)
                    {
                        var size1 = await _repo.UpdateSizeAsync(sizeDTO);
                        return StatusCode(200, new
                        {
                            Message = "Update size" + ok,
                            Data = size1
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a size with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Size",
                    });
                }
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

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
                else
                {
                    return StatusCode(400, new
                    {
                        Message = badRequest,
                    });
                }  
        }

        [HttpPatch("{sizeId}")]
        public async Task<ActionResult> RestoreSizeAsync(int sizeId)
        {
            try { 
            var size1 = await _repo.RestoreSizeAsync(sizeId);
                if (size1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Restore size " + ok,
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
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }
    }
}
