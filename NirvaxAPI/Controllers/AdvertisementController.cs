using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public  class AdvertisementController : ControllerBase
    {   
            private readonly ITransactionRepository _transactionRepository;
            private readonly IAdvertisementRepository _repo;
            private readonly INotificationRepository _notificationRepository;

   
            private readonly string ok = "successfully";
            private readonly string notFound = "Not found";
            private readonly string badRequest = "Failed!";

            public  AdvertisementController(IAdvertisementRepository repo, INotificationRepository notificationRepository, ITransactionRepository transactionRepository)
            {
                _repo = repo;
               _notificationRepository = notificationRepository;
               _transactionRepository = transactionRepository;
            }


            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsAsync(string? searchQuery, int page, int pageSize)
            {
                var list =await _repo.GetAllAdvertisementsAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    { 
                        Message = "Get list of advertisements " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
        }

            [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsWaitingAsync(string? searchQuery, int page, int pageSize)
            {
            try { 
                var list =await _repo.GetAllAdvertisementsWaitingAsync(searchQuery, page, pageSize);
                if (list.Any())
                {
                    return StatusCode(200, new
                    { 
                        Message = "Get list advertisement " + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsAcceptAsync(string? searchQuery, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllAdvertisementsAcceptAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsDenyAsync(string? searchQuery, int page, int pageSize)
        {
            try { 
            var list = await _repo.GetAllAdvertisementsDenyAsync(searchQuery, page, pageSize);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement " + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
            }
            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsForUserAsync(string? searchQuery)
        {
            var list =await _repo.GetAllAdvertisementsForUserAsync(searchQuery);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement for user" + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisementsByOwnerForUserAsync(string? searchQuery, int ownerId)
        {
                var list = await _repo.GetAdvertisementsByOwnerForUserAsync(searchQuery, ownerId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list advertisement by owner" + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisementsByOwnerAsync(string? searchQuery, int page, int pageSize, int ownerId)
        { 
            var list = await _repo.GetAdvertisementsByOwnerAsync(searchQuery, page, pageSize, ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Message = "Get list advertisement by owner" + ok,
                    Data = list
                });
            }
            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAllAdvertisementsByServiceAsync(int serviceId)
        {

                var list = await _repo.GetAllAdvertisementsByServiceAsync(serviceId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get list advertisement by service" + ok,
                        Data = list
                    });
                }
                return StatusCode(404, new
                {
                    Message = notFound + "any advertisement"
                });
           
        }


        [HttpGet("{adId}")]
            //  [Authorize]
            public async Task<ActionResult> GetAdvertisementByIdAsync(int adId)
            {
                var checkAdExist =await _repo.CheckAdvertisementExistAsync(adId);
                if (checkAdExist == true)
                {
                    var advertisement =await _repo.GetAdvertisementByIdAsync(adId);
                    return StatusCode(200, new
                    {
                        Message = "Get advertisement by id" + ok,
                        Data = advertisement
                    });
                }
                return StatusCode(404, new
                { 
                    Message = notFound + "any advertisement"
                });          
            }

        [HttpGet("{adId}")]
        //  [Authorize]
        public async Task<ActionResult> GetAdvertisementByIdForUserAsync(int adId)
        {
                var advertisement =await _repo.GetAdvertisementByIdForUserAsync(adId);
            if (advertisement != null)
            {

                return StatusCode(200, new
                {
                    Message = "Get advertisement for user" + ok,
                    Data = advertisement
                });
            }

            return StatusCode(404, new
            {
                Message = notFound + "any advertisement"
            });
        }

        [HttpPost]
            public async Task<ActionResult> CreateAdvertisementAsync([FromForm] AdvertisementCreateDTO advertisementCreateDTO)
            {
            try {
                if (ModelState.IsValid)
                {
                    var checkAdvertisement = await _repo.CheckAdvertisementCreateAsync(advertisementCreateDTO);
                    if (checkAdvertisement == true)
                    {

                        var advertisement1 = await _repo.CreateAdvertisementAsync(advertisementCreateDTO);
                            return StatusCode(200, new
                            {
                                Message = "Create advertisement " + ok,
                                Data = advertisement1
                            });                    
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "There already exists a Advertisement with that information",
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

            catch (Exception )
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }
        }
     

            [HttpPut]
            public async Task<ActionResult> UpdateAdvertisementAsync([FromForm] AdvertisementDTO advertisementDTO)
            {
            try { 
            if (ModelState.IsValid)
            {
                var checkAdvertisement =await _repo.CheckAdvertisementAsync(advertisementDTO);
                if (checkAdvertisement == true)
                {
                    
                    var advertisement1 =await _repo.UpdateAdvertisementAsync(advertisementDTO);          
                        return StatusCode(200, new
                        {
                            Message = "Update advertisement" + ok,
                            Data = advertisement1
                        });               
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "There already exists a Advertisement with that information",
                    });
                }

            }

            return StatusCode(400, new
            {
                Message = "Dont't accept empty information!",
            });
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
        public async Task<ActionResult> UpdateStatusAdvertisementAsync(int adId, string statusPost)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try { 
            var checkAdvertisement = await _repo.CheckAdvertisementExistAsync(adId);
            if (checkAdvertisement == true)
            {                
                    var advertisement = await _repo.UpdateStatusAdvertisementAsync(adId, statusPost);
                    
                    var notification = new Notification
                    {
                        AccountId = null,
                        OwnerId = advertisement.OwnerId,
                        Content = $"Your new Advertisement has been moderated and its status changed to {advertisement.StatusPost.Name}",
                        IsRead = false,
                        Url = null,
                        CreateDate = DateTime.Now
                    };
                    await _notificationRepository.AddNotificationAsync(notification);

                    await _transactionRepository.CommitTransactionAsync();
                    return StatusCode(200, new
                    {
                        Message = "Update advertisement" + ok,
                        Data = advertisement
                    });            
            }
            return StatusCode(400, new
            {
                
                Message = "There already exists a advertisement with that information!",
            });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + "Something went wrong, please try again."
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> AdversisementStatisticsAsync(int ownerId)
        {
            var total2 = await _repo.ViewOwnerAdversisementStatisticsAsync(ownerId);
            var total = await _repo.ViewAdversisementStatisticsAsync();

            return StatusCode(200, new
                {
                totalAdversisement = total,
                totalOwnerAdversisement = total2,
                });
        }
    }
}

