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

        public async Task<bool> CheckMessage(MessageDTO messageDTO) 
        {

            if (messageDTO.Content.Trim() != null)
            {
                    return true;           
            }
          
                return false;
        }

       

        //owner,staff
        public async Task<List<MessageDTO>> ViewAllMessageByRoom(int roomId)
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

        public async Task<bool> CreateMessage(MessageDTO messageDTO)
        {
           messageDTO.Timestamp = DateTime.Now;

            Message message = _mapper.Map<Message>(messageDTO);
          //  message.Room.Content = message.Content;
            await _context.Messages.AddAsync(message);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        public async Task<bool> CreateMessageFromOwner(Message message)
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





