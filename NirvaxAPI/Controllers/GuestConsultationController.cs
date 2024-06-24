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
            public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize)
            {
                var list = await _repo.GetAllGuestConsultationsAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        
                        Message = "Get list guest consultation " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Status = "Find fail",
                    Message = notFound + "any guest consultation"
                });
            }

    


            [HttpGet("{guestId}")]
            //  [Authorize]
            public async Task<ActionResult> GetGuestConsultationsByIdAsync(int guestId)
            {
                var checkSizeExist = await _repo.CheckGuestConsultationExistAsync(guestId);
                if (checkSizeExist == true)
                {
                    var guestConsultation = await _repo.GetGuestConsultationsByIdAsync(guestId);


                    return StatusCode(200, new
                    {
                        
                        Message = "Get guest consultation by id" + ok,
                        Data = guestConsultation
                    });


                }

                return StatusCode(404, new
                {
                    Status = "Find fail",
                    Message = notFound + "any guest consultation"
                });
            }

           

            [HttpPost]
            public async Task<ActionResult> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
            {
            if (ModelState.IsValid)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationAsync(guestConsultationCreateDTO);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.CreateGuestConsultationAsync(guestConsultationCreateDTO);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            
                            Message = "Create guest consultation " + ok,
                            Data = guestConsultation1
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
            public async Task<ActionResult> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationExistAsync(guestConsultationDTO.GuestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateGuestConsultationAsync(guestConsultationDTO);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            
                            Message = "Update guest consultation" + ok,
                            Data = guestConsultation1
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
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    
                    Message = "The name guest consultation is already exist",
                });

            }

            [HttpPut]
            public async Task<ActionResult> UpdateStatusGuestConsultationtAsync(int guestId, int statusGuestId)
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationExistAsync(guestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateStatusGuestConsultationtAsync(guestId, statusGuestId);
                    if (guestConsultation1)
                    {
                        return StatusCode(200, new
                        {

                            
                            Message = "Update guest consultation" + ok,
                            Data = guestConsultation1
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
                return StatusCode(400, new
                {
                    StatusCode = 400,
                    
                    Message = "The name guest consultation is already exist",
                });

            }
        [HttpGet]
        public async Task<ActionResult> ViewGuestConsultationStatisticsAsync()
        {
            var number = await _repo.ViewGuestConsultationStatisticsAsync();
            if (number != null)
            {
                return StatusCode(200, new
                {

                    
                    Message = number,

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
