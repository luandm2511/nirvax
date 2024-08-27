using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pipelines.Sockets.Unofficial.Buffers;
using StackExchange.Redis;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
        [ApiController]
    public class MessageController : ControllerBase
    {
        
       
     
        private readonly IMessageRepository  _repo;
        private readonly IRoomRepository _room;
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly string ok = "successfully";
        private readonly string notFound = "Not found";
        private readonly string badRequest = "Failed!";

        public MessageController(IMessageRepository repo, IRoomRepository room, IHubContext<ChatHub> hubContext)
        {
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
                return NoContent();

            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessageAsync(MessageCreateDTO messageCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkMessage = await _repo.CheckMessageAsync(messageCreateDTO);
                    if (checkMessage)
                    {
                        var messageCreated = await _repo.CreateMessageAsync(messageCreateDTO);
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

        [HttpPost]
        public async Task<ActionResult> CreateMessageFirstAsync(int accountId, int ownerId)
        {
            try
            {
                RoomCreateDTO roomCreateDTO = new RoomCreateDTO
                {
                    AccountId = accountId,
                    OwnerId = ownerId,
                    Content = "",
                    Timestamp = DateTime.Now
                };
                var roomResult = await _room.CreateRoomAsync(roomCreateDTO);

                MessageCreateDTO messageCreateDTO = new MessageCreateDTO
                {
                    RoomId = roomResult.RoomId,
                    SenderId = ownerId,
                    Content = "Chào bạn, Tôi giúp gì được cho bạn?",
                    SenderType = "Owner",
                    Timestamp = DateTime.Now,
                };
                var messageCreated = await _repo.CreateMessageFirstAsync(messageCreateDTO);
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
