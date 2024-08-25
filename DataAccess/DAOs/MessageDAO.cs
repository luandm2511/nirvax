using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.SignalR;

namespace DataAccess.DAOs
{
    public  class MessageDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;
       // private readonly IHubContext<ChatHub> _hubContext;




        public  MessageDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckMessageAsync(MessageCreateDTO messageCreateDTO) 
        {
            return !string.IsNullOrWhiteSpace(messageCreateDTO.Content);
        }

       

        //owner,staff
        public async Task<List<MessageDTO>> ViewAllMessageByRoomAsync(int roomId)
        {
            List<MessageDTO> list = new List<MessageDTO>();
                List<Message> getList = await _context.Messages
                 .Include(i => i.Room)
                    .Where(i => i.RoomId == roomId)
                    .OrderBy(i=> i.Timestamp)
                    .ToListAsync();
                list = _mapper.Map<List<MessageDTO>>(getList);
            
            return list;
        }

        public async Task<bool> CreateMessageAsync(MessageCreateDTO messageCreateDTO)
        {
           
            messageCreateDTO.Timestamp = DateTime.Now;
            Message message = _mapper.Map<Message>(messageCreateDTO);
          //  message.Room.Content = message.Content;
            await _context.Messages.AddAsync(message);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                //Room room = await _context.Rooms.FindAsync(messageCreateDTO.RoomId);
              //  if (room != null)
              //  {
                //    room.Content = message.Content; // Update room content to latest message
                //    _context.Rooms.Update(room);
                  //  await _context.SaveChangesAsync();
              //  }

                // Notify clients in the room about new message
           //    await chatHub.Clients.Group(messageCreateDTO.RoomId.ToString()).SendAsync("ReceiveMessage", message.SenderId, message.Content);

                return true;
            }
            else { return false; }

        }

        public async Task<bool> CreateMessageFirstAsync(MessageCreateDTO messageCreateDTO)
        {
            messageCreateDTO.Timestamp = DateTime.Now;
            Message message = _mapper.Map<Message>(messageCreateDTO);
            //  message.Room.Content = message.Content;
            await _context.Messages.AddAsync(message);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                //Room room = await _context.Rooms.FindAsync(messageCreateDTO.RoomId);
                //  if (room != null)
                //  {
                //    room.Content = message.Content; // Update room content to latest message
                //    _context.Rooms.Update(room);
                //  await _context.SaveChangesAsync();
                //  }

                // Notify clients in the room about new message
                //    await chatHub.Clients.Group(messageCreateDTO.RoomId.ToString()).SendAsync("ReceiveMessage", message.SenderId, message.Content);

                return true;
            }
            else { return false; }

        }

        public async Task<bool> CreateMessageFromOwnerAsync(Message message)
        {
           
            await _context.Messages.AddAsync(message);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }


    }
}





