using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GuestConsultationController : ControllerBase
    {
        

            private readonly IConfiguration _config;
            private readonly IGuestConsultationRepository  _repo;
            private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public GuestConsultationController(IConfiguration config, IGuestConsultationRepository repo)
            {
                _config = config;
                 _repo = repo;
            }

          

            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultations(string? searchQuery, int page, int pageSize)
            {
                var list = await _repo.GetAllGuestConsultations(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get list guest consultation " + ok,
                        Data = list
                    });
                }
                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any guest consultation"
                });
            }

    


            [HttpGet("{guestId}")]
            //  [Authorize]
            public async Task<ActionResult> GetGuestConsultationsById(int guestId)
            {
                var checkSizeExist = await _repo.CheckGuestConsultationExist(guestId);
                if (checkSizeExist == true)
                {
                    var guestConsultation = await _repo.GetGuestConsultationsById(guestId);


                    return StatusCode(200, new
                    {
                        Result = true,
                        Message = "Get guest consultation by id" + ok,
                        Data = guestConsultation
                    });


                }

                return StatusCode(500, new
                {
                    Status = "Find fail",
                    Message = notFound + "any guest consultation"
                });
            }

           

            [HttpPost]
            public async Task<ActionResult> CreateGuestConsultation(GuestConsultationDTO guestConsultationDTO)
            {
            if (ModelState.IsValid)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultation(guestConsultationDTO);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.CreateGuestConsultation(guestConsultationDTO);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create guest consultation " + ok,
                            Data = guestConsultation1
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
            public async Task<ActionResult> UpdateGuestConsultation(GuestConsultationDTO guestConsultationDTO)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationExist(guestConsultationDTO.GuestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateGuestConsultation(guestConsultationDTO);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update guest consultation" + ok,
                            Data = guestConsultation1
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
                    Message = "The name guest consultation is already exist",
                });

            }

            [HttpPut]
            public async Task<ActionResult> UpdateStatusGuestConsultationt(int guestId, int statusGuestId)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationExist(guestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateStatusGuestConsultationt(guestId, statusGuestId);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Update guest consultation" + ok,
                            Data = guestConsultation1
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
                    Message = "The name guest consultation is already exist",
                });

            }
        [HttpGet]
        public async Task<ActionResult> ViewGuestConsultationStatistics()
        {
            var number = await _repo.ViewGuestConsultationStatistics();
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
