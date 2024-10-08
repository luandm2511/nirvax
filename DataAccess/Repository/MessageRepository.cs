﻿using AutoMapper.Execution;
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



        public Task<IEnumerable<MessageDTO>> ViewAllMessageByRoomAsync(int roomId)
        {
            return _messageDAO.ViewAllMessageByRoomAsync(roomId);
        }

        public Task<bool> CheckMessageAsync(MessageCreateDTO messageCreateDTO)
        {
            return _messageDAO.CheckMessageAsync(messageCreateDTO);
        }
        public Task<bool> CreateMessageAsync(MessageCreateDTO messageCreateDTO)
        {
            return _messageDAO.CreateMessageAsync(messageCreateDTO);
        }

        public Task<bool> CreateMessageFirstAsync(MessageCreateDTO messageCreateDTO)
        {
            return _messageDAO.CreateMessageFirstAsync(messageCreateDTO);
        }

    

    }
}
