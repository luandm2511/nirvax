using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Diagnostics.Eventing.Reader;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GuestConsultationController : ControllerBase
    {
        private readonly IGuestConsultationRepository _repo;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";


        public GuestConsultationController(IGuestConsultationRepository repo, INotificationRepository notificationRepository, ITransactionRepository transactionRepository)
        {
            _repo = repo;
            _notificationRepository = notificationRepository;
            _transactionRepository = transactionRepository;
        }



        [HttpGet]
        //  [Authorize]
        public async Task<IActionResult> GetAllGuestConsultationsAsync(string? searchQuery, int page, int pageSize, int ownerId)
        {
            var list = await _repo.GetAllGuestConsultationsAsync(searchQuery, page, pageSize, ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {

                    Message = "Get list of guest consultations" + ok,
                    Data = list
                });
            }
            else
            {
                return StatusCode(204, new
                {
                    Message = "Empty!"
                });
            }
        }


        [HttpGet]
        //  [Authorize]
        public async Task<IActionResult> GetAllGuestConsultationsWaitingAsync(string? searchQuery, int page, int pageSize, int ownerId)
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
                else
                {
                    return StatusCode(204, new
                    {
                        Message = "Empty!"
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

        [HttpGet]
        //  [Authorize]
        public async Task<IActionResult> GetAllGuestConsultationsAcceptAsync(string? searchQuery, int page, int pageSize, int ownerId)
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
                else
                {
                    return StatusCode(204, new
                    {
                        Message = "Empty!"
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

        [HttpGet]
        //  [Authorize]
        public async Task<IActionResult> GetAllGuestConsultationsDenyAsync(string? searchQuery, int page, int pageSize, int ownerId)
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
                else
                {
                    return StatusCode(204, new
                    {
                        Message = "Empty!"
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




        [HttpGet("{guestId}")]
        //  [Authorize]
        public async Task<ActionResult> GetGuestConsultationsByIdAsync(int guestId)
        {

            var guestConsultation = await _repo.GetGuestConsultationsByIdAsync(guestId);
            if(guestConsultation != null) { 

            return StatusCode(200, new
            {

                Message = "Get guest consultation by id" + ok,
                Data = guestConsultation
            });
        }
       else {
                return StatusCode(404, new
                {      
                    Message = notFound + "any guest consultation"
                });    
    }
        }

           

            [HttpPost]
            public async Task<ActionResult> CreateGuestConsultationAsync(GuestConsultationCreateDTO guestConsultationCreateDTO)
            {
            using var transaction = await _transactionRepository.BeginTransactionAsync();

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
                            OwnerId = guestConsultation1.OwnerId, // Assuming Product model has OwnerId field
                            Content = $"You have just received a registration for a new consultation about products in the store",
                            IsRead = false,
                            Url = null,
                            CreateDate = DateTime.Now
                        };

                        await _notificationRepository.AddNotificationAsync(notification);
                        await _transactionRepository.CommitTransactionAsync();

                        return StatusCode(200, new
                            {
                                Message = "Send guest consultation " + ok,
                                Data = guestConsultation1
                            });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists guest consultation with that information by ads!",
                        });
                    }

                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Guest Consultation!",
                    });
                }
            }
            catch (Exception ex)


            {
                await _transactionRepository.RollbackTransactionAsync();
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
               
                    var guestConsultation1 = await _repo.UpdateStatusGuestConsultationtAsync(guestId, statusGuest);
                    return StatusCode(200, new
                    {
                        Message = "Update status guest consultation" + ok,
                        Data = guestConsultation1
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
        public async Task<ActionResult> ViewGuestConsultationStatisticsAsync(int ownerId)
        {
            
            try
            {
                var total = await _repo.ViewGuestConsultationStatisticsAsync(ownerId);
                return StatusCode(200, new
                {
                    Data = total
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        } 
    }
    }
