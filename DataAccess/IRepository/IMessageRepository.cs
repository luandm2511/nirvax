using AutoMapper.Execution;
using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IMessageRepository
    {
        Task<List<MessageDTO>> ViewAllMessageByRoom(int roomId);
        Task<bool> CheckMessage(MessageDTO messageDTO);
            Task<bool> CreateMessage(MessageDTO messageDTO);
        Task<bool> CreateMessageFromOwner(Message message);


    }
}
