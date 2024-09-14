using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository  _repo;
        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public RoomController(IRoomRepository repo)
        { 
             _repo = repo;           
        }

     
        [HttpGet]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> ViewUserHistoryChatAsync(int accountId)
        {
            var list = await _repo.ViewUserHistoryChatAsync(accountId);
            if (list.Any())
            {
                return StatusCode(200, new
                {                   
                    Message = "Get list room of this account " + ok,
                    Data = list
                });
            }
            else
            {
                return NoContent();

            }
        }

        [HttpGet]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> ViewOwnerHistoryChatAsync(int ownerId)
        {
            try { 
            var list = await _repo.ViewOwnerHistoryChatAsync(ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {               
                    Message = "Get list room of this owner " + ok,
                    Data = list
                });
            }
                else
                {     
                    return NoContent();
                
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


        [HttpGet]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<ActionResult> GetRoomIdByAccountIdAndOwnerIdAsync(int accountId, int ownerId)
        {
            try { 
            var room = await _repo.GetRoomIdByAccountIdAndOwnerIdAsync(accountId, ownerId);
                if (room != null)
                {
                    return StatusCode(200, new
                    {
                        Message = "Get room" + ok,
                        roomId = room
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any room"
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

        [HttpGet]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<ActionResult> GetRoomByAccountIdAndOwnerIdAsync(int accountId, int ownerId)
        {
            try { 
            var room = await _repo.GetRoomByAccountIdAndOwnerIdAsync(accountId, ownerId);
                if (room != null)
                {
                    return StatusCode(200, new
                    {
                        Message = "Get room" + ok,
                        Data = room
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any room"
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

        [HttpGet("{roomId}")]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<ActionResult> GetRoomByIdAsync(int roomId)
        {
            try { 
                var room = await _repo.GetRoomByIdAsync(roomId);
                if (room != null)
                {
                    return StatusCode(200, new
                    {
                        Message = "Get room by id" + ok,
                        Data = room
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "any room"
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

        [HttpPost]
        [Authorize(Roles = "User,Owner,Staff")]
        public async Task<ActionResult> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkRoom = await _repo.CheckRoomAsync(roomCreateDTO.AccountId, roomCreateDTO.OwnerId);
                    if (checkRoom == true)
                    {
                        var room1 = await _repo.CreateRoomAsync(roomCreateDTO);
                        return StatusCode(200, new
                        {
                            Message = "Create room " + ok,
                            Data = room1
                        });
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "Owner or User is is not exist!",
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Room!",
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

       
    }
}
