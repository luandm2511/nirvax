﻿using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GuestConsultationController : ControllerBase
    {
        

            private readonly IConfiguration _config;
            private readonly IGuestConsultationRepository  _repo;
            private readonly INotificationRepository _notificationRepository;
            private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public GuestConsultationController(IConfiguration config, IGuestConsultationRepository repo, INotificationRepository notificationRepository)
            {
                _config = config;
                _repo = repo;
                _notificationRepository = notificationRepository;
            }

          

            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId)
            {
            try { 
                var list = await _repo.GetAllGuestConsultationsAsync(searchQuery, page, pageSize, ownerId);
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
                    Message = notFound + "any guest consultation"
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


        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            try
            {
                var list = await _repo.GetAllGuestConsultationsWaitingAsync(searchQuery, page, pageSize, ownerId);
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
                    Message = notFound + "any guest consultation"
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

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            try
            {
                var list = await _repo.GetAllGuestConsultationsAcceptAsync(searchQuery, page, pageSize, ownerId);
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
                    Message = notFound + "any guest consultation"
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

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<GuestConsultation>>> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            try
            {
                var list = await _repo.GetAllGuestConsultationsDenyAsync(searchQuery, page, pageSize, ownerId);
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
                    Message = notFound + "any guest consultation"
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




        [HttpGet("{guestId}")]
            //  [Authorize]
            public async Task<ActionResult> GetGuestConsultationsByIdAsync(int guestId)
            {
            try { 
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
                    Message = notFound + "any guest consultation"
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

           

            [HttpPost]
            public async Task<ActionResult> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO, int ownerId)
            {
            using var transaction = await _repo.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid)
                {
                    var checkGuestConsultation = await _repo.CheckGuestConsultationAsync(guestConsultationCreateDTO);
                    if (checkGuestConsultation == true)
                    {
                        var guestConsultation1 = await _repo.CreateGuestConsultationAsync(guestConsultationCreateDTO);
                        var notification = new Notification
                        {
                            AccountId = null,
                            OwnerId = ownerId, // Assuming Product model has OwnerId field
                            Content = $"You have just received a registration for a new consultation about products in the store",
                            IsRead = false,
                            Url = null,
                            CreateDate = DateTime.UtcNow
                        };

                        await _notificationRepository.AddNotificationAsync(notification);
                        await _repo.CommitTransactionAsync();

                        return StatusCode(200, new
                            {
                                Message = "Create guest consultation " + ok,
                                Data = guestConsultation1
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
                        Message = "Please enter valid Consultation!",
                    });
                }
            }
            catch (Exception ex)
                await _repo.RollbackTransactionAsync();

            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }


            [HttpPut]
            public async Task<ActionResult> UpdateGuestConsultationAsync(GuestConsultationDTO guestConsultationDTO)
            {
            try {
                if (ModelState.IsValid)
                {
                    var checkGuestConsultation = await _repo.CheckGuestConsultationExistAsync(guestConsultationDTO.GuestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateGuestConsultationAsync(guestConsultationDTO);
                    return StatusCode(200, new
                    {
                        Message = "Update guest consultation" + ok,
                        Data = guestConsultation1
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "The name guest consultation is already exist",
                    });
                }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Consultation!",
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
            public async Task<ActionResult> UpdateStatusGuestConsultationtByIdAsync(int guestId, int statusGuestId)
            {
            try { 
                var checkGuestConsultation = await _repo.CheckGuestConsultationExistAsync(guestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateStatusGuestConsultationtByIdAsync(guestId, statusGuestId);
                        return StatusCode(200, new
                        {
                            Message = "Update guest consultation" + ok,
                            Data = guestConsultation1
                        });
                }
                return StatusCode(400, new
                {
                    Message = "The name guest consultation is already exist",
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

        [HttpPut]
        public async Task<ActionResult> UpdateStatusGuestConsultationtAsync(int guestId, string statusGuest)
        {
            try
            {
                var checkGuestConsultation = await _repo.CheckGuestConsultationExistAsync(guestId);
                if (checkGuestConsultation == true)
                {
                    var guestConsultation1 = await _repo.UpdateStatusGuestConsultationtAsync(guestId, statusGuest);
                    return StatusCode(200, new
                    {
                        Message = "Update guest consultation" + ok,
                        Data = guestConsultation1
                    });
                }
                return StatusCode(400, new
                {
                    Message = "The name guest consultation is already exist",
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
            else
            {
                return StatusCode(200, new
                {
                    Message = 0,
                });
            }

        } 
    }
    }
