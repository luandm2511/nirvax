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
        public async  Task<ActionResult<IEnumerable<Staff>>> GetAllStaffsAsync(string? searchQuery, int page, int pageSize)
        {
            var list =  await _repo.GetAllStaffsAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
           
                    Message = "Get list staff " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
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

            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
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

            return StatusCode(404, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateStaffAsync([FromForm] StaffCreateDTO staffCreateDTO)
        {
           
            if(ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckStaffAsync(0,staffCreateDTO.Email, staffCreateDTO.Phone);
                if (checkStaff == true)
                {
                  

                    var staff1 = await _repo.CreateStaffAsync(staffCreateDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                         
                            Message = "Create staff " + ok,
                            Data = staff1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                           
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
                       
                        Message = "There already exists a staff with that information",
                    });
                }
          
            }
               
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "Dont't accept empty information!",
            });

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
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                           
                            Message = "Change password of staff" + ok,
                            Data = staff1
                        });
                    }
                    return StatusCode(500, new
                    {

                       
                        Message = "Internet error"
                        // Data = staff1
                    });
                }
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    
                    Message = "Staff not exist",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateStaffAsync([FromForm] StaffDTO staffDTO)
        {
            if (ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckStaffAsync(staffDTO.StaffId, staffDTO.Email, staffDTO.Phone);
                if (checkStaff == true)
                {
                      
                    var staff1 = await _repo.UpdateStaffAsync(staffDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                           
                            Message = "Update staff" + ok,
                            Data = staff1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                           
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
                        
                        Message = "There already exists a staff with that information",
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "Please enter valid Staff",
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfileStaffAsync(StaffProfileDTO staffProfileDTO)
        {
            if (ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckProfileStaffAsync(staffProfileDTO);
                if (checkStaff == true)
                {
                    var staff1 = await _repo.UpdateProfileStaffAsync(staffProfileDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                           
                            Message = "Update profile staff" + ok,
                            Data = staff1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                           
                            Message = "Server error",
                            Data = "Internet server"
                        });
                    }
                }else
                {
                     return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "There already exists a staff with that information",
            });
                }
            } 
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = "Please fill in all information",
            });

        }
        [HttpPut]
        public async Task<ActionResult> UpdateAvatarStaffAsync([FromForm] StaffAvatarDTO staffAvatarDTO)
        {
            var checkOwner = await _repo.CheckStaffExistAsync(staffAvatarDTO.StaffId);
            if (checkOwner == true)
            {
                

                var owner1 = await _repo.UpdateAvatarStaffAsync(staffAvatarDTO);
                if (owner1 == true)
                {
                    return StatusCode(200, new
                    {

                       
                        Message = "Update avatar staff " + ok,
                        Data = owner1
                    });
                }
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = badRequest,
            });

        }
        [HttpPatch("{staffId}")]
        public async Task<ActionResult> BanStaffAsync(int staffId)
        {
            var staff1 = await _repo.BanStaffAsync(staffId);
            if (staff1 == true)
            {
                return StatusCode(200, new
                {

                   
                    Message = "Ban staff " + ok,

                });
            }
            return StatusCode(400, new
            {
                StatusCode = 400,
                
                Message = badRequest,
            });

        }

      

    }
}
