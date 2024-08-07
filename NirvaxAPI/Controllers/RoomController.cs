﻿using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
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
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Room>>> ViewUserHistoryChatAsync(int accountId)
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
            return StatusCode(404, new
            {                
                Message = notFound + "any room"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Room>>> ViewOwnerHistoryChatAsync(int ownerId)
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
            return StatusCode(404, new
            {               
                Message = notFound + "any room"
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpGet("{roomId}")]
        //  [Authorize]
        public async Task<ActionResult> GetRoomByIdAsync(int roomId)
        {
            try { 
                var size = await _repo.GetRoomByIdAsync(roomId);
                if (size != null)
                {
                    return StatusCode(200, new
                    {
                        Message = "Get room by id" + ok,
                        Data = size
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }
        }

        [HttpPost]
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

       
    }
}
