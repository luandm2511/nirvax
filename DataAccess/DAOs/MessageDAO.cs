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

namespace DataAccess.DAOs
{
    public  class MessageDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public  MessageDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckMessageAsync(MessageCreateDTO messageCreateDTO) 
        {

            if (messageCreateDTO.Content.Trim() != null)
            {
                    return true;           
            }
          
                return false;
        }

       

        //owner,staff
        public async Task<List<MessageDTO>> ViewAllMessageByRoomAsync(int roomId)
        {
            List<MessageDTO> listSizeDTO = new List<MessageDTO>();
                List<Message> getList = await _context.Messages
                 .Include(i => i.Room)
                    .Where(i => i.RoomId == roomId)
                    .OrderBy(i=> i.Timestamp)
                    .ToListAsync();
                listSizeDTO = _mapper.Map<List<MessageDTO>>(getList);
            
            return listSizeDTO;
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





