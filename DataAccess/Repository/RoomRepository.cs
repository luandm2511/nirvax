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
    public class RoomRepository : IRoomRepository
    {
       
        private readonly RoomDAO _roomDAO;
        public RoomRepository(RoomDAO roomDAO)
        {
            _roomDAO = roomDAO;
        }



        public Task<List<RoomDTO>> ViewUserHistoryChatAsync(int accountId)
        {
            
            return _roomDAO.ViewUserHistoryChatAsync(accountId); 
        }
        
        public Task<List<RoomDTO>> ViewOwnerHistoryChatAsync(int ownerId)
        {

            return _roomDAO.ViewOwnerHistoryChatAsync(ownerId);
        }

        public Task<bool> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
        {
            return _roomDAO.CreateRoomAsync(roomCreateDTO);
        }
        public Task<bool> CheckRoomAsync(int accountId, int ownerId)
        {
            return _roomDAO.CheckRoomAsync(accountId, ownerId);
        }

        public Task<bool> UpdateContentRoomAsync(int roomId)
        {
            return _roomDAO.UpdateContentRoomAsync(roomId);
        }



        public Task<RoomDTO> GetRoomByIdAsync(int roomId)
        {
            return _roomDAO.GetRoomByIdAsync(roomId);
        }

        public Task<int> GetRoomIdByAccountIdAndOwnerIdAsync(int accountId, int ownerId)
        {
            return _roomDAO.GetRoomIdByAccountIdAndOwnerIdAsync(accountId, ownerId);
        }
        public Task<RoomDTO> GetRoomByAccountIdAndOwnerIdAsync(int accountId, int ownerId)
        {
            return _roomDAO.GetRoomByAccountIdAndOwnerIdAsync(accountId, ownerId);
        }

    }
}
