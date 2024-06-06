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



        public Task<List<RoomDTO>> ViewUserHistoryChat(int accountId)
        {
            
            return _roomDAO.ViewUserHistoryChat(accountId); 
        }
        
        public Task<List<RoomDTO>> ViewOwnerHistoryChat(int ownerId)
        {

            return _roomDAO.ViewOwnerHistoryChat(ownerId);
        }

        public Task<bool> CreateRoom(RoomDTO roomDTO)
        {
            return _roomDAO.CreateRoom(roomDTO);
        }
        public Task<bool> CheckRoom(RoomDTO roomDTO)
        {
            return _roomDAO.CheckRoom(roomDTO);
        }

        public Task<bool> UpdateContentRoom(int roomId)
        {
            return _roomDAO.UpdateContentRoom(roomId);
        }



        public Task<RoomDTO> GetRoomById(int roomId)
        {
            return _roomDAO.GetRoomById(roomId);
        }

        public Task<int> GetRoomIdByAccountIdAndOwnerId(int accountId, int ownerId)
        {
            return _roomDAO.GetRoomIdByAccountIdAndOwnerId(accountId, ownerId);
        }
        public Task<RoomDTO> GetRoomByAccountIdAndOwnerId(int accountId, int ownerId)
        {
            return _roomDAO.GetRoomByAccountIdAndOwnerId(accountId, ownerId);
        }

    }
}
