using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
        [ApiController]
    public class MessageController : ControllerBase
    {
        
       
        private readonly IConfiguration _config;
        private readonly IMessageRepository  _repo;
        private readonly IRoomRepository _room;
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public MessageController(IConfiguration config, IMessageRepository repo, IRoomRepository room, IHubContext<ChatHub> hubContext)
        {
            _config = config;
            _repo = repo;
            _room = room;
            _hubContext = hubContext;
        }


        [HttpGet]
            //  [Authorize]
            public async Task<ActionResult<IEnumerable<Message>>> ViewAllMessageByRoomAsync(int roomId)
            {

                var list = await _repo.ViewAllMessageByRoomAsync(roomId);
                if (list.Any())
                {
                    return StatusCode(200, new
                    {
                        Message = "Get all message of this room " + ok,
                        Data = list
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Message = notFound + "all message of this room"
                    });
                }
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessageAsync(int ownerId, int accountId, int roomId,MessageCreateDTO messageCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (messageCreateDTO.RoomId == 0)
                    {
                        RoomCreateDTO room = new RoomCreateDTO
                        {
                            OwnerId = ownerId,
                            AccountId = accountId,
                            Content = "",
                            Timestamp = DateTime.Now
                        };

                        var roomResult = await _room.CreateRoomAsync(room);
                        roomId = roomResult.RoomId;
                    }
                    var checkMessage = await _repo.CheckMessageAsync(messageCreateDTO);
                    if (checkMessage)
                    {
                        var messageCreated = await _repo.CreateMessageAsync(roomId,messageCreateDTO);
                        if (messageCreated)
                        {
                            await _hubContext.Clients.Group(messageCreateDTO.RoomId.ToString()).SendAsync("ReceiveMessage", messageCreateDTO.SenderId.ToString(), messageCreateDTO.Content);

                            // Cập nhật nội dung của Room
                            await _room.UpdateContentRoomAsync(messageCreateDTO.RoomId);

                            return StatusCode(200, new
                            {
                                Message = "Create message " + ok,
                                Data = messageCreated
                            });
                        }
                        else
                        {
                            return StatusCode(400, new
                            {
                                Message = "Failed to send message!"
                            });
                        }
                    }
                    else
                    {
                        return StatusCode(400, new
                        {
                            Message = "Please enter the message content!"
                        });
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        Message = "Please enter valid Message!"
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
