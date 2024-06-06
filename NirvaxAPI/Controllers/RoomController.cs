using AutoMapper;
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
        private readonly IConfiguration _config;
        private readonly IRoomRepository  _repo;
        private readonly IMessageRepository _mess;
        private readonly IMapper _mapper;
    




        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public RoomController(IConfiguration config, IRoomRepository repo, IMessageRepository mess, IMapper mapper)
        {
            _config = config;
             _repo = repo;
            _mess = mess;
            _mapper = mapper;
        }

     
        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Room>>> ViewUserHistoryChat(int accountId)
        {
            var list = await _repo.ViewUserHistoryChat(accountId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list room of this account " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any room"
            });
        }

        [HttpGet]
        //  [Authorize]
        public async Task<ActionResult<IEnumerable<Room>>> ViewOwnerHistoryChat(int ownerId)
        {
            var list = await _repo.ViewOwnerHistoryChat(ownerId);
            if (list.Any())
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get list room of this owner " + ok,
                    Data = list
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any room"
            });
        }


        [HttpGet()]
        //  [Authorize]
        public async Task<ActionResult> GetRoomIdByAccountIdAndOwnerId(int accountId, int ownerId)
        {

            var room = await _repo.GetRoomIdByAccountIdAndOwnerId(accountId, ownerId);
            if (room != null)
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get room" + ok,
                    Data = room
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any room"
            });
        }

        [HttpGet()]
        //  [Authorize]
        public async Task<ActionResult> GetRoomByAccountIdAndOwnerId(int accountId, int ownerId)
        {
            
                var room = await _repo.GetRoomByAccountIdAndOwnerId(accountId, ownerId);
            if (room != null)
            {
                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get room" + ok,
                    Data = room
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any room"
            });
        }

        [HttpGet("{roomId}")]
        //  [Authorize]
        public async Task<ActionResult> GetRoomById(int roomId)
        {
           
                var size = await _repo.GetRoomById(roomId);
            if (size != null)
            {


                return StatusCode(200, new
                {
                    Result = true,
                    Message = "Get room by id" + ok,
                    Data = size
                });
            }
            return StatusCode(500, new
            {
                Status = "Find fail",
                Message = notFound + "any room"
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(RoomDTO roomDTO)
        {
            try
            {


                var checkSize = await _repo.CheckRoom(roomDTO);
                if (checkSize == true)
                {
                    var size1 = await _repo.CreateRoom(roomDTO);
                    if (size1)
                    {
                        //Message message = new Message();
                        //  message.SenderId = roomDTO.OwnerId;
                        //message.ReceiverId = roomDTO.AccountId;
                        //message.RoomId = roomDTO.RoomId;
                        //message.Content = "Hello! Shop của tôi có thể giúp gì được cho bạn?";
                        //message.Timestamp = DateTime.Now;
                        //await _mess.CreateMessageFromOwner(message);

                        return StatusCode(200, new
                        {

                            Result = true,
                            Message = "Create new room " + ok,
                            Data = size1
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
                    Message = "Owner or User is is not exist!",
                });
            }catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred: " + ex.Message
                });
            }

        }

  //      [HttpPatch("{roomId}")]
     //   public async Task<ActionResult> UpdateContentRoom(int roomId)
        //{
          //  var room = await _repo.UpdateContentRoom(roomId);
           // if (room)
            //{
              //  return StatusCode(200, new
              //  {

                //    Result = true,
                  //  Message = "Update content room " + ok,

                //});
            //}
            //return StatusCode(400, new
            //{
              //  StatusCode = 400,
                //Result = false,
                //Message = badRequest,
            //});

        //}
    }
}
