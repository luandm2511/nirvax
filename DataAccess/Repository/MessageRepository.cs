using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Repository.StaffRepository;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class MessageRepository : IMessageRepository
    {
       
        private readonly MessageDAO _messageDAO;
        public MessageRepository(MessageDAO messageDAO)
        {
            _messageDAO = messageDAO;
        }



        public Task<List<MessageDTO>> ViewAllMessageByRoom(int roomId)
        {
            return _messageDAO.ViewAllMessageByRoom(roomId);
        }

        public Task<bool> CheckMessage(MessageDTO messageDTO)
        {
            return _messageDAO.CheckMessage(messageDTO);
        }
        public Task<bool> CreateMessage(MessageDTO messageDTO)
        {
            return _messageDAO.CreateMessage(messageDTO);
        }

        public Task<bool> CreateMessageFromOwner(Message message)
        {
            return _messageDAO.CreateMessageFromOwner(message);
        }

    }
}
