using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Commons;


namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
   
    public class StaffController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IStaffRepository  _repo;
        
        private readonly string ok = "successfully ";
        private readonly string notFound = "Not found ";
        private readonly string badRequest = "Failed! ";

        public StaffController(IConfiguration config, IStaffRepository repo)
        {
            _config = config;
             _repo = repo;           
        }


        [HttpGet]
        //  [Authorize]
        public async  Task<ActionResult<IEnumerable<Staff>>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {     
            var list =  await _repo.GetAllStaffsAsync(searchQuery, page, pageSize, ownerId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list of staffs " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any staff"
                    });
                }         
        }


      

        [HttpGet("{staffId}")]
        //  [Authorize]
        public async Task<ActionResult> GetStaffByIdAsync(int staffId)
        {
            var checkStaff = await _repo.CheckStaffExistAsync(staffId);
                if (checkStaff == true)
                {
                    var staff = await _repo.GetStaffByIdAsync(staffId);
                    return StatusCode(200, new
                    {
                        Message = "Get staff by id " + ok,
                        Data = staff
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "staff"
                    });
                }
        }

        [HttpGet("{staffEmail}")]
        //  [Authorize]
        public async Task<ActionResult> GetStaffByEmailAsync(string staffEmail)
        { 
            var checkStaff = await _repo.CheckProfileExistAsync(staffEmail);
                if (checkStaff == true)
                {
                    var staff = await _repo.GetStaffByEmailAsync(staffEmail);
                    return StatusCode(200, new
                    {
                        Message = "Get staff by email " + ok,
                        Data = staff
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "staff"
                    });
                }      
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateStaffAsync([FromForm] StaffCreateDTO staffCreateDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkStaff = await _repo.CheckStaffAsync(0, staffCreateDTO.Email, staffCreateDTO.Phone, staffCreateDTO.OwnerId);
                    if (checkStaff == true)
                    {
                        var staff1 = await _repo.CreateStaffAsync(staffCreateDTO);

                            return StatusCode(200, new
                            {
                                Message = "Create staff " + ok,
                                Data = staff1
                            });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a staff with that information",
                        });
                    }

                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Don't accept empty information!",
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
        public async Task<ActionResult> ChangePasswordStaffAsync(int staffId, string oldPassword, string newPasswod, string confirmPassword)
        {
            try
            {
                var checkStaff = await _repo.CheckStaffExistAsync(staffId);
                if (checkStaff == true)
                {
                    var staff1 = await _repo.ChangePasswordStaffAsync(staffId, oldPassword, newPasswod, confirmPassword);
                    return StatusCode(200, new
                    {
                        Message = "Change password of staff" + ok,
                        Data = staff1
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Staff not exist",
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
        public async Task<ActionResult> UpdateStaffAsync([FromForm] StaffDTO staffDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkStaff = await _repo.CheckStaffAsync(staffDTO.StaffId, staffDTO.Email, staffDTO.Phone, staffDTO.OwnerId);
                    if (checkStaff == true)
                    {

                        var staff1 = await _repo.UpdateStaffAsync(staffDTO);
                        return StatusCode(200, new
                        {
                            Message = "Update staff" + ok,
                            Data = staff1
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "Please enter valid Staff!",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Staff!",
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
        public async Task<ActionResult> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    var checkStaff = await _repo.CheckProfileStaffAsync(staffProfileDTO);
                    if (checkStaff == true)
                    {
                        var staff1 = await _repo.UpdateProfileStaffAsync(staffProfileDTO);
                            return StatusCode(200, new
                            {
                                Message = "Update profile staff" + ok,
                                Data = staff1
                            });                  
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a staff with that information",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Staff",
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
        public async Task<ActionResult> UpdateAvatarStaffAsync([FromForm] StaffAvatarDTO staffAvatarDTO)
        {
            try { 
            var checkOwner = await _repo.CheckStaffExistAsync(staffAvatarDTO.StaffId);
                if (checkOwner == true)
                {
                    var owner1 = await _repo.UpdateAvatarStaffAsync(staffAvatarDTO);  
                        return StatusCode(200, new
                        {
                            Message = "Update avatar staff " + ok,
                            Data = owner1
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
                    Message = "An error occurred: " + ex.Message
                });
            }

        }
        [HttpDelete("{staffId}")]
        public async Task<ActionResult> DeleteStaffAsync(int staffId)
        {
           
            var staff1 = await _repo.DeleteStaffAsync(staffId);
                if (staff1 == true)
                {
                    return StatusCode(200, new
                    {
                        Message = "Delete staff " + ok,
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

      

    }
}
