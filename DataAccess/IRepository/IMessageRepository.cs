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
        Task<List<MessageDTO>> ViewAllMessageByRoomAsync(int roomId);
        Task<bool> CheckMessageAsync(MessageCreateDTO messageCreateDTO);
        Task<bool> CreateMessageAsync(MessageCreateDTO messageCreateDTO);
        Task<bool> CreateMessageFirstAsync(MessageCreateDTO messageCreateDTO);
        Task<bool> CreateMessageFromOwnerAsync(Message message);
    }
}
