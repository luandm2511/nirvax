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
        public async  Task<ActionResult<IEnumerable<Staff>>> GetAllStaffs(string? searchQuery, int page, int pageSize)
        {
            var list =  await _repo.GetAllStaffs(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list staff " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
        }


      

        [HttpGet("{staffId}")]
        //  [Authorize]
        public async Task<ActionResult> GetStaffById(int staffId)
        {
            var checkStaff = await _repo.CheckStaffExist(staffId);
            if (checkStaff == true)
            {
                var staff = await _repo.GetStaffById(staffId);
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get staff by id " + ok,
                    Data = staff
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
        }

        [HttpGet("{staffEmail}")]
        //  [Authorize]
        public async Task<ActionResult> GetStaffByEmail(string staffEmail)
        {
            var checkStaff = await _repo.CheckProfileExist(staffEmail);
            if (checkStaff == true)
            {
                var staff = await _repo.GetStaffByEmail(staffEmail);
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get staff by email " + ok,
                    Data = staff
                });
            }

            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any staff"
            });
        }

        //check exist
        [HttpPost]
        public async Task<ActionResult> CreateStaff([FromForm] StaffDTO staffDTO)
        {
           
            if(ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckStaff(staffDTO);
                if (checkStaff == true)
                {
                  

                    var staff1 = await _repo.CreateStaff(staffDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create staff " + ok,
                            Data = staff1
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
                Message = "Dont't accept empty information!",
            });

        }



        [HttpPut]
        public async Task<ActionResult> ChangePasswordStaff(int staffId, string oldPassword, string newPasswod, string confirmPassword)
        {
            try
            {
                var checkStaff = await _repo.CheckStaffExist(staffId);
                if (checkStaff == true)
                {
                    var staff1 = await _repo.ChangePasswordStaff(staffId, oldPassword, newPasswod, confirmPassword);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Change password of staff" + ok,
                            Data = staff1
                        });
                    }
                    return StatusCode(500, new
                    {

                        Result = true,
                        Message = "Internet error"
                        // Data = staff1
                    });
                }
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    Result = false,
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
        public async Task<ActionResult> UpdateStaff([FromForm] StaffDTO staffDTO)
        {
            if (ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckStaff(staffDTO);
                if (checkStaff == true)
                {
                      
                    var staff1 = await _repo.UpdateStaff(staffDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update staff" + ok,
                            Data = staff1
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

        [HttpPut]
        public async Task<ActionResult> UpdateProfileStaff(StaffProfileDTO staffProfileDTO)
        {
            if (ModelState.IsValid)
            {
                var checkStaff = await _repo.CheckProfileStaff(staffProfileDTO);
                if (checkStaff == true)
                {
                    var staff1 = await _repo.UpdateProfileStaff(staffProfileDTO);
                    if (staff1 == true)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update profile staff" + ok,
                            Data = staff1
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {

                            Result = true,
                            Message = "Server error",
                            Data = "Internet server"
                        });
                    }
                }else
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
                Message = "Please fill in all information",
            });

        }
        [HttpPut]
        public async Task<ActionResult> UpdateAvatarStaff([FromForm] StaffAvatarDTO staffAvatarDTO)
        {
            var checkOwner = await _repo.CheckStaffExist(staffAvatarDTO.StaffId);
            if (checkOwner == true)
            {
                

                var owner1 = await _repo.UpdateAvatarStaff(staffAvatarDTO);
                if (owner1 == true)
                {
                    return StatusCode(200, new
                    {

                        Result = true,
                        Message = "Update avatar staff " + ok,
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
        [HttpPatch("{staffId}")]
        public async Task<ActionResult> BanStaff(int staffId)
        {
            var staff1 = await _repo.BanStaff(staffId);
            if (staff1 == true)
            {
                return StatusCode(200, new
                {

                    Result = true,
                    Message = "Ban staff " + ok,

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
