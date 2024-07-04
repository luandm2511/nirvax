using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOwnerRepository  _repo;
        private readonly IEmailService _emailService;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public OwnerController(IConfiguration config, IOwnerRepository repo)
        {
            _config = config;
             _repo = repo;
      
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAllOwnersAsync(string? searchQuery, int page, int pageSize)
        {
            var list = await _repo.GetAllOwnersAsync( searchQuery, page,  pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {                
                    Message = "Get list owner " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {               
                Message = notFound + "any owner"
            });
        }


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAllOwnersForUserAsync(string? searchQuery)
        {
            var list = await _repo.GetAllOwnersForUserAsync(searchQuery);
            if (list.Any())
            {
                return StatusCode(200, new
                {           
                    Message = "Get list owner " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {               
                Message = notFound + "any owner"
            });
        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetOwnerByIdAsync(int ownerId)
        {
            var checkOwner = await _repo.CheckOwnerExistAsync(ownerId);
            if(checkOwner == true)
            {
                var owner = await _repo.GetOwnerByIdAsync(ownerId);
                return StatusCode(200, new
                {
                    
                    Message = "Get owner by id " + ok,
                    Data = owner
                });
            }
          
            return StatusCode(404, new
            {               
                Message = notFound + "any owner"
            });
        }

        [HttpGet("{ownerEmail}")]
        //  [Authorize]
        public async Task<ActionResult> GetOwnerByEmailAsync(string ownerEmail)
        {
            var checkOwner = await _repo.CheckProfileExistAsync(ownerEmail);
            if (checkOwner == true)
            {
                var owner = await _repo.GetOwnerByEmailAsync(ownerEmail);
                return StatusCode(200, new
                {                   
                    Message = "Get owner by email " + ok,
                    Data = owner
                });
            }

            return StatusCode(404, new
            {              
                Message = notFound + "any owner"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateOwnerAsync(OwnerDTO ownerDTO)
        {
            var owner1 = await _repo.CreateOwnerAsync(ownerDTO);
            if (owner1)
            {
                return StatusCode(200, new
                {                  
                    Message = "Create owner " + ok,
                    Data = owner1
                });
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

         

        [HttpPut]
        public async Task<ActionResult> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPasswod)
        {
            var checkOwner = await _repo.CheckOwnerExistAsync(ownerId);
            if (checkOwner == true)
            {
                var owner1 = await _repo.ChangePasswordOwnerAsync(ownerId, oldPassword, newPasswod);
                if (owner1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Change password of owner" + ok,
                        Data = owner1
                    });
                }
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            if (ModelState.IsValid)
            {
                var checkOwner = await _repo.CheckOwnerAsync(ownerDTO);
            if (checkOwner == true)
            {
                var owner1 = await _repo.UpdateOwnerAsync(ownerDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Update owner" + ok,
                        Data = owner1
                    });
                } else
                {
                    return StatusCode(500, new
                    {
                        Message = "Internet server error",
                    });
                }
            }
            else
            {
                return StatusCode(400, new
                {
                    Message = "There already exists a owner with that information",
                });
            }

        }
               
            return StatusCode(400, new
            {  
                Message = "Dont't accept empty information!",
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            if (ModelState.IsValid)
            {
                var checkOwner = await _repo.CheckProfileOwnerAsync(ownerProfileDTO);
            if (checkOwner == true)
            {
                var owner1 = await _repo.UpdateProfileOwnerAsync(ownerProfileDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    { 
                        Message = "Update profile owner" + ok,
                        Data = owner1
                    });
                } else
                    {
                        return StatusCode(400, new
                        {
                            Message = "Internet server error",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {   
                        Message = "There already exists a owner with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
                Message = "Dont't accept empty information!",
            });


        }


        [HttpPut]
        public async Task<ActionResult> UpdateAvatarOwnerAsync([FromForm] OwnerAvatarDTO ownerAvatarDTO)
        {
            var checkOwner = await _repo.CheckOwnerExistAsync(ownerAvatarDTO.OwnerId);
            if (checkOwner == true)
            {
                
                var owner1 = await _repo.UpdateAvatarOwnerAsync(ownerAvatarDTO);
                if (owner1)
                {
                    return StatusCode(200, new
                    {
                        Message = "Update avatar owner" + ok,
                        Data = owner1
                    });
                }
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }
        

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> BanOwnerAsync(int ownerId)
        {
            try
            {
                var owner1 = await _repo.BanOwnerAsync(ownerId);
                if (owner1)
                {
                    var email = await _repo.GetEmailAsync(ownerId);
                    await _emailService.SendEmailAsync(email, "Ban", "Your account violates the policy, so we temporarily and permanently block your account!");
                    return StatusCode(200, new
                    {
                        Message = "Ban owner " + ok
                    });
                }
                return StatusCode(400, new
                {
                    Message = badRequest,
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

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> UnBanOwnerAsync(int ownerId)
        {
            var owner1 = await _repo.UnBanOwnerAsync(ownerId);
            if (owner1)
            {
                return StatusCode(200, new
                {
                    Message = "UnBan owner " + ok,
                });
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

        [HttpGet]
        public async Task<ActionResult> NumberOfOwnerStatisticsAsync()
        {
            var number = await _repo.NumberOfOwnerStatisticsAsync();
            if (number != null)
            {
                return StatusCode(200, new
                {
                    Message = number,
                });
            }
            return StatusCode(400, new
            {
                Message = badRequest,
            });

        }

    }
}
