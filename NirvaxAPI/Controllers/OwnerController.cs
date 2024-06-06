using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using WebAPI.IServiceImage;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOwnerRepository  _repo;
        private readonly IImageService _serviceImg;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public OwnerController(IConfiguration config, IOwnerRepository repo, IImageService serviceImg)
        {
            _config = config;
             _repo = repo;
            _serviceImg= serviceImg;
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAllOwners(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllOwners( searchQuery, page,  pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list owner " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any owner"
            });
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAllOwnersForUser(string? searchQuery)
        {
            var list = await _repo.GetAllOwnersForUser(searchQuery);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list owner " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any owner"
            });
        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetOwnerById(int ownerId)
        {
            var checkOwner = await _repo.CheckOwnerExist(ownerId);
            if(checkOwner == true)
            {
                var owner = await _repo.GetOwnerById(ownerId);
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get owner by id " + ok,
                    Data = owner
                });
            }
          
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any owner"
            });
        }

        [HttpGet("{ownerEmail}")]
        //  [Authorize]
        public async Task<ActionResult> GetOwnerByEmail(string ownerEmail)
        {
            var checkOwner = await _repo.CheckProfileExist(ownerEmail);
            if (checkOwner == true)
            {
                var owner = await _repo.GetOwnerByEmail(ownerEmail);
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get owner by email " + ok,
                    Data = owner
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any owner"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateOwner(OwnerDTO ownerDTO)
        {
            var owner1 = await _repo.CreateOwner(ownerDTO);
            if (owner1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Create owner " + ok,
                    Data = owner1
                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

         

        [HttpPut]
        public async Task<ActionResult> ChangePasswordOwner(int ownerId, string oldPassword, string newPasswod)
        {
            var checkOwner = await _repo.CheckOwnerExist(ownerId);
            if (checkOwner == true)
            {
                var owner1 = await _repo.ChangePasswordOwner(ownerId, oldPassword, newPasswod);
                if (owner1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Change password of owner" + ok,
                        Data = owner1
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateOwner(OwnerDTO ownerDTO)
        {
            if (ModelState.IsValid)
            {
                var checkOwner = await _repo.CheckOwner(ownerDTO);
            if (checkOwner == true)
            {
                var owner1 = await _repo.UpdateOwner(ownerDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update owner" + ok,
                        Data = owner1
                    });
                } else
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Result = false,
                        Message = "Internet server error",

                    });
                }
            }
            else
            {
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
                    Message = "There already exists a owner with that information",
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
        public async Task<ActionResult> UpdateProfileOwner(OwnerProfileDTO ownerProfileDTO)
        {
            if (ModelState.IsValid)
            {
                var checkOwner = await _repo.CheckProfileOwner(ownerProfileDTO);
            if (checkOwner == true)
            {
                var owner1 = await _repo.UpdateProfileOwner(ownerProfileDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update profile owner" + ok,
                        Data = owner1
                    });
                } else
                    {
                        return StatusCode(400, new
                        {
                            StatusCode = 500,
                            Result = false,
                            Message = "Internet server error",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        StatusCode = 400,
                        Result = false,
                        Message = "There already exists a owner with that information",
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
        public async Task<ActionResult> UpdateAvatarOwner([FromForm] OwnerAvatarDTO ownerAvatarDTO)
        {
            var checkOwner = await _repo.CheckOwnerExist(ownerAvatarDTO.OwnerId);
            if (checkOwner == true)
            {
                if (ownerAvatarDTO.ImageFile != null)
                {
                    try
                    {
                        var imagePath = _serviceImg.SaveImage(ownerAvatarDTO.ImageFile, "owners");
                        var staffUpdate = await _repo.GetOwnerById(ownerAvatarDTO.OwnerId);
                        // Xóa ảnh cũ trước khi cập nhật ảnh mới
                        if (!string.IsNullOrEmpty(staffUpdate.Image))
                        {
                            _serviceImg.DeleteImage(staffUpdate.Image);
                        }
                        ownerAvatarDTO.Image = imagePath;
                    }
                    catch (InvalidOperationException ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }

                }
                var owner1 = await _repo.UpdateAvatarOwner(ownerAvatarDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update avatar owner" + ok,
                        Data = owner1
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }
        

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> BanOwner(int ownerId)
        {
            var owner1 = await _repo.BanOwner(ownerId);
            if (owner1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Ban owner " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                Result = false,
                Message = badRequest,
            });

        }

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> UnBanOwner(int ownerId)
        {
            var owner1 = await _repo.UnBanOwner(ownerId);
            if (owner1)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "UnBan owner " + ok,

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
        public async Task<ActionResult> NumberOfOwnerStatistics()
        {
            var number = await _repo.NumberOfOwnerStatistics();
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
