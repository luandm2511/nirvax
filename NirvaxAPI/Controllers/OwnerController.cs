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
        private readonly IOwnerRepository  _repo;
        private readonly IEmailService _emailService;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public OwnerController(IOwnerRepository repo, IEmailService emailService)
        {
             _repo = repo;
            _emailService = emailService;
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
                        Message = "Get list of owners " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any owner"
                    });
                }     
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
                        Message = "Get list of owners " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any owner"
                    });
                }
        }


        [HttpGet("{ownerId}")]
        //  [Authorize]
        public async Task<ActionResult> GetOwnerByIdAsync(int ownerId)
        {        
                    var owner = await _repo.GetOwnerByIdAsync(ownerId);
            if(owner != null) { 
                    return StatusCode(200, new
                    {

                        Message = "Get owner by id " + ok,
                        Data = owner
                    });
            }
             else
            {
                return StatusCode(404, new
                {
                    Message = notFound + "any owner"
                });
            }
        }

        [HttpGet("{ownerEmail}")]
        //  [Authorize]
        public async Task<ActionResult> ViewOwnerProfileAsync(string ownerEmail)
        {       
                    var owner = await _repo.ViewOwnerProfileAsync(ownerEmail);
            if (owner != null)
            {
                return StatusCode(200, new
                {
                    Message = "Get view owner profile  " + ok,
                    Data = owner
                });
        }
             else
            {
                return StatusCode(404, new
                {
                    Message = notFound + "any owner"
                });
            }
            
        }

           

        [HttpPut]
        public async Task<ActionResult> ChangePasswordOwnerAsync(int ownerId, string oldPassword, string newPassword, string confirmPassword)
        {
            try { 
                    var checkOwner = await _repo.ChangePasswordOwnerAsync(ownerId, oldPassword, newPassword, confirmPassword);
                if (checkOwner == true)
                {
                    return StatusCode(200, new
                    {
                        Message = "Change password of owner" + ok,
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateOwnerAsync(OwnerDTO ownerDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkOwner = await _repo.CheckOwnerAsync(ownerDTO);
                    if (checkOwner == true)
                    {
                        var owner1 = await _repo.UpdateOwnerAsync(ownerDTO);
                            return StatusCode(200, new
                            {
                                Message = "Update owner" + ok,
                                Data = owner1
                            });                    
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a owner with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Owner!",
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
        public async Task<ActionResult> UpdateProfileOwnerAsync(OwnerProfileDTO ownerProfileDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkOwner = await _repo.CheckProfileOwnerAsync(ownerProfileDTO);
                    if (checkOwner == true)
                    {
                        var owner1 = await _repo.UpdateProfileOwnerAsync(ownerProfileDTO);
                        return StatusCode(200, new
                        {
                            Message = "Update profile owner" + ok,
                            Data = owner1
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "Please enter valid Owner!",
                        });
                    }

                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Owner!",
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
        public async Task<ActionResult> UpdateAvatarOwnerAsync([FromForm] OwnerAvatarDTO ownerAvatarDTO)
        {
            try
            {
                var checkOwner = await _repo.UpdateAvatarOwnerAsync(ownerAvatarDTO);
            if (checkOwner == true)
            {
                return StatusCode(200, new
                {
                    Message = "Update avatar owner" + ok,
                });
            }
            else
            {
                return StatusCode(400, new
                {
                    Message = "Error!",
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
        

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> BanOwnerAsync(int ownerId)
        {
                var owner1 = await _repo.BanOwnerAsync(ownerId);
                if (owner1)
                {
                    var email = await _repo.GetEmailAsync(ownerId);
                    if(email == null)
                    {
                       return StatusCode(404, new
                       {
                        Message = "Not found email!",
                       });
                    }
                    await _emailService.SendEmailAsync(email, "Ban", "Your account violates the policy, so we temporarily and permanently block your account!");
                    return StatusCode(200, new
                    {
                        Message = "Ban owner " + ok
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

        [HttpPatch("{ownerId}")]
        public async Task<ActionResult> UnBanOwnerAsync(int ownerId)
        {
            var owner1 = await _repo.UnBanOwnerAsync(ownerId);
            if (owner1)
            {
                var email = await _repo.GetEmailAsync(ownerId);
                if (email == null)
                {
                    return StatusCode(404, new
                    {
                        Message = "Not found email!",
                    });
                }
                await _emailService.SendEmailAsync(email, "UnBan", "After review, we have made the decision to reopen your account.!");
                return StatusCode(200, new
                {
                    Message = "UnBan owner " + ok
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
            return StatusCode(200, new
            {
                Message = 0,
            });

        }

    }
}
