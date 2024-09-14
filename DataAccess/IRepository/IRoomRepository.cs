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
    public interface IRoomRepository
    {
        Task<IEnumerable<RoomDTO>> ViewUserHistoryChatAsync(int accountId);
        Task<IEnumerable<RoomDTO>> ViewOwnerHistoryChatAsync(int ownerId);
        Task<Room> CreateRoomAsync(RoomCreateDTO roomCreateDTO);
        Task<bool> CheckRoomAsync(int accountId, int ownerId);
        Task<bool> UpdateContentRoomAsync(int roomId);
        Task<RoomDTO> GetRoomByIdAsync(int roomId);
        Task<int> GetRoomIdByAccountIdAndOwnerIdAsync(int accountId, int ownerId);
        Task<RoomDTO> GetRoomByAccountIdAndOwnerIdAsync(int accountId, int ownerId);
    }
}
