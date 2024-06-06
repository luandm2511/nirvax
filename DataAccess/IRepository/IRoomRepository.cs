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
        Task<List<RoomDTO>> ViewUserHistoryChat(int accountId);
        Task<List<RoomDTO>> ViewOwnerHistoryChat(int ownerId);
        Task<bool> CreateRoom(RoomDTO roomDTO);
        Task<bool> CheckRoom(RoomDTO roomDTO);
        Task<bool> UpdateContentRoom(int roomId);
        Task<RoomDTO> GetRoomById(int roomId);
        Task<int> GetRoomIdByAccountIdAndOwnerId(int accountId, int ownerId);
        Task<RoomDTO> GetRoomByAccountIdAndOwnerId(int accountId, int ownerId);

    }
}
